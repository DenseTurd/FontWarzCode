using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayFacingForward : MonoBehaviour
{
    void Update()
    {
        transform.forward = Vector3.forward;    
    }
}
