using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DClimb : MonoBehaviour
{
    [SerializeField] SoFloat _gravityScale;

    Rigidbody2D _rb;
    
    public bool _isClimbing = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isClimbing)
            _rb.gravityScale = 0;
        else
            _rb.gravityScale = _gravityScale.Value;
    }

    public void Climb(float isClimbing)
    {
        if (isClimbing > 0)
            _isClimbing = true;
        else
            _isClimbing = false;
    }
}
