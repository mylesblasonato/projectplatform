using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CountUpTimerController : MonoBehaviour
{
    #region VARS
    [Header("---SHARED---", order = 0)] //Scriptable Object Floats
   // [SerializeField] SoFloat _duration;

    [Header("---EVENTS---", order = 1)] //EVENTS
    //[SerializeField] GameEvent _OnTimeUp;
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
            _textUI.text = $"" +
                $"{zeroBefore10Mins}" +
                $"{Mathf.Floor(_minutes).ToString()}" + $":" +
                $"{zeroBefore10Secs}{Mathf.Ceil(_seconds).ToString()}";
        } 
    }

    float _minutes;
    float _seconds;
    bool _countingUp = false;
    void CountUp()
    {
        if (!_countingUp) return;
        ElapsedTime += Time.deltaTime;
        _seconds = (_seconds >= 0) ? _seconds + Time.deltaTime : UpdateMinute();
    }
    public void StartTime()
    {
        ElapsedTime = 0;
        _minutes = 0;
        _seconds = 0;
        _countingUp = true;
    }

    #region UNITY
    void Awake() => _textUI = GetComponent<TextMeshProUGUI>();
    void Start() => StartTime();
    void Update() => CountUp();
    #endregion

    #region HELPER
    float UpdateMinute() { _minutes--; return _seconds = 59f; }
    #endregion
}
