 # ImageLibrary
 ### by Lena Ebner FHS MMT-B 2019 WS 2020 Mulimedia Processing

 ## Components
 - [PixelOperations:](./PixelManipulator.cs) Halve, Invert, Threshold Values, SetChannelTo0ExpectOne
 - [Filter:](./FilterManipulator.cs) SobelX, SobelY, Gaussian, Highpass, Lowpass, GradientOrientation
 - [Constrast Enhancement:](./ContrastEnhancer.cs): Autocontrast, Robust Constrast, Histogram Equalization
 - [Comparision:](./Comparer.cs) Difference Image, PSNR and MSE Calculation
 - [ColorSpace Converter:](./ColorSpaceConverter.cs) from RGB to YCbCr, from YCbCr to RGB, Horizontal Subsampling
 - [Transformations:](./Transformation.cs) DCT and Inverse DCT, Huffman Encoding
 - [Feature Detection:](./FeatureDetector.cs) Harris-Corner Detection Feature Point Detection and Visualisation

## Implementation Notes
- before a new manipalative operation use the ConvertBitmapToRGBChannels interface from ImageProcessor to get a RGBChannels struct with whom every new operation works
- later you can save your the RGBChannels to a image by using the SaveImageFromRGBChannels interface in ImageProcessor
- the subsystems are implemented with the Facade Pattern and also using Template Method and Hook Operations
- for the subsystem also the singleton Pattern is used
- Comparision works either with Bitmaps or RGBChannels struct
