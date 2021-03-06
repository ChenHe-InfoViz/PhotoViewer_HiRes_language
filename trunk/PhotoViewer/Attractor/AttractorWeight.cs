using System;
using System.Collections.Generic;
//using System.Text;

namespace Attractor
{
    public class AttractorWeight
    {
        private int nonOverlapWeight_;
        private int scaleWeight_;
        private int attractToMouseWeight_;
        private int scaleUpMouseWeight_;
        private int tagWeight_;
        private int noiseWeight_;
        public AttractorWeight(int nonoverlapw,
            int scalew,
            int attracttomousew,
            int scaleupmousew,
            int tagw,
            int noisew)
        {
            nonOverlapWeight_ = nonoverlapw;
            scaleWeight_ = scalew;
            attractToMouseWeight_ = attracttomousew;
            scaleUpMouseWeight_ = scaleupmousew;
            tagWeight_ = tagw;
            noiseWeight_ = noisew;
        }

        #region ��װ����
        public int NonOverlapWeight
        {
            get
            {
                return nonOverlapWeight_;
            }
        }
        public int ScaleWeight
        {
            get
            {
                return scaleWeight_;
            }
        }
        public int AttractToMouseWeight
        {
            get
            {
                return attractToMouseWeight_;
            }
        }
        public int ScaleUpMouseWeight
        {
            get
            {
                return scaleUpMouseWeight_;
            }
        }
        public int TagWeight
        {
            get
            {
                return tagWeight_;
            }
        }
        public int NoiseWeight
        {
            get
            {
                return noiseWeight_;
            }
        }
        #endregion
    }
}
