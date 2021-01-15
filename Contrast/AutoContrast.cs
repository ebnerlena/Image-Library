// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public class AutoContrast : Contrast
{
    public AutoContrast(RGBChannels image) : base(image)
    {
        image.SetMinMax();
    }

    protected override (double R, double G, double B) GetEnhancedValues(int x, int y)
    {
        new_R = (image.R[y,x] - image.minR) *255.0/(image.maxR-image.minR);
        new_G = (image.G[y,x] - image.minG) *255.0/(image.maxG-image.minG);
        new_B = (image.B[y,x] - image.minB) *255.0/(image.maxB-image.minB);

        return (new_R, new_G, new_B);
    }
}