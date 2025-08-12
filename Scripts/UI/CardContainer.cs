using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;
public partial class CardContainer : VBoxContainer {
    [Export]
    public required PackedScene MonsterCardScene { get; set; }

    [Export]
    public required PackedScene SpellTrapCardScene { get; set; }

    public void LoadCards(IEnumerable<uint> cards) {
        ClearCards();

        foreach (var cardCode in cards) {
            var card = CardDatabase.Instance.GetCard(cardCode);

            if (card.CardType == CardTypes.Monster) {
                var panel = MonsterCardScene.Instantiate<MonsterCardPanel>();
                panel.SetCard(card);
                AddChild(panel);
            } else if (card.CardType == CardTypes.Spell || card.CardType == CardTypes.Trap) {
                var panel = SpellTrapCardScene.Instantiate<SpellTrapCardPanel>();
                panel.SetCard(card);
                AddChild(panel);
            } else {
                GD.Print($"Unknown card type: {card.CardType} for card: {card.Name}");
            }
        }
    }

    public void ClearCards() {
        foreach (var child in GetChildren()) {
            child.QueueFree();
        }
    }
}
