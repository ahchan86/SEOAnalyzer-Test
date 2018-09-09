using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Data;
using SEOAnalyzer.Models;
using Newtonsoft.Json;

namespace SEOAnalyzer.Helper
{
    public class Util
    {

        public static bool IsURLValidAsync(string URL)
        {
            var isValidURL = false;
            try
            {
                var web = new HtmlAgilityPack.HtmlWeb();
                var docURL = web.LoadFromWebAsync(URL);
                if (docURL != null)
                {
                    isValidURL = true;
                }
            }
            catch (Exception)
            {
            }

            return isValidURL;
        }

        public static Dictionary<string, int> GetAllWordsInfo(string searchText, bool isPageFilterStopWords, bool isURL)
        {
            var listOfWords = new List<string>();


            if (isURL)
            {
                var web = new HtmlAgilityPack.HtmlWeb();
                var doc = web.Load(searchText);
                var root = doc.DocumentNode.SelectSingleNode("//body");
                var allText = NUglify.Uglify.HtmlToText(root.InnerHtml);
                listOfWords = GetAllWords(allText.Code);
            }
            else
            {
                listOfWords = GetAllWords(searchText);
            }


            if (isPageFilterStopWords)
            {
                listOfWords = FilterStopWords(listOfWords, "stopWords.json");
            }

            return GroupListOfString(listOfWords);
        }

        public static List<string> FilterStopWords(List<string> Words, string stopWordsPath)
        {
            var jsonText = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(stopWordsPath));
            var stopWords = JsonConvert.DeserializeObject<IList<string>>(jsonText);

            var matches = Words.Where(word => !stopWords.Contains(word));

            return matches.ToList<string>();
        }

        public static List<string> GetAllWords(string text)
        {

            var words = SplitSentenceIntoWords(text.ToLower(), 1);

            List<string> modifiedWords = new List<string>();

            foreach (var word in words)
            {
                var stripedWords = word;

                if (!string.IsNullOrWhiteSpace(stripedWords) &&
                    Regex.IsMatch(stripedWords, "^[a-z\u00c0-\u00f6]+$", RegexOptions.IgnoreCase) &&
                    stripedWords.Length > 1)
                {
                    modifiedWords.Add(stripedWords);
                }

            }

            return modifiedWords.ToList<string>();


        }

        public static IEnumerable<string> SplitSentenceIntoWords(string sentence, int wordMinLength)
        {
            var word = new StringBuilder();
            foreach (var chr in sentence)
            {
                if (Char.IsPunctuation(chr) || Char.IsSeparator(chr) || Char.IsWhiteSpace(chr))
                {
                    if (word.Length > wordMinLength)
                    {
                        yield return word.ToString();
                        word.Clear();
                    }
                }
                else
                {
                    word.Append(chr);
                }
            }

            if (word.Length > wordMinLength)
            {
                yield return word.ToString();
            }
        }

        public static Dictionary<string, int> GroupListOfString(List<string> listofString)
        {

            return listofString.GroupBy(word => word)
               .ToDictionary(group => group.Key, group => group.Count());


        }


        public static Dictionary<string, int> GetAllExternalLinks(string searchText)
        {

            var web = new HtmlAgilityPack.HtmlWeb();
            var doc = web.Load(searchText);
            var nodeSingle = doc.DocumentNode.SelectSingleNode("//html");



            var listofURL = new List<String>();

            listofURL = GetAllExternalLinksFromText(nodeSingle.OuterHtml);

            return GroupListOfString(listofURL);
        }

        public class FilterFormat
        {
            public const string GetAllLinks = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
        }

        public static List<string> GetAllExternalLinksFromText(string text)
        {

            List<string> listofURL = new List<string>();

            MatchCollection mc = Regex.Matches(text, FilterFormat.GetAllLinks);
            foreach (Match match in mc)
            {
                listofURL.Add(match.Value);
            }

            return listofURL;

        }


    }
}