using System.Collections.Generic;

namespace YGORulingPracticeTool.Scripts.Models;

public static class Names {
    public static string[] NAMES { get; } = [
        // Male Names
        "Rick",
        "James",
        "Jim",
        // Unisex Names
        "Jesse",
        "Charlie",
        // Female Names
        "Erika",
        "Beth",
        "Maggie"
    ];

    public static string ReplaceNames(this string str, List<string> names) {
        for (int i = 0; i < names.Count; i++) {
            str = str.Replace($"{{PLAYER_{i + 1}}}", names[i]);
        }
        return str;
    }
}
