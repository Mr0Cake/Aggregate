using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;
using System.IO;

namespace aggretateService
{
    public class Program
    {
        private static bool databaseWrite = false;
        private static  bool start = false;
        private static int errorCount = 0;
        private static string Feed = "";
        private static string ErrorMessages = "";
        private static ConcurrentQueue<FeedItem> DatabaseQueue;
        private static int delay = 0;
        private static Task dbtask;
        private static bool FirstItemDone = false;

        public static void Main(string[] args)
        {
            DatabaseQueue = new ConcurrentQueue<FeedItem>();
            start = true;
            databaseWrite = true;
            dbtask = Task.Factory.StartNew(() => DatabaseAdder());
            startThread();

            Console.WriteLine("Aggregate service started:");
            Console.ReadLine();
        }
        

        private static void AddFeedItems(string sp)
        {
            int count = 0;
            DAL.CommonMethods.GetDataTable(sp, (DataRow dr) => { return Convert.ToString(dr["FeedUrl"]); }).ForEach(
                x => { Task.Factory.StartNew(() => readXML(x)); count++; });
            if(count > 0)
            {
                delay = 90;
            }
        }

        


        private static void startThread()
        {
            while (start)
            {
                Console.WriteLine("loop started");
                Thread.Sleep(delay * 1000);
                AddFeedItems("usp_select_AllFeeds_ByTime");
                if (delay == 0)
                {
                    AddFeedItems("usp_select_AllFeeds");
                }
                if(dbtask != null)
                {
                    dbtask.Wait();
                    Console.Write("done waiting");
                }
                //Console.WriteLine("Started checking for items");
            }
            
        }

        static async Task PutTaskDelay(int delay)
        {
            await Task.Delay(delay);
        }

        private static void readXML(string feed)
        {
            Console.WriteLine("Reading " + feed);
            Uri feedUri;
            Uri.TryCreate(feed, UriKind.Absolute, out feedUri);
            feedUri.ToString();
            SyndicationFeed syndicationFeed;
            try {
                string Url = feed;
                //HtmlWeb web = new HtmlWeb();
                //HtmlDocument doc = web.Load(Url);
                //Console.WriteLine(doc.ToString());
                using (XmlReader reader = XmlReader.Create(feed))
                {
                    syndicationFeed = SyndicationFeed.Load(reader);
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Exception on feedreader");
                Console.WriteLine(ex.Message);
                return;
            }
            syndicationFeed.Id = feed;
            Feed = feed;
            foreach (SyndicationItem si in syndicationFeed.Items)
            {
                FeedItem feedItem = new FeedItem();
                try
                {
                    feedItem = new FeedItem();
                    feedItem.setValues();

                    #region Things that should not go wrong
                    feedItem.FeedID = feed;
                    
                    feedItem.Title = si.Title.Text;


                    //Console.WriteLine("FeedItem: " + feedItem.Title);
                    if (si.Summary.Text.Length > 5)
                    {
                        feedItem.Summary = si.Summary.Text;
                    }

                    if (si.BaseUri != null)
                    feedItem.BaseUri = si.BaseUri.ToString();

                    if (si.Id != null)
                        feedItem.ID = si.Id;

                    if (si.PublishDate != null)
                        feedItem.PublishDate = si.PublishDate;

                    if (si.LastUpdatedTime != null)
                        feedItem.LastUpdated = si.LastUpdatedTime;

                    #endregion

                    #region Things that can go wrong

                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(new StringReader(si.Summary.Text));
                    HtmlNode node = doc.DocumentNode.SelectNodes("//img").FirstOrDefault();
                    if(node != null)
                    {
                        Console.WriteLine(node.OuterHtml);
                        feedItem.ImageLink = node.OuterHtml;
                    }


                    int count = si.Authors != null ? si.Authors.Count : 0;
                    foreach (SyndicationPerson sp in si.Authors.Where(x => x != null))
                        feedItem.Authors += sp.Name + (count-- > 1 ? ", " : "");


                    if (si.Links != null) {
                        count = si.Links.Where(x => x != null).Count();
                        foreach (SyndicationLink sl in si.Links.Where(x => x != null))
                            feedItem.Links += sl.BaseUri + (count-- > 1 ? ", " : "");
                    }

                    

                    if(si.SourceFeed != null && si.SourceFeed.BaseUri != null && !string.IsNullOrEmpty(si.SourceFeed.BaseUri.ToString()))
                    feedItem.SourceFeed = si.SourceFeed.BaseUri.ToString();

                    if (si.Categories != null)
                    {
                        count = si.Categories.Count;
                        foreach (SyndicationCategory sc in si.Categories.Where(x => x != null))
                            feedItem.Categories += sc.Name + (count-- > 1 ? ", " : "");
                        if (si.Content != null)
                        {
                            TextSyndicationContent content = (TextSyndicationContent)si.Content;
                            feedItem.Content = content.Text;
                        }
                    }

                    if (si.Contributors != null)
                    {
                        count = si.Contributors.Where(x => x != null).Count();
                        foreach (SyndicationPerson spc in si.Contributors.Where(x => x != null))
                            feedItem.Contributors += spc.Name + (count-- > 1 ? ", " : "");
                    }
                    if (si.Copyright != null)
                        feedItem.Copyright = string.IsNullOrEmpty(si.Copyright.Text) ? "" : si.Copyright.Text;

                    #endregion
                }
                catch (Exception ex)
                {
                    errorCount++;
                    ErrorMessages += ex.Message + Environment.NewLine;
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }
                finally
                {
                    //check if Summary, Title, date are present
                    if (feedItem.Title != null && feedItem.Title.Length > 0 &&
                        feedItem.PublishDate != null)
                    {
                        DatabaseQueue.Enqueue(feedItem);
                        FirstItemDone = true;
                        //Console.WriteLine("FeedItem added:");
                    }
                }
            }
            if (errorCount > 0)
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Credentials = new System.Net.NetworkCredential("PermissionGranter123@hotmail.com", "Azerty123");
                client.EnableSsl = true;
                mail.IsBodyHtml = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.live.com";
                mail.To.Add(new MailAddress("wuestenbergs.kevin@gmail.com"));
                mail.From = new MailAddress("PermissionGranter123@hotmail.com");

                mail.Subject = "Failure to add items";
                mail.Body = "Errors: " + errorCount + Environment.NewLine + ErrorMessages;
                //client.Send(mail);
                Console.WriteLine("Mail sent to admin");
            }
            errorCount = 0;
            ErrorMessages = "";
        }

        static int counter = 0;
        static int addedCounter = 0;

        private static async void DatabaseAdder()
        {
            await Task.Factory.StartNew(() => { while (!FirstItemDone) ; });
            Console.Write("item added");
            while (databaseWrite)
            {
                if (DatabaseQueue.Count > 0)
                {
                    FeedItem towrite;
                    if (DatabaseQueue.TryDequeue(out towrite))
                    {
                        Console.Write(".");
                        counter++;
                        WriteToDatabase(towrite);
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Processed items "+ counter);
                    Console.WriteLine("Added items " + addedCounter);
                    Console.WriteLine("Sleep");
                    counter = 0;
                    addedCounter = 0;
                    await PutTaskDelay(20000);
                }
            }
            Console.Write("Database adder stopped");
        }


        private static void WriteToDatabase(FeedItem feedItem)
        {
            Create(feedItem);
        }

       

        private static void Create(FeedItem u)
        {

            List<SqlParameter> sqlparams = new List<SqlParameter> {
              DAL.CommonMethods.parameter("p_Title",u.Title)
              ,DAL.CommonMethods.parameter("p_PublishDate",u.PublishDate.DateTime)
              ,DAL.CommonMethods.parameter("p_Summary",u.Summary)
              ,DAL.CommonMethods.parameter("p_ArticleLink",u.BaseUri)
              ,DAL.CommonMethods.parameter("p_FeedID",u.FeedID)
              ,DAL.CommonMethods.parameter("p_Authors",u.Authors)
              ,DAL.CommonMethods.parameter("p_Contributors",u.Contributors)
              ,DAL.CommonMethods.parameter("p_Links",u.Links)
              ,DAL.CommonMethods.parameter("p_SourceFeed",u.SourceFeed)
              ,DAL.CommonMethods.parameter("p_Copyright",u.Copyright)
              ,DAL.CommonMethods.parameter("p_Content",u.Content)
              ,DAL.CommonMethods.parameter("p_Image", u.ImageLink)
              };
            int errorcode;
            int identity;
            DAL.CommonMethods.UpdateDataTable("usp_insert_FeedItem_Check", out errorcode, out identity, sqlparams.ToArray());
            if (errorcode == 0)
            {
                addedCounter++;
            }
        }
        
        private struct FeedItem
        {

            public void setValues()
            {
                FeedID = "";
                Authors = "";
                BaseUri = "";
                Categories = "";
                Content = "";
                Contributors = "";
                Copyright = "";
                ID = "";
                LastUpdated = DateTimeOffset.MinValue;
                PublishDate = DateTime.Now.ToUniversalTime();
                SourceFeed = "";
                Summary = "";
                Title = "";
                Links = "";
                ImageLink = "";
            }
            public string FeedID;
            public string Authors;
            public string BaseUri;
            public string Categories;
            public string Content;
            public string Contributors;
            public string Copyright;
            public string ID;
            public DateTimeOffset LastUpdated;
            public string Links;
            public DateTimeOffset PublishDate;
            public string SourceFeed;
            public string Summary;
            public string Title;
            public string ImageLink;
        }
    }
}
