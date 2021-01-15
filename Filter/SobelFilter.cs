// ImageLibrary by Lena Ebner FHS MMT-B 2019 Multimedia Processing WS 2020
using System;

public class SobelFilter : Filter
{
    private double[,] filterMatrix;

    public SobelDirection Direction { get; private set; }
    public SobelFilter(SobelDirection direction, int dimX = 3, int dimY = 3) : base(dimX, dimY)
    {
       SetMatrix(direction);
       this.Direction = direction;
    }

    protected override double ApplyFilterToMatrix(double[,] matrix)
    {
        double value=0;
        for(int i=0; i<matrix.GetLength(0);i++)
        {
            for(int j=0; j<matrix.GetLength(1); j++)
            {
                value += matrix[i,j]*filterMatrix[i,j];
            }
        }
        return value;
    }

    private void SetMatrix(SobelDirection direction) 
    {
         if (direction == SobelDirection.X)
        {
            filterMatrix = new double[,]{{-1.0,0.0,1.0}, {-2.0,0.0,2.0}, {-1.0,0.0,1.0}};   
        }  
        else if (direction == SobelDirection.Y)
        {
            filterMatrix = new double[,]{{-1.0,-2.0,-1.0}, {0.0,0.0,0.0}, {1.0,2.0,1.0}};  
        }
        else 
        {
            throw new ArgumentOutOfRangeException("Error: Direction should either be X or Y for image dimension");
        }
    }
}