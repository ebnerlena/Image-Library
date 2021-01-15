// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public sealed class ContrastEnhancer
{
    private Contrast contrast;
    private static ContrastEnhancer _instance = null;
    private static readonly object instancelock = new object();
    
    private ContrastEnhancer()
    {
    }
    public static ContrastEnhancer Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new ContrastEnhancer();
                }
            }
        }
        return _instance;
    }
    public RGBChannels AutoContrast(RGBChannels image)
    {
    contrast = new AutoContrast(image);
       return contrast.Apply();
    }
    
    public RGBChannels RobustContrast(RGBChannels image, double quantile = 0.1)
    {
       contrast = new RobustContrast(image, quantile);
       return contrast.Apply();
    }

    public RGBChannels HistogramEqualization(RGBChannels image)
    {
        contrast = new HistogramEqualization(image);
        return contrast.Apply();
    }   
}
