using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] float _timeTillDestroy;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, _timeTillDestroy);
    }
}
