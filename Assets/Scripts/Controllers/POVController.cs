using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POVController : MonoBehaviour
{
    [SerializeField] float _pov;

    Camera _mainCamera;
    Camera _thisCamera;

    // Start is called before the first frame update
    void Awake()
    {
        _mainCamera = transform.parent.GetComponent<Camera>();
        _thisCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _mainCamera.fieldOfView = _pov;
        _thisCamera.fieldOfView = _pov;
    }
}
