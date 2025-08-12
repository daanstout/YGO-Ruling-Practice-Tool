using Godot;
using System.Linq;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class ResultsPanel : Panel {
    [Export]
    public required VBoxContainer QuestionContainer { get; set; }

    [Export]
    public required PackedScene QuestionResultScene { get; set; }

    [Export]
    public required Label CorrectQuestionsLabel { get; set; }

    [Export]
    public required Label TotalQuestionsLabel { get; set; }

    [Export]
    public required Label ScoreLabel { get; set; }

    public override void _EnterTree() {
        PracticeRunManager.Instance.RunStartedEvent += OnRunStartedEvent;
        PracticeRunManager.Instance.RunEndedEvent += OnRunEndedEvent;
    }

    private void OnRunStartedEvent() {
        foreach(var child in QuestionContainer.GetChildren()) {
            child.QueueFree();
        }
    }

    private void OnRunEndedEvent(RunData runData) {
        int correctQuestions = runData.Results.Count(result => result.IsCorrect);
        CorrectQuestionsLabel.Text = runData.QuestionsCorrect.ToString();
        TotalQuestionsLabel.Text = runData.QuestionsAsked.ToString();
        ScoreLabel.Text = runData.Score.ToString();

        foreach(var question in runData.Results) {
            QuestionResultPanel questionResultPanel = QuestionResultScene.Instantiate<QuestionResultPanel>();
            questionResultPanel.SetQuestionResult(question);
            QuestionContainer.AddChild(questionResultPanel);
        }
    }
}