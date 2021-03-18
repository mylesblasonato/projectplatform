using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] InputAction _contextButton;
    [SerializeField] UnityEvent _event;

    bool _triggered = false;
    GameObject _player;

    private void Awake() => _player = GameObject.FindGameObjectWithTag("Player");

    void Start() => _contextButton.performed += ctx => TriggerEvent(ctx.ReadValue<float>());

    void OnDestroy() => _contextButton.performed -= ctx => TriggerEvent(ctx.ReadValue<float>());

    void OnEnable() => _contextButton.Enable();

    void OnDisable() => _contextButton.Disable();
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
             _triggered = true;
    }

    private void Update()
    {
        if (Vector2.Distance(_player.transform.position, gameObject.transform.position) >= 1)
            _triggered = false;
    }

    void TriggerEvent(float ctx)
    {
        if (ctx < 1 && !_triggered) return;
        if (_triggered)
        {
            _triggered = false;
            _event?.Invoke();
        }
    }
}
