using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideOutPlatform : MonoBehaviour
{

    [SerializeField] Transform marker;
    [SerializeField] Transform slidePlatform;
    [SerializeField] float speed;
    [SerializeField] float stayTime;
    Vector3 originalPosition;
    [SerializeField] AK.Wwise.Event slideEvent;

    bool moveOut;

    private void Start()
    {
        originalPosition = slidePlatform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveOut = true;
            StartCoroutine(LetsMove());
            Invoke(nameof(MoveBack), stayTime);
        }
    }


    private void MoveBack()
    {
        moveOut = false;
        StartCoroutine(LetsMove());
    }

    private IEnumerator LetsMove()
    {
        float elapse = 0f;
        elapse += speed * Time.deltaTime;
        slideEvent.Post(gameObject);
        if (elapse >= 1f) elapse = 1f;
        while (elapse < 1)
        {
            if (moveOut) slidePlatform.position = Vector3.Lerp(slidePlatform.position, marker.position, elapse);
            else slidePlatform.position = Vector3.Lerp(slidePlatform.position, originalPosition, elapse);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


}
