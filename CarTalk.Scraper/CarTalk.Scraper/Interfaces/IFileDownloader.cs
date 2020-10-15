using System;
using System.Collections.Generic;
using System.Text;

namespace CarTalk.Scraper.Interfaces
{
    public interface IFileDownloader
    {
        string DownloadFile(Podcast podcast);
    }
}
