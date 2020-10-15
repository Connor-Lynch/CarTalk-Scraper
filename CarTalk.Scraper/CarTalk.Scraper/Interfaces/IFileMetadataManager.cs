using System;
using System.Collections.Generic;
using System.Text;

namespace CarTalk.Scraper.Interfaces
{
    public interface IFileMetadataManager
    {
        bool SetFileMetadata(string filePath, Podcast podcast);
    }
}
