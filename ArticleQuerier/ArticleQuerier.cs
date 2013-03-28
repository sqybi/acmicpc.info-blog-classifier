using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ArticleQuery
{
    public static class ArticleQuerier
    {
        /// <summary>
        /// Default HTML to plain text extractor
        /// </summary>
        private static IHtmlPlainTextExtractor defaultExtractor = new HtmlAgilityPackPlainTextExtractor();

        /// <summary>
        /// Query all articles in specific dataset
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        /// <returns>List of articles</returns>
        public static List<Article> Query(QueryDataset dataset)
        {
            return Query(dataset, defaultExtractor);
        }

        /// <summary>
        /// Query all articles in specific dataset, and use a given extractor as the HTML extractor
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        /// <param name="extractor">The plain text extractor for article</param>
        /// <returns>List of articles</returns>
        public static List<Article> Query(QueryDataset dataset, IHtmlPlainTextExtractor extractor)
        {
            List<Article> articles = new List<Article>();

            // Generate dataset path
            string datasetPath;
            switch (dataset)
            {
                case QueryDataset.TrainDataset:
                    datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TrainDatasetDir);
                    break;
                case QueryDataset.TestDataset:
                    datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TestDatasetDir);
                    break;
                default:
                    throw new NotImplementedException();
            }

            string[] files = Directory.GetFiles(datasetPath);

            for (int i = 0; i < files.Length; ++i)
            {
                articles.Add(Query(dataset, i, extractor));
            }

            return articles;
        }

        /// <summary>
        /// Query an article with given ID in specific dataset
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        /// <param name="id">The ID of article</param>
        /// <returns>The article</returns>
        public static Article Query(QueryDataset dataset, int id)
        {
            return Query(dataset, id, defaultExtractor);
        }

        /// <summary>
        /// Query an article with given ID in specific dataset, and use a given extractor as the HTML extractor
        /// </summary>
        /// <param name="dataset">The query dataset</param>
        /// <param name="id">The ID of article</param>
        /// <param name="extractor">The plain text extractor for article</param>
        /// <returns>The article</returns>
        public static Article Query(QueryDataset dataset, int id, IHtmlPlainTextExtractor extractor)
        {
            string title;
            string summary;
            ArticleType type = ArticleType.NotSpecified;

            // Generate dataset path
            string datasetPath;
            switch (dataset)
            {
                case QueryDataset.TrainDataset:
                    datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TrainDatasetDir);
                    break;
                case QueryDataset.TestDataset:
                    datasetPath = Path.Combine(General.Constants.DatasetPath, General.Constants.TestDatasetDir);
                    break;
                default:
                    throw new NotImplementedException();
            }
            string titlePath = Path.Combine(datasetPath, "title", id.ToString());
            string summaryPath = Path.Combine(datasetPath, "summary", id.ToString());
            string typePath = Path.Combine(datasetPath, "type", id.ToString());

            // Read title and summary
            try
            {
                FileStream titleFileStream = new FileStream(titlePath, FileMode.Open, FileAccess.Read);
                StreamReader titleFileStreamReader = new StreamReader(titleFileStream);
                title = titleFileStreamReader.ReadToEnd();
                titleFileStreamReader.Close();

                FileStream summaryFileStream = new FileStream(summaryPath, FileMode.Open, FileAccess.Read);
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
                FileStream typeFileStream = new FileStream(typePath, FileMode.Open, FileAccess.Read);
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
            }

            // Construct article
            Article article = new Article(extractor, title, summary, type);

            return article;
        }
    }
}
