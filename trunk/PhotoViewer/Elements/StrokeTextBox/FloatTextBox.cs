﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using PhotoViewer.Supplement;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using PhotoViewer.Element;

namespace PhotoViewer
{
    public partial class FloatTextBox : Form
    {
        const int width = 500;
        const int height = 30;
        //private int index;

        //public List<string> tags
        //{
        //    get;
        //    private set;
        //}

        Stroke s = null;

        #region 属性封装
        public bool IsShown
        {
            get;
            private set;
        }
        #endregion

        public FloatTextBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            
        }

        public void ShowTextBox(Vector2 pos, Stroke st)
        {
            this.Location = new System.Drawing.Point((int)(pos.X + Browser.Instance.clientBounds.Min.X), (int)(pos.Y + Browser.Instance.clientBounds.Min.Y));
            //this.Location = new System.Drawing.Point((int)pos.X, (int)pos.Y);
            this.Show();
            IsShown = true;
            boundingBox = new BoundingBox2D(new Vector2(pos.X , pos.Y), new Vector2(pos.X + width, pos.Y + height), 0f);
            s = st;
        }

        public void showAgain(Vector2 pos)
        {
            this.Location = new System.Drawing.Point((int)(pos.X + Browser.Instance.clientBounds.Min.X), (int)(pos.Y + Browser.Instance.clientBounds.Min.Y));
            //this.Location = new System.Drawing.Point((int)pos.X, (int)pos.Y);
            this.Show();
            IsShown = true;
            boundingBox = new BoundingBox2D(new Vector2(pos.X, pos.Y), new Vector2(pos.X + width, pos.Y + height), 0f);
        }

        public void PostTags()
        {
            char[] spritChar = { ' ', '　', ',', '，', '、' };
            string[] tempText = this.textBox1.Text.Split(spritChar, StringSplitOptions.RemoveEmptyEntries);
            if (tempText.Length < 1)
            {
                if (s != null)
                {
                    s.Tags = new List<string>();
                    s.photoCal();
                }
                //this.IsShown = false;
                //this.Hide();
                return;
            }
            //this.IsShown = false;
            //this.Hide();
            if (s != null)
            {
                s.Tags = tempText.ToList<string>();
                s.photoCal();
            }
        }

        public BoundingBox2D boundingBox
        {
            get;
            private set;
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            //base.OnKeyPress(e);

            if ((Keys)e.KeyChar == Keys.Enter)
            {
                this.PostTags();
            }
        }

    }
}
