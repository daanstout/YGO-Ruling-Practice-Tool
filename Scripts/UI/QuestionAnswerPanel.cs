using Godot;
using System.Collections.Generic;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class QuestionAnswerPanel : PanelContainer {
    [Export]
    public required CheckBox AnswerCheckBox { get; set; }

    [Export]
    public required Label AnswerLabel { get; set; }

    public bool IsPressed => AnswerCheckBox.ButtonPressed;

    public Answer? Answer { get; private set; }

    private List<string>? names;

    public void SetAnswer(Answer answer, List<string> names, ButtonGroup? buttonGroup) {
        Answer = answer;
        this.names = names;
        AnswerCheckBox.ButtonGroup = buttonGroup;
    }

    public override void _Ready() {
        if (Answer == null || names == null) {
            GD.Print("Answer has not been provided!");
            return;
        }

        AnswerLabel.Text = Answer.Prompt.ReplaceNames(names);
    }

    public void SetButtonEnabled(bool enable) {
        AnswerCheckBox.Disabled = !enable;
    }
}
