using System.Collections.Generic;
using Attractor;
using PhotoInfo;
using PhotoViewer.Element;

namespace PhotoViewer.Manager
{
    public class SystemState
    {
        private static readonly IAttractorSelection[] Attractors =
        {
            new AttractorBound(),
            new AttractorTime(),            
            new AttractorGeograph(),
            new AttractorFrame(),
            new AttractorAvoid(),
            new AttractorAvoidScale(),
            new AttractorTag(),
            //new AttracterWord(),
            //new AttractorColor(),
            
            //new AttractorPeople(),
            new AttractorScaleUpMouse(),
            new AttractorScaleUp(),
            
        };
        

        public const int ATTRACTOR_NONE = 0;
        public const int ATTRACTOR_BOUND = 1;
        public const int ATTRACTOR_TIME = 2;
        public const int ATTRACTOR_GEOGRAPH = 4;
        public const int ATTRACTOR_FRAME = 8;
        public const int ATTRACTOR_AVOID = 16;
        public const int ATTRACTOR_AVOIDSCALE = 32;
        public const int ATTRACTOR_SCALEUP = 256;        
        public const int ATTRACTOR_TAG = 64;
        //public const int ATTRACTOR_COLOR = 128;
        //public const int ATTRACTOR_PEOPLE = 256;
        public const int ATTRACTOR_SCALEUPMOUSE = 128;

        public const int FILE_OPEN = 8;
        public int curState
        {
            get;
            set;
        }

        // 現在使用しているアトラクターのフラグ
        private int attractor_ = ATTRACTOR_NONE;

        public enum PICTURE
        {
            ART,
            LAN,
            UNKNOWN,
        }

        public PICTURE pType = PICTURE.UNKNOWN; 

        public int Attractor
        {
            get
            {
                return attractor_;
            }
            private set
            {
                attractor_ = value;
            }
        }
        public SystemState()
        {

            attractor_ |= ATTRACTOR_BOUND;
            attractor_ |= ATTRACTOR_AVOID;
            attractor_ |= ATTRACTOR_AVOIDSCALE;
            attractor_ |= ATTRACTOR_SCALEUP;
            //attractor_ |= ATTRACTOR_ANCHOR;
            attractor_ |= ATTRACTOR_SCALEUPMOUSE;
            attractor_ |= ATTRACTOR_TAG;
            //attractor_ |= ATTRACTOR_COLOR;
            //attractor_ |= ATTRACTOR_GEOGRAPH;
            attractor_ |= ATTRACTOR_FRAME;
            //attractor_ |= ATTRACTOR_TIME;
            //attractor_ |= ATTRACTOR_PEOPLE;


            // ラベルの名前変更
            {
#if LABEL_JAPANESE
                labelNonOverlap.Text = "画像を重ねない";
                labelScale.Text = "大きさを揃える";
                labelAttractToMouse.Text = "注目画像を固定する";
                labelScaleUpMouse.Text = "注目画像を拡大する";
                labelTag.Text = "同じ種類の画像を集める";
                labelNoise.Text = "ゆらぎの大きさ";
#else
                /*labelNonOverlap.Text = "NonOverlap";
                labelScale.Text = "Scale";
                labelAttractToMouse.Text = "AttractToMouse";
                labelScaleUpMouse.Text = "ScaleUpMouse";
                labelTag.Text = "Tag";
                labelNoise.Text = "Noise";*/
#endif
            }

            //if (Directory.Exists(homeDirectory_))
            //{
            //    if (!Directory.Exists(homeDirectory_ + "\\Content"))
            //    {
            //        Directory.CreateDirectory(homeDirectory_ + "\\Content");
            //    }
            //}
        }

        //// added by Gengdai
        //public void ReadPeopleLogs(List<PeopleTags> ptags)
        //{
        //    string tagFileName = "\\people.cfg";
        //    string fullFileName = homeDirectory_ + tagFileName;
        //    if (File.Exists(fullFileName))
        //    {
        //        ptags.Clear();

        //        string photoname;
        //        string people;
        //        Microsoft.Xna.Framework.Rectangle box;
        //        StreamReader sr = new StreamReader(fullFileName, Encoding.GetEncoding("Shift_JIS"));
        //        string[] sep = new string[1];
        //        sep[0] = "\r\n";
        //        string[] tags = (sr.ReadToEnd()).Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = 0, len = tags.Length; i < len; i++)
        //        {
        //            List<PeopleTag> list = new List<PeopleTag>();
        //            //list.Clear();
        //            sep[0] = "|";
        //            string[] tag = tags[i].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            photoname = tag[0];
        //            for (int j = 1; j < tag.Length; j++)
        //            {
        //                sep[0] = ":";
        //                string[] subtag = tag[j].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //                people = subtag[0];
        //                box = new Microsoft.Xna.Framework.Rectangle(int.Parse(subtag[1]), int.Parse(subtag[2]), int.Parse(subtag[3]), int.Parse(subtag[4]));
        //                list.Add(new PeopleTag(people, box));
        //            }
        //            ptags.Add(new PeopleTags(photoname, list));
        //        }
        //    }
        //}


        //public void ReadPhotoLogs(List<PhotoLog> pl)
        //{
        //    string exePath = homeDirectory_;
        //    if (System.IO.File.Exists(exePath + "\\profile.ini"))
        //    {
        //        pl.Clear();
        //        System.IO.StreamReader sr = new System.IO.StreamReader(exePath + "\\profile.ini", System.Text.Encoding.GetEncoding("Shift_JIS"));
        //        string[] sep = new string[1];
        //        sep[0] = "\r\n";
        //        string[] logs = (sr.ReadToEnd()).Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //        if(logs.Length > 0)
        //        homeDirectory_ = logs[0];
        //        for (int i = 1, len = logs.Length; i < len; ++i)
        //        {
        //            sep[0] = "|";
        //            PhotoLog p = new PhotoLog();
        //            string[] log = logs[i].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            p.FilePath = log[0];
        //            p.FileName = log[1];
        //            sep[0] = ":";
        //            p.CapturedTimeStamp = log[2].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            p.CreateTimeStamp = log[3].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            p.Tags = log[4].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            if (log.Length > 5)
        //            {
        //                p.Feature = log[5].Split(sep, StringSplitOptions.RemoveEmptyEntries);
        //            }
        //            else
        //            {
        //                p.Feature = null;
        //            }
        //            if (log.Length > 6)
        //            {
        //                p.Variance = log[6];
        //            }
        //            else
        //            {
        //                p.Variance = "0.0";
        //            }
        //            pl.Add(p);
        //        }
        //        sr.Close();
        //        pl.Sort();
        //    }
        //}

        public void invokeAttractorSelection(Dock dock, ScrollBar sBar, AttractorWeight weight, List<Photo> photos, List<Photo> activePhotos, List<Stroke> strokes, SystemState systemState)
        {
            for (int i = 0, size = Attractors.Length; i < size; ++i)
            {
                if ((attractor_ & (1 << i)) != 0)
                {
                    Attractors[i].select(dock, sBar, weight, photos, activePhotos, strokes, systemState);
                }
            }
        }

        #region another
        // アトラクター選択の無効化
        /*private void attractorResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            attractorAllReset();
        }

        private void attractorAllReset()
        {
            attractorResetToolStripMenuItem.Checked = false;
            attractor_ = ATTRACTOR_NONE;
            avoidToolStripMenuItem.Checked = false;
            scaleUpToolStripMenuItem.Checked = false;
            anchorToolStripMenuItem.Checked = false;
            scaleUpMouseToolStripMenuItem.Checked = false;
            tagToolStripMenuItem.Checked = false;
            colorToolStripMenuItem.Checked = false;
            geographToolStripMenuItem.Checked = false;
            frameToolStripMenuItem.Checked = false;
            timeToolStripMenuItem.Checked = false;
            noiseToolStripMenuItem.Checked = false;
        }*/

        // ノイズの有無や大きさ
        /*public void SwapNoise()
        {
            noiseToolStripMenuItem.Checked = !noiseToolStripMenuItem.Checked;
        }
        private void noiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapNoise();
        }
        private void trackBarNoise_Scroll(object sender, EventArgs e)
        {
            if (trackBarNoise.Value == 0)
            {
                noiseToolStripMenuItem.Checked = false;
            }
            else
            {
                noiseToolStripMenuItem.Checked = true;
            }
        }*/

        // 各アトラクター選択の有無と強さ
        public void SwapBound()
        {
            //boundToolStripMenuItem.Checked = !boundToolStripMenuItem.Checked;
            if ((attractor_ & ATTRACTOR_BOUND) == 0)
            {
                attractor_ |= ATTRACTOR_BOUND;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_BOUND;
            }
        }
        /*private void boundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapBound();
        }

        public void SwapAvoid()
        {
            avoidToolStripMenuItem.Checked = !avoidToolStripMenuItem.Checked;
            if (avoidToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_AVOID;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_AVOID;
            }
        }
        private void avoidStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapAvoid();
        }
        private void trackBarAvoid_Scroll(object sender, EventArgs e)
        {
            if (trackBarNonOverlap.Value == 0)
            {
                avoidToolStripMenuItem.Checked = false;
                attractor_ &= ~ATTRACTOR_AVOID;
            }
            else
            {
                avoidToolStripMenuItem.Checked = true;
                attractor_ |= ATTRACTOR_AVOID;
            }
        }

        public void SwapAvoidScale()
        {
            avoidScaleToolStripMenuItem.Checked = !avoidScaleToolStripMenuItem.Checked;
            if (avoidScaleToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_AVOIDSCALE;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_AVOIDSCALE;
            }
        }
        private void avoidScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapAvoidScale();
        }*/

        public void SwapScaleUp()
        {
            /*scaleUpToolStripMenuItem.Checked = !scaleUpToolStripMenuItem.Checked;
            if (scaleUpToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_SCALEUP;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_SCALEUP;
            }*/
            if ((attractor_ & ATTRACTOR_SCALEUP) == 0)
            {
                attractor_ |= ATTRACTOR_SCALEUP;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_SCALEUP;
            }
        }
        /*private void scaleUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapScaleUp();
        }
        private void trackBarScaleUp_Scroll(object sender, EventArgs e)
        {
            if (trackBarScale.Value == 0)
            {
                scaleUpToolStripMenuItem.Checked = false;
                attractor_ &= ~ATTRACTOR_SCALEUP;
            }
            else
            {
                scaleUpToolStripMenuItem.Checked = true;
                attractor_ |= ATTRACTOR_SCALEUP;
            }
        }

        public void SwapAnchor()
        {
            anchorToolStripMenuItem.Checked = !anchorToolStripMenuItem.Checked;
            if (anchorToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_ANCHOR;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_ANCHOR;
            }
        }
        private void anchorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapAnchor();
        }
        private void trackBarAnchor_Scroll(object sender, EventArgs e)
        {
            if (trackBarAttractToMouse.Value == 0)
            {
                anchorToolStripMenuItem.Checked = false;
                attractor_ &= ~ATTRACTOR_ANCHOR;
            }
            else
            {
                anchorToolStripMenuItem.Checked = true;
                attractor_ |= ATTRACTOR_ANCHOR;
            }
        }

        public void SwapScaleUpMouse()
        {
            scaleUpMouseToolStripMenuItem.Checked = !scaleUpMouseToolStripMenuItem.Checked;
            if (scaleUpMouseToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_SCALEUPMOUSE;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_SCALEUPMOUSE;
            }
        }
        private void scaleUpMouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapScaleUpMouse();
        }
        private void trackBarScaleUpMouse_Scroll(object sender, EventArgs e)
        {
            if (trackBarScaleUpMouse.Value == 0)
            {
                scaleUpMouseToolStripMenuItem.Checked = false;
                attractor_ &= ~ATTRACTOR_SCALEUPMOUSE;
            }
            else
            {
                scaleUpMouseToolStripMenuItem.Checked = true;
                attractor_ |= ATTRACTOR_SCALEUPMOUSE;
            }
        }

        public void SwapTag()
        {
            tagToolStripMenuItem.Checked = !tagToolStripMenuItem.Checked;
            //if (tagToolStripMenuItem.Checked)
            //{
            //    attractor_ |= ATTRACTOR_TAG;
            //}
            //else
            //{
            //    attractor_ &= ~ATTRACTOR_TAG;
            //}
        }
        private void tagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapTag();
        }
        private void trackBarTag_Scroll(object sender, EventArgs e)
        {
            //if (trackBarTag.Value == 0)
            //{
            //    tagToolStripMenuItem.Checked = false;
            //    attractor_ &= ~ATTRACTOR_TAG;
            //}
            //else
            //{
            //    tagToolStripMenuItem.Checked = true;
            //    attractor_ |= ATTRACTOR_TAG;
            //}
        }
        */
        public void SwapFrame()
        {
            //frameToolStripMenuItem.Checked = !frameToolStripMenuItem.Checked;
            //if (frameToolStripMenuItem.Checked)
            if((attractor_ & ATTRACTOR_FRAME) == 0)
            {
                attractor_ |= ATTRACTOR_FRAME;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_FRAME;
            }
        }
        
        //private void frameToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    SwapFrame();
        //}

        public void SwapTime()
        {
            //timeToolStripMenuItem.Checked = !timeToolStripMenuItem.Checked;
            //if (timeToolStripMenuItem.Checked)
            if((attractor_ & ATTRACTOR_TIME) == 0)
            {
                attractor_ |= ATTRACTOR_TIME;
                curState = ATTRACTOR_TIME;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_TIME;
                curState = ATTRACTOR_NONE;
            }
        }

        /*public void SwapPeople()
        {
            peopleToolStripMenuItem.Checked = !peopleToolStripMenuItem.Checked;
            if (peopleToolStripMenuItem.Checked)
            {
                attractor_ |= ATTRACTOR_PEOPLE;
            }
            else
                attractor_ &= ~ATTRACTOR_PEOPLE;
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapTime();
        }*/

        public void SwapGeograph()
        {
            //geographToolStripMenuItem.Checked = !geographToolStripMenuItem.Checked;
            if ((attractor_ & ATTRACTOR_GEOGRAPH) == 0)
            {
                attractor_ |= ATTRACTOR_GEOGRAPH;
                curState = ATTRACTOR_GEOGRAPH;
            }
            else
            {
                attractor_ &= ~ATTRACTOR_GEOGRAPH;
                curState = ATTRACTOR_NONE;
            }
        }
        /*private void geographToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapGeograph();
        }*/

        //public void SwapColor()
        //{
        //    //colorToolStripMenuItem.Checked = !colorToolStripMenuItem.Checked;
        //    //if (colorToolStripMenuItem.Checked)
        //    //{
        //    //    attractor_ |= ATTRACTOR_COLOR;
        //    //}
        //    //else
        //    //{
        //    //    attractor_ &= ~ATTRACTOR_COLOR;
        //    //}

        //    if ((attractor_ & ATTRACTOR_COLOR) == 0)
        //    {
        //        attractor_ |= ATTRACTOR_COLOR;
                
        //    }
        //    else
        //    {
        //        attractor_ &= ~ATTRACTOR_COLOR;
        //    }
        //}
        /*
        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapColor();
        }*/


        #endregion

        #region プロパティ群

        public bool IsBound
        {
            get
            {
                int temp = attractor_ & SystemState.ATTRACTOR_BOUND;
                return temp == 0 ? false : true;
            }
        }

        public bool IsFrame
        {
            get
            {
                int temp = attractor_ & SystemState.ATTRACTOR_FRAME;
                return temp == 0 ? false : true;
            }
        }
        public bool IsTime
        {
            get
            {
                int temp = attractor_ & SystemState.ATTRACTOR_TIME;
                return temp == 0 ? false : true;
            }
        }
        public bool IsGeograph
        {
            get
            {
                //return geographToolStripMenuItem.Checked;
                int temp = attractor_ & SystemState.ATTRACTOR_GEOGRAPH;
                return temp == 0? false : true;
            }
        }
        
        /*public bool Actived
        {
            get
            {
                return (trackBarNoise.Focused || trackBarNonOverlap.Focused || trackBarScale.Focused ||
                    trackBarAttractToMouse.Focused || trackBarScaleUpMouse.Focused || trackBarTag.Focused);
            }
        }*/
        
        

        //public List<string> FileNames
        //{
        //    get
        //    {
        //        return filenames_;
        //    }
        //}
        //public int Attractor
        //{
        //    get
        //    {
        //        return attractor_;
        //    }
        //    set
        //    {
        //        attractor_ = value;
        //    }
        //}
        /*public string HomeDirectory
        {
            get
            {
                return homeDirectory_;
            }
        }*/
        /*public bool IsShown
        {
            get
            {
                return isShown_;
            }
            set
            {
                isShown_ = value;
            }
        }*/
        /*public bool IsFocused
        {
            get
            {
                return (this.Focused || trackBarNoise.Focused);
            }
        }

        private void ControlPanel_LocationChanged(object sender, EventArgs e)
        {
            boundingBox_.Min = new Microsoft.Xna.Framework.Vector2(this.Left, this.Top);
            boundingBox_.Max = new Microsoft.Xna.Framework.Vector2(this.Left + this.Size.Width, this.Top + this.Size.Height);
        }*/

        #endregion


        /*private void tagToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SwapTag();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SwapPeople();
        }*/


    }
}