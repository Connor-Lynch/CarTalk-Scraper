using CarTalk.Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace CarTalk.Scraper
{
    public class FileDownloader : IFileDownloader
    {
        public string _downloadDirectory;

        public FileDownloader(string downloadDirectory)
        {
            _downloadDirectory = downloadDirectory;
        }

        public string DownloadFile(Podcast podcast)
        {
            var fileName = string.Concat(podcast.Title.Split(Path.GetInvalidFileNameChars()));
            var completeFilePath = $@"{_downloadDirectory}\{fileName}.mp3";

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(podcast.DownloadUrl, completeFilePath);
            }
            return completeFilePath;
        }
    }
}
