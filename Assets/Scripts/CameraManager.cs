using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public Vector3 followVec;
    void Update()
    {
        transform.position = target.position + followVec;
    }
}
