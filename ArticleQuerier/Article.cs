using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleQuery
{
    public class Article
    {
        #region Variables

        /// <summary>
        /// Article title
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Article body (plain text)
        /// </summary>
        public string PlainText
        {
            get;
            private set;
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
            set
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

        public ArticleType Type
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public Article(IHtmlPlainTextExtractor htmlPlainTextExtractor)
        {
            this.htmlPlainTextExtractor = htmlPlainTextExtractor;
        }

        public Article(IHtmlPlainTextExtractor htmlPlainTextExtractor, string title, string htmlText, ArticleType type = ArticleType.NotSpecified)
        {
            this.htmlPlainTextExtractor = htmlPlainTextExtractor;
            this.Title = title;
            this.HtmlText = htmlText;
            this.Type = type;
        }

        #endregion
    }
}
