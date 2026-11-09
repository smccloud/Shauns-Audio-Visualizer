using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class CyanTealRipplesEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Cyan teal ripples";

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

        for (var i = 0; i < 12; i++)
        {
            var r = (maxR * 0.05f + i * maxR * 0.08f + _time * 40) % maxR;
            var alpha = (int)(220 * (1f - r / maxR));
            using var pen = new Pen(Color.FromArgb(alpha, 0, 200, 220), 2f);
            graphics.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
        }

        // hollow center
        var cr = maxR * 0.12f;
        using var holeBrush = new SolidBrush(Color.Black);
        graphics.FillEllipse(holeBrush, cx - cr, cy - cr, cr * 2, cr * 2);
    }
}