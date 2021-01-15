// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public abstract class Contrast 
{
    protected RGBChannels image;
    protected double new_R;
    protected double new_G;
    protected double new_B;
    public Contrast(RGBChannels image)
    {
        this.image = image;
    }

    public RGBChannels Apply() 
    {
        RGBChannels output = new RGBChannels(image.Width, image.Height);
    
        for(int x=0; x<image.Width; x++)
        {
            for(int y=0; y<image.Height; y++)
            {
                var enhancedValues = GetEnhancedValues(x,y);

                output.R[y,x] = enhancedValues.R;
                output.G[y,x] = enhancedValues.G;
                output.B[y,x] = enhancedValues.B;
            }
        }

        return output;
    }

    protected abstract (double R, double G, double B) GetEnhancedValues(int x, int y);

}