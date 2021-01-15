// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public class HighPassFilter : Filter
{
    private int[] filterMatrix;
    public HighPassFilter(int dimX = 5, int dimY = 3) : base(dimX, dimY)
    {
        filterMatrix = new int[] {-1,-2, 6, -2, -1};
    }

    protected override double ApplyFilterToMatrix(double[,] matrix)
    {
       double heighpass=0.0;

        for (int x = 0; x < matrix.GetLength(1); x++)
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                heighpass += matrix[y,x]*(filterMatrix[y]*(1.0/8.0));              
            }
        }

        return heighpass;
    }
}
