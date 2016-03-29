using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PhotoViewer.Element;
using PhotoViewer.InputDevice;
using PhotoInfo;
using PhotoViewer.Supplement;

namespace PhotoViewer.Manager
{
    public class StrokeBoxCollection
    {
        Dictionary<Stroke, FloatTextBox> StrokeBox = new Dictionary<Stroke, FloatTextBox>();
        //public Dictionary<Stroke, List<Photo>> photoInStroke = new Dictionary<Stroke,List<Photo>>();
        //public Dictionary<Stroke, BoundingBox2D> strokeBoundingbox = new Dictionary<Stroke, BoundingBox2D>();
        FloatTextBox box = new FloatTextBox();
        Stroke st; 

        public List<Photo> photos
        {
            get;
            set;
        }

        public StrokeBoxCollection()
        {
            photos = new List<Photo>();
            st = new Stroke(new Vector2(Browser.Instance.ClientWidth / 2 - 300, Browser.Instance.ClientHeight / 2 - 200), photos);
            st.AddStroke(new Vector2(Browser.Instance.ClientWidth / 2 + 300, Browser.Instance.ClientHeight / 2 - 200));
            st.AddStroke(new Vector2(Browser.Instance.ClientWidth / 2 + 300, Browser.Instance.ClientHeight / 2 + 200));
            st.AddStroke(new Vector2(Browser.Instance.ClientWidth / 2 - 300, Browser.Instance.ClientHeight / 2 + 200));
            st.End();
            //createTextBox(new Vector2(Browser.Instance.ClientWidth / 2 - 300, Browser.Instance.ClientHeight / 2 - 200), st);
            //box.ShowTextBox(Vector2.Zero, st);
            StrokeBox[st] = null;
        }

        public void recalPhoto()
        {
            foreach (var s in StrokeBox.Keys)
            {
                s.photos = photos;
                s.photoCal();

            }
        }

        public Stroke createStroke(Vector2 pos)
        {
            Stroke s = new Stroke(pos, photos);
            StrokeBox[s] = null;
            setColor(s);
            return s;
        }

        public void remove(Stroke s)
        {
            if (StrokeBox.ContainsKey(s))
            {
                if (StrokeBox[s] != null)
                    StrokeBox[s].Dispose();
                StrokeBox.Remove(s);
                //photoInStroke.Remove(s);
                //strokeBoundingbox.Remove(s);
            }

        }

        public int Count
        {
            get
            {
                return StrokeBox.Count;
            }
        }

        float hue = 0f;
        Vector3 tempColor;

        private void setColor(Stroke s)
        {
            tempColor = new Vector3(hue, 1f, 1f);
            Vector3 strokeColor = Vector3.Zero;
            ResourceManager.hsv2rgb(ref tempColor, out strokeColor);
            //Color sColor = new Color(strokeColor);
            s.Color = new Color(strokeColor);
            hue += 0.3f;
            if (hue > 1f)
                hue -= (int)hue;
        }

        public void renderStatic()
        {
            foreach (Stroke s in StrokeBox.Keys)
            {
                if (s.IsClosed && !s.IsDragged && s.Tags.Count > 0)
                {
                    s.Render();
                }
            }

        }

        public void renderDynamic()
        {
            //foreach (Stroke s in StrokeBox.Keys)
            //{
            //    if (!s.IsClosed)
            //    {
            //        s.Render();
            //    }
            //    else if (s.IsDragged)
            //    {
            //        s.RenderBezier();
            //    }
            //}
        }

        //public void renderTags()
        //{
        //    foreach (Stroke s in StrokeBox.Keys)
        //    {
        //        if (s.Tags.Count > 0 && StrokeBox[s].IsShown == false)
        //        {
        //            Vector2 size = s.renderTag();
        //            s.boundingbox = new BoundingBox2D(s.Strokes[0], s.Strokes[0] + size, 0);
        //        }
        //    }
        //}

        

        public List<Stroke> strokeList
        {
            get
            {
                return new List<Stroke>(StrokeBox.Keys);
            }
        }
                
        //public void remove()
        //{
        //    List<Stroke> strokes = new List<Stroke>(StrokeBox.Keys);
        //    for (int i = 0; i < strokes.Count; i++)
        //    {
        //        FloatTextBox box = StrokeBox[strokes[i]];
        //        if (box.tags.Equals && box.IsShown == false)
        //        {
        //            strokes[i].AddTags(box.tags);
        //            //box.Dispose();
        //            //StrokeBox[strokes[i]] = null;
        //            if (photoInStroke.ContainsKey(s))
        //                photoInStroke.Remove(s);
        //        }
        //    }
        //}
        
        public void createTextBox(Vector2 pos, Stroke s)
        {
            FloatTextBox box = new FloatTextBox();
            //textBoxes.Add(box);
            StrokeBox[s] = box;
            box.ShowTextBox(pos, s);
        }

        public bool underMouse(PointingDevice pd)
        {
            //foreach (Photo p in st.relatedPhotos)
            //{
            //    p.underMouse = true;
            //    p.KeepGazed();
            //}
            foreach (var tb in StrokeBox.Values)
            {
                if ( tb != null && tb.boundingBox.Contains(pd.GamePosition) == ContainmentType.Contains && tb.IsShown)
                {
                    return true;
                }
            }
            foreach (var s in StrokeBox.Keys)
            {
                var box = s.boundingbox;
                if (box.Contains(pd.GamePosition) == ContainmentType.Contains)
                {
                    if (pd.oldLeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released && pd.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        StrokeBox[s].showAgain(s.Strokes[0]);
                    return true;
                }
            }
            return false;
        }
    }


}
