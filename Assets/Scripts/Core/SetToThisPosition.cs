using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToThisPosition : MonoBehaviour
{
    [SerializeField] float _xOffset = 5f;
    [SerializeField] float _yOffset = 5f;
    public void SetObjectToThisPosition(GameObject obj) => obj.transform.position = new Vector3((this.transform.position.x - (this.transform.localScale.x / 2)) + _xOffset, (this.transform.position.y / 2) + _yOffset, this.transform.position.z);
}
