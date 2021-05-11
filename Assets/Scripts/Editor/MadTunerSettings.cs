using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JMF/Mad Tuner/Settings", fileName = "New Tuner Settings", order = 1)]
public class MadTunerSettings : ScriptableObject
{
    public List<SoFloat> _scriptableData;
}