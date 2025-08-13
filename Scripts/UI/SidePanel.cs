using Godot;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class SidePanel : Panel {
    [Export]
    public required VBoxContainer Container { get; set; }

    [Export]
    public required Label AddedByLabel { get; set; }

    [Export]
    public required Label CreatedByLabel { get; set; }

    [Export]
    public required Label SourceLabel { get; set; }

    public override void _Ready() {
        PracticeRunManager.Instance.QuestionSelectedEvent += OnQuestionSelectedEvent;
        PracticeRunManager.Instance.RunEndedEvent += OnRunEndedEvent;
    }

    private void OnQuestionSelectedEvent(Question question) {
        Container.Visible = true;
        AddedByLabel.Text = question.AddedBy;
        CreatedByLabel.Text = question.Author;
        SourceLabel.Text = question.Source;
    }

    private void OnRunEndedEvent(RunData runData) {
        Container.Visible = false;
        AddedByLabel.Text = string.Empty;
        CreatedByLabel.Text = string.Empty;
        SourceLabel.Text = string.Empty;
    }
}
