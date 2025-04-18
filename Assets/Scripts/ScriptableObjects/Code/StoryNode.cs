using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoryNode
{
    [SerializeField] Sprite _portrait;
    [SerializeField] int _portraitDirection = 1;
    [SerializeField] string _name;
    [SerializeField] [TextArea(0,10)] string _dialogue;
    [SerializeField] float _duration = 1f;

    public Sprite Portrait => _portrait;
    public int PortraitDirection => _portraitDirection;
    public string Name => _name;
    public string Dialogue => _dialogue;
    public float Duration => _duration;
}
