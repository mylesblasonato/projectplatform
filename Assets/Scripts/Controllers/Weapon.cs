using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] float _power;
    [SerializeField] LayerMask _layerMask;

    public float Range => _range;
    public float Power => _power;
    public LayerMask LayerMask => _layerMask;
}
