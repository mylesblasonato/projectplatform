using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerController : MonoBehaviour
{   
    [SerializeField] SoFloat _duration;
    TextMeshProUGUI _textUI;

    float _minutes;
    float _seconds;

    float _elapsedTime = 0;
    public float ElapsedTime 
    { 
        get => _elapsedTime;
        set
        { 
            _elapsedTime = value;
            var zeroBefore10Mins = _minutes <= 9 ? "0" : "";
            var zeroBefore10Secs = _seconds <= 9 ? "0" : "";
            _textUI.text = $"TIME: {zeroBefore10Mins}{Mathf.Floor(_minutes).ToString()}:{zeroBefore10Secs}{Mathf.Ceil(_seconds).ToString()}";
        } 
    }
    
    bool _countingDown = false;

    void Awake()
    {
        _textUI = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        StartTime();
    }

    public void StartTime()
    {
        ElapsedTime = _duration.Value;
        _minutes = (_duration.Value / 60f < 1) ? 0 : _duration.Value / 60f;
        _seconds = _duration.Value - (Mathf.Floor(_minutes) * 60f);
        _countingDown = true;
    }

    void Update()
    {
        if (!_countingDown) return;
        ElapsedTime -= Time.deltaTime;
        _seconds = (_seconds >= 0) ? _seconds - Time.deltaTime : UpdateMinute();
        if (_minutes <= 0 && _seconds <= 0)
            EventManager.Instance.TriggerEvent("OnTimeUp");
    }

    float UpdateMinute()
    {
        _minutes--;
        return _seconds = 59f;
    }
}
