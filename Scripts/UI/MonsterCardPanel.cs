using Godot;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class MonsterCardPanel : PanelContainer {
    [Export]
    public required Label CardNameLabel { get; set; }

    [Export]
    public required Label CardLevelLabel { get; set; }

    [Export]
    public required Label AttributeLabel { get; set; }

    [Export]
    public required Label TypeLabel { get; set; }

    [Export]
    public required Label ModifierLabel { get; set; }

    [Export]
    public required Label AttackLabel { get; set; }

    [Export]
    public required Label DefenseLabel { get; set; }

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
        CardLevelLabel.Text = card.Level.ToString();
        AttributeLabel.Text = card.Attribute.ToString();
        TypeLabel.Text = card.Type.ToString();
        ModifierLabel.Text = string.Join(" / ", card.SubTypes);
        AttackLabel.Text = card.Attack.ToString();
        DefenseLabel.Text = card.Defense.ToString();

        foreach (var effect in card.Effects) {
            var effectScene = EffectScene.Instantiate<EffectPanel>();
            effectScene.SetCardEffect(effect);
            EffectParent.AddChild(effectScene);
        }
    }
}
