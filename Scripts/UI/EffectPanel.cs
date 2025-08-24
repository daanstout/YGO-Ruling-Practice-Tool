using Godot;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class EffectPanel : HBoxContainer {
    [Export]
    public required RichTextLabel EffectLabel { get; set; }

    private CardEffect? effect;

    public void SetCardEffect(CardEffect effect) {
        this.effect = effect;
    }

    public override void _Ready() {
        if (effect == null) {
            GD.Print("No effect has been provided!");
            return;
        }

        EffectLabel.Text = effect.ToString();
    }
}
