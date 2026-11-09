using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class CrimsonVortexLightningEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Crimson vortex lightning";

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
        var maxR = Math.Min(bounds.Width, bounds.Height) / 2f - 4;

        // vortex
        for (var i = 0; i < 12; i++)
        {
            var r = maxR * (0.1f + i * 0.07f) * (1f + 0.1f * (float)Math.Sin(_time * 2 + i));
            using var pen = new Pen(Color.FromArgb(60, 180, 20, 40), 2f);
            graphics.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
        }

        // lightning discharges
        var rng = new Random(42);
        for (var i = 0; i < 8; i++)
        {
            var angle = i * (float)(2 * Math.PI / 8) + _time;
            var x = cx;
            var y = cy;
            using var pen = new Pen(Color.FromArgb(200, 255, 100, 200), 1.5f);
            for (var s = 0; s < 6; s++)
            {
                var nx = x + (float)(Math.Cos(angle) * 20 + (rng.NextDouble() - 0.5) * 30);
                var ny = y + (float)(Math.Sin(angle) * 20 + (rng.NextDouble() - 0.5) * 30);
                graphics.DrawLine(pen, x, y, nx, ny);
                x = nx; y = ny;
                angle += 0.3f;
            }
        }

        // core
        var cr = 10 + _level * 20;
        using var coreBrush = new SolidBrush(Color.FromArgb(200, 220, 20, 60));
        graphics.FillEllipse(coreBrush, cx - cr, cy - cr, cr * 2, cr * 2);
    }
}