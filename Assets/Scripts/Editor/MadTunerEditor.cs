using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MadTunerEditor : EditorWindow
{
    // Method to open the window
    [MenuItem("JMF/MadTuner")]
    static void OpenWindow()
    {
        var win = GetWindow<MadTunerEditor>();
        win.titleContent = new GUIContent("Mad Tuner");
    }

    public MadTunerSettings _settings;
    private SerializedObject _so;
    Vector2 _scrollPos;
    
    void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(100), GUILayout.Height(this.position.height));
        EditorGUILayout.BeginVertical();
        _settings = (MadTunerSettings) EditorGUILayout.ObjectField(_settings, typeof(MadTunerSettings), false);
        if (_settings == null) return;
        for (int i = 0; i < _settings._scriptableData.Count; ++i)
        {
            Undo.RecordObject(_settings._scriptableData[i], "Undo Tuner Setting Change");
            _settings._scriptableData[i] = (SoFloat)EditorGUILayout.ObjectField(_settings._scriptableData[i], typeof(SoFloat), false);
            _settings._scriptableData[i].Value = EditorGUILayout.FloatField(_settings._scriptableData[i].Value);
        }
        EditorGUILayout.EndScrollView();
    }
}