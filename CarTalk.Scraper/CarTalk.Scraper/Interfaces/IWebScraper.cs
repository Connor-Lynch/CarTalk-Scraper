using System;
using System.Collections.Generic;
using System.Text;

namespace CarTalk.Scraper.Interfaces
{
    public interface IWebScraper
    {
        List<Podcast> GetCurrentResults(int startingIndex);
    }
}
