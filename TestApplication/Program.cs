﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArticleQuery;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ArticleQuerier querier = new ArticleQuerier(QueryDataset.TrainDataset);
            Article article = querier.Query(1);
            Console.WriteLine(article.Title);
            Console.WriteLine(article.PlainText);
            Console.ReadKey();
        }
    }
}
