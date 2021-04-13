using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerController : MonoBehaviour
{
    #region VARS
    [Header("---SHARED---", order = 0)] //Scriptable Object Floats
    [SerializeField] SoFloat _duration;

    [Header("---EVENTS---", order = 1)] //EVENTS
    [SerializeField] GameEvent _OnTimeUp;
    #endregion

    TextMeshProUGUI _textUI;
    float _elapsedTime = 0;
    public float ElapsedTime 
    { 
        get => _elapsedTime;
        set
        { 
            _elapsedTime = value;
            var zeroBefore10Mins = _minutes <= 9 ? "0" : "";
            var zeroBefore10Secs = _seconds <= 9 ? "0" : "";
            _textUI.text = $"TIME: " +
                $"{zeroBefore10Mins}" +
                $"{Mathf.Floor(_minutes).ToString()}" + $":" +
                $"{zeroBefore10Secs}{Mathf.Ceil(_seconds).ToString()}";
        } 
    }

    float _minutes;
    float _seconds;
    bool _countingDown = false;
    void CountDown()
    {
        if (!_countingDown) return;
        ElapsedTime -= Time.deltaTime;
        _seconds = (_seconds >= 0) ? _seconds - Time.deltaTime : UpdateMinute();
        if (_minutes <= 0 && _seconds <= 0)
            _OnTimeUp?.Invoke();
    }
    public void StartTime()
    {
        ElapsedTime = _duration.Value;
        _minutes = (_duration.Value / 60f < 1) ? 0 : _duration.Value / 60f;
        _seconds = _duration.Value - (Mathf.Floor(_minutes) * 60f);
        _countingDown = true;
    }

    #region UNITY
    void Awake() => _textUI = GetComponent<TextMeshProUGUI>();
    void Start() => StartTime();
    void Update() => CountDown();
    #endregion

    #region HELPER
    float UpdateMinute() { _minutes--; return _seconds = 59f; }
    #endregion
}
