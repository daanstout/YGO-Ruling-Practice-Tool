using Godot;
using System.Collections.Generic;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.Services;

/// <summary>
/// Contains information about the questions that have been provided.
/// </summary>
public class QuestionDatabase {
    /// <summary>
    /// A static instance of the question database.
    /// </summary>
    public static QuestionDatabase Instance { get; } = new QuestionDatabase();

    private readonly Dictionary<string, Question> questions = [];

    private readonly Dictionary<string, List<Question>> questionsByTag = [];

    /// <summary>
    /// Loads a question into the database.
    /// </summary>
    /// <param name="questions">The collection of questions to load.</param>
    public void LoadQuestions(IEnumerable<Question> questions) {
        foreach (var question in questions) {
            if (this.questions.ContainsKey(question.QuestionIdentifier)) {
                GD.Print($"There already exists a question with the identifier of: {question.QuestionIdentifier}.");
                continue;
            }

            this.questions[question.QuestionIdentifier] = question;

            foreach (var tag in question.Tags) {
                if (!questionsByTag.TryGetValue(tag, out var list)) {
                    list = [];
                    questionsByTag[tag] = list;
                }

                list.Add(question);
            }
        }
    }

    /// <summary>
    /// Gets all questions from the database with the provided tag.
    /// <para>If no tag is provided, all questions will be returned.</para>
    /// </summary>
    /// <param name="tag">The tag to get the questions from.</param>
    /// <returns>The questions that are for the provided tag.</returns>
    public IEnumerable<Question> GetQuestions(string? tag = null) {
        if (string.IsNullOrWhiteSpace(tag)) {
            return questions.Values;
        } else if (questionsByTag.TryGetValue(tag, out var questions)) {
            return questions;
        } else {
            GD.Print($"No questions with the tag {tag} have been registered!");
            return [];
        }
    }

    /// <summary>
    /// Get all tags that are present within the questions.
    /// </summary>
    /// <returns>A collection of all tags present.</returns>
    public IEnumerable<string> GetTags() {
        return questionsByTag.Keys;
    }
}
