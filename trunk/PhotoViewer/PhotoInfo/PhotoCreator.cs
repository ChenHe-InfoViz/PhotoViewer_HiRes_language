﻿using System;
using System.Collections.Generic;
using System.IO;
using database;
using PhotoViewer.Supplement;
using PhotoViewer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhotoInfo;
using PhotoViewer.Element;

namespace PhotoConstruction
{
    class PhotoCreator
    {
        TableProcessor table;
        ColorTable colorTable = new ColorTable();
        public PhotoCreator(TableProcessor t)
        {
            table = t;
            photos = new List<Photo>();
        }

        public PhotoCreator()
        {
            photos = new List<Photo>();
        }

        public List<Photo> photos
        {
            get;
            private set;
        }

        protected void searchTable()
        {

        }

        private Random random_ = new Random();
        private ProgressBarForm progressBar;
        public void createPhoto(List<string> filename)
        {

            //filename indicates the whole directory, names contains only filename
            photos.Clear();
            //photos = new List<Photo>();
            progressBar = new ProgressBarForm();
            progressBar.Location = new System.Drawing.Point((int)Browser.Instance.Window.ClientBounds.Left, (int)Browser.Instance.Window.ClientBounds.Top);
            List<String> names = new List<string>();
            foreach (String file in filename)
            {
                String[] tempText = file.Split(new Char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
                tempText = tempText[tempText.Length - 1].Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                names.Add(tempText[0]);
            }
            Dictionary<string, PhotoTag> tags = new Dictionary<string,PhotoTag>();
            if(table != null)
                tags =  table.select(names);
            Dictionary<string, Photo.colorFeature> colors = colorTable.select(names);
            Dictionary<string, Photo.colorFeature> newColors = new Dictionary<string, Photo.colorFeature>();

            progressBar.Begin(filename.Count);
            for(int i = 0; i < filename.Count; i++)
            {
                PhotoTag ptag;
                if (tags.ContainsKey(names[i]))
                {
                    ptag = tags[names[i]];
                }
                else {
                    ptag = new PhotoTag();
                }
                Texture2D t;
                using (FileStream fileStream = new FileStream(filename[i], FileMode.Open))
                {
                    t = Texture2D.FromStream(Browser.Instance.GraphicsDevice, fileStream);
                }
                Photo newP = null;
                //problem: two images cannot have the same filename
                if (colors.ContainsKey(names[i]))
                    newP = new Photo(i, filename[i], new Vector2(
                          (float)(random_.NextDouble() * Browser.Instance.ClientWidth),
                          (float)(random_.NextDouble() * Browser.Instance.ClientHeight)), (float)(random_.NextDouble() * 0.1) + 0.25f, 0f, ptag, t, colors[names[i]]);
                else {
                    newColors[names[i]] = Photo.CalcFeature(t);
                    newP = new Photo(i, filename[i], new Vector2(
                          (float)(random_.NextDouble() * Browser.Instance.ClientWidth),
                          (float)(random_.NextDouble() * Browser.Instance.ClientHeight)), (float)(random_.NextDouble() * 0.1) + 0.25f, 0f, ptag, t, newColors[names[i]]);
                
                }
                photos.Add(newP);

                
                //photos[photos.Count - 1].SetTexture(t);

                newP.Center = new Vector2(t.Width, t.Height) * 0.5f;
                newP.BoundingBox = new BoundingBox2D(newP.Position - newP.Center * newP.Scale - Vector2.One * (float)Browser.MAR, newP.Position + newP.Center * newP.Scale + Vector2.One * (float)Browser.MAR, newP.Angle);

                progressBar.ProgressName();
                //PeopleTags nowPeopleTags = new PeopleTags(filename[i]);
                //foreach (PeopleTags p in peopleTags)
                //{
                //    if (p.FileName == filename[i])
                //    {
                //        photos[photos.Count-1].PTags = p;

                //        // add tags directly
                //        foreach (PeopleTag pt in p.pTags)
                //        {
                //            photos[photos.Count - 1].Tag.Add(pt.People);
                //        }
                //        break;
                //    }
                //}
            }
            progressBar.End();

            colorTable.insert(newColors);
        }

        public void UnloadPhoto()
        {
            foreach (Photo photo in photos)
            {
                photo.Unload();
            }
        }

        public void UnloadPeopleTag()
        {
            //foreach (PeopleTags pt in peopleTags)
            //{
            //    pt.Release();
            //}
        }
    }
    
                
                
}
