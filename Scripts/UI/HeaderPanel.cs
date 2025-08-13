using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class HeaderPanel : Panel {
    [Export]
    public required OptionButton TagList { get; set; }

    [Export]
    public required CheckButton QuestionAnsweringTypeButton { get; set; }

    [Export]
    public required Button StartButton { get; set; }

    [Export]
    public required Button ExportLastRunButton { get; set; }

    [Export]
    public required Panel OptionsPanel { get; set; }

    [Export]
    public required Panel RunInfoPanel { get; set; }

    [Export]
    public required OptionButton QuestionsToAskList { get; set; }

    public override void _EnterTree() {
        PracticeRunManager.Instance.RunEndedEvent += OnRunEndedEvent;
    }

    public override void _Ready() {
        TagList.AddItem("None");
        foreach (var tag in QuestionDatabase.Instance.GetTags()) {
            TagList.AddItem(tag);
        }
        TagList.Select(0);

        StartButton.Pressed += OnStartButtonClickedEvent;
        ExportLastRunButton.Pressed += OnExportLastRunButtonClicked;
    }

    private void OnStartButtonClickedEvent() {
        string? tag = TagList.GetItemText(TagList.Selected);

        if (tag == "None") {
            tag = null;
        }

        string? questionsToAskString = QuestionsToAskList.GetItemText(QuestionsToAskList.Selected);
        int questionsToAsk = int.MaxValue;

        if (questionsToAskString != "Endless") {
            questionsToAsk = int.Parse(questionsToAskString);
        }

        PracticeRunManager.Instance.StartRun(tag, questionsToAsk, !QuestionAnsweringTypeButton.ButtonPressed);

        OptionsPanel.Visible = false;
        RunInfoPanel.Visible = true;
    }

    private void OnRunEndedEvent(RunData runData) {
        OptionsPanel.Visible = true;
        RunInfoPanel.Visible = false;
        ExportLastRunButton.Visible = true;
        ExportLastRunButton.Disabled = false;
    }

    private void OnExportLastRunButtonClicked() {
        PracticeRunManager.Instance.ExportRun();
        ExportLastRunButton.Disabled = true;
    }
}
