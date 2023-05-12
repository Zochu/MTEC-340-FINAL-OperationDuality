using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] KeyCode pickUpKey;
    [SerializeField] AK.Wwise.Event keyPickUpEvent;
    bool playerIn;
    void Update()
    {
        playerIn = Physics.CheckSphere(transform.position, checkDistance, playerLayer);
        if(playerIn && Input.GetKeyUp(pickUpKey))
        {
            //add key to GameBehaviour
            GameBehaviour.Instance.PlayerGetKey();
            Destroy(gameObject, 0.2f);
            
        }
    }

    private void OnDestroy()
    {
        keyPickUpEvent.Post(gameObject);
        GameBehaviour.Instance.SetCheckPoint("key");
    }
}
