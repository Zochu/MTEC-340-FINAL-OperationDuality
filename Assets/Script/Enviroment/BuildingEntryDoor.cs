using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingEntryDoor : MonoBehaviour
{
    [SerializeField] Animator anime;
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask playerLayer;

    bool nearby;

    private void Start()
    {
        anime = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, checkDistance, playerLayer))
        {
            //nearby = true;
            anime.SetBool("character_nearby", true);
            Debug.Log("Nearby");
        }
        else
        {
            //nearby = false;
            anime.SetBool("character_nearby", false);
        }
    }
}
