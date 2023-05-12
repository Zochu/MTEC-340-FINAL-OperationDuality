using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    [SerializeField] float returnSpeed;
    Rigidbody _rigidBody;

    //private void OnCollisionExit(Collision collision)
    //{
    //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed * Time.deltaTime);
    //}

    Vector3 targetRotation;
    Vector3 currentRotation;
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //if (transform.rotation.z != 0)
        //{
        //    if (transform.rotation.z <= rotationSpeed)
        //    {
        //        transform.rotation = Quaternion.Euler(0, 0, 0);
        //    }
        //    else
        //    {
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), rotationSpeed);
        //    }
        //}

        Vector3 direction = transform.rotation.eulerAngles;
        _rigidBody.angularVelocity = -direction * returnSpeed;
    }

    //void Update()
    //{
    //    if (transform.rotation.z != 0)
    //    {
    //        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
    //        float currentRotationSpeed = 0.0f;
    //        float smoothTime = 0.1f;
    //        float currentVelocity = 0.0f;

    //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);
    //        currentRotationSpeed = Mathf.SmoothDamp(currentRotationSpeed, rotationSpeed, ref currentVelocity, smoothTime);
    //    }
    //}

}
