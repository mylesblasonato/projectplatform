using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] string _mainMusic = "Main";
    [SerializeField] string _pauseScreenName;
    [SerializeField] string _startingPositionName;
    [SerializeField] string _playerName;
    [SerializeField] bool _showCursor = false;

    [Header("---EVENTS---", order = 1)] //EVENTS
    [SerializeField] GameEvent _OnGameStart;
    [SerializeField] GameEvent _OnLoseLife;
    [SerializeField] GameEvent _OnPause;
    [SerializeField] GameEvent _OnUnPause;

    #endregion

    GameObject _player;
    GameObject _pauseScreen;
    AudioManager _audioManager;
    void Awake()
    {
        _pauseScreen = GameObject.FindGameObjectWithTag(_pauseScreenName);
        _pauseScreen.SetActive(false);
        _player = GameObject.FindGameObjectWithTag(_playerName);
        _startingPosition = GameObject.FindGameObjectWithTag(_startingPositionName).transform;
        _player.transform.position = _startingPosition.transform.position;
        _audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        _OnGameStart?.Invoke();
        _audioManager.PlayMusic(_mainMusic, 0f);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Pause();
        Cursor.visible = _showCursor;
    }

    Transform _startingPosition;
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

        if(_isPaused)
            _OnPause?.Invoke();
        else
            _OnUnPause?.Invoke();

        Time.timeScale = _isPaused ? 0 : 1f;
    }
}
