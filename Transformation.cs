//Lena Ebner - MMTB2019
using System;
using System.Drawing;
using System.IO;

public class Transformations 
{    
    private static Transformations _instance = null;
    private static readonly object instancelock = new object();
    
    private Transformations()
    {
    }
    public static Transformations Instance()
    {
        if(_instance == null)
        {
            lock (instancelock)
            {
                if(_instance == null)
                {
                    _instance = new Transformations();
                }
            }
        }
        return _instance;
    }


    public RGBChannels InverseDCT(RGBChannels img, int n, int m, double radius)
    {
        double kx=1.0;
        double ky=1.0;
        double value = 0.0;
        double dct = 0.0;

        RGBChannels original = new RGBChannels(img.R.GetLength(0), img.R.GetLength(1));
        for(int i = 0; i<m; i++)
        {
            for (int j = 0; j<n; j++)
            {
                value=0.0;
                for (int x = 0; x < m; x++)
                {
                    for (int y = 0; y < n; y++)
                    {   
                        if (x==0)
                            kx=(1.0/Math.Sqrt(2));
                        else
                            kx=1.0;
                        if (y==0)
                            ky=(1.0/Math.Sqrt(2));  
                        else
                            ky=1.0;

                        dct = CalcDCTValue(i,j,x,y);

                        if(dct > radius)
                            dct = 0.0;

                        value += (double)(kx*ky*0.25)*(img.R[y,x])*dct;
                    }
                }
                original.R[j,i] = value;
                original.G[j,i] = value;
                original.B[j,i] = value;
            }
        }
        return original;
    }

    public RGBChannels DCT(RGBChannels img, int n, int m)
    {
        double kx=1.0;
        double ky=1.0;
        double dct=0.0;
        
        RGBChannels channels = img;

        for(int i = 0; i<m; i++)
        {
            for (int j = 0; j<n; j++)
            {
                if (i==0)
                    kx=(1.0/Math.Sqrt(2));
                else
                    kx=1.0;
                if (j==0)
                    ky=(1.0/Math.Sqrt(2));  
                else
                    ky=1.0; 

                dct=0.0;
                for (int x = 0; x < m; x++)
                {
                    for (int y = 0; y < n; y++)
                    {   
                        dct += (img.R[y,x])*CalcDCTValue(x,y,i,j);                           
                    }
                }
                channels.SetValueToAllChannels((dct*((kx*ky)/4.0)),i,j);                           
            }
        }
        return channels;
    }

    private double CalcDCTValue(int x, int y, double nx, double my)
    {
        return GetCosValue(x, nx)*GetCosValue(y,my);
    }

    private double GetCosValue(int pos, double dach)
    {
        return Math.Cos((((2.0*pos)+1.0)*dach*Math.PI)/16.0);
    }

    public Node HuffmanEncoding(Bitmap img)
    {
        Heap myHeap = Heap.Setup(img); 
        Node rootNode = CalculateHuffmanCode(myHeap);            
        Console.WriteLine("Traversing Tree and adding Codes to Nodes");
        TraverseTreeAndAddCode(rootNode, "");
        return rootNode;
    }

    private Node CalculateHuffmanCode(Heap minHeap)
    {
        Console.WriteLine("Calculating the huffman codex");
        while (minHeap.Size() > 1)
        {
            minHeap.PairMinimas();
        }

        return minHeap.GetRootNode();
    }

    private void TraverseTreeAndAddCode(Node root, string code)
    {
        if (root.leftChild == null && root.rightChild == null){

            Console.WriteLine("assign code : "+ code + " to "+ root.Color);
            root.code = code;
        }

        if (root.leftChild != null)
        {   
            TraverseTreeAndAddCode(root.leftChild, (code+"0"));
        }
        if (root.rightChild != null)
            TraverseTreeAndAddCode(root.rightChild, (code+"1"));    
    }

}
