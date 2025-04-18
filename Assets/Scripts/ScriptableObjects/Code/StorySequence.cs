using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sequence", menuName = "Scriptable Story Sequence/Create Story Sequence")]
public class StorySequence : ScriptableObject
{
    [SerializeField] StoryNode[] _storyNodes;
    public StoryNode[] StoryNodes => _storyNodes;

    public bool _onlyPlayOnce = false;
}
