using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFanTrigger : MonoBehaviour
{
    [SerializeField] LayerMask player;
    [SerializeField] float fanForce;
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("triggered");
        if (other.CompareTag("Player"))
        {
            //Debug.Log("PushPlayer");
            other.gameObject.GetComponentInParent<Rigidbody>().AddForce(transform.up * fanForce, ForceMode.Force);
        }
    }
}
