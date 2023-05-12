using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float sensitivityX;
    [SerializeField] float sensitivityY;

    [SerializeField] float walkFreq;
    [SerializeField] float walkAmp;

    [SerializeField] Transform playerPosition;
    [SerializeField] Transform cam;

    [SerializeField] Vector3 cameraOffset = new Vector3(0, 0, 0); //Doesn't work, could be deleted;

    float rotationX;
    float rotationY;

    private Vector3 FootStepMostion()
    {
        if (GameBehaviour.Instance.isDead)
            return Vector3.zero;
        float freq;
        float amp;
        if (Input.GetKey(KeyCode.LeftShift))
            freq = walkFreq * 1.3f;
        else
            freq = walkFreq;

        if (WallRunning.isWallRunning)
            amp = walkAmp * 0.8f;
        else
            amp = walkAmp;
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * freq) * amp;
        pos.x += Mathf.Cos(Time.time * freq / 2) * amp * 2;
        return pos;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotationY = 90f;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90, 90);

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
            cam.transform.position = playerPosition.position + cameraOffset + FootStepMostion();
        else
            cam.transform.position = playerPosition.position + cameraOffset;

        cam.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        playerPosition.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    public void Fov(float endPoint)
    {
        GetComponent<Camera>().DOFieldOfView(endPoint, 0.25f);
    }

    public void Tilt(float value)
    {
        //float degrees = Mathf.Rad2Deg * value;
        //Quaternion to = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, degrees * 10);
        //transform.DOLocalRotate(new Vector3(0, 0, value), 0.25f);
        //transform.rotation = Quaternion.Slerp(transform.rotation, to, 0.1f);
        //transform.rotation = to;

        //Vector3 angles = transform.eulerAngles;
        //angles.z = Mathf.Rad2Deg * value;
        //transform.eulerAngles = angles;

        //Debug.Log(transform.rotation.eulerAngles);

        transform.DOLocalRotate(new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, value), 0.25f);
    }

}
