using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JMF/Mad Tuner/New SoFloat", fileName = "New SoFloat Variable", order = 2)]
public class SoFloat : ScriptableObject
{
    [SerializeField] float _value;
    public float Value { get { return _value; } set { _value = value; } }
}
