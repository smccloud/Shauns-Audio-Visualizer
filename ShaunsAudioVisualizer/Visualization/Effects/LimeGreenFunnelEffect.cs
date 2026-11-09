using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class LimeGreenFunnelEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Lime green funnel";

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

        var cx = bounds.Width / 2f;
        var cy = bounds.Height / 2f;

        for (var i = 0; i < 20; i++)
        {
            var r = 5 + i * 8 + (float)Math.Sin(_time * 2 + i) * 4;
            var alpha = 200 - i * 8;
            using var pen = new Pen(Color.FromArgb(alpha, 100, 220, 50), 2f);
            graphics.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
        }

        // funnel clouds
        for (var i = 0; i < 8; i++)
        {
            var angle = i * (float)(2 * Math.PI / 8) + _time;
            var r = 30 + i * 15;
            var x = cx + (float)Math.Cos(angle) * r;
            var y = cy + (float)Math.Sin(angle) * r;
            using var brush = new SolidBrush(Color.FromArgb(120, 200, 255, 50));
            graphics.FillEllipse(brush, x - 15, y - 15, 30, 30);
        }
    }
}