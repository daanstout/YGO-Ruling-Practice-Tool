using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace YGORulingPracticeTool.Scripts.Models;

/// <summary>
/// Contains information about the current run.
/// </summary>
public class RunData {
    /// <summary>
    /// The questions that have been asked this run together with the user's submitted answers.
    /// </summary>
    public List<QuestionResult> Results { get; } = [];

    /// <summary>
    /// If <see langword="true"/>, the user needs to write their own answer. If <see langword="false"/>, the user can choose from provided answers.
    /// </summary>
    public required bool OpenQuestions { get; init; }

    /// <summary>
    /// The tag used to filter the questions by.
    /// </summary>
    public required string? QuestionTag { get; init; }

    /// <summary>
    /// How many questions are asked of the user for this run.
    /// </summary>
    public required int QuestionToAsk { get; init; }

    /// <summary>
    /// Whether or not the data has been exported or not.
    /// </summary>
    [JsonIgnore]
    public bool HasBeenExported { get; set; } = false;

    /// <summary>
    /// Whether or not the run is currently live.
    /// </summary>
    [JsonIgnore]
    public bool IsRunLive { get; set; } = false;

    /// <summary>
    /// How many questions have been asked so far.
    /// </summary>
    public int QuestionsAsked => Results.Count;

    /// <summary>
    /// How many questions have been answered correctly.
    /// </summary>
    public int QuestionsCorrect => Results.Count(result => result.IsCorrect);

    /// <summary>
    /// The score of the user this run.
    /// </summary>
    public float Score => (float)QuestionsCorrect / QuestionsAsked * 100;
}
