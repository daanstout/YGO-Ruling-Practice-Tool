using Godot;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class MainViewPanel : Panel {
    [Export]
    public required QuestionPanel QuestionPanel { get; set; }

    [Export]
    public required ResultsPanel ResultsPanel { get; set; }

    public override void _EnterTree() {
        PracticeRunManager.Instance.RunStartedEvent += OnRunStarted;
    }

    private void OnRunStarted() {
        QuestionPanel.Visible = true;
        ResultsPanel.Visible = false;
    }
}
