using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGORulingPracticeTool.Scripts.Models;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class QuestionAnswerPanel : PanelContainer {
    [Export]
    public required CheckBox AnswerCheckBox { get; set; }

    [Export]
    public required Label AnswerLabel { get; set; }

    public bool IsPressed => AnswerCheckBox.ButtonPressed;

    public Answer? Answer { get; private set; }

    public void SetAnswer(Answer answer, ButtonGroup? buttonGroup) {
        Answer = answer;
        AnswerCheckBox.ButtonGroup = buttonGroup;
    }

    public override void _Ready() {
        if (Answer == null) {
            GD.Print("Answer has not been provided!");
            return;
        }

        AnswerLabel.Text = Answer.Prompt;
    }

    public void SetButtonEnabled(bool enable) {
        AnswerCheckBox.Disabled = !enable;
    }
}
