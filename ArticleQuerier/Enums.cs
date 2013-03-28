using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleQuery
{
    public enum QueryDataset
    {
        TrainDataset,
        TestDataset
    }

    public enum ArticleType
    {
        NotSpecified = 0,
        ACMRelated = 1,
        TechnologyRelated = 2,
        Others = 3
    }
}
