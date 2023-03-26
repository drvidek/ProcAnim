using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class HighscoreKeeper
{
    #region Variables
    private static List<KeyValuePair<string, float>> _entries = new();
    public static int maxEntriesToKeep = 10;
    public static string saveFilePath = "/QuickitResources/Highscores/Save.txt";
    public static string defaultFilePath = "/QuickitResources/Highscores/Default.txt";
    private static string _savePath = Path.Combine(Application.streamingAssetsPath + saveFilePath);
    private static string _defaultPath = Path.Combine(Application.streamingAssetsPath + defaultFilePath);
    #endregion

    #region Properties
    /// <summary>
    /// A sorted list of all highscore entries, where Entries[i].Key is the name and Entries[i].Value is the score
    /// </summary>
    public static List<KeyValuePair<string, float>> Entries => _entries;
    /// <summary>
    /// A string of \n separated names in order from high to low
    /// </summary>
    public static string NameList
    {
        get
        {
            string names = "";
            for (int i = 0; i < _entries.Count; i++)
            {
                names += _entries[i].Key;
                if (i + 1 != _entries.Count)
                    names += "\n";
            }
            return names;
        }
    }
    /// <summary>
    /// A string of \n separated scores in order from high to low
    /// </summary>
    public static string ScoreList
    {
        get
        {
            string scores = "";
            for (int i = 0; i < _entries.Count; i++)
            {
                scores += _entries[i].Value;
                if (i + 1 != _entries.Count)
                    scores += "\n";
            }
            return scores;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Attempt to add a new entry to the list with Key name and Value score. Returns true if the score makes it onto the list.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public static bool ValidateNewEntry(string name, float score)
    {
        KeyValuePair<string, float> entry = new(name, score);
        _entries.Add(entry);
        SortEntries();

        if (_entries.Count <= maxEntriesToKeep)
            return true;

        _entries.RemoveAt(_entries.Count - 1);
        return (_entries.Contains(entry));
    }

    /// <summary>
    /// Attempt to add a new entry to the list with Key name and Value score. Returns true if the score makes it onto the list, and outputs the position on the list.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    public static bool ValidateNewEntry(string name, float score, out int positionInList)
    {
        KeyValuePair<string, float> entry = new(name, score);
        _entries.Add(entry);
        SortEntries();

        if (_entries.Count <= maxEntriesToKeep)
        {
            positionInList = _entries.IndexOf(entry);
            return true;
        }

        positionInList = _entries.Count;
        _entries.RemoveAt(_entries.Count - 1);
        return (_entries.Contains(entry));
    }

    /// <summary>
    /// Load the saved high score entries from file into memory, using the default file if no save exists
    /// </summary>
    public static void LoadEntries()
    {
        _entries.Clear();
        UnpackEntriesFromStringArray(LoadStringArrayFromFile(File.Exists(_savePath) ? _savePath : _defaultPath));
        SortEntries();
    }

    /// <summary>
    /// Save the current list of high scores to file
    /// </summary>
    public static void SaveEntries()
    {
        SortEntries();
        SaveStringArrayToFile(PackEntriesToStringArray(), _savePath);
    }

    /// <summary>
    /// Resets the saved high score file to default - this will permanently delete existing high score data
    /// </summary>
    public static void ResetEntriesToDefault()
    {
        LoadDefaultEntries();
        SaveEntries();
    } 

    public static void RefreshList()
    {

    }
    #endregion

    #region Private Methods
    private static void LoadDefaultEntries()
    {
        _entries.Clear();
        UnpackEntriesFromStringArray(LoadStringArrayFromFile(_defaultPath));
        SortEntries();
    }

    private static string[] LoadStringArrayFromFile(string path)
    {
        StreamReader reader = new(path);
        string[] array = new string[maxEntriesToKeep];
        string line;
        int i = 0;
        while ((line = reader.ReadLine()) != null && line != "" && i < maxEntriesToKeep)
        {
            array[i] = line;
            i++;
        }
        reader.Close();
        return array;
    }

    private static void SaveStringArrayToFile(string[] array, string path)
    {
        StreamWriter reader = new(path, false);
        var i = 0;
        foreach (var item in array)
        {
            if (i == maxEntriesToKeep)
                break;

            string line = item;
            reader.WriteLine(line);
            i++;
        }
        reader.Close();
    }

    private static string[] PackEntriesToStringArray()
    {
        string[] array = new string[maxEntriesToKeep];
        for (int i = 0; i < maxEntriesToKeep && i < _entries.Count; i++)
        {
            array[i] = $"{_entries[i].Key}:{_entries[i].Value}";
        }
        return array;
    }

    private static void UnpackEntriesFromStringArray(string[] saveStrings)
    {
        foreach (string save in saveStrings)
        {
            if (save == null)
                continue;

            string[] splitData = save.Split(':');
            KeyValuePair<string, float> pair = new(splitData[0], float.Parse(splitData[1]));
            _entries.Add(pair);
        }
    }

    private static void SortEntries()
    {
        _entries.Sort((x, y) => y.Value.CompareTo(x.Value));
    } 
    #endregion

}
