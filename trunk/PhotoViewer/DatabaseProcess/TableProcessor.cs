using System.Collections.Generic;
using PhotoViewer;
using PhotoInfo;

namespace database
{
    interface TableProcessor
    {
        Dictionary<string, PhotoTag> select(List<string> filename);
    }
}
