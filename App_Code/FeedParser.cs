using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for FeedParser
/// </summary>
public class FeedParser
{

    private List<SyndicationFeed> _Feeds;

    public List<SyndicationFeed> Feeds
    {
        get { return _Feeds; }
        set { _Feeds = value; }
    }
    
	public FeedParser()
	{
		
	}
    public void addFeeds(List<string> items)
    {
        foreach (string item in items)
        {
            Uri feedUri = new Uri(item); 
            SyndicationFeed syndicationFeed;
            using (XmlReader reader = XmlReader.Create(feedUri.AbsoluteUri))
            {
                syndicationFeed = SyndicationFeed.Load(reader);
            }
            syndicationFeed.Id = item;

            Feeds.Add(syndicationFeed);
        }
    }
}