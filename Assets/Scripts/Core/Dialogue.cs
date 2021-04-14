using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string _name;
    [TextArea(5, 5)] public string _text;
    public string _gameObjectName;
    public float _duration;
}
