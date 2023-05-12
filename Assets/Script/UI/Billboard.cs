using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Transform cam;
    void Start()
    {
        cam = GameObject.Find("PlayerController/CameraHolder/CameraRecoil/MainCamera").transform;
    }

    void LateUpdate()
    {
        if (cam  != null) transform.LookAt(transform.position + cam.forward);
    }
}
