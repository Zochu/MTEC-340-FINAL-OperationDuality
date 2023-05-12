using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    [SerializeField] string checkPoint;
    [SerializeField] bool needProjector;

    private void OnTriggerEnter(Collider other)
    {
        if (needProjector)
        {
            if (other.CompareTag("Player") && GameBehaviour.Instance.projecterON) GameBehaviour.Instance.SetCheckPoint(checkPoint);
        }
        else
        {
            if (other.CompareTag("Player") && !GameBehaviour.Instance.projecterON) GameBehaviour.Instance.SetCheckPoint(checkPoint);
        }

    }
}
