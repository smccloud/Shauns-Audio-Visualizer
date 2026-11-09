using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class MagentaSkyBlueRibbonsEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Magenta sky blue ribbons";

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

        for (var i = 0; i < 8; i++)
        {
            using var path = new GraphicsPath();
            var pts = new PointF[30];
            for (var j = 0; j < pts.Length; j++)
            {
                var t = j / (float)(pts.Length - 1);
                var x = cx + (float)Math.Sin(t * 4 + _time + i) * (60 + i * 15) * (1 - t) + t * (cx - cx);
                var y = cy + (float)Math.Cos(t * 3 + i) * 40 * (1 - t);
                // converge toward focal point
                var fx = cx;
                var fy = cy;
                x = x * (1 - t) + fx * t;
                y = y * (1 - t) + fy * t;
                pts[j] = new PointF(x, y);
            }
            path.AddCurve(pts, 0.5f);
            var color = i % 2 == 0 ? Color.FromArgb(180, 220, 60, 180) : Color.FromArgb(180, 60, 160, 255);
            using var pen = new Pen(color, 2.5f);
            graphics.DrawPath(pen, path);
        }

        // focal point
        using var brush = new SolidBrush(Color.FromArgb(220, 255, 255, 255));
        graphics.FillEllipse(brush, cx - 6, cy - 6, 12, 12);
    }
}