using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathCheck : MonoBehaviour
{
    [SerializeField] float _fallBoundary = -4f;

    void DeathHandler()
    {
        EventManager.Instance.TriggerEvent("OnDeath");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _fallBoundary)
        {
            DeathHandler();
        }
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector2(0, 0), new Vector2(0, _fallBoundary));
    }
    #endregion
}
