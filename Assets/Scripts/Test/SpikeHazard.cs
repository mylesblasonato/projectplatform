using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeHazard : MonoBehaviour
{
    [SerializeField] Vector2 _positionOffset;
    [SerializeField] Vector2 _spikeColliderSize;
    [SerializeField] LayerMask _spikeLayer;

    bool _spikeCheck = false;

    void Update()
    {
        SpikeCheck();
    }

    void DeathHandler()
    {
        EventManager.Instance.TriggerEvent("OnDeath");
    }

    void SpikeCheck()
    {
        _spikeCheck = Physics2D.OverlapBox(new Vector3(transform.position.x + _positionOffset.x, transform.position.y - _positionOffset.y, 0), _spikeColliderSize, 0f, _spikeLayer);

        if (_spikeCheck)
            DeathHandler();
    }

    #region HELPERS
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(transform.position.x + _positionOffset.x, transform.position.y - _positionOffset.y, 0), _spikeColliderSize);
    }
    #endregion
}
