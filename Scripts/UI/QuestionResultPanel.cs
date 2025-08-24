using Godot;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class QuestionResultPanel : PanelContainer {
    [Export]
    public required Label QuestionTitleLabel { get; set; }

    [Export]
    public required Label QuestionResultLabel { get; set; }

    [Export]
    public required Button CollapseButton { get; set; }

    [Export]
    public required VBoxContainer Contents { get; set; }

    [Export]
    public required Label QuestionPromptLabel { get; set; }

    [Export]
    public required PackedScene AnswerPackedScene { get; set; }

    [Export]
    public required VBoxContainer CorrectAnswersVBoxContainer { get; set; }

    [Export]
    public required VBoxContainer IncorrectAnswersVBoxContainer { get; set; }

    [Export]
    public required VBoxContainer CorrectAnswersForQuestionVBoxContainer { get; set; }

    [Export]
    public required VBoxContainer UserSubmittedAnswersVBoxContainer { get; set; }

    [Export]
    public required Label UserSubmittedAnswerLabel { get; set; }

    private QuestionResult? question;

    public void SetQuestionResult(QuestionResult questionResult) {
        question = questionResult;
    }

    public override void _Ready() {
        CollapseButton.Pressed += OnCollapseButtonPressedEvent;

        if (question == null) {
            return;
        }

        QuestionTitleLabel.Text = question.Question.Title;
        QuestionPromptLabel.Text = question.Question.Prompt.ReplaceNames(question.Question.NamesUsed);

        if (PracticeRunManager.Instance.RunData.OpenQuestions) {
            QuestionResultLabel.Text = string.Empty;
            CorrectAnswersVBoxContainer.Visible = false;
            IncorrectAnswersVBoxContainer.Visible = false;
            UserSubmittedAnswerLabel.Visible = true;
            UserSubmittedAnswerLabel.Text = question.SubmittedAnswer;
        } else {
            if (question.IsCorrect) {
                QuestionResultLabel.Text = "Answered correctly";
                CorrectAnswersVBoxContainer.Visible = true;
                IncorrectAnswersVBoxContainer.Visible = false;
                UserSubmittedAnswerLabel.Visible = false;

                if (question.Answers == null || question.Answers.Length == 0) {
                    return;
                }

                foreach (var answer in question.Answers) {
                    AnswerPanel answerPanel = AnswerPackedScene.Instantiate<AnswerPanel>();
                    answerPanel.SetAnswer(answer, question.Question.NamesUsed);
                    CorrectAnswersVBoxContainer.AddChild(answerPanel);
                }
            } else {
                QuestionResultLabel.Text = "Answered incorrectly";
                CorrectAnswersVBoxContainer.Visible = false;
                IncorrectAnswersVBoxContainer.Visible = true;
                UserSubmittedAnswerLabel.Visible = false;

                if (question.Answers == null || question.Answers.Length == 0) {
                    return;
                }

                foreach(var answer in question.Question.Answers) {
                    if (!answer.IsCorrect) {
                        continue;
                    }

                    AnswerPanel answerPanel = AnswerPackedScene.Instantiate<AnswerPanel>();
                    answerPanel.SetAnswer(answer, question.Question.NamesUsed);
                    CorrectAnswersForQuestionVBoxContainer.AddChild(answerPanel);
                }

                foreach (var answer in question.Answers) {
                    AnswerPanel answerPanel = AnswerPackedScene.Instantiate<AnswerPanel>();
                    answerPanel.SetAnswer(answer, question.Question.NamesUsed);
                    UserSubmittedAnswersVBoxContainer.AddChild(answerPanel);
                }
            }
        }
    }

    private void OnCollapseButtonPressedEvent() {
        Contents.Visible = !Contents.Visible;
    }
}
