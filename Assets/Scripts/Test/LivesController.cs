using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesController : MonoBehaviour
{
    [SerializeField] GameObject _livesPrefab;
    [SerializeField] int _startingLives = 3;
    [SerializeField] SoFloat _currentLives;

    Queue<GameObject> _livesUIList;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener("OnGainLife", AddLife);
        EventManager.Instance.RemoveListener("OnLoseLife", RemoveLife);
    }

    void Initialize()
    {
        _livesUIList = new Queue<GameObject>();
        _currentLives.Value = 0;
        for (int i = 0; i < _startingLives; i++)
        {
            AddLife();
        }

        EventManager.Instance.AddListener("OnGainLife", AddLife);
        EventManager.Instance.AddListener("OnLoseLife", RemoveLife);
    }

    public void AddLife()
    {
        _currentLives.Value++;
        _livesUIList.Enqueue(Instantiate(_livesPrefab, transform));
    }

    public void RemoveLife()
    {
        _currentLives.Value--;
        Destroy(_livesUIList.Dequeue());

        if (_currentLives.Value <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
