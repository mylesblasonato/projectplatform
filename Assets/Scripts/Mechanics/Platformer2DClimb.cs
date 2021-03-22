using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer2DClimb : MonoBehaviour
{
    public void Climb(float isClimbing)
    {
        if(isClimbing > 0)
            Debug.Log("CLIMBING");
    }
}
