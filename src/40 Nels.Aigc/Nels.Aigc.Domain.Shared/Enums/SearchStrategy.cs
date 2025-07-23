using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nels.Aigc.Enums;
public enum SearchStrategy
{
    Hybrid = 0,
    Semantic = 1,
    FullText = 2
}

public enum ReplyMode
{
    Default = 0,
    Custom = 1
}

public enum SourceDisplayMode
{
    Card = 0,
    Text = 1
}
