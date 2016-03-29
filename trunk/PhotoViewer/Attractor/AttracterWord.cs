using System;
using System.Collections.Generic;
using System.Linq;
using PhotoViewer.Element;
using PhotoInfo;

namespace Attractor
{
    class AttractorWord
    {
        private Dictionary<string, List<Photo>> wordDis = new Dictionary<string, List<Photo>>();
        private int candidates = 4;
        List<Photo> photos = null;
        
        public List<Photo> Photos
        {
            get
            {
                return photos;
            }
            set
            {
                photos = value;
                wordDis.Clear();
            }
        }

        public int match(Photo pivot, Photo photo)
        {
            if (pivot.ptag.SoundexRomajic == null)
                return -1;
            if (!wordDis.ContainsKey(pivot.ptag.SoundexRomajic))
                distanceCal(pivot);
            return wordDis[pivot.ptag.SoundexRomajic].IndexOf(photo);
        }

        public void distanceCal(Photo pivot)
        {
            string focus = pivot.ptag.SoundexRomajic;
            Dictionary<Photo, int> firstSame = new Dictionary<Photo, int>();
            Dictionary<Photo, int> firstDiff = new Dictionary<Photo, int>();
            foreach (Photo p in photos)
            {
                if (p.ptag.SoundexEng != null)
                {
                    if (p.ptag.SoundexEng[0] == focus[0])
                    {
                        firstSame[p] = LevenshteinDistance.Compute(focus, p.ptag.SoundexEng);
                    }
                    else firstDiff[p] = LevenshteinDistance.Compute(focus, p.ptag.SoundexEng);
                }
            }

            if (firstSame.Count < candidates)
            {
                wordDis[pivot.ptag.SoundexRomajic] = firstSame.Keys.ToList();
                if (firstSame.Count < 4)
                {
                    List<KeyValuePair<Photo, int>> diffList = firstDiff.ToList();
                    diffList.Sort(
                        delegate(KeyValuePair<Photo, int> firstPair,
                                KeyValuePair<Photo, int> nextPair)
                        {
                            return firstPair.Value.CompareTo(nextPair.Value);
                        }
                        );
                    for (int i = 0; i < candidates - firstSame.Count; i++)
                    {
                        wordDis[pivot.ptag.SoundexRomajic].Add(diffList[i].Key);
                    }
                }
            }
            else
            {
                List<KeyValuePair<Photo, int>> sameList = firstSame.ToList();
                sameList.Sort(
                    delegate(KeyValuePair<Photo, int> firstPair,
                            KeyValuePair<Photo, int> nextPair)
                    {
                        return firstPair.Value.CompareTo(nextPair.Value);
                    }
                    );
                List<Photo> list = new List<Photo>();
                for (int i = 0; i < candidates; i++)
                {
                    list.Add(sameList[i].Key);
                }
                wordDis[pivot.ptag.SoundexRomajic] = list;
            }
        }
    }
}
