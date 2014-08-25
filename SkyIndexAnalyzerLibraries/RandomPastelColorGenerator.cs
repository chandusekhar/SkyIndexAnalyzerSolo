using System;
using System.Collections.Generic;
using System.Drawing;
using ColorMine;
using ColorMine.ColorSpaces;


namespace SkyIndexAnalyzerLibraries
{
    public class RandomPastelColorGenerator
    {
        private readonly Random _random;

        public RandomPastelColorGenerator()
        {
            //const int RandomSeed = 2;
            _random = new Random();
        }

        public Color GetNext()
        {
            byte[] colorBytes = new byte[3];
            colorBytes[0] = (byte)(_random.Next(128) + 127);
            colorBytes[1] = (byte)(_random.Next(128) + 127);
            colorBytes[2] = (byte)(_random.Next(128) + 127);

            Color color = Color.FromArgb(255, colorBytes[0], colorBytes[1], colorBytes[2]);

            return color;
        }
    }



    public class RandomColorGenerator
    {
        private ColorMine.ColorSpaces.Hsv currentColor;
        private readonly Random _random;

        public RandomColorGenerator()
        {
            _random = new Random();
            currentColor = new ColorMine.ColorSpaces.Hsv {H = 0.0d, S = 1.0d, V = 1.0d};
        }

        public Color GetNext()
        {
            double hueStep = (double)_random.Next(60, 120);
            double newHue = currentColor.H + hueStep;
            newHue = (newHue >= 360.0d) ? (newHue - 360.0d) : (newHue);
            currentColor.H = newHue;

            Rgb retColor = currentColor.To<Rgb>();
            return Color.FromArgb(255, Convert.ToInt32(retColor.R), Convert.ToInt32(retColor.G), Convert.ToInt32(retColor.B));
        }
    }
}
