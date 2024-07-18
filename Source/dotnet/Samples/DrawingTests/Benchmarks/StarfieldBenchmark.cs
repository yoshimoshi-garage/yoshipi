using Meadow;
using Meadow.Foundation.Graphics;
using System;

namespace DrawingTests;

internal class StarfieldBenchmark : IBenchmark
{
    public string Name => "Starfield";

    private MicroGraphics graphics;

    private const int NumberOfStars = 128;

    private Random random;

    private byte[] starsX;
    private byte[] starsY;
    private byte[] starsZ;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;

        random = new Random(0); //forced standard seed

        starsX = new byte[NumberOfStars];
        starsY = new byte[NumberOfStars];
        starsZ = new byte[NumberOfStars];

        for (int i = 0; i < NumberOfStars; i++)
        {
            starsX[i] = (byte)(Graphics.Width / 2 - Graphics.Height / 2 + random.Next(256));
            starsY[i] = (byte)random.Next(256);
            starsZ[i] = (byte)(255 - i);
        }
    }

    public void Run(int numberOfFrames)
    {
        graphics.Clear();

        int halfWidth = graphics.Width / 2;
        int halfHeight = graphics.Height / 2;

        for (int frame = 0; frame < numberOfFrames; frame++)
        {
            for (int i = 0; i < NumberOfStars; i++)
            {
                if (starsZ[i] <= 1)
                {
                    starsX[i] = (byte)(halfWidth + random.Next(256) - 128);
                    starsY[i] = (byte)random.Next(256);
                    starsZ[i] = (byte)(255 - i);
                }
                else
                {
                    int oldPosX = (starsX[i] - halfWidth) * 256 / starsZ[i] + halfWidth;
                    int oldPosY = (starsY[i] - halfHeight) * 256 / starsZ[i] + halfHeight;

                    graphics.DrawPixel(oldPosX, oldPosY, Color.Black);

                    starsZ[i] -= 2;
                    if (starsZ[i] > 1)
                    {
                        int posX = (starsX[i] - halfWidth) * 256 / starsZ[i] + halfWidth;
                        int posY = (starsY[i] - halfHeight) * 256 / starsZ[i] + halfHeight;

                        if (posX >= 0 && posY >= 0 &&
                            posX < graphics.Width && posY < graphics.Height)
                        {
                            byte intensity = (byte)(255 - starsZ[i]);
                            graphics.DrawPixel(posX, posY, Color.FromRgb(intensity, intensity, intensity));
                        }
                        else
                        {
                            starsZ[i] = 0;
                        }
                    }
                }
            }

            graphics.Show();
        }
    }
}