using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleQuery
{
    /// <summary>
    /// An article with complete information, including title, summary, etc.
    /// </summary>
    public class Article
    {
        #region Variables

        /// <summary>
        /// Article title
        /// </summary>
        public string Title
        {
            get;
            protected set;
        }

        /// <summary>
        /// Article body (plain text)
        /// </summary>
        public string PlainText
        {
            get;
            protected set;
        }

        /// <summary>
        /// Article body (HTML text)
        /// </summary>
        public string HtmlText
        {
            get
            {
                return htmlText;
            }
            protected set
            {
                htmlText = value;
                if (htmlPlainTextExtractor == null)
                {
                    PlainText = htmlText;
                }
                else
                {
                    PlainText = htmlPlainTextExtractor.Extract(htmlText);
                }
            }
        }
        protected string htmlText;

        /// <summary>
        /// Extractor to extract plain text from HTML
        /// </summary>
        public IHtmlPlainTextExtractor HtmlPlainTextExtractor
        {
            get
            {
                return htmlPlainTextExtractor;
            }
            set
            {
                htmlPlainTextExtractor = value;
                if (htmlText != null)
                {
                    if (htmlPlainTextExtractor != null)
                    {
                        PlainText = htmlPlainTextExtractor.Extract(htmlText);
                    }
                    else
                    {
                        PlainText = htmlText;
                    }
                }
            }
        }
        protected IHtmlPlainTextExtractor htmlPlainTextExtractor;

        /// <summary>
        /// Type of article
        /// </summary>
        public ArticleType Type
        {
            get;
            set;
        }

        /// <summary>
        /// The real file path of article title
        /// </summary>
        public string TitlePath
        {
            get;
            protected set;
        }

        /// <summary>
        /// The real file path of article summary
        /// </summary>
        public string SummaryPath
        {
            get;
            protected set;
        }

        /// <summary>
        /// The real file path of article type
        /// </summary>
        public string TypePath
        {
            get;
            protected set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// An empty article with given HTML extractor
        /// </summary>
        /// <param name="htmlPlainTextExtractor">The specified HTML extractor</param>
        protected Article(IHtmlPlainTextExtractor htmlPlainTextExtractor)
            : this(htmlPlainTextExtractor, "", "")
        {
        }

        /// <summary>
        /// An article with given HTML extractor, title, HTML text and article type
        /// </summary>
        /// <param name="htmlPlainTextExtractor">The specified HTML extractor</param>
        /// <param name="title">Title of article</param>
        /// <param name="htmlText">HTML text of article</param>
        /// <param name="type">Type of article</param>
        public Article(IHtmlPlainTextExtractor htmlPlainTextExtractor, string title, string htmlText, ArticleType type = ArticleType.NotSpecified)
            : this(htmlPlainTextExtractor, title, htmlText, "", "", "", type)
        {
        }

        /// <summary>
        /// An article with given HTML extractor, title, HTML text and article type
        /// </summary>
        /// <param name="htmlPlainTextExtractor">The specified HTML extractor</param>
        /// <param name="title">Title of article</param>
        /// <param name="htmlText">HTML text of article</param>
        /// <param name="titlePath">File path of title</param>
        /// <param name="summaryPath">File path of summary</param>
        /// <param name="typePath">File path of type</param>
        /// <param name="type">Type of article</param>
        public Article(IHtmlPlainTextExtractor htmlPlainTextExtractor, string title, string htmlText, string titlePath, string summaryPath, string typePath, ArticleType type = ArticleType.NotSpecified)
        {
            this.htmlPlainTextExtractor = htmlPlainTextExtractor;
            this.Title = title;
            this.HtmlText = htmlText;
            this.TitlePath = titlePath;
            this.SummaryPath = summaryPath;
            this.TypePath = typePath;
            this.Type = type;
        }

        #endregion
    }
}
