using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringPlatform : MonoBehaviour
{
    [SerializeField] float springForce;
    Animator anime;

    [SerializeField] AK.Wwise.Event springEvent;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            anime.SetBool("SpringTrigger", true);
            springEvent.Post(gameObject);
            collision.transform.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * springForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            anime.SetBool("SpringTrigger", false);
        }
    }
}
