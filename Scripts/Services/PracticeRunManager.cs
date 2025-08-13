using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.Services;

/// <summary>
/// Manages the current run being done.
/// </summary>
public class PracticeRunManager {
    private static readonly JsonSerializerSettings SETTINGS = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    /// <summary>
    /// A static instance of the manager.
    /// </summary>
    public static PracticeRunManager Instance { get; } = new PracticeRunManager();

    /// <summary>
    /// An event that signals a run being started.
    /// </summary>
    public event Action? RunStartedEvent;

    /// <summary>
    /// An event that signals the user submitted an answer.
    /// <para>The boolean parameter contains whether the user answered correctly or not.</para>
    /// </summary>
    public event Action<bool>? AnswerSubmittedEvent;

    /// <summary>
    /// An event that signals the user wants to continue the run to the next question.
    /// </summary>
    public event Action? ContinueRunEvent;

    /// <summary>
    /// An event that is fired when a question 
    /// </summary>
    public event Action<Question>? QuestionSelectedEvent;

    /// <summary>
    /// An event that signals the run has ended.
    /// </summary>
    public event Action<RunData>? RunEndedEvent;

    /// <summary>
    /// Data about the current run.
    /// </summary>
    public RunData RunData { get; private set; } = null!;

    /// <summary>
    /// An instance of the <see cref="Random"/> class for this run to randomize various parts.
    /// </summary>
    public Random Random { get; private set; } = null!;

    private Question? currentQuestion;

    /// <summary>
    /// Start a run with the provided parameters.
    /// </summary>
    /// <param name="tag">The tag to filter the questions by.</param>
    /// <param name="questionsToAsk">How many questions to ask this run.</param>
    /// <param name="openQuestions">Whether the user needs to provide their own answers or can choose from provided answers.</param>
    public void StartRun(string? tag = null, int questionsToAsk = 20, bool openQuestions = false) {
        if (RunData != null && !RunData.HasBeenExported) {
            ExportRun();
        }

        RunData = new RunData {
            OpenQuestions = openQuestions,
            QuestionToAsk = questionsToAsk,
            QuestionTag = tag,
            IsRunLive = true
        };

        Random = new Random();

        RunStartedEvent?.Invoke();
    }

    /// <summary>
    /// Finish the current run.
    /// </summary>
    public void FinishRun() {
        RunData.IsRunLive = false;
        RunEndedEvent?.Invoke(RunData);
    }

    /// <summary>
    /// Continue the run to the next question.
    /// </summary>
    public void ContinueRun() {
        if (RunData.Results.Count >= RunData.QuestionToAsk) {
            FinishRun();
            return;
        }

        ContinueRunEvent?.Invoke();
    }

    /// <summary>
    /// Get a new question to ask of the user.
    /// <para>This function returns null if all valid questions have been asked.</para>
    /// </summary>
    /// <returns>The next question to ask of the user.</returns>
    public Question? GetNextQuestion() {
        var questions = QuestionDatabase.Instance.GetQuestions(RunData.QuestionTag).ToArray();

        if (RunData.Results.Count == questions.Length) {
            return null;
        } else {
            questions = [.. questions.Where(question => RunData.Results.Count == 0 || !RunData.Results.Any(result => result.Question == question))];
        }

        currentQuestion = questions[Random.Next(0, questions.Length)];

        QuestionSelectedEvent?.Invoke(currentQuestion);

        return currentQuestion;
    }

    /// <summary>
    /// Submit an answer to the current question.
    /// </summary>
    /// <param name="answers">The answers the user choose from.</param>
    /// <param name="submittedAnswer">The answer the user submitted.</param>
    /// <returns>The result of this question.</returns>
    public QuestionResult? SubmitAnswer(Answer[]? answers, string? submittedAnswer) {
        if (currentQuestion == null) {
            return null;
        }

        var result = new QuestionResult {
            Question = currentQuestion,
            Answers = answers,
            SubmittedAnswer = submittedAnswer
        };

        RunData.Results.Add(result);

        AnswerSubmittedEvent?.Invoke(result.IsCorrect);

        return result;
    }

    public void ExportRun(string? path = null) {
        if (RunData.IsRunLive) {
            return;
        }

        if (string.IsNullOrWhiteSpace(path)) {
            if (RunData.HasBeenExported) {
                return;
            }
            path = Path.Combine("History", $"{DateTime.Now.ToString().Replace('-', '_').Replace(':', '_')}.json");
            Directory.CreateDirectory("History");
        }

        string json = JsonConvert.SerializeObject(RunData, SETTINGS);
        File.WriteAllText(path, json);

        RunData.HasBeenExported = true;
    }
}
