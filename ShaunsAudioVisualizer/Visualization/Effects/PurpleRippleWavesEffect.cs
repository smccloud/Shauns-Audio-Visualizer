using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class PurpleRippleWavesEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Purple ripple waves";

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

        for (var i = 0; i < 10; i++)
        {
            var r = (maxR * 0.1f + i * maxR * 0.09f + _time * 30) % maxR;
            var alpha = (int)(200 * (1f - r / maxR));
            using var pen = new Pen(Color.FromArgb(alpha, 120, 60, 200), 2f);
            graphics.DrawEllipse(pen, cx - r, cy - r, r * 2, r * 2);
        }
    }
}