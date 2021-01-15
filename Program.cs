//Ebner Lena - MMTB2019
using System;
using System.IO;
using System.Drawing;

/* 
. Überarbeiten Sie Ihre Abgaben zu einer kleinen Image-Processing-Library (3 Punkte).
Dokumentieren und argumentieren Sie Ihre Design-Entscheidungen in einem Text-Dokument.
*/

class Program
{
    static void Main(string[] args)
    {
        try 
        {
            var inputFile = args[0];            
            var outputFile = args[1];

            if (args.Length > 2)
                throw new ArgumentOutOfRangeException();

            if(!File.Exists (inputFile))
                throw new FileNotFoundException();

            Bitmap image = new Bitmap(inputFile);

            Console.WriteLine("Image Library Tester");
            ImageProcessor ip = new ImageProcessor();
            RGBChannels c = ip.ConvertBitmapToRGBChannels(image);
            c = ip.ApplyPixelOperation(c, PixelOperation.InvertValues);
            ip.SaveImageFromRGBChannels(c, outputFile);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error: "+ex.Message);
        }
    }
}
