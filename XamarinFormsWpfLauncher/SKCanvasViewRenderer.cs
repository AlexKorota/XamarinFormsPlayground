using XamarinFormsWpfLauncher.SkiaRenderer;
using Xamarin.Forms.Platform.WPF;
using SKFormsView = SkiaSharp.Views.Forms.SKCanvasView;
using SKNativeView = SkiaSharp.Views.WPF.SKElement;

[assembly: ExportRenderer(typeof(SKFormsView), typeof(SKCanvasViewRenderer))]
namespace XamarinFormsWpfLauncher.SkiaRenderer
{
    public class SKCanvasViewRenderer : SKCanvasViewRendererBase<SKFormsView, SKNativeView>
    {
    }
}