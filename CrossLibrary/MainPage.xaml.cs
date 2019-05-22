using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using SkiaSharp;

namespace CrossLibrary
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            const int brickWidth = 64;
            const int brickHeight = 24;
            const int morterThickness = 6;
            const int bitmapWidth = brickWidth + morterThickness;
            const int bitmapHeight = 2 * (brickHeight + morterThickness);

            SKBitmap bitmap = new SKBitmap(bitmapWidth, bitmapHeight);

            using (SKCanvas canvas = new SKCanvas(bitmap))
            using (SKPaint brickPaint = new SKPaint())
            {
                brickPaint.Color = new SKColor(0xB2, 0x22, 0x22);

                canvas.Clear(new SKColor(0xF0, 0xEA, 0xD6));
                canvas.DrawRect(new SKRect(morterThickness / 2,
                                           morterThickness / 2,
                                           morterThickness / 2 + brickWidth,
                                           morterThickness / 2 + brickHeight),
                                           brickPaint);

                int ySecondBrick = 3 * morterThickness / 2 + brickHeight;

                canvas.DrawRect(new SKRect(0,
                                           ySecondBrick,
                                           bitmapWidth / 2 - morterThickness / 2,
                                           ySecondBrick + brickHeight),
                                           brickPaint);

                canvas.DrawRect(new SKRect(bitmapWidth / 2 + morterThickness / 2,
                                           ySecondBrick,
                                           bitmapWidth,
                                           ySecondBrick + brickHeight),
                                           brickPaint);
            }

            // Save as public property for other programs
            BrickWallTile = bitmap;
        }

        public static SKBitmap BrickWallTile { private set; get; }

        SKBitmap monkeyBitmap = BitmapExtensions.LoadBitmapResource(
          typeof(MainPage),
          "CrossLibrary.Media.SeatedMonkey.jpg");

        SKBitmap matteBitmap = BitmapExtensions.LoadBitmapResource(
            typeof(MainPage),
            "CrossLibrary.Media.SeatedMonkeyMatte.png");

        int step = 0;


        void OnButtonClicked(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            step = (step + 1) % 5;

            switch (step)
            {
                case 0: btn.Text = "Show sitting monkey"; break;
                case 1: btn.Text = "Draw matte with DstIn"; break;
                case 2: btn.Text = "Draw sidewalk with DstOver"; break;
                case 3: btn.Text = "Draw brick wall with DstOver"; break;
                case 4: btn.Text = "Reset"; break;
            }

            canvasView.InvalidateSurface();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            float x = (info.Width - monkeyBitmap.Width) / 2;
            float y = info.Height - monkeyBitmap.Height;

            // Draw monkey bitmap
            if (step >= 1)
            {
                canvas.DrawBitmap(monkeyBitmap, x, y);
            }

            // Draw matte to exclude monkey's surroundings
            if (step >= 2)
            {
                using (SKPaint paint = new SKPaint())
                {
                    paint.BlendMode = SKBlendMode.DstIn;
                    canvas.DrawBitmap(matteBitmap, x, y, paint);
                }
            }

            const float sidewalkHeight = 80;
            SKRect rect = new SKRect(info.Rect.Left, info.Rect.Bottom - sidewalkHeight,
                                     info.Rect.Right, info.Rect.Bottom);

            // Draw gravel sidewalk for monkey to sit on
            if (step >= 3)
            {
                using (SKPaint paint = new SKPaint())
                {
                    paint.Shader = SKShader.CreateCompose(
                                        SKShader.CreateColor(SKColors.SandyBrown),
                                        SKShader.CreatePerlinNoiseTurbulence(0.1f, 0.3f, 1, 9));

                    paint.BlendMode = SKBlendMode.DstOver;
                    canvas.DrawRect(rect, paint);
                }
            }

            // Draw bitmap tiled brick wall behind monkey
            if (step >= 4)
            {
                using (SKPaint paint = new SKPaint())
                {
                    SKBitmap bitmap = BrickWallTile;
                    float yAdjust = (info.Height - sidewalkHeight) % bitmap.Height;

                    paint.Shader = SKShader.CreateBitmap(bitmap,
                                                         SKShaderTileMode.Repeat,
                                                         SKShaderTileMode.Repeat,
                                                         SKMatrix.MakeTranslation(0, yAdjust));
                    paint.BlendMode = SKBlendMode.DstOver;
                    canvas.DrawRect(info.Rect, paint);
                }
            }
        }
    }
}