using Godot;
using System.Collections.Generic;
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
    private List<string>? names;

    public void SetAnswer(Answer answer, List<string> names) {
        this.answer = answer;
        this.names = names;
    }

    public override void _Ready() {
        if (answer == null || names == null) {
            return;
        }

        CorrectnessLabel.Visible = !answer.IsCorrect;

        CorrectnessLabel.Text = answer.IsCorrect ? "Correct" : "Incorrect";
        PromptLabel.Text = answer.Prompt.ReplaceNames(names);
        ReasoningLabel.Text = answer.Reasoning.ReplaceNames(names);
    }
}
