// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020
using System;

public class PixelManipulator
{
    private static PixelManipulator _instance = null;
    private static readonly object instancelock = new object();
    
    private PixelManipulator()
    {
    }
    public static PixelManipulator Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new PixelManipulator();
                }
            }
        }
        return _instance;
    }

    public RGBChannels HalveValues(RGBChannels image)
    {
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {
                image.R[y,x] = image.R[y,x]/2;
                image.G[y,x] = image.G[y,x]/2;
                image.B[y,x] = image.B[y,x]/2;
            }
        }
        return image;
    }

    public RGBChannels InvertValues(RGBChannels image)
    {
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {
                image.R[y,x] = 255 - image.R[y,x];
                image.G[y,x] = 255 - image.G[y,x];
                image.B[y,x] = 255 - image.B[y,x];
            }
        }
        return image;
    }

    public RGBChannels ThresholdValues(RGBChannels image, int threshold)
    {
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {
                image.R[y,x] = image.R[y,x] <= threshold ? 0 : 255;
                image.G[y,x] = image.G[y,x] <= threshold ? 0 : 255;
                image.B[y,x] = image.B[y,x] <= threshold ? 0 : 255;
            }
        }
        return image;
    }

    public RGBChannels SetChannelsToZeroExcept(RGBChannels image, string channel)
    {
        channel = channel.ToUpper();
        if (channel != "R" && channel != "G" && channel != "B")
                throw new ArgumentOutOfRangeException("Error: Channel not valid - it should be either R, G or B");
                
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {
                image.R[y,x] = channel == "R" ? image.R[y,x] : 255;
                image.G[y,x] = channel == "G" ? image.G[y,x] : 255;
                image.B[y,x] = channel == "B" ? image.B[y,x] : 255;
            }
        }
        return image;
    }

}