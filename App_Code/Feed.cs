using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Feed
/// </summary>
public class Feed
{
	public Feed()
    {

    }
	private string _Title;

	public string Title
	{
		get { return _Title;}
		set { _Title = value;}
	}

    private string _Description;

    public string Description
    {
        get { return _Description; }
        set { _Description = value; }
    }

    private string _Content;

    public string Content
    {
        get { return _Content; }
        set { _Content = value; }
    }

    private DateTime _Date;

    public DateTime Date
    {
        get { return _Date; }
        set { _Date = value; }
    }

    private string _Creator;

    public string Creator
    {
        get { return _Creator; }
        set { _Creator = value; }
    }

    private string[] _Category;

    public string[] Category
    {
        get { return _Category; }
        set { _Category = value; }
    }

    private string _Link;

    public string Link
    {
        get { return _Link; }
        set { _Link = value; }
    }

	
	
}