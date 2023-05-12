using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [SerializeField] float maxRecoilX;
    [SerializeField] float recoilXResetTime;
    [SerializeField] float recoilXDefault;
    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;

    [SerializeField] float snapiness;
    [SerializeField] float returnSpeed;

    float recoilXCounter;

    private void LateUpdate()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapiness * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        if (recoilXCounter > 0) recoilXCounter -= Time.deltaTime;
        else recoilX = recoilXDefault;
    }

    public void FireRecoil()
    {
        recoilX--;
        recoilXCounter = recoilXResetTime;
        if (recoilX < maxRecoilX) recoilX = maxRecoilX;
        targetRotation = new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
