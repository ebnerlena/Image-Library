// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public struct RGBChannels {

    public int Width { get; private set; }
    public int Height {get; private set; }

    public double [,] R {get; set;}
    public double [,] G {get; set;}
    public double[,] B {get; set;}

    public double minR {get; set;}
    public double maxR {get; set;}
    public double minG {get; set;}
    public double maxG {get; set;}
    public double minB {get; set;}
    public double maxB{get; set;}
 

    public RGBChannels(int width, int height)
    {
        this.Width = width;
        this.Height = height;
        R = new double[height, width];
        G = new double[height, width];
        B = new double[height, width];

        minR=255;
        minG=255;
        minB=255;
        maxR=0;
        maxG=0;
        maxB=0;

    }

    public void SetMinMax()
    {
        var minmaxR = FindMinMaxIntensity(R);
        minR = minmaxR.min;
        maxR = minmaxR.max;

        var minmaxG = FindMinMaxIntensity(G);
        minG = minmaxG.min;
        maxG = minmaxG.max;

        var minmaxB = FindMinMaxIntensity(B);
        minB = minmaxB.min;
        maxB = minmaxB.max;
    }
    

    private (double min, double max) FindMinMaxIntensity(double[,] channel)
    {
        double min=255;
        double max = 0;

        for(int x=0; x<channel.GetLength(1); x++)
        {
            for(int y=0; y<channel.GetLength(0); y++)
            {  
                double pixelColor = channel[y,x];

                if(pixelColor < min)
                    min = pixelColor;
                if(pixelColor > max)
                    max = pixelColor;
            }
        }
        return (min,max);    
    }  

    private int AutoContrast(double value, double min, double max)
    {
        return (int)((value-min)*(255/(max-min)));
    }

    public void AutoContrastForAllChannels()
    {
        SetMinMax();
        for (int x = 0; x < R.GetLength(1); x++)
        {
            for (int y = 0; y < R.GetLength(0); y++)
            {
                R[y,x] = AutoContrast(R[y,x], minR, maxR);
                G[y,x] = AutoContrast(G[y,x], minG, maxG);
                B[y,x] = AutoContrast(B[y,x], minB, maxB);
            }
        }
    }
    public void SetValueToAllChannels( double value, int x, int y) 
    {
        this.R[y,x]=value;
        this.G[y,x]=value;
        this.B[y,x]=value;
    }
}
