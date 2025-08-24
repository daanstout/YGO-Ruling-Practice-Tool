using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;
public partial class QuestionPanel : Panel {
    [Export]
    public required Label QuestionTitleLabel { get; set; }

    [Export]
    public required Label QuestionPromptLabel { get; set; }

    [Export]
    public required PackedScene QuestionAnswerPanelScene { get; set; }

    [Export]
    public required VBoxContainer QuestionAnswerContainer { get; set; }

    [Export]
    public required Label ReasonLabel { get; set; }

    [Export]
    public required TextEdit TextInput { get; set; }

    [Export]
    public required Button SubmitButton { get; set; }

    private readonly List<QuestionAnswerPanel> answerPanels = [];
    private Question? question;
    private bool showingReason = false;

    public void UpdateQuestion(Question question) {
        this.question = question;

        PopulateUI();
    }

    public override void _EnterTree() {
        PracticeRunManager.Instance.RunStartedEvent += OnRunStartedEvent;
    }

    public override void _Ready() {
        SubmitButton.Pressed += OnSubmitEvent;
    }

    private void PopulateUI() {
        if (question == null) {
            GD.Print("Question has not been provided");
            return;
        }

        foreach (var child in QuestionAnswerContainer.GetChildren()) {
            child.QueueFree();
        }
        answerPanels.Clear();

        QuestionTitleLabel.Text = question.Title;
        QuestionPromptLabel.Text = question.Prompt.ReplaceNames(question.NamesUsed);

        ButtonGroup? buttonGroup = question.IsMultipleChoice switch {
            true => null,
            false => new ButtonGroup()
        };

        if (PracticeRunManager.Instance.RunData.OpenQuestions) {
            TextInput.Text = string.Empty;
        } else {
            List<int> indices = [.. Enumerable.Range(0, question.Answers.Length)];

            List<int> questionOrder = [];

            do {
                int index = PracticeRunManager.Instance.Random.Next(0, indices.Count);
                int questionIndex = indices[index];
                indices.RemoveAt(index);
                questionOrder.Add(questionIndex);
            } while (indices.Count > 0);

            for (int i = 0; i < questionOrder.Count; i++) {
                var questionAnswerScene = QuestionAnswerPanelScene.Instantiate<QuestionAnswerPanel>();
                answerPanels.Add(questionAnswerScene);
                questionAnswerScene.SetAnswer(question.Answers[questionOrder[i]], question.NamesUsed, buttonGroup);
                QuestionAnswerContainer.AddChild(questionAnswerScene);
            }
        }
    }

    private void OnSubmitEvent() {
        if (question == null) {
            return;
        }

        if (showingReason) {
            foreach (var questionPanel in answerPanels) {
                questionPanel.SetButtonEnabled(true);
            }

            ReasonLabel.Text = string.Empty;
            SubmitButton.Text = "Submit";
            PracticeRunManager.Instance.ContinueRun();
            showingReason = false;
        } else {
            foreach (var questionPanel in answerPanels) {
                questionPanel.SetButtonEnabled(false);
            }

            Answer[] submittedAnswers = [.. answerPanels.Where(panel => panel.IsPressed && panel.Answer != null).Select(panel => panel.Answer!)];

            var result = PracticeRunManager.Instance.SubmitAnswer(submittedAnswers, TextInput.Text);

            if (PracticeRunManager.Instance.RunData.OpenQuestions) {
                PracticeRunManager.Instance.ContinueRun();
                return;
            }

            StringBuilder builder = new StringBuilder();

            builder.AppendLine(result!.IsCorrect ? "Correct" : "Wrong");

            if (submittedAnswers.Length == 1) {
                builder.AppendLine(submittedAnswers.First().Reasoning);
            } else {
                foreach (var answer in submittedAnswers) {
                    builder.Append("(1): ");
                    builder.AppendLine(answer.Reasoning);
                }
            }

            ReasonLabel.Text = builder.ToString();

            SubmitButton.Text = "Next Question";
            showingReason = true;
        }
    }

    private void OnRunStartedEvent() {
        if (PracticeRunManager.Instance.RunData.OpenQuestions) {
            QuestionAnswerContainer.Visible = false;
            ReasonLabel.Visible = false;
            TextInput.Visible = true;
        } else {
            QuestionAnswerContainer.Visible = true;
            ReasonLabel.Visible = true;
            TextInput.Visible = false;
        }
    }
}
