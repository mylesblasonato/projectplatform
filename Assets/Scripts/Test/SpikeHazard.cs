using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpikeHazard : MonoBehaviour
{
    #region VARS
    [Header("---LOCAL---", order = 0)] //Component variables
    [SerializeField] Vector2 _positionOffset;
    [SerializeField] Vector2 _spikeColliderSize;
    [SerializeField] LayerMask _spikeLayer;
    #endregion

    #region EVENTS
    [Header("---EVENTS---", order = 1)] //EVENTS
    [SerializeField] GameEvent _OnDeath;
    #endregion

    bool _spikeCheck = false;
    void SpikeCheck()
    {
        var overlapPoint = new Vector2(
            transform.position.x + _positionOffset.x, 
            transform.position.y - _positionOffset.y);
        _spikeCheck = Physics2D.OverlapBox(overlapPoint, _spikeColliderSize, 0f, _spikeLayer);
        if (_spikeCheck) DeathHandler();
    }

    #region HELPERS
    void Update() => SpikeCheck();
    void DeathHandler() => _OnDeath?.Invoke();
    #endregion

    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(new Vector3(transform.position.x + _positionOffset.x, transform.position.y - _positionOffset.y, 0), _spikeColliderSize);
    }
    #endregion
}