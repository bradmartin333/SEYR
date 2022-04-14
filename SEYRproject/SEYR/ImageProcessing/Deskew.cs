using System;
using System.Drawing;

namespace SEYR.ImageProcessing
{
    internal class Deskew
    {
        public Bitmap cBmp;

        // The range of angles to search for lines
        public double cAlphaStart = -20;
        public double cAlphaStep = 0.2;
        public int cSteps = 40 * 5;

        // Precalculation of sin and cos.
        public double[] cSinA;
        public double[] cCosA;

        // Range of d
        public double cDMin;
        public double cDStep = 1;
        public int cDCount;

        // Count of points that fit in a line.
        public int[] cHMatrix;

        public Deskew(Bitmap bmp)
        {
            cBmp = bmp;
        }

        // Representation of a line in the image.
        public class HougLine
        {
            // Count of points in the line.
            public int Count;
            // Index in Matrix.
            public int Index;
            // The line is represented as all x,y that solve y*cos(alpha)-x*sin(alpha)=d
            public double Alpha;
            public double d;
        }

        // Calculate the skew angle of the image cBmp.
        public double GetSkewAngle()
        {
            Deskew.HougLine[] hl;
            double sum = 0.0;
            int count = 0;

            // Hough Transformation
            Calc();
            // Top 20 of the detected lines in the image.
            hl = GetTop(20);
            // Average angle of the lines
            for (int i = 0; i <= 19; i++)
            {
                sum += hl[i].Alpha;
                count += 1;
            }
            return -(sum / count);
        }

        // Calculate the Count lines in the image with most points.
        private HougLine[] GetTop(int Count)
        {
            HougLine[] hl;
            int i;
            int j;
            HougLine tmp;
            int AlphaIndex;
            int dIndex;

            hl = new HougLine[Count + 1];
            for (i = 0; i <= Count - 1; i++)
                hl[i] = new HougLine();
            for (i = 0; i <= cHMatrix.Length - 1; i++)
            {
                if (cHMatrix[i] > hl[Count - 1].Count)
                {
                    hl[Count - 1].Count = cHMatrix[i];
                    hl[Count - 1].Index = i;
                    j = Count - 1;
                    while (j > 0 && hl[j].Count > hl[j - 1].Count)
                    {
                        tmp = hl[j];
                        hl[j] = hl[j - 1];
                        hl[j - 1] = tmp;
                        j -= 1;
                    }
                }
            }
            for (i = 0; i <= Count - 1; i++)
            {
                dIndex = hl[i].Index / cSteps;
                AlphaIndex = hl[i].Index - (dIndex * cSteps);
                hl[i].Alpha = GetAlpha(AlphaIndex);
                hl[i].d = dIndex + cDMin;
            }
            return hl;
        }

        // Hough Transforamtion:
        private void Calc()
        {
            int x;
            int y;
            int hMin = (int)(cBmp.Height / 4d);
            int hMax = (int)(cBmp.Height * 3d / 4d);

            Init();
            for (y = hMin; y <= hMax; y++)
            {
                for (x = 1; x <= cBmp.Width - 2; x++)
                {
                    // Only lower edges are considered.
                    if (IsBlack(x, y))
                    {
                        if (!IsBlack(x, y + 1))
                            Calc(x, y);
                    }
                }
            }
        }

        // Calculate all lines through the point (x,y).
        private void Calc(int x, int y)
        {
            int alpha;
            double d;
            int dIndex;
            int Index;

            for (alpha = 0; alpha <= cSteps - 1; alpha++)
            {
                d = (y * cCosA[alpha]) - (x * cSinA[alpha]);
                dIndex = (int)CalcDIndex(d);
                Index = (dIndex * cSteps) + alpha;
                try
                {
                    cHMatrix[Index] += 1;
                }
                catch (Exception) {}
            }
        }

        private double CalcDIndex(double d)
        {
            return Convert.ToInt32(d - cDMin);
        }

        private bool IsBlack(int x, int y)
        {
            Color c;
            double luminance;

            c = cBmp.GetPixel(x, y);
            luminance = (c.R * 0.299) + (c.G * 0.587) + (c.B * 0.114);
            return luminance < 140;
        }

        private void Init()
        {
            int i;
            double angle;

            // Precalculation of sin and cos.
            cSinA = new double[cSteps - 1 + 1];
            cCosA = new double[cSteps - 1 + 1];
            for (i = 0; i <= cSteps - 1; i++)
            {
                angle = GetAlpha(i) * Math.PI / 180.0;
                cSinA[i] = Math.Sin(angle);
                cCosA[i] = Math.Cos(angle);
            }
            // Range of d:
            cDMin = -cBmp.Width;
            cDCount = (int)(2 * (cBmp.Width + cBmp.Height) / cDStep);
            cHMatrix = new int[(cDCount * cSteps) + 1];
        }

        public double GetAlpha(int Index)
        {
            return cAlphaStart + (Index * cAlphaStep);
        }
    }
}
