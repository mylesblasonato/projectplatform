using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class MadTunerEditor : EditorWindow
{
    // Method to open the window
    [MenuItem("JMF/MadTuner")]
    static void OpenWindow()
    {
        var win = GetWindow<MadTunerEditor>();
        win.titleContent = new GUIContent("Mad Tuner");
    }
    MadTunerSettings _settings;
    Vector2 _scrollPos;
    bool _changeSOFloat = false;
    void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Width(100), GUILayout.Height(this.position.height));
        EditorGUILayout.BeginVertical();
        _settings = (MadTunerSettings) EditorGUILayout.ObjectField(_settings, typeof(MadTunerSettings), false);
        if (_settings == null || _settings._scriptableData.Count == 0 ) return;
        _changeSOFloat = EditorGUILayout.Toggle(new GUIContent("CHANGE FLOATS"), _changeSOFloat);
        EditorGUILayout.Space();
        for (int i = 0; i < _settings._scriptableData.Count; ++i)
        {
            Undo.RecordObject(_settings._scriptableData[i], "Undo Tuner Setting Change");
            _settings._scriptableData[i].Value = EditorGUILayout.FloatField(new GUIContent(_settings._scriptableData[i].name), _settings._scriptableData[i].Value);
            if(_changeSOFloat)
                _settings._scriptableData[i] = (SoFloat)EditorGUILayout.ObjectField(_settings._scriptableData[i], typeof(SoFloat), false);
        }
        EditorGUILayout.EndScrollView();
        SceneView.RepaintAll();
    }
}
#endif