// ImageLibrary by Lena Ebner MMT-B 2019 Multimedia Processing WS 2020

using System.Drawing;
public class HarrisNode 
{
    public double Value { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public Color OrientationColor {get; set; }
    public HarrisNode(double value, int x, int y)
    {
        this.Value = value;
        this.X = x;
        this.Y = y;
    }
}