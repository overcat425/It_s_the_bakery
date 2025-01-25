using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public Camera cam;
    public Transform stoveEvent;
    public Transform target;
    public Vector3 followVec;
    void Update()
    {
        transform.position = target.position + followVec;
    }
}