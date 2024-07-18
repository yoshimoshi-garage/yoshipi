using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.Buffers;
using Meadow.Peripherals.Displays;
using SimpleJpegDecoder;

namespace DrawingTests;

internal class SpriteBenchmark : IBenchmark
{
    public string Name => "Sprite";

    private MicroGraphics graphics;

    IPixelBuffer pacman, pacman2;
    IPixelBuffer ghost, ghost2;

    private int pacmanX;
    private int pacmanY;
    private int ghostX;
    private int ghostY;

    readonly int updateAmount = 8;

    static readonly int y = 92;
    static readonly int xGhostStart = -120;
    static readonly int xPacmanStart = 0;

    Rect updateRect = new(0, y, 320, y + 64);

    bool useFrame1 = true;

    public void Initialize(MicroGraphics Graphics)
    {
        graphics = Graphics;

        pacmanX = xPacmanStart;
        pacmanY = y;
        ghostX = xGhostStart;
        ghostY = y;

        LoadImages();
    }

    public void Run(int numberOfFrames)
    {
        graphics.Clear();
        for (int i = 0; i < numberOfFrames; i++)
        {
            Update();
            graphics.Show();
        }
    }

    public void Update()
    {
        graphics.DrawRectangle(updateRect.Left, updateRect.Top, updateRect.Width, updateRect.Height, Color.Black, true);

        graphics.DrawBuffer(pacmanX, pacmanY, useFrame1 ? pacman : pacman2);

        graphics.DrawBuffer(ghostX, ghostY, useFrame1 ? ghost : ghost2);
        useFrame1 = !useFrame1;

        pacmanX += updateAmount;
        ghostX += updateAmount;
    }

    void LoadImages()
    {
        pacman = LoadImage("mspacman2-56.jpg");
        pacman2 = LoadImage("mspacman3-56.jpg");
        ghost = LoadImage("redghost-56.jpg");
        ghost2 = LoadImage("redghost2-56.jpg");
    }

    public IPixelBuffer LoadImage(string name)
    {
        var jpgData = MeadowApp.LoadResource(name);

        var decoder = new JpegDecoder();
        var jpg = decoder.DecodeJpeg(jpgData);

        return new BufferRgb888(decoder.Width, decoder.Height, jpg).Convert<BufferRgb444>();
    }
}