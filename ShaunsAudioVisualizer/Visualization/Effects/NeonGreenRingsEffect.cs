using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class NeonGreenRingsEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Neon green rings";

    private float _level;
    private float _time;

    public void Reset() { _level = 0f; _time = 0f; }

    public void Update(VisualizationFrame frame)
    {
        _level = frame.Levels.Rms;
        _time = (float)frame.Elapsed.TotalSeconds;
    }

    public void Render(Graphics graphics, Rectangle bounds)
    {
        graphics.Clear(Color.Black);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;

        var cx = bounds.Width / 2f;
        var cy = bounds.Height / 2f;

        for (var i = 0; i < 12; i++)
        {
            var r = 20 + i * 12 + (float)Math.Sin(_time * 3 + i) * 6;
            using var pen = new Pen(Color.FromArgb(180, 0, 255, 100), 2.5f);
            graphics.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
        }

        // ribbons
        for (var i = 0; i < 5; i++)
        {
            using var path = new GraphicsPath();
            var pts = new PointF[30];
            for (var j = 0; j < pts.Length; j++)
            {
                var t = j / (float)(pts.Length - 1);
                var angle = t * (float)(2 * Math.PI) + _time + i;
                var r = 40 + i * 20 + (float)Math.Sin(t * 8 + _time) * 10;
                pts[j] = new PointF(cx + (float)Math.Cos(angle) * r, cy + (float)Math.Sin(angle) * r);
            }
            path.AddCurve(pts, 0.5f);
            using var pen = new Pen(Color.FromArgb(150, 200, 255, 50), 2f);
            graphics.DrawPath(pen, path);
        }

        // magenta core
        var cr = 8 + _level * 15;
        using var brush = new SolidBrush(Color.FromArgb(220, 255, 0, 200));
        graphics.FillEllipse(brush, cx - cr, cy - cr, cr * 2, cr * 2);
    }
}