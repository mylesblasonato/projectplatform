using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] Transform _startingPosition;
    [SerializeField] GameObject _player;
    [SerializeField] bool _showCursor = false;

    [Header("---EVENTS---", order = 1)] //EVENTS
    [SerializeField] GameEvent _OnLoseLife;
    #endregion

    void Start()
    {
        _pauseScreen.SetActive(false);
        _player.transform.position = _startingPosition.position;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Pause();
        Cursor.visible = _showCursor;
    }

    public void Death()
    {
        _player.transform.position = _startingPosition.position;
        _OnLoseLife?.Invoke();
    }

    private bool _isPaused = false;
    public bool IsPaused => _isPaused;
    public void Pause()
    {
        _isPaused = !_isPaused;
        _pauseScreen.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1f;
    }
}
