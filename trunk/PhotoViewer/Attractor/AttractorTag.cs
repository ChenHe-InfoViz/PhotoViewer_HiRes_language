using System;
using System.Collections.Generic;
using PhotoInfo;
using PhotoViewer.Manager;
using PhotoViewer.Element;
//using System.Text;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;


namespace Attractor
{
    class AttractorTag : IAttractorSelection
    {
        private readonly Random rand = new Random();
        private float weight_ = 50;
        AttractorColor colorCal = new AttractorColor();
        AttractorWord wordCal = new AttractorWord();

        public void select(Dock dock, ScrollBar sBar, AttractorWeight weight, List<Photo> photos, List<Photo> activePhotos, List<Stroke> strokes, SystemState systemState)
        {
            weight_ = weight.TagWeight;

            if (colorCal.Photos != photos)
                colorCal.Photos = photos;

            if (wordCal.Photos != photos)
                wordCal.Photos = photos;

            // íçñ⁄Ç≥ÇÍÇƒÇ¢ÇÈâÊëúÇtemActivePhotoÇ∆Ç∑ÇÈ
            
                // äeâÊëúÇÃà⁄ìÆ

                //attract only by color
                if (systemState.pType == SystemState.PICTURE.UNKNOWN)
                {
                    foreach (Photo a in activePhotos)
                    {
                        foreach (Photo photo in photos)
                        {
                            if (photo.ID == a.ID)
                                continue;
                            Vector2 v = colorCal.velocityCal(a, photo);
                            photo.AddPosition(v);
                        }
                    }
                }
                //attract by tag and color
                else if (systemState.pType == SystemState.PICTURE.ART)
                {
                    foreach (Photo a in activePhotos)
                    {
                        if (a.activeTag.Count == 0)// || (a.activeTag.Count == 1 && a.activeTag.Contains("Color")))
                            continue;
                        foreach (Photo photo in photos)
                        {
                            if (photo.ID == a.ID)
                                continue;
                            Vector2 v = Vector2.Zero;
                            if (a.activeTag.Contains("Color"))
                            {
                                v = colorCal.velocityCal(a, photo);
                            }
                            bool matched = false;
                            foreach (String tag in a.activeTag)
                            {
                                if (tag.Equals("Color"))
                                    continue;
                                if (photo.containTag(tag))
                                {
                                    matched = true;
                                    break;
                                }
                            }
                            Vector2 vTag = a.Position - photo.Position;
                            if (matched)
                            {
                                photo.IsFollowing = true;
                                Vector2 tempdir = Vector2.Zero;
                                float tempdira = 0f;
                                if (a.boundingBox_.Overrap(photo.boundingBox_, ref tempdir, ref tempdira))
                                {
                                    vTag *= -1f;
                                }
                                vTag *= (float)3f;
                            }
                            else vTag = Vector2.Zero;
                            vTag *= weight_ / 128f;

                            // noise
                            if (v != Vector2.Zero && false) // noise enabled
                            {
                                float noise = (float)((1 - Math.Exp(-rand.NextDouble())) * Math.PI);
                                noise *= (float)Math.Log(photo.Adjacency.Count + 1);
                                if (rand.NextDouble() < 0.5)
                                {
                                    noise *= -1;
                                }
                                float cnoise = (float)Math.Cos(noise);
                                float snoise = (float)Math.Sin(noise);
                                Vector2 noisyv = new Vector2(v.X * cnoise - v.Y * snoise, v.X * snoise + v.Y * cnoise);
                                v = noisyv;
                            }

                            photo.AddPosition(v + vTag);
                        }
                    }
                }

                //attract by words
                else if (systemState.pType == SystemState.PICTURE.LAN)
                {
                    foreach (Photo a in activePhotos)
                    {
                        foreach (Photo p in photos)
                        {
                            if (a == p) continue;
                            int rank = wordCal.match(a, p);
                            Vector2 vTag = a.Position - p.Position;
                            if (rank >= 0)
                            {
                                
                                p.IsFollowing = true;
                                Vector2 tempdir = Vector2.Zero;
                                float tempdira = 0f;
                                if (a.boundingBox_.Overrap(p.boundingBox_, ref tempdir, ref tempdira))
                                {
                                    vTag *= -1f;
                                }
                                vTag *= (float)3f;

                                p.ptag.rank = rank + 1;
                                vTag *= weight_ / 128f;
                                
                            }
                            else
                            {
                                vTag = Vector2.Zero;
                            }
                            p.AddPosition(vTag);
                        }
                    }
                }
                
        }


        
    }
}
