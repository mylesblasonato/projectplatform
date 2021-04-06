using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] Transform _startingPosition;
    [SerializeField] GameObject _player;
    [SerializeField] bool _showCursor = false;
    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    void Start()
    {
        _pauseScreen.SetActive(false);
        _player.transform.position = _startingPosition.position;

        EventManager.Instance.AddListener("OnDeath", Death);
        EventManager.Instance.AddListener("OnTimeUp", Death);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener("OnDeath", Death);
        EventManager.Instance.RemoveListener("OnTimeUp", Death);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }

        Cursor.visible = _showCursor;
    }

    void Death()
    {
        _player.transform.position = _startingPosition.position;
        EventManager.Instance.TriggerEvent("OnLoseLife");
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        _pauseScreen.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1f;
    }
}
