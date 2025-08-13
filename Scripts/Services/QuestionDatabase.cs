using Godot;
using System;
using System.Collections.Generic;
using System.Text;
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
        StringBuilder builder = new StringBuilder();

        foreach (var question in questions) {
            if (string.IsNullOrWhiteSpace(question.QuestionIdentifier)) {
                GD.PrintErr("Question loaded with no identifier!");
                continue;
            }

            if (this.questions.ContainsKey(question.QuestionIdentifier)) {
                GD.Print($"There already exists a question with the identifier of: {question.QuestionIdentifier}.");
                continue;
            }

            bool valid = ValidateQuestion(question, builder);

            if (!valid) {
                builder.AppendLine();
                continue;
            }

            this.questions[question.QuestionIdentifier] = question;

            foreach (var tag in question.Tags) {
                if (string.IsNullOrWhiteSpace(tag)) {
                    continue;
                }

                if (!questionsByTag.TryGetValue(tag, out var list)) {
                    list = [];
                    questionsByTag[tag] = list;
                }

                list.Add(question);
            }
        }

        GD.Print(builder.ToString());
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

    private static bool ValidateQuestion(Question question, StringBuilder builder) {
        bool isValid = true;

        Validate(string.IsNullOrWhiteSpace, question.Title, ref isValid, builder, "The question's title is empty", question.QuestionIdentifier);
        Validate(difficulty => difficulty == Difficulties.None, question.Difficulty, ref isValid, builder, "The question's difficulty is unknown", question.QuestionIdentifier);
        Validate(string.IsNullOrWhiteSpace, question.Prompt, ref isValid, builder, "The question's prompt is empty", question.QuestionIdentifier);
        ValidateQuestionAnswers(question.Answers, ref isValid, builder, question.QuestionIdentifier);
        Validate(string.IsNullOrWhiteSpace, question.AddedBy, ref isValid, builder, "The question's added by field is empty", question.QuestionIdentifier);
        Validate(string.IsNullOrWhiteSpace, question.Author, ref isValid, builder, "The question's author field is empty", question.QuestionIdentifier);
        Validate(string.IsNullOrWhiteSpace, question.Source, ref isValid, builder, "The question's source field is empty", question.QuestionIdentifier);

        return isValid;
    }

    private static void Validate<T>(Func<T, bool> predicate, T value, ref bool isValid, StringBuilder builder, string error, string questionIdentifier) {
        if (predicate(value)) {
            if (isValid) {
                builder.Append("Found errors in card: ");
                builder.AppendLine(questionIdentifier);
            }

            builder.Append('\t');
            builder.AppendLine(error);
            builder.Append(". Encountered value: ");
            builder.AppendLine(value?.ToString());
            isValid = false;
        }
    }

    private static void ValidateQuestionAnswers(IEnumerable<Answer> answers, ref bool isValid, StringBuilder builder, string identifier) {
        int index = 0;

        foreach(var answer in answers) {
            Validate(string.IsNullOrWhiteSpace, answer.AnswerId, ref isValid, builder, $"\tAnswer at index {index} has an invalid identifier", identifier);
            Validate(string.IsNullOrWhiteSpace, answer.Prompt, ref isValid, builder, $"\tAnswer at index {index} has an invalid prompt", identifier);
            Validate(string.IsNullOrWhiteSpace, answer.Reasoning, ref isValid, builder, $"\tAnswer at index {index} has an invalid reasoning", identifier);
        }
    }
}
