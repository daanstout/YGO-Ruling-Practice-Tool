using System.Text;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information about a card's effect.
/// </summary>
public class CardEffect {
    /// <summary>
    /// The condition that needs to be true to activate this effect.
    /// </summary>
    public string? Condition { get; init; }

    /// <summary>
    /// What needs to be done when the effect is activated.
    /// </summary>
    public string? Activation { get; init; }

    /// <summary>
    /// What happens when the effect resolves.
    /// </summary>
    public required string Effect { get; init; }

    /// <summary>
    /// Allows for adding multiple choices for an effect with bullet points.
    /// </summary>
    public string[]? Bullets { get; init; }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(Condition)) {
            builder.Append(Condition);
            builder.Append(": ");
        }

        if (!string.IsNullOrWhiteSpace(Activation)) {
            builder.Append(Activation);
            builder.Append("; ");
        }

        builder.Append(Effect);

        if (Bullets != null && Bullets.Length > 0) {
            builder.AppendLine();
            foreach (string bullet in Bullets) {
                builder.Append("- ");
                builder.AppendLine(bullet);
            }
        }

        return builder.ToString();
    }
}
