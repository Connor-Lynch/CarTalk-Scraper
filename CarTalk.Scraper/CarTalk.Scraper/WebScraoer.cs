using CarTalk.Scraper.Interfaces;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarTalk.Scraper
{
    public class WebScraper : IWebScraper
    {
        private ScrapingBrowser _browser;
        private string _rootUrl;

        public WebScraper(string rootUrl)
        {
            _rootUrl = rootUrl;
            _browser = new ScrapingBrowser();
        }

        public List<Podcast> GetCurrentResults(int startingIndex)
        {
            var finalPodcasts = new List<Podcast>();
            var currentResultsPage = GetHtml(_rootUrl + startingIndex);
            var rawPodcastEpisodes = currentResultsPage.CssSelect("article").Where(r => r.FirstChild.Name == "#text").ToList();

            foreach (var rawPodcast in rawPodcastEpisodes)
            {
                finalPodcasts.Add(GetPodcastFromRaw(rawPodcast));
            };

            return finalPodcasts;
        }

        private HtmlNode GetHtml(string url)
        {
            WebPage webpage = _browser.NavigateToPage(new Uri(url));
            return webpage.Html;
        }

        private Podcast GetPodcastFromRaw(HtmlNode rawPodcast)
        {
            var title = rawPodcast.CssSelect("h2").First().InnerText;
            var releaseDate = DateTime.Parse(rawPodcast.CssSelect("time").First().InnerText);
            var description = rawPodcast.CssSelect("p").First().InnerText;
            var downloadUrl = rawPodcast.CssSelect("a").Where(a => a.InnerText == "Download").First().Attributes["href"].Value;

            return new Podcast()
            {
                Title = title,
                TrackNumber = GetTrackNumber(title),
                Description = GetCleanDescription(description),
                ReleaseDate = releaseDate,
                DownloadUrl = GetRootDownloadUrl(downloadUrl)
            };
        }

        private int GetTrackNumber(string title)
        {
            var baseStringTrackNumber = title.Split(":")[0].Trim();
            if (baseStringTrackNumber.Contains("#"))
            {
                baseStringTrackNumber = baseStringTrackNumber.Split("#")[1].Trim();
            } else if (baseStringTrackNumber.Contains("!"))
            {
                baseStringTrackNumber = baseStringTrackNumber.Split("!")[1].Trim();
            } else
            {
                return 0;
            }
            var numericTrackNumber = GetNumbers(baseStringTrackNumber);
            return string.IsNullOrEmpty(baseStringTrackNumber) ? 0 : int.Parse(numericTrackNumber);
        }
        private string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        private string GetCleanDescription(string rawDescription)
        {
            return rawDescription.Split("???")[1].Trim();
        }

        private string GetRootDownloadUrl(string rawDownloadUrl)
        {
            return rawDownloadUrl.Split(".mp3")[0] + ".mp3";
        }
    }
}
