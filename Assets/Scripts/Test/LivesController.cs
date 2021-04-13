using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesController : MonoBehaviour
{   
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] GameObject _livesPrefab;
    [SerializeField] int _startingLives = 3;

    [Header("---SHARED---", order = 1)] //Scriptable Object Floats
    [SerializeField] SoFloat _currentLives;
    #endregion

    // Start is called before the first frame update
    void Start() => Initialize();

    Queue<GameObject> _livesUIList;
    void Initialize()
    {
        _livesUIList = new Queue<GameObject>();
        _currentLives.Value = 0;
        for (int i = 0; i < _startingLives; i++)
            AddLife();
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
