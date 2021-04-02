using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject _pauseScreen;
    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
    }

    void Start()
    {
        _pauseScreen.SetActive(false);
        Cursor.visible = false;
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        _pauseScreen.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1f;
    }
}
