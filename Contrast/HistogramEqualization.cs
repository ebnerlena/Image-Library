// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public class HistogramEqualization : Contrast
{
    private int[][] histograms;
    private int[][] cum_histograms;
    private int mn;

    public HistogramEqualization(RGBChannels image) : base(image)
    {
        mn = image.Width*image.Height;
        SetHistograms();
        SetCumulativHistograms();
    }

    protected override (double R, double G, double B) GetEnhancedValues(int x, int y)
    {
        new_R = cum_histograms[0][(int)image.R[y,x]] * 255.0/mn;
        new_G = cum_histograms[1][(int)image.G[y,x]] * 255.0/mn;
        new_B = cum_histograms[2][(int)image.B[y,x]] * 255.0/mn;

        return (new_R, new_G, new_B);
    }

    private void SetHistograms()
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

    private void SetCumulativHistograms()
    {
        cum_histograms = new int[][] {
            new int[256],
            new int[256],
            new int[256]
        };

        for (int i=0; i<cum_histograms[0].Length; i++)
        {
            if (i == 0)
            {
                cum_histograms[0][0]=0;
                cum_histograms[1][0]=0;
                cum_histograms[2][0]=0;
            } 
            else
            {
                cum_histograms[0][i]=cum_histograms[0][i-1]+histograms[0][i];
                cum_histograms[1][i]=cum_histograms[1][i-1]+histograms[1][i];
                cum_histograms[2][i]=cum_histograms[2][i-1]+histograms[2][i];
            }
        }
    }

}