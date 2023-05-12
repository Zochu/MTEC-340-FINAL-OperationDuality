using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] KeyCode interacte;
    [SerializeField] WeaponSystem myGun;
    [SerializeField] Rigidbody rb;
    [SerializeField] BoxCollider coll;
    [SerializeField] Transform player, weaponHolder, cam;

    [SerializeField] float pickUpRange;
    [SerializeField] float dropForwardForce;
    [SerializeField] float dropUpwardForce;

    public static bool slotFull;
    public bool isEquipped;
    [SerializeField] Animator anime;
    [SerializeField] Vector3 pickUpOffset;
    [SerializeField] GameObject reloadUI;

    [Space(20)]
    [SerializeField] bool isPistol;
    [SerializeField] bool isWayPointHere;
    [SerializeField] WayPoint wayPoint;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Switch weaponType;
    [SerializeField] AK.Wwise.Event weaponPickUp;
    [SerializeField] AK.Wwise.Event weaponDrop;

    private void Start()
    {
        if(!isEquipped)
        {
            myGun.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            anime.enabled = false;
        }
        if (isEquipped)
        {
            myGun.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            anime.enabled = true;
        }
    }

    private void Update()
    {
        Vector3 playerDistance = player.position - transform.position;

        if (Input.GetKeyUp(interacte) && !slotFull && !isEquipped && playerDistance.magnitude < pickUpRange)
            PickUp();
        else if (Input.GetKeyUp(interacte) && isEquipped)
            Drop();
;    }

    private void PickUp()
    {
        isEquipped = true;
        slotFull = true;
        myGun.bulletUI.enabled = true;
        transform.SetParent(weaponHolder);
        transform.localPosition = Vector3.zero + pickUpOffset;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;
        coll.isTrigger = true;
        myGun.SetBulletUI();
        myGun.enabled = true;

        weaponType.SetValue(gameObject);
        weaponPickUp.Post(gameObject);

        anime.enabled = true;

        if (myGun.bulletLeft <= 0) reloadUI.SetActive(true);

        if (isPistol && isWayPointHere) wayPoint?.SetToKey();
    }

    private void Drop()
    {
        reloadUI.SetActive(false);
        isEquipped = false;
        slotFull = false;

        transform.SetParent(null);
        myGun.bulletUI.enabled = false;
        rb.isKinematic = false;
        coll.isTrigger = false;

        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(cam.forward * dropForwardForce, ForceMode.Impulse); //Not adding force;
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-10, 10);
        rb.AddTorque(new Vector3(random, random, random));

        weaponDrop.Post(gameObject);

        myGun.enabled = false;

        anime.enabled = false;
    }
}
