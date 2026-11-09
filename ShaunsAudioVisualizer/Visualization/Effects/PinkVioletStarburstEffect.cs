using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class PinkVioletStarburstEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Pink violet starburst";

    private float _level;
    private float _time;

    public void Reset() { _level = 0f; _time = 0f; }

    public void Update(VisualizationFrame frame)
    {
        _level = frame.Levels.Peak;
        _time = (float)frame.Elapsed.TotalSeconds;
    }

    public void Render(Graphics graphics, Rectangle bounds)
    {
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var cx = bounds.Width / 2f;
        var cy = bounds.Height / 2f;
        var maxR = Math.Min(bounds.Width, bounds.Height) / 2f - 4;

        var spikes = 24;
        var points = new PointF[spikes * 2];
        for (var i = 0; i < spikes; i++)
        {
            var angle = i * (float)(2 * Math.PI / spikes) + _time * 0.5f;
            var outer = maxR * (0.6f + 0.4f * (float)Math.Sin(_time * 3 + i));
            var inner = maxR * 0.15f;
            points[i * 2] = new PointF(cx + (float)Math.Cos(angle) * outer, cy + (float)Math.Sin(angle) * outer);
            var a2 = angle + (float)(Math.PI / spikes);
            points[i * 2 + 1] = new PointF(cx + (float)Math.Cos(a2) * inner, cy + (float)Math.Sin(a2) * inner);
        }

        using var path = new GraphicsPath();
        path.AddPolygon(points);
        using var brush = new SolidBrush(Color.FromArgb(220, 220, 40, 120));
        graphics.FillPath(brush, path);
        using var pen = new Pen(Color.FromArgb(200, 255, 100, 200), 2f);
        graphics.DrawPath(pen, path);
    }
}