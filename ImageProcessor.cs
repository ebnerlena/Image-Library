// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020

using System;
using System.Drawing;
using System.Collections.Generic;

public class ImageProcessor 
{
    private ContrastEnhancer contrastEnhancer;
    private FilterManipulator filterManipulator;
    private PixelManipulator pixelManipulator;
    private Comparer comparer;
    private FeatureDetector featureDetector;

    public ImageProcessor()
    {
    }

    public RGBChannels ApplyFilter(RGBChannels image, FilterType type, int times = 1)
    {
        if (filterManipulator == null)
        {
            filterManipulator = FilterManipulator.Instance();
        }
        RGBChannels output = image;

        switch(type)
        {
            case FilterType.SobelX:
            output = filterManipulator.SobelXFilter(image);
            output.AutoContrastForAllChannels();
            break;
            case FilterType.SobelY:
            output = filterManipulator.SobelYFilter(image);
            output.AutoContrastForAllChannels();
            break;
            case FilterType.Gaussian:
            output = filterManipulator.GaussianFilter(image);
            output.AutoContrastForAllChannels();
            break;
            case FilterType.Highpass:
            output = filterManipulator.HighPassFilter(image);
            output.AutoContrastForAllChannels();
            break;
            case FilterType.GaussianMultipleTimes:
            output = filterManipulator.GaussianFilterNTimes(image, times);
            break;
            case FilterType.Lowpass:
            output = filterManipulator.LowPassFilter(image);
            output.AutoContrastForAllChannels();
            break;
            case FilterType.GradientOrientation:
            output = filterManipulator.GradientOrientation(output);
            break;
            case FilterType.GradientMagnitude:
            output = filterManipulator.GradientMagnitude(image);
            break;
        }
        return output;
    }

    public RGBChannels ApplyConstrastEnhancement(RGBChannels image, ContrastType type, double quantil = 0.1)
    {
        if (contrastEnhancer == null)
        {
            contrastEnhancer = ContrastEnhancer.Instance();
        }
        RGBChannels output = image;

        switch(type)
        {
            case ContrastType.AutoContrast:
            output = contrastEnhancer.AutoContrast(image);
            break;
            case ContrastType.RobustContrast:
            output = contrastEnhancer.RobustContrast(image, quantil);
            break;
            case ContrastType.HistogramEqualization:
            output = contrastEnhancer.HistogramEqualization(image);
            break;
        }
        return output;
    }

    public RGBChannels ApplyPixelOperation(RGBChannels image, PixelOperation type, int threshold = 120, string channel = "G")
    { 
        if (pixelManipulator == null)
        {
            pixelManipulator = PixelManipulator.Instance();
        }
        RGBChannels output = image;

        switch(type)
        {
            case PixelOperation.HalveValues:
            output = pixelManipulator.HalveValues(image);
            break;
            case PixelOperation.InvertValues:
            output = pixelManipulator.InvertValues(image);
            break;
            case PixelOperation.ThresholdValues:
            output = pixelManipulator.ThresholdValues(image, threshold);
            break;
            case PixelOperation.SetChannelsToZero:
            output = pixelManipulator.SetChannelsToZeroExcept(image, channel);
            break;
        }

        return output;
    }

    public RGBChannels ApplyComparison(Bitmap image1, Bitmap image2, CompareType type)
    {
        if (comparer == null)
        {
            comparer = Comparer.Instance();
        }
        RGBChannels channels = ConvertBitmapToRGBChannels(image1);
        RGBChannels output = channels;

        switch(type)
        {
            case CompareType.MSEandPSNR:
            comparer.CalculateMSEandPSNR(image1, image2);
            break;
            case CompareType.DifferenceImage:
            output = comparer.CalcDifferenceImage(image1, image2);
            break;
        }
        return output;
    }

    public RGBChannels ApplyComparison(RGBChannels image1, RGBChannels image2, CompareType type)
    {
        if (comparer == null)
        {
            comparer = Comparer.Instance();
        }
        RGBChannels channels = image1;
        RGBChannels output = channels;

        switch(type)
        {
            case CompareType.MSEandPSNR:
            comparer.CalculateMSEandPSNR(image1, image2);
            break;
            case CompareType.DifferenceImage:
            output = comparer.CalcDifferenceImage(image1, image2);
            break;
        }
        return output;
    }

    public RGBChannels ApplyFeatureDetection(RGBChannels image, HarrisCornerDetection type, double threshold = 0.0000000002)
    {
        if (featureDetector == null)
        {
            featureDetector = FeatureDetector.Instance();
        }
        RGBChannels output = image;
        List<HarrisNode> maximas;

        switch(type)
        {
            case HarrisCornerDetection.fValues:
            output = featureDetector.HarrisCornerDetection(image);
            break;
            case HarrisCornerDetection.LokalMaxima9x9:
            output = featureDetector.HarrisCornerDetection(image);
            maximas = featureDetector.GetMaximas(9, 9, output);
            output = featureDetector.GetColorFromFValue(output, maximas);
            break;
            case HarrisCornerDetection.fValues4bin:
            output = featureDetector.HarrisCornerDetection(image);
            maximas = featureDetector.GetMaximas(9, 9, output);
            maximas = featureDetector.ThresholdMaximas(maximas, threshold);
            RGBChannels orientation = filterManipulator.GradientOrientation(image);
            maximas = featureDetector.GetOrientationBin(orientation, maximas);
            output = featureDetector.GetColorFromOrientation(output,maximas);

            break;
        }

        return output;
    }

#region ColorSparceConversion
    public YCBCRNode[,] ConvertRGBtoYCbCr(RGBChannels image)
    {
        return ColorSpaceConverter.Instance().ConvertRGBToYCbCr(image);
    }

    public RGBChannels ConvertYCrCbToRGB(YCBCRNode[,] image)
    {
        return ColorSpaceConverter.Instance().ConvertYCbCrToRGB(image);
    }

    public RGBChannels HorizontalSubsampling(RGBChannels image)
    {
        return ColorSpaceConverter.Instance().HorizontalSubsampling(image);
    }
    
#endregion
#region DCT Transformations
    public RGBChannels ApplyDCTTransformation(RGBChannels image)
    {
        return Transformations.Instance().DCT(image, 8, 8);
    }

    public RGBChannels ApplyInverseDCTTransformation(RGBChannels image, double radius)
    {
        return Transformations.Instance().InverseDCT(image, 8, 8, radius);
    }

    public Node HuffmanEncoding(Bitmap image)
    {
        return Transformations.Instance().HuffmanEncoding(image);
    }

#endregion

    public RGBChannels ConvertBitmapToRGBChannels(Bitmap image)
    {   
        RGBChannels channels = new RGBChannels(image.Width, image.Height);
        for (var x = 0; x < image.Width; x++)
        {
            for (var y = 0; y < image.Height; y++)
            {
                Color color = image.GetPixel(x,y);
                channels.R[y,x] = color.R;
                channels.G[y,x] = color.G;
                channels.B[y,x] = color.B;
            }
        }

        return channels;
    }

    public Bitmap SaveImageFromRGBChannels(RGBChannels channels, string newFilePath = "out.png")
    {
        Bitmap result = new Bitmap(channels.Width, channels.Height);

        try {
            for (int x = 0; x < channels.Width; x++)
            {
                for (int y = 0; y < channels.Height; y++)
                {
                    result.SetPixel(x,y, Color.FromArgb(
                        (Byte)channels.R[y,x],
                        (Byte)channels.G[y,x],
                        (Byte)channels.B[y,x]
                    ));
                }
            }
            result.Save(newFilePath);
            Console.WriteLine("Saved "+newFilePath);
            return result;
        }
        catch {
            throw new Exception("Problems saving the RGBChannels to a Bitmap");
        }
    }
}
