using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ArticleQuery
{
    public class HtmlAgilityPackPlainTextExtractor : IHtmlPlainTextExtractor
    {
        public string Extract(string htmlText)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlText);

            return htmlDocument.DocumentNode.InnerText;
        }
    }
}
