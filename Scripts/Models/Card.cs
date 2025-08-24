using Godot;
using System;
using System.ComponentModel.DataAnnotations;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information regarding a card.
/// </summary>
public class Card
{
    /// <summary>
    /// The name of the card.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The type of card it is.
    /// </summary>
    public required CardTypes CardType { get; init; }

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's attribute.
    /// </summary>
    public Attributes Attribute { get; init; } = Attributes.None;

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's type.
    /// </summary>
    public Types Type { get; init; } = Types.None;

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's level.
    /// </summary>
    [Range(0, 12)]
    public byte Level { get; init; } = 0;

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's attack.
    /// </summary>
    public ushort Attack { get; init; } = 0;

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's defense.
    /// </summary>
    public ushort Defense { get; init; } = 0;

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Monster"/>, represents the monster's sub types.
    /// </summary>
    public SubTypes[] SubTypes { get; init; } = [];

    /// <summary>
    /// If <see cref="CardType"/> is <see cref="CardTypes.Spell"/> or <see cref="CardTypes.Trap"/>, represents the card's subtype.
    /// </summary>
    public SpellTrapTypes SpellTrapType { get; init; } = SpellTrapTypes.None;

    /// <summary>
    /// Contains the effects on the card.
    /// </summary>
    /// <remarks>
    /// Each effect on a card is an entry in the array.
    /// </remarks>
    public required CardEffect[] Effects { get; init; }

    /// <summary>
    /// The unique code of the card.
    /// </summary>
    public required uint Code { get; init; }
}
