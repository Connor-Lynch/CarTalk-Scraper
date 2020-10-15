using CarTalk.Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using TagLib;
using TagLib.Id3v2;

namespace CarTalk.Scraper
{
    public class FileCleaner : IFileCleaner
    {
        public DirectoryInfo _downloadDirectory;
        public string _downloadDirectoryString;
        public FileCleaner(string downloadDirectory)
        {
            _downloadDirectoryString = downloadDirectory;
            _downloadDirectory = new DirectoryInfo(downloadDirectory);
        }

        public void CleanFiles()
        {
            FileInfo[] Files = _downloadDirectory.GetFiles("*.mp3");
            string str = "";
            foreach (FileInfo file in Files)
            {
                str = str + ", " + file.Name;
                UpdateSingleFile(file.Name);
            }
        }

        private void UpdateSingleFile(string fileName)
        {
            var newFileName = GetCorectlyFormattedFileName(fileName);
            var newTitle = newFileName;
            var newTrackNumber = 0;
            if (newFileName.Contains("#"))
            {
                newTitle = GetCorectlyFormattedTitle(newFileName);
                newTrackNumber = GetTrackNumber(newTitle);
            }

            System.IO.File.Move($@"{_downloadDirectoryString}\{fileName}", $@"{_downloadDirectoryString}\{newFileName}");

            var podcastFile = TagLib.File.Create($@"{_downloadDirectoryString}\{newFileName}");

            podcastFile.Tag.Title = newTitle;
            podcastFile.Tag.Performers = new List<string>() { "Car Talk", "Click and Clack" }.ToArray();
            if(newTrackNumber != 0)
            {
                podcastFile.Tag.Track = Convert.ToUInt32(newTrackNumber);
            }
            podcastFile.Save();
        }

        public string GetCorectlyFormattedFileName(string title)
        {
            title = title.Replace(".mp3", "");
            title = title.Replace("Car Talk ", "").Trim();
            if (title.Contains("Show"))
            {
                var rootTitle = title.Split("Show")[0].Trim();
                var showNumber = title.Split("Show")[1].Trim().Replace(" ", "");
                title = $"{showNumber} {rootTitle}";
            }
            if (!title.Contains("#") && char.IsNumber(title, 0))
            {
                title = $"#{title}";
            }
            return $"{title}.mp3";
        }

        public string GetCorectlyFormattedTitle(string fileName)
        {
            var newFileName = fileName.Replace(".mp3", "");
            var showNumber = newFileName.Substring(0, 5);
            var showName = newFileName.Replace(showNumber, "").Trim();

            if(showName.EndsWith(","))
            {
                showName.TrimEnd(',');
            }

            return $"{showNumber}: {showName}";
        }

        private int GetTrackNumber(string title)
        {
            var baseStringTrackNumber = title.Split(":")[0].Trim().Split('#')[1].Trim();

            return int.Parse(baseStringTrackNumber);
        }
    }
}
