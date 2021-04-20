using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
            TimeSpan timeSpan = TimeSpan.FromSeconds(_elapsedTime);
            string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            _textUI.text = timeText;
        } 
    }

    bool _countingUp = false;
    void CountUp()
    {
        if (!_countingUp) return;
        ElapsedTime += Time.deltaTime;
    }
    public void StartTime()
    {
        ElapsedTime = 0;
        TimeSpan timeSpan = TimeSpan.FromSeconds(0);
        _countingUp = true;
    }

    #region UNITY
    void Awake() => _textUI = GetComponent<TextMeshProUGUI>();
    void Start() => StartTime();
    void Update() => CountUp();
    #endregion

    #region HELPER
    #endregion
}
