using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2D Platformer/Dialogue Sequence", fileName = "New Dialogue Sequence")]
public class DialogueSequence : ScriptableObject
{
    public TextAsset _dialogueFile;
    public List<Dialogue> _dialogue;
    [HideInInspector] public int _currentIndex = 0;
}