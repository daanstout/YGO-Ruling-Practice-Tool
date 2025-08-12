using Godot;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class AnswerPanel : PanelContainer {
    [Export]
    public required Label CorrectnessLabel { get; set; }

    [Export]
    public required Label PromptLabel { get; set; }

    [Export]
    public required Label ReasoningLabel { get; set; }

    private Answer? answer;

    public void SetAnswer(Answer answer) {
        this.answer = answer;
    }

    public override void _Ready() {
        if (answer == null) {
            return;
        }

        CorrectnessLabel.Visible = !answer.IsCorrect;

        CorrectnessLabel.Text = answer.IsCorrect ? "Correct" : "Incorrect";
        PromptLabel.Text = answer.Prompt;
        ReasoningLabel.Text = answer.Reasoning;
    }
}
