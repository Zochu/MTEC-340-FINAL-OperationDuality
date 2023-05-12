using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ProjectorTarget : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject pointLight;
    [SerializeField] GameObject bubble;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] KeyCode interactKey;
    [SerializeField] float checkDistance;
    [SerializeField] GameObject message;
    ParticleSystem.MainModule mainModule;
    bool playerIn;
    bool projectorOn;
    [SerializeField] ShowPickUpMessage canvas;
    [SerializeField] GameObject tutorial;
    [SerializeField] AK.Wwise.Event projectorOnEvent;

    void Start()
    {
        projectorOn = false;
        pointLight.SetActive(false);
        bubble.SetActive(false);
        if (particle.isPlaying) particle.Stop();
        mainModule = particle.main;
        mainModule.simulationSpeed = 3f;
    }

    void Update()
    {
        if (projectorOn == true) return;
        playerIn = Physics.CheckSphere(transform.position, checkDistance, playerLayer);
        if (playerIn && Input.GetKeyUp(interactKey) && GameBehaviour.Instance.haveKey)
        {
            //Debug.Log(GameBehaviour.Instance.haveKey);
            //add key to GameBehaviour
            ProjectorOn();
        }
    }


    void ProjectorOn()
    {
        projectorOn = true;
        pointLight.SetActive(true);
        bubble.SetActive(true);
        particle.Play();
        projectorOnEvent.Post(gameObject);
        canvas.HideMessage();
        GameBehaviour.Instance.ProjectorOn();
        GameBehaviour.Instance.SetCheckPoint("projector");
        //canvas.enabled = false;
        message.SetActive(false);
        if (tutorial != null) Destroy(tutorial);
    }
    
}
