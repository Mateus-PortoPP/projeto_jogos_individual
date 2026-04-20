using UnityEngine;
using System.Collections.Generic;

public static class RankingManager
{
    private const string RankingKey = "ranking";
    private const int MaxEntries = 10;

    [System.Serializable]
    public class Entry
    {
        public string name;
        public int score;
    }

    [System.Serializable]
    private class EntryList
    {
        public List<Entry> entries = new List<Entry>();
    }

    public static List<Entry> GetRanking()
    {
        string json = PlayerPrefs.GetString(RankingKey, "");
        if (string.IsNullOrEmpty(json))
            return new List<Entry>();
        return JsonUtility.FromJson<EntryList>(json).entries;
    }

    public static void AddEntry(string name, int score)
    {
        if (string.IsNullOrWhiteSpace(name))
            name = "Jogador";

        var entries = GetRanking();
        entries.Add(new Entry { name = name, score = score });
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (entries.Count > MaxEntries)
            entries.RemoveRange(MaxEntries, entries.Count - MaxEntries);

        PlayerPrefs.SetString(RankingKey, JsonUtility.ToJson(new EntryList { entries = entries }));
        PlayerPrefs.Save();
    }

    public static string FormatRanking()
    {
        var entries = GetRanking();
        if (entries.Count == 0)
            return "Nenhum recorde ainda.";

        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < entries.Count; i++)
            sb.AppendLine($"{i + 1}. {entries[i].name} — {entries[i].score} pts");

        return sb.ToString().TrimEnd();
    }
}
