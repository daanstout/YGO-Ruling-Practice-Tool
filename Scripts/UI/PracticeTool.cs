using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using YGORulingPracticeTool.Scripts.Models;
using YGORulingPracticeTool.Scripts.Services;

namespace YGORulingPracticeTool.Scripts.UI;

public partial class PracticeTool : Panel {
    private static readonly JsonSerializer SERIALIZER = new() {
        ContractResolver = new DefaultContractResolver {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };

    [Export]
    public required CardContainer CardContainer { get; set; }

    [Export]
    public required QuestionPanel QuestionPanel { get; set; }

    [Export]
    public required ResultsPanel ResultsPanel { get; set; }

    public override void _EnterTree() {
        PracticeRunManager.Instance.RunStartedEvent += OnRunStartedEvent;
        PracticeRunManager.Instance.ContinueRunEvent += OnNextQuestionEvent;
        PracticeRunManager.Instance.RunEndedEvent += OnRunEndedEvent;

        ImportFiles();
    }

    private static void ImportFiles() {
        Queue<DirectoryInfo> directories = [];
        //string documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //string path = Path.Combine(documentsFolder, "Ruling Practice Tool");

        //string path = Path.Combine(Path.GetDirectoryName(System.Environment.ProcessPath)!, "Data");
        string path = "Data";
        GD.Print(path);
        directories.Enqueue(new DirectoryInfo(path));

        do {
            DirectoryInfo directory = directories.Dequeue();

            foreach (var subDirectory in directory.GetDirectories()) {
                directories.Enqueue(subDirectory);
            }

            foreach (var file in directory.GetFiles()) {
                if (file.Extension != ".json") {
                    continue;
                }

                if (file.Name == "Cards.json") {
                    ImportCards(file);
                } else if (file.Name == "Questions.json") {
                    ImportQuestions(file);
                } else {
                    ImportQuestion(file);
                }
            }
        } while (directories.Count > 0);
    }

    private static void ImportCards(FileInfo file) {
        using var stream = file.OpenRead();
        using StreamReader reader = new StreamReader(stream);
        using JsonReader jsonReader = new JsonTextReader(reader);
        Card[]? cards = SERIALIZER.Deserialize<Card[]>(jsonReader);

        if (cards != null) {
            CardDatabase.Instance.LoadCards(cards);
        }
    }

    private static void ImportQuestions(FileInfo file) {
        using var stream = file.OpenRead();
        using StreamReader reader = new StreamReader(stream);
        using JsonReader jsonReader = new JsonTextReader(reader);
        Question[]? questions = SERIALIZER.Deserialize<Question[]>(jsonReader);

        if (questions != null) {
            QuestionDatabase.Instance.LoadQuestions(questions);
        }
    }

    private static void ImportQuestion(FileInfo file) {
        using var stream = file.OpenRead();
        using StreamReader reader = new StreamReader(stream);
        using JsonReader jsonReader = new JsonTextReader(reader);
        Question? question = SERIALIZER.Deserialize<Question>(jsonReader);

        if (question != null) {
            QuestionDatabase.Instance.LoadQuestions([question]);
        }
    }

    private void OnNextQuestionEvent() {
        NextQuestion();
    }

    private void OnRunStartedEvent() {
        NextQuestion();
        QuestionPanel.Visible = true;
        ResultsPanel.Visible = false;
    }

    private void OnRunEndedEvent(RunData runData) {
        QuestionPanel.Visible = false;
        ResultsPanel.Visible = true;

        CardContainer.ClearCards();
    }

    private void NextQuestion() {
        Question? nextQuestion = PracticeRunManager.Instance.GetNextQuestion();

        if (nextQuestion == null) {
            PracticeRunManager.Instance.FinishRun();
            return;
        }

        QuestionPanel.UpdateQuestion(nextQuestion);
        CardContainer.LoadCards(nextQuestion.CardsUsed);
    }
}
