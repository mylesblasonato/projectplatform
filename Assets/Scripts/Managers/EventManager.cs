using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Event
{
    [SerializeField] string _eventName;

    public string Name => _eventName;
    public UnityEvent EventAction => new UnityEvent();
}

public class EventManager : MonoSingleton<EventManager>
{
    [SerializeField] List<Event> _eventList;
    Dictionary<string, UnityEvent> _eventDic;

    protected override void Awake()
    {
        base.Awake();
        _eventDic = new Dictionary<string, UnityEvent>();
        foreach (Event e in _eventList)
            _eventDic.Add(e.Name, e.EventAction);
    }

    public void TriggerEvent(string name) 
    {
        var eventAction = _eventDic[name]; 
        eventAction?.Invoke();
    }
    public void AddListener(string name, UnityAction uEvent) => 
        _eventDic[name].AddListener(uEvent);
    public void RemoveListener(string name, UnityAction uEvent) => _eventDic[name].RemoveListener(uEvent);
}
