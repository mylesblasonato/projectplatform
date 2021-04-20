using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMesh;
    [SerializeField] Button _nextBtn; 
    [SerializeField] string _nextBtnAxis;

    DialogueSequence _currentDialogueSequence;
    int _currentIndex = 0;
    bool _isPrinting = false;
    bool _isDialogueComplete = true;
    float _currentDuration = 0;

    void Update()
    {
        if (!_isDialogueComplete && Input.GetButtonDown(_nextBtnAxis))
        {
            if (_currentIndex < _currentDialogueSequence._dialogue.Count - 1)
                NextDialogue();
        }
    }

    public void PlayDialogue(DialogueSequence dialogueSequence)
    {
        _currentDialogueSequence = dialogueSequence;
        _currentDialogueSequence._currentIndex = 0;
        RunDialogue();
    }
    
    void RunDialogue()
    {
        _isDialogueComplete = false;
        _currentIndex = _currentDialogueSequence._currentIndex;
        _textMesh.text = $"{_currentDialogueSequence._dialogue[_currentIndex]._name}";
        _currentDuration = _currentDialogueSequence._dialogue[_currentIndex]._duration / _currentDialogueSequence._dialogue[_currentIndex]._text.Length;
        _textMesh.gameObject.transform.parent.parent.transform.position = GameObject.Find(_currentDialogueSequence._dialogue[_currentIndex]._gameObjectName).transform.position;
        _nextBtn.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PrintDialogue());
    }

    public void NextDialogue()
    {
        if (_currentIndex <= (_currentDialogueSequence._dialogue.Count - 1))
        {
            if (_currentIndex == (_currentDialogueSequence._dialogue.Count - 1))
            {
                _isDialogueComplete = true;
            }

            if (!_isPrinting)
            {
                _currentIndex++;
                _currentDialogueSequence._currentIndex = _currentIndex;
                RunDialogue();
            }
            else
            {
                _currentDuration = 0;
            }
        }
    }

    IEnumerator PrintDialogue()
    {
        _isPrinting = true;
        foreach (char letter in _currentDialogueSequence._dialogue[_currentIndex]._text)
        {
            _textMesh.text += letter;
            yield return new WaitForSeconds(_currentDuration);
        }
        _isPrinting = false;

        if (_currentIndex == (_currentDialogueSequence._dialogue.Count - 1))
            _nextBtn.gameObject.SetActive(false);
    }

    public void ClearDialogue()
    {
        _currentIndex = 0;
        if (_currentDialogueSequence != null)
            _currentDialogueSequence._currentIndex = 0;
        _textMesh.text = "";
        _isDialogueComplete = true;
    }
}
