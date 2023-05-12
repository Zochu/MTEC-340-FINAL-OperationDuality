using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    //[SerializeField] bool reversed;
    //[SerializeField] float rotationSpeed = 500f;
    //[SerializeField] float rotationAngle = 180f;
    //[SerializeField] float waitTime = 5f;

    //private float startTime;
    //float elapsedTime;
    //float rotationAmount;

    //bool flipped;

    //private void Start()
    //{
    //    rotationSpeed = 500f;
    //    startTime = Time.time;
    //}

    //private void Update()
    //{
    //    elapsedTime = Time.time - startTime;


    //    if (!flipped) Flip();

    //    if (!reversed) transform.rotation = Quaternion.Euler(rotationAmount, 0f, 0f);
    //    else transform.rotation = Quaternion.Euler(180 - rotationAmount, 0f, 0f);
    //}

    ////private void Flip()
    ////{
    ////    rotationAmount = Mathf.PingPong(elapsedTime * rotationSpeed, rotationAngle);

    ////    if ((elapsedTime * rotationSpeed) % rotationAngle < 1.7) StartCoroutine(Pause());
    ////    //Debug.Log((elapsedTime * rotationSpeed) % rotationAngle);
    ////}

    //private void Flip()
    //{
    //    rotationAmount = Mathf.PingPong(elapsedTime * rotationSpeed, rotationAngle);

    //    if (elapsedTime * rotationSpeed > 180)
    //}


    //private IEnumerator Pause()
    //{
    //    //Debug.Log("Pause");
    //    flipped = true;
    //    yield return new WaitForSeconds(waitTime);
    //    startTime = Time.time;
    //    flipped = false;
    //}
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] bool reverse;
    [SerializeField] AK.Wwise.Event flipperEvent;
    private bool isRotating = false;


    private void Update()
    {
        if (isRotating)
        {
            transform.rotation = reverse ? Quaternion.Lerp(transform.rotation, Quaternion.Euler(180f, 0f, 0f), Time.deltaTime * rotationSpeed)
                                         : Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = reverse ? Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * rotationSpeed)
                                         : Quaternion.Lerp(transform.rotation, Quaternion.Euler(180f, 0f, 0f), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float chance = Random.Range(0, 10);
        if (chance < 6)
        {
            isRotating = !isRotating;
            flipperEvent.Post(gameObject);
        }
    }

}





