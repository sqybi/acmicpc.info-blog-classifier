using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleQuery
{
    public interface IHtmlPlainTextExtractor
    {
        string Extract(string htmlText);
    }
}
