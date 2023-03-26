using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HighscoreKeeperTool : EditorWindow
{
    [MenuItem("Quickit/HighscoreKeeper")]
    public static void ShowWindow()
    {
        thisWindow = GetWindow(typeof(HighscoreKeeperTool), false, "HighscoreKeeper");
        windowRect = thisWindow.position;
        HighscoreKeeper.LoadEntries();
        tempMaxEntriesToKeep = HighscoreKeeper.maxEntriesToKeep;
    }

    private static Rect windowRect;
    private static EditorWindow thisWindow;

    private Vector2 windowScrollPos;
    private static int tempMaxEntriesToKeep;

    private void OnGUI()
    {
        float defaultLabelWidth = EditorGUIUtility.labelWidth;

        if (thisWindow == null)
            thisWindow = GetWindow(typeof(HighscoreKeeperTool));
        if (windowRect.size != thisWindow.position.size)
        {
            windowRect = thisWindow.position;
        }

        windowScrollPos = EditorGUILayout.BeginScrollView(windowScrollPos, GUILayout.MaxWidth(windowRect.width));

        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        tempMaxEntriesToKeep = EditorGUILayout.IntField("Maximum entries to keep", tempMaxEntriesToKeep);

        if (GUILayout.Button("Confirm"))
        {
            HighscoreKeeper.maxEntriesToKeep = tempMaxEntriesToKeep;
            HighscoreKeeper.SaveEntries();
            HighscoreKeeper.LoadEntries();
        }
        EditorGUILayout.EndHorizontal();

        float stringWidth = 106f;

        EditorGUILayout.LabelField("Save File path:");
        EditorGUIUtility.labelWidth = stringWidth;
        HighscoreKeeper.saveFilePath = EditorGUILayout.TextField("~/StreamingAssets", HighscoreKeeper.saveFilePath);
        EditorGUIUtility.labelWidth = defaultLabelWidth;

        EditorGUILayout.LabelField("Default File path:");
        EditorGUIUtility.labelWidth = stringWidth;
        HighscoreKeeper.defaultFilePath = EditorGUILayout.TextField("~/StreamingAssets", HighscoreKeeper.defaultFilePath);
        EditorGUIUtility.labelWidth = defaultLabelWidth;

        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Current high scores", EditorStyles.boldLabel);
        for (int i = 0; i < HighscoreKeeper.maxEntriesToKeep; i++)
        {
            string displayString = i >= HighscoreKeeper.Entries.Count ? $"{i + 1}." :
                $"{i + 1}. {HighscoreKeeper.Entries[i].Key}: {HighscoreKeeper.Entries[i].Value}";
            EditorGUILayout.LabelField(displayString);
        }

        if (GUILayout.Button("Reset to default scores"))
        {
            HighscoreKeeper.ResetEntriesToDefault();
        }
        EditorGUILayout.EndScrollView();
    }

}
