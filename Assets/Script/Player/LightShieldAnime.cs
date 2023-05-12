using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShieldAnime : MonoBehaviour
{
    [SerializeField] Animator anime;
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    public void ShieldUp()
    {
        anime.SetTrigger("ShieldUp");
    }

    public void ShieldDown()
    {
        anime.SetTrigger("ShieldDown");
        //Destroy(this, 0.2f);
    }

    
}
