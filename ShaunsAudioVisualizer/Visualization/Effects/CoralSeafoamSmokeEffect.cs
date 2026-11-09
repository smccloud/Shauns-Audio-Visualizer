using System.Drawing;
using System.Drawing.Drawing2D;

namespace ShaunsAudioVisualizer.Visualization.Effects;

public sealed class CoralSeafoamSmokeEffect : IVisualizationEffect
{
    public int PaletteIndex { get; set; }
    public string Name => "Coral seafoam smoke";

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

        var rng = new Random(7);
        for (var i = 0; i < 30; i++)
        {
            var x = (float)(rng.NextDouble() * bounds.Width);
            var y = (float)(rng.NextDouble() * bounds.Height);
            var r = 40 + (float)rng.NextDouble() * 80;
            var color = i % 2 == 0
                ? Color.FromArgb(60, 255, 120, 120)
                : Color.FromArgb(60, 100, 220, 180);
            using var brush = new SolidBrush(color);
            graphics.FillEllipse(brush, x - r, y - r, r * 2, r * 2);
        }
    }
}