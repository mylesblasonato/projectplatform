using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] GameObject _pauseScreen;
    [SerializeField] bool _showCursor = false;
    private bool _isPaused = false;
    public bool IsPaused => _isPaused;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
        
        Cursor.visible = _showCursor;
    }

    void Start()
    {
        _pauseScreen.SetActive(false);

        

        EventManager.Instance.AddListener("OnDeath", Death);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener("OnDeath", Death);
    }

    void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        _pauseScreen.SetActive(_isPaused);
        Time.timeScale = _isPaused ? 0 : 1f;
    }
}
