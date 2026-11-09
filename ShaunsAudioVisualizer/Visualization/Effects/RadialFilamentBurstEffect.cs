using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class RadialFilamentBurstEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Radial filament burst";

    private float[] _bands = Array.Empty<float>();
    private float _level;
    private float _time;

    public void Reset()
    {
        _bands = Array.Empty<float>();
        _level = 0f;
        _time = 0f;
    }

    public void Update(VisualizationFrame frame)
    {
        _bands = frame.LeftBands.ToArray();
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
        var count = _bands.Length == 0 ? 1 : _bands.Length;

        using var pen = new Pen(Color.White, 1.5f);
        for (var i = 0; i < count; i++)
        {
            var level = _bands.Length > 0 ? _bands[i] : 0.3f;
            var angle = i * (float)(2 * Math.PI / count) + _time * 0.3f;
            var len = (0.2f + level) * maxR;
            var x2 = cx + (float)Math.Cos(angle) * len;
            var y2 = cy + (float)Math.Sin(angle) * len;

            var t = i / (float)count;
            var color = t < 0.33f ? Color.FromArgb(255, 80, 0)
                      : t < 0.66f ? Color.FromArgb(255, 200, 0)
                      : Color.White;
            pen.Color = Color.FromArgb((int)(120 + 135 * level), color);
            graphics.DrawLine(pen, cx, cy, x2, y2);

            // filament tendrils
            for (var j = 1; j <= 3; j++)
            {
                var a2 = angle + (j - 1) * 0.15f - 0.15f;
                var r2 = len * (0.4f + 0.2f * j);
                var tx = cx + (float)Math.Cos(a2) * r2;
                var ty = cy + (float)Math.Sin(a2) * r2;
                using var p2 = new Pen(Color.FromArgb(80, color), 1f);
                graphics.DrawLine(p2, cx + (float)Math.Cos(angle) * (len * 0.3f),
                    cy + (float)Math.Sin(angle) * (len * 0.3f), tx, ty);
            }
        }
    }
}