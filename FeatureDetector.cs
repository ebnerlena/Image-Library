//Ebner Lena - MMTB2019
using System;
using System.Drawing;
using System.Collections.Generic;

public sealed class FeatureDetector
{
    private RGBChannels image;
    private static FeatureDetector _instance = null;
    private static readonly object instancelock = new object();
    
    private FeatureDetector()
    {
    }
    public static FeatureDetector Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new FeatureDetector();
                }
            }
        }
        return _instance;
    }

    public RGBChannels HarrisCornerDetection(RGBChannels img)
    {
        image = img;
        ImageProcessor ip = new ImageProcessor();

        RGBChannels sobelX = ip.ApplyFilter(img, FilterType.SobelX);
        RGBChannels sobelY = ip.ApplyFilter(img, FilterType.SobelY);
        RGBChannels gaus = ip.ApplyFilter(img, FilterType.Gaussian);

        RGBChannels a = new RGBChannels(img.Width, img.Height);
        RGBChannels b = new RGBChannels(img.Width, img.Height);
        RGBChannels c = new RGBChannels(img.Width, img.Height);

        RGBChannels f = new RGBChannels(img.Width, img.Height);

        double valueA = 0;
        double valueB = 0;
        double valueC = 0;

        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                valueA = Math.Pow(sobelX.R[y, x], 2) * gaus.R[y, x];
                valueB = Math.Pow(sobelY.R[y, x], 2) * gaus.R[y, x];
                valueC = sobelX.R[y, x] * sobelY.R[y, x] * gaus.R[y, x];

                a.SetValueToAllChannels(valueA, x, y);
                b.SetValueToAllChannels(valueB, x, y);
                c.SetValueToAllChannels(valueC, x, y);

                var lambdas = GetEigenValues(a.R[y, x], b.R[y, x], c.R[y, x]);
                f.SetValueToAllChannels(GetFValue(lambdas.lambda1, lambdas.lambda2), x, y);
            }
        }
        return f;
    }

    private (double lambda1, double lambda2) GetEigenValues(double a, double b, double c)
    {
        double lambda1 = 0;
        double lambda2 = 0;

        double root = Math.Sqrt((Math.Pow(a, 2.0)) - (2.0 * a * b) + (Math.Pow(b, 2.0)) + (4.0 * Math.Pow(c, 2)));

        lambda1 = (1.0 / 2.0) * (a + b + root);
        lambda2 = (1.0 / 2.0) * (a + b - root);
        return (lambda1, lambda2);
    }

    private static double GetFValue(double lambda1, double lambda2)
    {
        double f = 0;
        if (lambda1 >= lambda2 && lambda1 > 4.0)
            f = (lambda1 * lambda2) / (lambda1 + lambda2);

        return f;
    }

    public List<HarrisNode> GetMaximas(int dimX, int dimY, RGBChannels f)
    {
        double[,] maximas = new double[f.R.GetLength(0), f.R.GetLength(1)];
        List<HarrisNode> maxis = new List<HarrisNode>();
        double[,] localMax = new double[9, 9];

        for (int x = 0; x < f.R.GetLength(0) - 8; x += dimX)
        {
            for (int y = 0; y < f.R.GetLength(1) - 8; y += dimY)
            {
                for (int i = 0; i < dimX; i++)
                {
                    for (int j = 0; j < dimY; j++)
                    {
                        localMax[i, j] = f.R[x + i, y + j];
                    }
                }
                SetLocalMaximas(ref maxis, localMax, x, y);
            }
        }
        return maxis;

    }

    private void SetLocalMaximas(ref List<HarrisNode> maxis, double[,] localMaximas, int x, int y)
    {
        int maxIndexX = 0;
        int maxIndexY = 0;
        double maxValue = localMaximas[0, 0];

        //find maxima
        for (int i = 0; i < localMaximas.GetLength(0); i++)
        {
            for (int j = 0; j < localMaximas.GetLength(1); j++)
            {
                if (localMaximas[i, j] > maxValue)
                {
                    maxValue = localMaximas[i, j];
                    maxIndexX = i;
                    maxIndexY = j;
                }
            }
        }
        //apply 1 for maximum, 0 for rest
        for (int i = 0; i < localMaximas.GetLength(0); i++)
        {
            for (int j = 0; j < localMaximas.GetLength(1); j++)
            {
                if (i == maxIndexX && j == maxIndexY && localMaximas[i, j] != 0)
                {
                    maxis.Add(new HarrisNode(localMaximas[i, j], (i + x), (j + y)));
                }
            }
        }
    }

    public List<HarrisNode> ThresholdMaximas(List<HarrisNode> maximas, double threshold)
    {
        List<HarrisNode> realMaxis = new List<HarrisNode>();
        foreach (HarrisNode max in maximas)
        {
            List<HarrisNode> neighbours = maximas.FindAll(m => m.X > max.X - 4 && m.X < max.X + 4 && m.Y > max.Y - 4 && m.Y < max.Y + 4);
            HarrisNode maximum = neighbours[0];

            foreach (HarrisNode n in neighbours)
            {
                if (n.Value > maximum.Value)
                {
                    maximum = n;
                }
            }

            if (maximum.Value > threshold)
                realMaxis.Add(maximum);
        }
        return realMaxis;
    }

    public List<HarrisNode> GetOrientationBin(RGBChannels orientation, List<HarrisNode> maximas)
    {
        foreach (HarrisNode max in maximas)
        {
            List<HarrisNode> neighbours = maximas.FindAll(m => m.X > max.X - 4 && m.X < max.X + 4 && m.Y > max.Y - 4 && m.Y < max.Y + 4);
            int[] or = new int[4];
            foreach (HarrisNode n in neighbours)
            {
                if (orientation.R[n.X, n.Y] >= 90)
                {
                    or[0]++;
                }
                else if (orientation.R[n.X, n.Y] >= 0)
                {
                    or[1]++;
                }
                else if (orientation.R[n.X, n.Y] >= -90)
                {
                    or[2]++;
                }
                else
                {
                    or[3]++;
                }
            }

            //get maximum of 4bin
            int maxOrIndex = 0;
            int maxOr = 0;
            for (int i = 0; i < or.Length; i++)
            {
                if (or[i] > maxOr)
                {
                    maxOr = or[i];
                    maxOrIndex = i;
                }
            }

            if (maxOrIndex == 0)
                max.OrientationColor = Color.Blue;
            else if (maxOrIndex == 1)
                max.OrientationColor = Color.Red;
            else if (maxOrIndex == 2)
                max.OrientationColor = Color.Yellow;
            else
                max.OrientationColor = Color.Green;
        }
        return maximas;
    }

    public RGBChannels GetColorFromFValue(RGBChannels f, List<HarrisNode> maximas)
    {
        RGBChannels result = new RGBChannels(f.Width, f.Height);
        for (int x = 0; x < f.Width; x++)
        {
            for (int y = 0; y < f.Height; y++)
            {
                HarrisNode node = maximas.Find( max => max.X == x && max.Y == y);
                if (node != null) 
                {
                    result.R[y,x] = 255;
                    result.G[y,x] = 0;
                    result.B[y,x] = 0;
                }
                else {
                    result.R[y,x] = image.R[y,x];
                    result.G[y,x] = image.G[y,x];
                    result.B[y,x] = image.B[y,x];
                }
            }
        }

        return result;
    }

    public RGBChannels GetColorFromOrientation(RGBChannels f, List<HarrisNode> maximas)
    {
        RGBChannels result = new RGBChannels(f.Width, f.Height);
        for (int x = 0; x < f.Width; x++)
        {
            for (int y = 0; y < f.Height; y++)
            {
                HarrisNode node = maximas.Find(max => max.X == x && max.Y == y);
                if (node != null)
                {
                    result.R[y,x] = node.OrientationColor.R;
                    result.G[y,x] = node.OrientationColor.G;
                    result.B[y,x] = node.OrientationColor.B;
                }
                else
                {
                    result.R[y,x] = image.R[y,x];
                    result.G[y,x] = image.G[y,x];
                    result.B[y,x] = image.B[y,x];
                }
            }
        }

        return result;
    }

}
