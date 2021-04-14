using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2D Platformer/Dialogue Sequence", fileName = "New Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public Dialogue[] _dialogue;
    public int _currentIndex = 0;
}