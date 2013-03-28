using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using General;

namespace ArticleQuery
{
    public class ArticleQuerier
    {
        /// <summary>
        /// Default HTML to plain text extractor
        /// </summary>
        private IHtmlPlainTextExtractor defaultExtractor;

        // Pathes
        private string datasetPath;
        private string titlePath;
        private string summaryPath;
        private string typePath;

        /// <summary>
        /// Constructor with specified dataset
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        public ArticleQuerier(QueryDataset dataset)
            : this(dataset, new HtmlAgilityPackPlainTextExtractor())
        {
        }

        /// <summary>
        /// Constructor with specified dataset and default HTML extractor
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        /// <param name="defaultExtractor">The HTML extractor for articles</param>
        public ArticleQuerier(QueryDataset dataset, IHtmlPlainTextExtractor defaultExtractor)
        {
            // Generate dataset path
            switch (dataset)
            {
                case QueryDataset.TrainDataset:
                    this.datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TrainDatasetDir);
                    break;
                case QueryDataset.TestDataset:
                    this.datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TestDatasetDir);
                    break;
                default:
                    throw new NotImplementedException();
            }
            this.titlePath = Path.Combine(datasetPath, Constants.TitleDir);
            this.summaryPath = Path.Combine(datasetPath, Constants.SummaryDir);
            this.typePath = Path.Combine(datasetPath, Constants.TypeDir);

            this.defaultExtractor = defaultExtractor;
        }

        #region Query Functions

        /// <summary>
        /// Query all articles in specific dataset
        /// </summary>
        /// <returns>List of articles</returns>
        public List<Article> Query()
        {
            // Query with default HTML extractor
            return Query(defaultExtractor);
        }

        /// <summary>
        /// Query all articles in specific dataset, and use a given extractor as the HTML extractor
        /// </summary>
        /// <param name="extractor">The plain text extractor for article</param>
        /// <returns>List of articles</returns>
        public List<Article> Query(IHtmlPlainTextExtractor extractor)
        {
            List<Article> articles = new List<Article>();

            // Get file IDs
            // Must assume:
            // * The filenames in title folder must be same as the ones in summary folder
            // * Filename starts from 0, increase by 1
            string[] files = Directory.GetFiles(titlePath);
            System.Diagnostics.Debug.Assert(files.Length == Directory.GetFiles(summaryPath).Length);

            // Query each article
            for (int i = 0; i < files.Length; ++i)
            {
                articles.Add(Query(i, extractor));
            }

            return articles;
        }

        /// <summary>
        /// Query an article with given ID in specific dataset
        /// </summary>
        /// <param name="id">The ID of article</param>
        /// <returns>The article</returns>
        public Article Query(int id)
        {
            // Query with default HTML extractor
            return Query(id, defaultExtractor);
        }

        /// <summary>
        /// Query an article with given ID in specific dataset, and use a given extractor as the HTML extractor
        /// </summary>
        /// <param name="id">The ID of article</param>
        /// <param name="extractor">The HTML text extractor for article</param>
        /// <returns>The article</returns>
        public Article Query(int id, IHtmlPlainTextExtractor extractor)
        {
            string title;
            string summary;
            ArticleType type = ArticleType.NotSpecified;

            string titleFilePath = Path.Combine(titlePath, id.ToString());
            string summaryFilePath = Path.Combine(summaryPath, id.ToString());
            string typeFilePath = Path.Combine(typePath, id.ToString());

            // Read title and summary
            try
            {
                FileStream titleFileStream = new FileStream(titleFilePath, FileMode.Open, FileAccess.Read);
                StreamReader titleFileStreamReader = new StreamReader(titleFileStream);
                title = titleFileStreamReader.ReadToEnd();
                titleFileStreamReader.Close();

                FileStream summaryFileStream = new FileStream(summaryFilePath, FileMode.Open, FileAccess.Read);
                StreamReader summaryFileStreamReader = new StreamReader(summaryFileStream);
                summary = summaryFileStreamReader.ReadToEnd();
                summaryFileStreamReader.Close();
            }
            catch (IOException)
            {
                return null;
            }

            // Read marked type
            try
            {
                FileStream typeFileStream = new FileStream(typeFilePath, FileMode.Open, FileAccess.Read);
                StreamReader typeFileStreamReader = new StreamReader(typeFileStream);
                string typeString = typeFileStreamReader.ReadToEnd();
                typeFileStreamReader.Close();
                int typeInt = Convert.ToInt32(typeString);
                if (Enum.IsDefined(typeof(ArticleType), typeInt))
                {
                    type = (ArticleType)typeInt;
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (Exception)
            {
                // Type not specified yet
                type = ArticleType.NotSpecified;
            }

            // Construct article
            Article article = new Article(extractor, title, summary, type);

            return article;
        }

        #endregion Query Functions
    }
}
