// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020

using System;

public sealed class ColorSpaceConverter
{
    private static ColorSpaceConverter _instance = null;
    private static readonly object instancelock = new object();
    
    private ColorSpaceConverter()
    {
    }
    public static ColorSpaceConverter Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new ColorSpaceConverter();
                }
            }
        }
        return _instance;
    }

    public YCBCRNode[,] ConvertRGBToYCbCr(RGBChannels channels)
    {
        YCBCRNode[,] yCbCrValues = new YCBCRNode[channels.R.GetLength(1), channels.R.GetLength(0)];

        for(int x=0; x<channels.R.GetLength(0); x++)
        {
            for(int y=0; y<channels.R.GetLength(1); y++)
            {
                double fY = Math.Round((0.0 + (0.299*channels.R[y,x]) + (0.587 * channels.G[y,x]) + (0.114* channels.B[y,x])),0);
                double fCB =  Math.Round((128 + (-0.1768736*channels.R[y,x]) + (-0.331264 * channels.G[y,x]) + (0.5 * channels.B[y,x])),0);
                double fCR =  Math.Round((128 + (0.5*channels.R[y,x]) + (-0.418688 * channels.G[y,x]) + (-0.081312 *channels.B[y,x])),0);

                yCbCrValues[y,x] = new YCBCRNode(fY, fCB, fCR);
            }
        }
        return yCbCrValues;
    }

    public RGBChannels ConvertYCbCrToRGB(YCBCRNode[,] values)
    {
        RGBChannels channels = new RGBChannels(values.GetLength(0), values.GetLength(1));
        for(int x=0; x<channels.Width; x++)
        {
            for(int y=0; y<channels.Height; y++)
            {
                YCBCRNode node = values[y,x];
                double r,g,b = 0.0;

                r = node.Y + 1.40200 * (node.Cr - 0x80);
                g = node.Y - 0.34414 * (node.Cb - 0x80) - 0.71414 * (node.Cr - 0x80);
                b = node.Y + 1.77200 * (node.Cb - 0x80);
                    

                channels.R[y,x] = Math.Clamp(r,0,255);
                channels.G[y,x] = Math.Clamp(g,0,255);
                channels.B[y,x] = Math.Clamp(b,0,255);           
            }
        }
        return channels;
    }

    public RGBChannels HorizontalSubsampling(RGBChannels channels)
    {
        //4:2:2 -> 1/2 horizontale auflösung von cb cr,, volle vertikale auflösung
        YCBCRNode[,] ycbcr = ConvertRGBToYCbCr(channels);

        for (int x = 0; x <ycbcr.GetLength(0);x+=4)
        {
            for(int y = 0; y <ycbcr.GetLength(1);y+=2)
            {
                SubSample(ycbcr, x, y);
            }
        }
        return ConvertYCbCrToRGB(ycbcr);
    }

    public void SubSample(YCBCRNode[,] ybcr, int posx, int posy)
    {
        for (int x = posx; x <posx+4;x+=2)
        {
            //first row
            ybcr[posy,posx+1].Cb = ybcr[posy,posx].Cb;
            ybcr[posy,posx+1].Cr = ybcr[posy,posx].Cr;

            //second row
            ybcr[posy+1,posx].Cb = ybcr[posy+1,posx+1].Cb;
            ybcr[posy+1,posx].Cr = ybcr[posy+1,posx+1].Cr;
        }
    }
    
}
