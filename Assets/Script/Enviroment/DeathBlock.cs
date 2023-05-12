using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour
{
    [SerializeField] PlayerBehaviour pb;
    [SerializeField] float damageAmount;
    //public bool playerOnDeath;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) pb.playerOnDeath = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) pb.playerOnDeath = false;
    }

    private void Update()
    {
        if (pb.playerOnDeath)  pb.TakeDamage(damageAmount/10 * Time.deltaTime); 
    }
}
