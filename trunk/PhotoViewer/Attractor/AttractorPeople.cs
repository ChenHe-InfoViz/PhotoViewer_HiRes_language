using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhotoViewer.Element;
using PhotoViewer.Manager;
using PhotoInfo;
//using Microsoft.Xna.Framework.Input;

namespace Attractor
{
    // added by Gengdai
    class AttractorPeople : IAttractorSelection
    {
        private readonly Random rand = new Random();
        //private int weight_1 = 20;
        //private int weight_2 = 5;
        public void select(Dock dock, ScrollBar sBar, AttractorWeight weight, List<Photo> photos, List<Photo> activePhotos, List<Stroke> strokes, SystemState systemState)
        {
            //Photo clickedPhoto = null;

            //// 找到被点击的图片
            //foreach (Photo a in photos)
            //{
            //    if (a.IsClicked == true)
            //    {
            //        clickedPhoto = a;
            //        break;
            //    }
            //}

            //// 如果某图被点中
            //if (clickedPhoto != null)
            //{
            //    List<PeopleTag> currentTagList = clickedPhoto.PTags.pTags;
            //    string selectedPeopleName = null;
            //    // 如果被选中的图片没有人物标签则返回，不予处理
            //    if (currentTagList == null)
            //        return;
            //    foreach (PeopleTag p in currentTagList)
            //    {
            //        Rectangle box = p.Box;
            //        Rectangle newbox = new Rectangle((int)((float)box.X * clickedPhoto.ScaleDisplay), (int)((float)box.Y * clickedPhoto.ScaleDisplay)
            //            , (int)((float)box.Width * clickedPhoto.ScaleDisplay), (int)((float)box.Height * clickedPhoto.ScaleDisplay));
                    
            //        if (newbox.Contains((int)clickedPhoto.ClickedPoint.X,(int)clickedPhoto.ClickedPoint.Y))
            //        {
            //            selectedPeopleName = p.People;
            //            break;
            //        }
            //    }

            //    // 如果选中了某人
            //    if (selectedPeopleName != null)
            //    {
            //        foreach (Photo a in photos)
            //        {
            //            Vector2 v = Vector2.Zero;
            //            if (a.Tag.Contains(selectedPeopleName)) // 对于那些被吸引的图像
            //            {                            
            //                v = clickedPhoto.Position - a.Position; // 吸引
            //                v *= weight_1 / 2f;// 10f;  
            //            }
            //            else // 为被吸引的图像
            //            {
            //                v = a.Position - clickedPhoto.Position; // 排斥
            //                v *= weight_2 / 3.0f;// 10f;  
            //            }
                            
            //            // 改变噪声方向
            //            if (v != Vector2.Zero && input.EnabledNoise)
            //            {
            //                float noise = (float)((1 - Math.Exp(-rand.NextDouble())) * Math.PI);
            //                noise *= (float)Math.Log(a.Adjacency.Count + 1);
            //                if (rand.NextDouble() < 0.5)
            //                {
            //                    noise *= -1;
            //                }
            //                float cnoise = (float)Math.Cos(noise)/3f;
            //                float snoise = (float)Math.Sin(noise)/3f;
            //                Vector2 noisyv = new Vector2(v.X * cnoise - v.Y * snoise, v.X * snoise + v.Y * cnoise);
            //                v = noisyv;
            //            }

            //            // 将改变的向量v作用于图像
            //            a.AddPosition(v);
            //        }                       
            //    }
            //}
        }
    }
}
