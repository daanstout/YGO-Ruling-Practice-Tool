using System;
using System.Collections.Generic;
using System.Linq;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information regarding a question that can be asked of the user.
/// </summary>
public class Question : IEquatable<Question?> {
    /// <summary>
    /// The identifier of the question.
    /// </summary>
    public required string QuestionIdentifier { get; init; }

    /// <summary>
    /// The title of the question.
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// The codes of the cards used in this question.
    /// </summary>
    public required uint[] CardsUsed { get; init; }

    /// <summary>
    /// Tags relevant to the question.
    /// </summary>
    public required string[] Tags { get; init; }

    /// <summary>
    /// The difficulty of the question.
    /// </summary>
    public required Difficulties Difficulty { get; init; } = Difficulties.None;

    /// <summary>
    /// The question the user needs to answer.
    /// </summary>
    public required string Prompt { get; init; }

    /// <summary>
    /// The possible answers the user can choose from.
    /// </summary>
    public required Answer[] Answers { get; init; }

    /// <summary>
    /// The group or person who added the question.
    /// </summary>
    public required string AddedBy { get; init; }

    /// <summary>
    /// The group or person who initially created the question.
    /// </summary>
    public required string Author { get; init; }

    /// <summary>
    /// Where the question was sourced from.
    /// </summary>
    public required string Source { get; init; }

    /// <summary>
    /// The names used with this question.
    /// </summary>
    public List<string> NamesUsed { get; init; } = [];

    /// <summary>
    /// Whether the question contains multiple correct answers or not.
    /// </summary>
    public bool IsMultipleChoice => Answers.Count(answer => answer.IsCorrect) > 1;

    public override bool Equals(object? obj) => Equals(obj as Question);
    public bool Equals(Question? other) => other is not null && QuestionIdentifier == other.QuestionIdentifier;
    public override int GetHashCode() => HashCode.Combine(QuestionIdentifier);

    public static bool operator ==(Question? left, Question? right) => EqualityComparer<Question>.Default.Equals(left, right);
    public static bool operator !=(Question? left, Question? right) => !(left == right);
}