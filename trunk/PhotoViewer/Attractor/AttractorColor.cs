using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using PhotoViewer.Manager;
using PhotoViewer.Element;
using PhotoViewer.Supplement;
using PhotoInfo;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

namespace Attractor
{
    class AttractorColor
    {
        private readonly Random rand = new Random();
        private float weight_ = 100;

        private Dictionary<Photo, Dictionary<Photo, double>> dists_ = new Dictionary<Photo, Dictionary<Photo, double>>();
        private Dictionary<Photo, double> threshold = new Dictionary<Photo, double>();
        private List<Photo> photos = null;
        public List<Photo> Photos
        {
            get { return photos; }
            set { photos = value; dists_.Clear(); threshold.Clear(); }
        }
        
        private const int attractNum_ = 3;
        
        //private class photoDis:IComparable<photoDis>
        //{
        //    public Photo photo;
        //    public double dis;
        //    public photoDis(Photo p, double d)
        //    {
        //        photo = p;
        //        dis = d;
        //    }
        //    public int CompareTo(photoDis other)
        //    {
        //        return dis.CompareTo(other.dis);
        //    }
        //}

        public Vector2 velocityCal(Photo activePhoto, Photo photo)
        {
            //weight_ = weight.TagWeight;
            
            
            //while (activeOld_.Count < input.PointingDevices.Count)
            //{
            //    activeOld_.Add(null);
            //}
            //while (dists_.Count < input.PointingDevices.Count)
            //{
            //    dists_.Add(new List<SIntDouble>());
            //}
            //while (threshold_.Count < input.PointingDevices.Count)
            //{
            //    threshold_.Add(0d);
            //}
            //for (int i = 0; i < input.PointingDevices.Count; i++)
            //{
                //Photo activePhoto = null;

                // 注目されている画像を activePhoto とする
            //foreach (Photo a in activePhotos)
            {
                //if (!a.activeTag.Contains("Color"))
                //{
                //    continue;
                //}
                if (!dists_.ContainsKey(activePhoto))
                {
                    Dictionary<Photo, double> dis = new Dictionary<Photo, double>();

                    foreach (Photo p in photos)
                    {
                        if (activePhoto != p)
                        {
                            dis[p] = p.PhotoDist(activePhoto);
                        }

                    }
                    // 距離の閾値を 全体の 1/attractNum_ が寄ってくる値にする
                    List<KeyValuePair<Photo, double>> disList= dis.ToList();
                    disList.Sort(
                        delegate(KeyValuePair<Photo, double> firstPair,
                        KeyValuePair<Photo, double> nextPair)
                        {
                            return firstPair.Value.CompareTo(nextPair.Value);
                        }
                        );
                    disList.RemoveRange(dis.Count / attractNum_ + 1, dis.Count - dis.Count / attractNum_ - 2);
                    threshold[activePhoto] = disList[disList.Count - 1].Value;
                    dis.Clear();
                    foreach (var pair in disList)
                    {
                        dis[pair.Key] = pair.Value;
                    }
                    //threshold = dists_[i][dists_[i].Count / attractNum_].value;
                    dists_[activePhoto] = dis;
                }
                Dictionary<Photo, double> distance = dists_[activePhoto];
                if(distance.ContainsKey(photo))
                //for (int i = 0; i < distance.Count / attractNum_; i++)
                {
                    
                    //Photo p = distance[i].photo;
                    photo.IsFollowing = true;

                    Vector2 v = activePhoto.Position - photo.Position;
                    v *= (float)(threshold[activePhoto] - distance[photo]);
                    v *= weight_ / 2f;// 10f;
                    return v;
                    // 改变方向的噪音
                    //if (v != Vector2.Zero && false)
                    //{
                    //    float noise = (float)((1 - Math.Exp(-rand.NextDouble())) * Math.PI);
                    //    noise *= (float)Math.Log(a.Adjacency.Count + 1);
                    //    if (rand.NextDouble() < 0.5)
                    //    {
                    //        noise *= -1;
                    //    }
                    //    float cnoise = (float)Math.Cos(noise);
                    //    float snoise = (float)Math.Sin(noise);
                    //    Vector2 noisyv = new Vector2(v.X * cnoise - v.Y * snoise, v.X * snoise + v.Y * cnoise);
                    //    v = noisyv;
                    //}
                    //p.AddPosition(v);
                }
                return Vector2.Zero;
            }
            
        }
    }
}
