using Godot;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class SpellTrapCardPanel : PanelContainer {
    [Export]
    public required Label CardNameLabel { get; set; }

    [Export]
    public required Label SubTypeLabel { get; set; }

    [Export]
    public required Label TypeLabel { get; set; }

    [Export]
    public required PackedScene EffectScene { get; set; }

    [Export]
    public required Node EffectParent { get; set; }

    private Card? card;

    public void SetCard(Card card) {
        this.card = card;
    }

    public override void _Ready() {
        if (card == null) {
            GD.Print("No card has been provided!");
            return;
        }

        CardNameLabel.Text = card.Name;
        SubTypeLabel.Text = card.SpellTrapType.ToString();
        TypeLabel.Text = card.CardType.ToString();

        foreach (var effect in card.Effects) {
            var effectScene = EffectScene.Instantiate<EffectPanel>();
            effectScene.SetCardEffect(effect);
            EffectParent.AddChild(effectScene);
        }
    }
}
