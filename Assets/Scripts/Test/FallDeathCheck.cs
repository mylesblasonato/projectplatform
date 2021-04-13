using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathCheck : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] float _fallBoundary = -4f;

    [Header("---EVENTS---", order = 1)] //EVENTS
    [SerializeField] GameEvent _OnDeath;
    #endregion

    void DeathHandler() => _OnDeath?.Invoke();
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _fallBoundary)
            DeathHandler();
    }

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(0, 0), new Vector2(0, _fallBoundary));
    }
    #endregion
}
