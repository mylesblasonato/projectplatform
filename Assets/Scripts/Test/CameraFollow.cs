using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform _objectToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_objectToFollow.position.x, transform.position.y, transform.position.z);
    }
}
