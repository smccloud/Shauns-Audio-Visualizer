using Compat.Drawing;

namespace ShaunsAudioVisualizer.Visualization.Effects;

/// <summary>
/// A music-reactive visualization effect.
/// </summary>
public interface IVisualizationEffect
{
    /// <summary>Display name shown in the window title.</summary>
    string Name { get; }

    /// <summary>Resets any internal state.</summary>
    void Reset();

    /// <summary>Index of the color palette currently used by the effect.</summary>
    int PaletteIndex { get; set; }

    /// <summary>Advances the effect using the latest audio frame.</summary>
    void Update(VisualizationFrame frame);

    /// <summary>Renders the effect into the supplied graphics surface.</summary>
    void Render(Graphics graphics, Rectangle bounds);
}