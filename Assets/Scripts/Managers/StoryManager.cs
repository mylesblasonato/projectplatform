using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StoryManager : MonoBehaviour
{
    [SerializeField] InputAction _skipInputAction, _nextInputAction;
    [SerializeField] GameObject _storyUI;
    [SerializeField] Image _portraitUI;
    [SerializeField] TextMeshProUGUI _nameText, _dialogueText;

    StorySequence _currentStorySequence;
    int _currentNode = 0;
    bool _playOnce = false;
    bool _triggered = false;
    bool _isPrinting = false;
    bool _finishPrinting = false;
    bool _lastNode = false;

    void Start()
    {
        _storyUI.SetActive(false);
        _skipInputAction.performed += ctx => Skip(ctx.ReadValue<float>());
        _nextInputAction.performed += ctx => Next(ctx.ReadValue<float>());
    }

    void OnDestroy()
    {
        _skipInputAction.performed -= ctx => Skip(ctx.ReadValue<float>());
        _nextInputAction.performed -= ctx => Next(ctx.ReadValue<float>());
    }

    public void StartSequence(StorySequence storySequence)
    {
        _currentStorySequence = storySequence;
        if (!_playOnce && !_triggered)
        {
            if (_currentStorySequence._onlyPlayOnce)
                _playOnce = true;

            _currentNode = 0;
            _storyUI.SetActive(true);
            _triggered = true;
            Play();
            Invoke("NextButtonActivate", 0.2f);
        }
    }

    void NextButtonActivate()
    {
        _nextInputAction.Enable();
        _skipInputAction.Enable();
    }

    public void Next(float ctx)
    {
        if (ctx < 1 && !_triggered && _storyUI.activeSelf) return;
        if (!_finishPrinting)
        {
            if (!_isPrinting)
            {
                Play();
            }
            else
            {
                FinishPrinting();
            }
        }
        else
        {
            if (!_lastNode)
            {
                _currentNode++;
                Play();
            }
            else
            {
                End();
            }
        }
    }

    void FinishPrinting()
    {
        _portraitUI.sprite = _currentStorySequence.StoryNodes[_currentNode].Portrait;
        _nameText.text = _currentStorySequence.StoryNodes[_currentNode].Name;
        _dialogueText.text = _currentStorySequence.StoryNodes[_currentNode].Dialogue;
        _finishPrinting = true;
        _isPrinting = false;

        if (_currentNode == _currentStorySequence.StoryNodes.Length - 1)
        {
            _lastNode = true;
        }

        StopAllCoroutines();
    }

    public void Play()
    {
        _finishPrinting = false;
        _portraitUI.transform.localScale = new Vector3(_currentStorySequence.StoryNodes[_currentNode].PortraitDirection, 1,1);
        _portraitUI.sprite = _currentStorySequence.StoryNodes[_currentNode].Portrait;
        _nameText.text = _currentStorySequence.StoryNodes[_currentNode].Name;
        StopAllCoroutines();
        StartCoroutine("Print");
    }

    public void Skip(float ctx)
    {
        if (ctx < 1 && !_triggered) return;
        FinishPrinting();
        End();
    }

    void End()
    {
        _storyUI.SetActive(false);
        _triggered = false;
        _lastNode = false;
        _nextInputAction.Disable();
        _skipInputAction.Disable();
    }

    IEnumerator Print()
    {
        _isPrinting = true;
        _dialogueText.text = "";
        foreach (char letter in _currentStorySequence.StoryNodes[_currentNode].Dialogue)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_currentStorySequence.StoryNodes[_currentNode].Duration / _currentStorySequence.StoryNodes[_currentNode].Dialogue.Length);
        }       
        FinishPrinting();
    }
}
