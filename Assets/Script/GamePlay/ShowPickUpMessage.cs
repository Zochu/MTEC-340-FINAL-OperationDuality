using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPickUpMessage : MonoBehaviour
{
    [SerializeField] bool isWeapon;
    [SerializeField] GameObject canvasToShow;
    [SerializeField] float detectRange;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] PickupWeapon pickupWeapon;
    private bool playerInRange;
    private bool showed;
    

    private void Start()
    {
        HideMessage();
    }

    private void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, detectRange, playerLayer);

        

        if (isWeapon && pickupWeapon != null)
        {
            if (pickupWeapon.isEquipped) HideMessage();
            else
            {
                if (playerInRange) ShowMessage();
                else HideMessage();
            }
        }

        else
        {
            if (playerInRange) ShowMessage();
            else HideMessage();
        }
    }

    public void HideMessage()
    {
        canvasToShow.SetActive(false);
        showed = false;
    }

    private void ShowMessage()
    {
        canvasToShow.SetActive(true);
        showed = true;
    }

    //private void IsEquipped()
    //{
    //    HideMessage();
    //}

}
