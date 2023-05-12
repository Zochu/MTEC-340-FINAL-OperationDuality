using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParentPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.root.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.transform.GetChild(0).transform.parent = null;
        }
    }
}
