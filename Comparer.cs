// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020

using System;
using System.Drawing;

public sealed class Comparer
{
    private static Comparer _instance = null;
    private static readonly object instancelock = new object();
    
    private Comparer()
    {
    }
    public static Comparer Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new Comparer();
                }
            }
        }
        return _instance;
    }


    public void CalculateMSEandPSNR(Bitmap image1, Bitmap image2)
    {
        var mse = CalculateMSE(image1, image2);
        var psnr = CalculatePSNRForAllChannels(mse.R, mse.G, mse.B);
        
    }

    public void CalculateMSEandPSNR(RGBChannels image1, RGBChannels image2)
    {
        var mse = CalculateMSE(image1, image2);
        var psnr = CalculatePSNRForAllChannels(mse.R, mse.G, mse.B);
        
    }

    private (double R, double G, double B) CalculateMSE(Bitmap image1, Bitmap image2)
    {
        double mseRed=0, mseBlue=0, mseGreen=0;
        int sum = image1.Width * image1.Height;

        for(int x=0; x<image1.Width; x++)
        {
            for(int y=0; y<image2.Height; y++)
            {
                Color pixelColor1 = image1.GetPixel(x, y);
                Color pixelColor2 = image2.GetPixel(x, y);

                mseRed += Math.Pow(Math.Abs(pixelColor1.R - pixelColor2.R), 2.0);
                mseGreen += Math.Pow(Math.Abs(pixelColor1.G - pixelColor2.G), 2.0);
                mseBlue += Math.Pow(Math.Abs(pixelColor1.B - pixelColor2.B), 2.0);
            }
        }

        mseBlue /= sum;
        mseRed /= sum;
        mseGreen /= sum;
        Console.WriteLine($"The MSE for Channel\n\tR: {mseRed}, \n\tG: {mseGreen}, \n\tB: {mseBlue}.");

        return (mseRed, mseGreen, mseBlue);
    }

    private (double R, double G, double B) CalculateMSE(RGBChannels image1, RGBChannels image2)
    {
        double mseRed=0, mseBlue=0, mseGreen=0;
        int sum = image1.Width * image1.Height;

        for(int x=0; x<image1.Width; x++)
        {
            for(int y=0; y<image2.Height; y++)
            {
                mseRed += Math.Pow(Math.Abs(image1.R[y,x] - image2.R[y,x]), 2.0);
                mseGreen += Math.Pow(Math.Abs(image1.G[y,x] - image2.G[y,x]), 2.0);
                mseBlue += Math.Pow(Math.Abs(image1.B[y,x] - image2.B[y,x]), 2.0);
            }
        }

        mseBlue /= sum;
        mseRed /= sum;
        mseGreen /= sum;
        Console.WriteLine($"The MSE for Channel\n\tR: {mseRed}, \n\tG: {mseGreen}, \n\tB: {mseBlue}.");

        return (mseRed, mseGreen, mseBlue);
    }


    private double CalculatePSNR(double mse)
    {   
        //größter wert unendlich 8
        return 20.0*Math.Log(255 / Math.Sqrt(mse), 10.0);
    }

    private (double, double, double) CalculatePSNRForAllChannels(double mseR, double mseG, double mseB)
    {
        double psnrR = CalculatePSNR(mseR);
        double psnrG = CalculatePSNR(mseG);
        double psnrB = CalculatePSNR(mseB);

        Console.WriteLine("PSNR for Channel \n\t R: "+psnrB +"\n\t G: " 
                + psnrG + "\n\t B: " +psnrB);

        return (psnrR, psnrG, psnrB);
    }

    public RGBChannels CalcDifferenceImage(Bitmap img1, Bitmap img2)
    {
        if (img1.Width != img2.Width || img1.Height != img2.Height)
            throw new FieldAccessException("Error: Input files are not in same dimensions");

        Color col1,col2;
        RGBChannels diffImg = new RGBChannels(img1.Width, img1.Height);

        for (int x = 0; x < img1.Width; x++)
        {
            for (int y = 0; y < img1.Height; y++)
            {
                col1 = img1.GetPixel(x,y);
                col2 = img2.GetPixel(x,y);
                diffImg.R[y,x] = Math.Abs(col1.R-col2.R);
                diffImg.G[y,x] = Math.Abs(col1.G-col2.G);
                diffImg.B[y,x] = Math.Abs(col1.B-col2.B);
            }
        }
        return diffImg;
    }

    public RGBChannels CalcDifferenceImage(RGBChannels img1, RGBChannels img2)
    {
        if (img1.Width != img2.Width || img1.Height != img2.Height)
            throw new FieldAccessException("Error: Input files are not in same dimensions");

        RGBChannels diffImg = new RGBChannels(img1.Width, img1.Height);

        for (int x = 0; x < img1.Width; x++)
        {
            for (int y = 0; y < img1.Height; y++)
            {
                diffImg.R[y,x] = Math.Abs(img1.R[y,x] - img2.R[y,x]);
                diffImg.G[y,x] = Math.Abs(img1.G[y,x] - img2.G[y,x]);
                diffImg.B[y,x] = Math.Abs(img1.B[y,x] - img2.B[y,x]);
            }
        }
        return diffImg;
    }

}