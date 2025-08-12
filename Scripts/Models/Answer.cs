namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information regarding a possible answer for a question.
/// </summary>
public class Answer {
    /// <summary>
    /// The id of the answer.
    /// </summary>
    public required string AnswerId { get; init; }
    
    /// <summary>
    /// The answer shown to the user.
    /// </summary>
    public required string Prompt { get; init; }

    /// <summary>
    /// Whether the answer is correct.
    /// </summary>
    public required bool IsCorrect { get; init; }

    /// <summary>
    /// The reasoning behing whether the answer is correct or not.
    /// </summary>
    public required string Reasoning { get; init; }
}