using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform markerA;
    [SerializeField] Transform markerB;
    [SerializeField] float speed;
    [SerializeField] float waitTime;
    float elapse;
    public bool AorB; // false = A, true = B
    [SerializeField] float distanceThreshold;
    bool isWatingInvoke = false;
    bool positionSet;

    Vector3 nextPosition;
    Vector3 currentPosition;
    private void Start()
    {
        elapse = 0f;
        transform.position = markerA.position;
        currentPosition = markerA.position;
        nextPosition = markerB.position;
        AorB = true;
        positionSet = true;
        //Debug.Log(markerLayer.value);
    }

    private void Update()
    {           

        LetsMove();

    }

    private void LetsMove()
    {
        //Debug.Log(positionSet);
        if(!isWatingInvoke) elapse += speed * Time.deltaTime;

        if (elapse > 1f)
        {
            Invoke(nameof(SetPosition), waitTime);
            elapse = 1f;
            isWatingInvoke = true;
        }
        //Debug.Log(elapse);
        if (!isWatingInvoke) transform.position = Vector3.Lerp(currentPosition, nextPosition, elapse);
    }


    private void SetPosition()
    {
        elapse = 0;
        isWatingInvoke = false;
        //Debug.Log(nextPosition);
        if (nextPosition == markerA.position)
        {
            nextPosition = markerB.position;
            currentPosition = markerA.position;
            AorB = true;
            return;
        }
        else if (nextPosition == markerB.position)
        {
            nextPosition = markerA.position;
            currentPosition = markerB.position;
            AorB = false;
            return;
        }
    }

    
}
