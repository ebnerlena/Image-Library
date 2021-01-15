//Ebner Lena - MMTB2019
using System;

public sealed class FilterManipulator
{   
    private static FilterManipulator _instance = null;
    private static readonly object instancelock = new object();
    private Filter filter;
    
    private FilterManipulator()
    {
    }
    public static FilterManipulator Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new FilterManipulator();
                }
            }
        }
        return _instance;
    }

    public RGBChannels HighPassFilter(RGBChannels img)
    {
        filter = new HighPassFilter();
        return filter.Apply(img);
    }

    public RGBChannels LowPassFilter(RGBChannels img)
    {
        filter = new LowPassFilter();
        return filter.Apply(img);
    }

    public RGBChannels GaussianFilter(RGBChannels img)
    {
        filter = new GausFilter();
        return filter.Apply(img);
    }

    public RGBChannels GaussianFilterNTimes(RGBChannels img, int n)
    {
        filter = new GausFilter();
        RGBChannels result = img; 

        for(int i = 0; i < n; i++)
        {
            result = filter.Apply(result);
        }
        return result;
    }
    public RGBChannels SobelXFilter(RGBChannels img)
    {           
        filter = new SobelFilter(SobelDirection.X);
        return filter.Apply(img);
    }
    public RGBChannels SobelYFilter(RGBChannels img)
    {           
        filter = new SobelFilter(SobelDirection.Y);
        return filter.Apply(img);
    }

    public RGBChannels GradientOrientation(RGBChannels img)
    {
        RGBChannels sobelX = SobelXFilter(img);
        RGBChannels sobelY = SobelYFilter(img);

        RGBChannels orientation = new RGBChannels(img.Width, img.Height);

        for (int x = 0; x < img.Width; x++)
        {
        for (int y = 0; y < img.Height; y++)
            {
                orientation.R[y,x] =  GetGradientOrientation(sobelX.R[y,x], sobelY.R[y,x]);
                orientation.G[y,x] =  GetGradientOrientation(sobelX.G[y,x], sobelY.G[y,x]);
                orientation.B[y,x] =  GetGradientOrientation(sobelX.B[y,x], sobelY.B[y,x]);
            }
        }
        orientation.AutoContrastForAllChannels();
        return orientation;
    }

    public RGBChannels GradientMagnitude(RGBChannels img)
    {
        RGBChannels sobelX = SobelXFilter(img);
        RGBChannels sobelY = SobelYFilter(img);

        RGBChannels gradMag = new RGBChannels(img.Width, img.Height);

        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                gradMag.R[y,x] = GetGradientMagnitue(sobelX.R[y,x], sobelY.R[y,x]);
                gradMag.G[y,x] = GetGradientMagnitue(sobelX.G[y,x], sobelY.G[y,x]);
                gradMag.B[y,x] = GetGradientMagnitue(sobelX.B[y,x], sobelY.B[y,x]);
                
            }
        }
        gradMag.AutoContrastForAllChannels();
        return gradMag;
    }

    private double GetGradientMagnitue(double colx, double coly)
    {
        return Math.Sqrt(Math.Pow(colx,2)+Math.Pow(coly,2));
    }


    private double GetGradientOrientation(double sobelX, double sobelY)
    {
        return Math.Atan2(sobelY,sobelX);
    }

}