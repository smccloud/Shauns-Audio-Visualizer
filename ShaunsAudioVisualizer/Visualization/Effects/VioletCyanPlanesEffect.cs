using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class VioletCyanPlanesEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Violet cyan planes";

    private float[] _bands = Array.Empty<float>();
    private float _time;

    public void Reset() { _bands = Array.Empty<float>(); _time = 0f; }

    public void Update(VisualizationFrame frame)
    {
        _bands = frame.LeftBands.ToArray();
        _time = (float)frame.Elapsed.TotalSeconds;
    }

    public void Render(Graphics graphics, Rectangle bounds)
    {
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // translucent planes
        for (var i = 0; i < 4; i++)
        {
            var pts = new PointF[]
            {
                new(100 + i * 40, 50 + i * 30),
                new(bounds.Width - 100 - i * 20, 80 + i * 20),
                new(bounds.Width - 120 + i * 30, bounds.Height - 80 - i * 20),
                new(80 + i * 50, bounds.Height - 60 + i * 10),
            };
            using var path = new GraphicsPath();
            path.AddPolygon(pts);
            using var brush = new SolidBrush(Color.FromArgb(60, 120, 60, 200));
            graphics.FillPath(brush, path);
            using var pen = new Pen(Color.FromArgb(120, 80, 200, 255), 1.5f);
            graphics.DrawPath(pen, path);
        }

        // waveform
        if (_bands.Length > 0)
        {
            using var pen = new Pen(Color.White, 2f);
            var pts = new PointF[_bands.Length];
            for (var i = 0; i < _bands.Length; i++)
            {
                var x = i * (bounds.Width / (float)(_bands.Length - 1));
                var y = bounds.Height / 2f + (float)Math.Sin(i * 0.5 + _time) * _bands[i] * 80;
                pts[i] = new PointF(x, y);
            }
            graphics.DrawLines(pen, pts);
        }
    }
}