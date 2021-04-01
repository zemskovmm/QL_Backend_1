using SkiaSharp;
using System;
using System.IO;

namespace QuartierLatin.Backend.Utils
{
    public class ImageScaler
    {
        private const int MaximumDimension = 600; // Increased for better look on hi-dpi screens.
        private readonly int _maxDim;

        public ImageScaler(int maxDim = MaximumDimension) => _maxDim = maxDim;

        public void Scale(Stream input, Stream output, bool squareShape = false)
        {
            using var bitmap = SKBitmap.Decode(input);
            var dimensions = Scale(bitmap.Width, bitmap.Height, squareShape);
            var newBitmap = bitmap.Resize(dimensions, SKFilterQuality.High);
            newBitmap.Encode(output, SKEncodedImageFormat.Jpeg, 70);
        }

        private SKSizeI Scale(int w, int h, bool squareShape) =>
            squareShape
                ? new SKSizeI(_maxDim, _maxDim)
                : w > h
                    ? new SKSizeI(_maxDim, ScaleDim(h, w))
                    : new SKSizeI(ScaleDim(w, h), _maxDim);

        private int ScaleDim(int dim, int other)
        {
            var ratio = (double)dim / (double)other;
            var newDim = _maxDim * ratio;
            return Math.Max(1, Math.Min(_maxDim, (int)newDim));
        }
    }
}
