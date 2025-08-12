using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
