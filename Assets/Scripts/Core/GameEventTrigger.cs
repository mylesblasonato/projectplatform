using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventTrigger : MonoBehaviour
{
    [SerializeField] GameEvent _gameEvent;
    [SerializeField] bool _isTriggerEnter = true;
    [SerializeField] bool _isOnInteraction = false;
    [SerializeField] string _startDialogueAxis = "";

    bool _isInsideTrigger = false;

    void Update()
    {
        if (Input.GetAxis(_startDialogueAxis) > 0 && 
            _isInsideTrigger && 
            _isOnInteraction && 
            _startDialogueAxis != "")
        {
            InvokeEvent();
            _isInsideTrigger = false;
        }
    }
    void InvokeEvent()
    {
        _gameEvent?.Invoke();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        _isInsideTrigger = true;
        if (other.CompareTag("Player") && _isTriggerEnter && !_isOnInteraction)
            InvokeEvent();
    }
    void OnTriggerExit2D(Collider2D other)
    {
        _isInsideTrigger = false;
        if (other.CompareTag("Player") && !_isTriggerEnter && !_isOnInteraction)
            InvokeEvent();
    }
}
