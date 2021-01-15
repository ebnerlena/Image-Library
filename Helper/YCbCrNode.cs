// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020

public class YCBCRNode 
{
    public double Y { get; set; }
    public double Cb { get; set; }

    public double Cr { get; set; }

    public YCBCRNode(double y, double cb, double cr)
    {
        this.Y = y;
        this.Cb = cb;
        this.Cr = cr;
    }
}