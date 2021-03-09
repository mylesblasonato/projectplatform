using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New SO Float", menuName ="Scriptable Data Types/Create New Float")]
public class SoFloat : ScriptableObject
{
    [SerializeField] float _value;
    public float Value { get { return _value; } set { _value = value; } }
}
