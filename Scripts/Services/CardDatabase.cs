using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.Services;

/// <summary>
/// Contains information about the cards used in the questions.
/// </summary>
public class CardDatabase {
    /// <summary>
    /// A static instance of the card database.
    /// </summary>
    public static CardDatabase Instance { get; } = new CardDatabase();

    private readonly Dictionary<uint, Card> cardsByCode = [];
    private readonly Dictionary<string, Card> cardsByName = [];

    /// <summary>
    /// Loads a collection of cards into the database.
    /// </summary>
    /// <param name="cards">The cards to load.</param>
    public void LoadCards(IEnumerable<Card> cards) {
        StringBuilder builder = new StringBuilder();
        foreach (var card in cards) {
            if (card.Code == 0) {
                GD.Print("Card with no code found. Cannot be loaded!");
                continue;
            }

            if (cardsByCode.ContainsKey(card.Code)) {
                continue;
            }

            bool valid = ValidateCard(card, builder);

            if (!valid) {
                builder.AppendLine();
                continue;
            }

            GD.Print($"Loading card: {card.Name}");
            cardsByCode[card.Code] = card;
            cardsByName[card.Name] = card;
        }
        GD.Print(builder.ToString());
    }

    /// <summary>
    /// Get a card by its code.
    /// </summary>
    /// <param name="code">The code of the card to get.</param>
    /// <returns>The requested card.</returns>
    public Card GetCard(uint code) => cardsByCode[code];

    /// <summary>
    /// Get a card by its name.
    /// </summary>
    /// <param name="name">The name of the card to get.</param>
    /// <returns>The requested card.</returns>
    public Card GetCard(string name) => cardsByName[name];

    /// <summary>
    /// Get a collection of cards by their codes.
    /// </summary>
    /// <param name="codes">The codes of the cards to get.</param>
    /// <returns>The requested cards.</returns>
    public IEnumerable<Card> GetCards(IEnumerable<uint> codes) => cardsByCode.Where(kvp => codes.Contains(kvp.Key)).Select(kvp => kvp.Value);

    /// <summary>
    /// Gets all the cards in the database.
    /// </summary>
    /// <returns>The cards in the database.</returns>
    public IEnumerable<Card> GetCards() => cardsByCode.Values;

    private static bool ValidateCard(Card card, StringBuilder builder) {
        bool isValid = true;

        Validate(string.IsNullOrWhiteSpace, card.Name, ref isValid, builder, "The card's name is empty", card.Code);
        Validate(type => type == CardTypes.None, card.CardType, ref isValid, builder, "The card's card type is not valid", card.Code);
        if (card.CardType == CardTypes.Monster) {
            Validate(attribute => attribute == Attributes.None, card.Attribute, ref isValid, builder, "The monster's attribute is not valid", card.Code);
            Validate(type => type == Types.None, card.Type, ref isValid, builder, "The monster's type is not valid", card.Code);
            Validate(level => level < 1 || level > 12, card.Level, ref isValid, builder, "The monster's level is not valid", card.Code);
            Validate(subType => subType.Length == 0, card.SubTypes, ref isValid, builder, "The monster's sub typing is not valid", card.Code);
        } else if (card.CardType == CardTypes.Spell || card.CardType == CardTypes.Trap) {
            Validate(spellTrapType => spellTrapType == SpellTrapTypes.None, card.SpellTrapType, ref isValid, builder, "The card's spell or trap type is not valid", card.Code);
        }

        ValidateCardEffect(card.Effects, ref isValid, builder, card.Code);

        return isValid;
    }

    private static void Validate<T>(Func<T, bool> predicate, T value, ref bool isValid, StringBuilder builder, string error, uint code) {
        if (predicate(value)) {
            if (isValid) {
                builder.Append("Found errors in card: ");
                builder.AppendLine(code.ToString());
            }

            builder.Append('\t');
            builder.AppendLine(error);
            builder.Append(". Encountered value: ");
            builder.AppendLine(value?.ToString());
            isValid = false;
        }
    }

    private static void ValidateCardEffect(IEnumerable<CardEffect> cardEffects, ref bool isValid, StringBuilder builder, uint code) {
        int index = 0;

        foreach (var effect in cardEffects) {
            Validate(string.IsNullOrWhiteSpace, effect.Effect, ref isValid, builder, $"\tEffect at index {index} has an invalid effect", code);
            index++;
        }
    }
}
