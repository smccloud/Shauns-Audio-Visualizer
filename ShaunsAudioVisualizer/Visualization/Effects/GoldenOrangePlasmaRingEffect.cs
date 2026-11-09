using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class GoldenOrangePlasmaRingEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Golden orange plasma ring";

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

        // plasma embers
        var rng = new Random(99);
        for (var i = 0; i < 40; i++)
        {
            var angle = (float)(rng.NextDouble() * 2 * Math.PI);
            var r = maxR * (0.5f + (float)rng.NextDouble() * 0.5f);
            var x = cx + (float)Math.Cos(angle) * r;
            var y = cy + (float)Math.Sin(angle) * r;
            var size = 2 + (float)rng.NextDouble() * 4;
            var color = i % 2 == 0 ? Color.FromArgb(180, 0, 120, 255) : Color.FromArgb(180, 255, 160, 40);
            using var brush = new SolidBrush(color);
            graphics.FillEllipse(brush, x - size, y - size, size * 2, size * 2);
        }

        // golden-orange ring
        var ringR = maxR * 0.45f;
        using var pen = new Pen(Color.FromArgb(230, 255, 140, 20), 4f);
        graphics.DrawEllipse(pen, cx - ringR, cy - ringR, ringR * 2, ringR * 2);

        // inner glow
        var innerR = ringR * 0.7f;
        using var glowBrush = new SolidBrush(Color.FromArgb(80, 255, 180, 60));
        graphics.FillEllipse(glowBrush, cx - innerR, cy - innerR, innerR * 2, innerR * 2);
    }
}