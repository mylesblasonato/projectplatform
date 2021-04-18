using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetToThisPosition : MonoBehaviour
{
    [SerializeField] float _xOffset = 5f;
    [SerializeField] float _yOffset = 5f;
    public void SetObjectToThisPosition(GameObject obj) => obj.transform.localPosition = new Vector3((this.transform.localPosition.x - (this.transform.localScale.x / 2)) + _xOffset, this.transform.localPosition.y + _yOffset, this.transform.localPosition.z);
}
