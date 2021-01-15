// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public class RobustContrast : Contrast
{
    private double quantile;
    private double q_low;
    private double q_high;
    private int mn;
    private int[][] histograms;

    public RobustContrast(RGBChannels image, double quantile = 0.1) : base(image)
    {
        this.quantile = quantile;

        q_low = (quantile/2);
        q_high = (q_low);

        mn = image.Width*image.Height;
        SetHistograms(image);
        SetMinMaxCumulative(image);
    }

    protected override (double R, double G, double B) GetEnhancedValues(int x, int y)
    {
        new_R = (image.R[y,x]-image.minR)*255.0/(image.maxR-image.minR);
        new_G = (image.G[y,x]-image.minG)*255.0/(image.maxG-image.minG);
        new_B = (image.B[y,x]-image.minB)*255.0/(image.maxB-image.minB);

        return (new_R, new_G, new_B);
    }

    private (int Min, int Max) FindMinMaxCumaltive(int[] cumH)
    {
        int alow = int.MaxValue;
        int min=255;
        int ahigh = int.MinValue;
        int max=0;
            
        for(int i = 0; i<cumH.Length; i++)
        {
            if((cumH[i] >= mn*q_low) && (cumH[i] < alow)){
                alow = cumH[i];
                min=i;
            }
                
            if ((cumH[i] <= mn*(1-q_high)) && (cumH[i] > ahigh)){
                ahigh = cumH[i];
                max=i;
            }       
        }
        return (min, max);
    }   

    public void SetMinMaxCumulative(RGBChannels image)
    {
        var minmaxR = FindMinMaxCumaltive(histograms[0]);
        image.minR = minmaxR.Min;
        image.maxR = minmaxR.Max;

        var minmaxG = FindMinMaxCumaltive(histograms[1]);
        image.minG = minmaxG.Min;
        image.maxG = minmaxG.Max;

        var minmaxB = FindMinMaxCumaltive(histograms[2]);
        image.minB = minmaxB.Min;
        image.maxB = minmaxB.Max;
    }

    private void SetHistograms(RGBChannels image)
    {
        histograms = new int[][] {
            new int[256],
            new int[256],
            new int[256]
        };
    
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {  
                histograms[0][(int)image.R[y,x]]++;
                histograms[1][(int)image.G[y,x]]++;
                histograms[2][(int)image.B[y,x]]++;
            }
        }
    }

}