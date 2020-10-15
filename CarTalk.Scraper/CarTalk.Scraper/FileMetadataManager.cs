using CarTalk.Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using TagLib;
using TagLib.Id3v2;

namespace CarTalk.Scraper
{
    public class FileMetadataManager : IFileMetadataManager
    {
        public string _downloadDirectory;
        public FileMetadataManager(string downloadDirectory)
        {
            _downloadDirectory = downloadDirectory;
        }

        public bool SetFileMetadata(string filePath, Podcast podcast)
        {
            var podcastFile = File.Create(filePath);

            podcastFile.Tag.Description = podcast.Description;
            podcastFile.Tag.Album = "Car Talk";
            podcastFile.Tag.AlbumArtists = new List<string>() {"NPR"}.ToArray();
            podcastFile.Tag.DateTagged = podcast.ReleaseDate;
            podcastFile.Tag.Year = Convert.ToUInt32(podcast.ReleaseDate.Year);
            podcastFile.Tag.Pictures = SetAlbumArt();
            if (podcast.TrackNumber != 0)
            {
                podcastFile.Tag.Track = Convert.ToUInt32(podcast.TrackNumber);
            }
            podcastFile.Save();

            return true;
        }

        public IPicture[] SetAlbumArt()
        {            
            var imageBytes = System.IO.File.ReadAllBytes($@"{_downloadDirectory}\cartalkCover.jpg");

            AttachmentFrame cover = new AttachmentFrame()
            {
                Type = PictureType.FrontCover,
                Description = "Cover",
                MimeType = MediaTypeNames.Image.Jpeg,
                Data = imageBytes,
                TextEncoding = StringType.UTF16
            };
            return new IPicture[] { cover };
        }
    }
}
