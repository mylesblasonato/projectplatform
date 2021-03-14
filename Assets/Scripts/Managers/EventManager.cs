using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoSingleton
{
    [HideInInspector] public UnityAction _OnIdle, _OnWalk, _OnRun, _OnJump, _OnLand, _OnCrouch, _OnStand, _OnShoot, _OnStopShoot;

    public void Idle() => _OnIdle?.Invoke();
    public void Walk() => _OnWalk?.Invoke();
    public void Run() => _OnRun?.Invoke();
    public void Jump() => _OnJump?.Invoke();
    public void Land() => _OnLand?.Invoke();
    public void Crouch() => _OnCrouch?.Invoke();
    public void Stand() => _OnStand?.Invoke();
    public void Shoot() => _OnShoot?.Invoke();
    public void StopShoot() => _OnStopShoot?.Invoke();

    private void Awake()
    {
        MakeSingleton(this);
    }
}
