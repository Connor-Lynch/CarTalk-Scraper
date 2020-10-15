using CarTalk.Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarTalk.Scraper
{
    public class Scraper
    {
        private IWebScraper _webScraper;
        private IFileDownloader _fileDownloader;
        private IFileMetadataManager _fileMetadataManager;
        private IFileCleaner _fileCleaner;

        private int _currentIndex;

        public Scraper(int startingIndex)
        {
            _webScraper = new WebScraper("https://www.npr.org/get/510208/render/partial/next?start=");
            _fileDownloader = new FileDownloader(@"C:\Users\Connor\Documents\CarTalkDownloads");
            _fileMetadataManager = new FileMetadataManager(@"C:\Users\Connor\Documents\CarTalkDownloads");
            _fileCleaner = new FileCleaner(@"C:\Users\Connor\Documents\CarTalkDownloads");

            _currentIndex = startingIndex;
        }

        public Scraper(int startingIndex, IWebScraper webScraper, IFileDownloader fileDownloader, 
            IFileMetadataManager fileMetadataManager)
        {
            _webScraper = webScraper;
            _fileDownloader = fileDownloader;
            _fileMetadataManager = fileMetadataManager;

            _currentIndex = startingIndex;
        }

        public void StartScraper()
        {
            while (true)
            {
                Console.WriteLine($"Current Index: {_currentIndex}");

                var currentPodcasts = _webScraper.GetCurrentResults(_currentIndex);

                Console.WriteLine($"Found {currentPodcasts.Count} podcasts, starting download.");

                if (currentPodcasts.Count == 0)
                {
                    Console.WriteLine($"There were no results found when searching with index {_currentIndex}");
                    break;
                }

                foreach (var podcast in currentPodcasts)
                {
                    Console.WriteLine($"Downloading Podcast Number {_currentIndex}, with Name {podcast.Title}");
                    var filePath = _fileDownloader.DownloadFile(podcast);
                    var result = _fileMetadataManager.SetFileMetadata(filePath, podcast);
                    _currentIndex++;
                }
            };

            _fileCleaner.CleanFiles();
        }
    }
}
