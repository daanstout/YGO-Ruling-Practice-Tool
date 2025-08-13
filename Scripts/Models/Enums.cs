using System;
using System.Runtime.Serialization;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// The different card types possible.
/// </summary>
public enum CardTypes {
    None = 0,
    Monster,
    Spell,
    Trap
}

/// <summary>
/// The different monster attributes possible.
/// </summary>
public enum Attributes {
    None = 0,
    Dark,
    Light,
    Water,
    Earth,
    Wind,
    Fire,
    [EnumMember(Value = "Divine-Beast")]
    DivineBest
}

/// <summary>
/// The different monster types possible
/// </summary>
public enum Types {
    None = 0,
    Reptile,
    Spellcaster,
    [EnumMember(Value = "Beast-Warrior")]
    BeastWarrior,
    Zombie,
    Fiend,
    Dinosaur,
    Dragon,
    Beast,
    Illusion,
    Insect,
    [EnumMember(Value = "Winged Beast")]
    WingedBeast,
    Warrior,
    [EnumMember(Value = "Sea Serpent")]
    SeaSerpent,
    Machine,
    Aqua,
    Pyro,
    Thunder,
    Plant,
    Rock,
    Fairy,
    Fish,
    [EnumMember(Value = "Divine-Beast")]
    DivineBeast,
    Psychic,
    [EnumMember(Value = "Creator God")]
    CreatorGod,
    Wyurm,
    Cyberse
}

/// <summary>
/// The different monster sub types possible.
/// </summary>
[Flags]
public enum SubTypes {
    None = 0,
    Normal = 1 << 0,
    Effect = 1 << 1,
    Ritual = 1 << 2,
    Fusion = 1 << 3,
    Synchro = 1 << 4,
    Xyz = 1 << 5,
    Pendulum = 1 << 6,
    Link = 1 << 7,
    Flip = 1 << 8,
    Gemini = 1 << 9,
    Spirit = 1 << 10
}

/// <summary>
/// The different spell and trap types possible.
/// </summary>
public enum SpellTrapTypes {
    None = 0,
    Normal,
    Continuous,
    [EnumMember(Value = "Quick-Play")]
    QuickPlay,
    Ritual,
    Equip,
    Field,
    Counter
}

public enum Difficulties {
    None = 0,
    Basic,
    Intermediate,
    Expert
}
