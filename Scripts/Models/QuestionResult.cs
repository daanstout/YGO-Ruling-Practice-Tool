using System;
using System.Linq;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information about the user's answer to a question.
/// </summary>
public class QuestionResult {
    /// <summary>
    /// The question that was asked.
    /// </summary>
    public required Question Question { get; init; }

    /// <summary>
    /// The answers the user submitted as correct.
    /// </summary>
    public Answer[]? Answers { get; init; }

    /// <summary>
    /// The answer the user submitted.
    /// </summary>
    public string? SubmittedAnswer { get; init; }

    /// <summary>
    /// Checks whether the user submitted the correct answers.
    /// </summary>
    /// <remarks>
    /// If the question was asked as an open answer, will always return <see langword="true"/>.
    /// </remarks>
    /// <returns>Whether the user answered the question correct or not.</returns>
    public bool IsCorrect {
        get {
            if (Answers == null) {
                return true;
            }

            Answer[] correctAnswers = [.. Question.Answers.Where(answer => answer.IsCorrect)];

            if (correctAnswers.Length != Answers.Length) {
                return false;
            }

            return correctAnswers.All(answer => Answers.Contains(answer));
        }
    }
}
