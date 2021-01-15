// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020

public class GausFilter : Filter
{
    private double[,] filterMatrix;
    public GausFilter(int dimX = 3, int dimY = 3) : base(dimX, dimY)
    {
        filterMatrix = new double[,]{{1.0,2.0,1.0}, {2.0,4.0,2.0}, {1.0,2.0,1.0}}; 
    }

    protected override double ApplyFilterToMatrix(double[,] matrix)
    {
        double average =0;
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                average += matrix[x,y]*filterMatrix[x,y];              
            }
        }
        return average/(matrix.GetLength(0)+matrix.GetLength(1));
    }
}
