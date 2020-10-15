using System;

namespace CarTalk.Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new Scraper(1);

            scraper.StartScraper();
        }
    }
}
