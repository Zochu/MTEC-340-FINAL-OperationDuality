using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameBehaviour.Instance.projecterON)
        {
            if (other.CompareTag("Player"))
            {
                GameBehaviour.Instance.PlayerEscape();
            }
        }
    }
}
