using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoInfo
{
    class LanTag: PhotoTag
    {
        //String SoundexEng;
        //String SoundexRomajic;

        public LanTag(List<string> tags, string s, string sr): base(tags)
        {
            SoundexEng = s;
            SoundexRomajic = sr;
            English = tags[0];
        }
    }
}
