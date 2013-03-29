using ArticleQuery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrainDataLabeler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        
        /// <summary>
        /// The default query dataset
        /// </summary>
        private QueryDataset dataset = QueryDataset.TrainDataset;

        /// <summary>
        /// ID of article, -1 means no article for current dataset
        /// </summary>
        public int articleID
        {
            get
            {
                return _articleID;
            }
            set
            {
                _articleID = value;
                RefreshArticle();
            }
        }
        private int _articleID = -1;

        /// <summary>
        /// List of all articles
        /// </summary>
        List<Article> articles = new List<Article>();

        #endregion Variables

        #region Constructors

        /// <summary>
        /// MainWindow constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // get all articles under current dataset
            ArticleQuerier querier = new ArticleQuerier(dataset);
            articles = querier.Query();

            // set default article ID
            if (articles.Count > 0)
            {
                articleID = 0;
            }
        }

        #endregion Constructors

        #region Events

        #region Key Events

        /// <summary>
        /// Press a key combo to label current article or navigate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LabelWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                switch (e.Key)
                {
                    case Key.D1:
                    case Key.NumPad1:
                        LabelArticle(ArticleType.ACMRelated);
                        break;

                    case Key.D2:
                    case Key.NumPad2:
                        LabelArticle(ArticleType.TechnologyRelated);
                        break;

                    case Key.D3:
                    case Key.NumPad3:
                        LabelArticle(ArticleType.Others);
                        break;

                    case Key.OemPlus:
                    case Key.Add:
                        NavigateForward();
                        break;

                    case Key.OemMinus:
                    case Key.Subtract:
                        NavigateBackward();
                        break;
                }
            }
        }

        private void articleIDTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // when press enter in articleIDTextBox
            if (e.Key == Key.Enter)
            {
                try
                {
                    // Load new article
                    articleID = Convert.ToInt32(articleIDTextBox.Text);
                }
                catch (FormatException)
                {
                    articleID = -1;
                }
                catch (OverflowException)
                {
                    articleID = -1;
                }
            }
        }

        #endregion

        #region Button Events

        private void markAsACMButton_Click(object sender, RoutedEventArgs e)
        {
            LabelArticle(ArticleType.ACMRelated);
        }

        private void markAsTechnologyButton_Click(object sender, RoutedEventArgs e)
        {
            LabelArticle(ArticleType.TechnologyRelated);
        }

        private void markAsOthersButton_Click(object sender, RoutedEventArgs e)
        {
            LabelArticle(ArticleType.Others);
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBackward();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateForward();
        }

        #endregion Button Events

        #endregion Events

        #region Functions

        /// <summary>
        /// Load new article by articleID
        /// </summary>
        private void RefreshArticle()
        {
            if (articleID >= articles.Count || articleID < 0)
            {
                titleTextBox.Text = "";
                summaryTextBox.Text = "";
                articleIDTextBox.Text = "-1";
                RefreshArticleType(ArticleType.NotSpecified);
            }
            else
            {
                Article article = articles[articleID];

                titleTextBox.Text = article.Title;
                summaryTextBox.Text = article.PlainText;
                articleIDTextBox.Text = articleID.ToString();
                RefreshArticleType(article.Type);
            }
        }

        private void RefreshArticleType(ArticleType type)
        {
            articleTypeLabel.Content = Enum.GetName(type.GetType(), type);
            if (type == ArticleType.NotSpecified)
            {
                articleTypeLabel.BorderBrush = Brushes.Red;
            }
            else
            {
                articleTypeLabel.BorderBrush = Brushes.Green;
            }
        }

        /// <summary>
        /// Label current article by given article type
        /// </summary>
        /// <param name="type">New article type</param>
        private void LabelArticle(ArticleType type)
        {
            if (articleID >= 0 && articleID < articles.Count)
            {
                Article article = articles[articleID];
                try
                {
                    FileStream typeFileStream = new FileStream(article.TypePath, FileMode.Create, FileAccess.Write);
                    StreamWriter typeFileStreamWriter = new StreamWriter(typeFileStream);
                    typeFileStreamWriter.Write((int)type);
                    typeFileStreamWriter.Close();
                    
                    article.Type = type;
                    RefreshArticleType(type);
                }
                catch (IOException)
                {
                    MessageBox.Show("Failed to save label file!");
                }
            }
        }

        /// <summary>
        /// Navigate to next article
        /// </summary>
        private void NavigateForward()
        {
            if (articleID == articles.Count - 1)
            {
                articleID = 0;
            }
            else
            {
                articleID++;
            }
        }

        /// <summary>
        /// Navigate to previous article
        /// </summary>
        private void NavigateBackward()
        {
            if (articleID == 0)
            {
                articleID = articles.Count - 1;
            }
            else
            {
                articleID--;
            }
        }

        #endregion Functions
    }
}
