using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoViewer.Supplement
{
    class RandomBoxMuller
    {
        private readonly Random rand = new Random();
        public RandomBoxMuller()
        {
        }

        public double NextDouble()
        {
            return (Math.Sqrt(-2.0 * Math.Log(rand.NextDouble())) * Math.Cos(2.0 * Math.PI * rand.NextDouble()));
        }

        public double NextDouble(double variance)
        {
            return (NextDouble() * Math.Sqrt(variance));
        }
    }
}
