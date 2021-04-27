using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformerAnimatorController))]
public class PlatformerAnimatorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformerAnimatorController pac = (PlatformerAnimatorController)target;
        if (GUILayout.Button("Update Parameters"))
        {
            pac.AddParameters();
        }
    }
}
