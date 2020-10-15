using System;
using System.Collections.Generic;
using System.Text;

namespace CarTalk.Scraper
{
    public class Podcast
    {
        public string Title { get; set; }
        public int TrackNumber { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DownloadUrl { get; set; }
    }
}
