﻿using System;
using Microsoft.Xna.Framework;
using PhotoViewer.Supplement;
using Microsoft.Xna.Framework.Graphics;
namespace PhotoViewer.Element
{
    public class ScrollBar
    {
        private const int W = 32;

        //private bool isShown_ = false;
        public static int Height = W;
        private int width_ = W;
        private int min_ = 0;
        private int max_ = W;
        private BoundingBox2D boundingBox_ = new BoundingBox2D(Vector2.Zero, new Vector2(W, W), 0f);
        private DateTime oldest_ = DateTime.MinValue;
        private DateTime newest_ = DateTime.Now;

        public enum DragRegion
        {
            NONE,
            LEFT,
            CENTER,
            RIGHT,
        }

        #region プロパティ
        public int Min
        {
            get
            {
                return min_;
            }
        }
        public int Max
        {
            get
            {
                return max_;
            }
        }
        public int Width
        {
            get
            {
                return width_;
            }
        }

        public BoundingBox2D BoundingBox
        {
            get
            {
                return boundingBox_;
            }
        }
        public DateTime Oldest
        {
            get
            {
                return oldest_;
            }
            set
            {
                oldest_ = value;
            }
        }
        public DateTime Newest
        {
            get
            {
                return newest_;
            }
            set
            {
                newest_ = value;
            }
        }
        public DateTime MinDT
        {
            get
            {
                double ts = newest_.Subtract(oldest_).TotalSeconds;
                double mints = ts * (double)min_ / (double)width_;
                return oldest_.AddSeconds(mints);
            }
        }
        public DateTime MaxDT
        {
            get
            {
                double ts = newest_.Subtract(oldest_).TotalSeconds;
                double maxts = ts * (double)max_ / (double)width_;
                return oldest_.AddSeconds(maxts);
            }
        }
        #endregion

        public ScrollBar(int width)
        {
            width_ = width;
            min_ = 0;
            max_ = width_;
            boundingBox_ = new BoundingBox2D(Vector2.Zero, new Vector2(width_, W),0f);
        }
        public void MoveBar(int min, int max)
        {
            if (min < 0)
            {
                min_ = 0;
            }
            else if (min > max_ - 3*W)
            {
                min_ = max_ - 3*W;
            }
            else
            {
                min_ = min;
            }
            if (max > width_)
            {
                max_ = width_;
            }
            else if (max < min_ + 3 * W)
            {
                max_ = min_ + 3 * W;
            }
            else
            {
                max_ = max;
            }
        }
        
        public ContainmentType LeftContains(Vector2 pos)
        {
            if (pos.Y > 0 && pos.Y < Height)
            {
                if (max_ - min_ < 2 * W)
                {
                    if (pos.X > min_ - (max_ - min_) / 2 && pos.X < (min_ + max_) / 2)
                    {
                        return ContainmentType.Contains;
                    }
                }
                else
                {
                    if (pos.X > min_ - W && pos.X < min_ + W)
                    {
                        return ContainmentType.Contains;
                    }
                }
            }
            return ContainmentType.Disjoint;
        }
        public ContainmentType RightContains(Vector2 pos)
        {
            if (pos.Y > 0 && pos.Y < Height)
            {
                if (max_ - min_ < 2 * W)
                {
                    if (pos.X > (min_ + max_) / 2 && pos.X < max_ + (max_ - min_) / 2)
                    {
                        return ContainmentType.Contains;
                    }
                }
                else
                {
                    if (pos.X > max_ - W && pos.X < max_ + W)
                    {
                        return ContainmentType.Contains;
                    }
                }
            }
            return ContainmentType.Disjoint;
        }
        public ContainmentType CenterContains(Vector2 pos)
        {
            if (pos.Y > 0 && pos.Y < Height)
            {
                if (max_ - min_ < 2 * W)
                {
                }
                else
                {
                    if (pos.X > min_ + W && pos.X < max_ - W)
                    {
                        return ContainmentType.Contains;
                    }
                }
            }
            return ContainmentType.Disjoint;
        }

        public void Render(SpriteBatch batch, int windowWidth, Texture2D sb1, Texture2D sb2)
        {
            //if (isShown_)
            {
                if (width_ != windowWidth)
                {
                    min_ = (int)((double)min_ * (double)windowWidth / (double)width_);
                    max_ = (int)((double)max_ * (double)windowWidth / (double)width_);
                    width_ = windowWidth;
                }
                boundingBox_ = new BoundingBox2D(Vector2.Zero, new Vector2(width_, W), 0f);
                Vector2 scale1 = new Vector2((float)width_ / (float)sb1.Width, 1f);
                Vector2 scale2 = new Vector2((float)(max_ - min_) / (float)sb2.Width, 1f);
                Color whiteAlpha = new Color(255, 255, 255, 128);
                //batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                batch.Draw(sb1, Vector2.Zero, null, whiteAlpha, 0f, Vector2.Zero, scale1, SpriteEffects.None, 0f);
                whiteAlpha.A = (byte)192;
                batch.Draw(sb2, Vector2.UnitX * (float)min_, null, whiteAlpha, 0f, Vector2.Zero, scale2, SpriteEffects.None, 0f);
                //batch.End();
            }
        }
    }
}
