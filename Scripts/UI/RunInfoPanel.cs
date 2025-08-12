using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class RunInfoPanel : Panel {
    [Export]
    public required Label SelectedTagTextLabel { get; set; }

    [Export]
    public required Label CorrectQuestionsLabel { get; set; }

    [Export]
    public required Label TotalQuestionsLabel { get; set; }

    [Export]
    public required Label QuestionsToAskNumberLabel { get; set; }

    [Export]
    public required Button FinishRunEarlyButton { get; set; }

    private int correctQuestions;
    private int totalQuestions;

    public override void _EnterTree() {
        PracticeRunManager.Instance.AnswerSubmittedEvent += OnAnswerSubmittedEvent;
        PracticeRunManager.Instance.RunStartedEvent += OnRunStartedEvent;
    }

    public override void _Ready() {
        FinishRunEarlyButton.Pressed += OnFinishRunEarlyButtonClickedEvent;
    }

    private void OnRunStartedEvent() {
        SelectedTagTextLabel.Text = PracticeRunManager.Instance.RunData.QuestionTag ?? "No Tag Selected";

        correctQuestions = 0;
        totalQuestions = 0;

        CorrectQuestionsLabel.Text = correctQuestions.ToString();
        TotalQuestionsLabel.Text = totalQuestions.ToString();

        QuestionsToAskNumberLabel.Text = PracticeRunManager.Instance.RunData.QuestionToAsk.ToString();
    }

    private void OnAnswerSubmittedEvent(bool isCorrect) {
        correctQuestions += isCorrect ? 1 : 0;
        totalQuestions++;

        CorrectQuestionsLabel.Text = correctQuestions.ToString();
        TotalQuestionsLabel.Text = totalQuestions.ToString();
    }

    private void OnFinishRunEarlyButtonClickedEvent() {
        PracticeRunManager.Instance.FinishRun();
    }
}
