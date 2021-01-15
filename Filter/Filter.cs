// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020
using System;

public abstract class Filter 
{
    public int FilterDimensionX { get; protected set; }
    
    public int FilterDimensionY { get; protected set; }

    public Filter(int dimX, int dimY)
    {
        FilterDimensionX = dimX;
        FilterDimensionY = dimY;
    }
    public virtual RGBChannels Apply(RGBChannels img)
    {
        RGBChannels channels = new RGBChannels(img.Width, img.Height);      
        for (int x = 0; x < img.Width; x++)
        {
            for (int y = 0; y < img.Height; y++)
            {
                var boxFilters = GetFilterMatrix(img, x, y);
                channels.R[y,x] = ApplyFilterToMatrix(boxFilters.R);
                channels.G[y,x] = ApplyFilterToMatrix(boxFilters.G);
                channels.B[y,x] = ApplyFilterToMatrix(boxFilters.B);     
            }
        }
        return channels;
    }

    private (double[,] R, double[,] G, double[,] B) GetFilterMatrix(RGBChannels channels, int x, int y)
    {   
        int offsetX = (x-(FilterDimensionX/2));
        int offsetY = (y-(FilterDimensionY/2));
        double [,] boxFilterR = new double[FilterDimensionX, FilterDimensionY];
        double [,] boxFilterG = new double[FilterDimensionX, FilterDimensionY];
        double [,] boxFilterB = new double[FilterDimensionX, FilterDimensionY];


        for(int i=offsetX, r=0; i<offsetX+FilterDimensionX && r<FilterDimensionX; i++, r++)
        {
            for(int j=offsetY, t=0; j<offsetY+FilterDimensionY && t<FilterDimensionY; j++, t++)
            {
                if (i>=channels.Width)
                    i-=FilterDimensionX;
                if (j>=channels.Height)
                    j-=FilterDimensionY;

                boxFilterR[r,t] = channels.R[Math.Abs(j),Math.Abs(i)];
                boxFilterG[r,t] = channels.G[Math.Abs(j),Math.Abs(i)];
                boxFilterB[r,t] = channels.B[Math.Abs(j),Math.Abs(i)];
            }
        }

        return (boxFilterR, boxFilterG, boxFilterB); 
    }

    protected abstract double ApplyFilterToMatrix(double[,] boxFilter);

}