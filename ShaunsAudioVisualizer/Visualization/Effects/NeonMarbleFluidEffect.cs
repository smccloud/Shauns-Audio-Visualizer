using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class NeonMarbleFluidEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Neon marble fluid";

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

        // flowing ribbons
        for (var i = 0; i < 6; i++)
        {
            using var path = new GraphicsPath();
            var points = new PointF[40];
            for (var j = 0; j < points.Length; j++)
            {
                var t = j / (float)(points.Length - 1);
                var x = t * bounds.Width;
                var y = cy + (float)Math.Sin(t * 6 + _time + i) * (60 + i * 20) + (float)Math.Cos(t * 3 + i) * 20;
                points[j] = new PointF(x, y);
            }
            path.AddCurve(points, 0.5f);

            var color = i % 3 == 0 ? Color.FromArgb(200, 0, 255, 100)
                      : i % 3 == 1 ? Color.FromArgb(200, 0, 100, 255)
                      : Color.FromArgb(200, 255, 0, 200);
            using var pen = new Pen(color, 3f) { LineJoin = LineJoin.Round };
            graphics.DrawPath(pen, path);
        }
    }
}