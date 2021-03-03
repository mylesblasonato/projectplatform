using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] LayerMask _layerMask;

    public float Range => _range;
    public LayerMask LayerMask => _layerMask;
}
