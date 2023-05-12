using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Setting")]

    [SerializeField] int damage;
    [SerializeField] int magzineSize, bulletPerShot;
    [SerializeField] float spread, range, reloadTime, timeBetween, readyShootTime;

    //
    //[SerializeField] float recoilForce;
    //

    public float damageMultiply;
    public int bulletLeft;
    int bulletShot;
    bool shooting, ready, reloading;

    [SerializeField] bool automatic;

    //[SerializeField] float bobFreq;
    //[SerializeField] float bobAmp;


    [Space(5)]
    [Header("Recoil")]
    [SerializeField] Transform recoilPosition;
    [SerializeField] Transform rotationPoint;
    [SerializeField] float positionRecoilSpeed;
    [SerializeField] float rotationRecoilSpeed;
    [SerializeField] float positionReturnSpeed;
    [SerializeField] float rotationReturnSpeed;
    [SerializeField] Vector3 recoilRotation = new Vector3(10, 5, 7);
    [SerializeField] Vector3 recoilKickBack = new Vector3(-0.02f, 0, 0.2f);
    Vector3 originalPosition;
    Vector3 rotationRecoil;
    Vector3 positionRecoil;
    Vector3 rot;


    [Space(10)]
    [Header("References")]

    [SerializeField] Transform weaponHolder;
    [SerializeField] PlayerMovement pm;
    [SerializeField] Camera cam;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask triggerLayer;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletParent;

    [SerializeField] CameraRecoil recoil;
    [SerializeField] CameraShake camShake;
    [SerializeField] float camShakeDuration;
    [SerializeField] float camShakeStrength;

    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject bulletHole;
    [SerializeField] public TextMeshProUGUI bulletUI;
    [SerializeField] GameObject reloadUI;
    [SerializeField] GameObject realoadBarUI;
    [SerializeField] MovingBar reloadBar;

    public Animator anime;
    bool reloadUIOn;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event shootEvent;
    [SerializeField] AK.Wwise.Event reloadEvent;
    [SerializeField] AK.Wwise.Event reloadDoneEvent;
    [SerializeField] AK.Wwise.Switch[] magazine;

    //Vector3 m_WeaponRecoilLocalPosition;
    //Vector3 m_AccumulatedRecoil;
    //float recoilSharpness = 50f;
    //float maxRecoilDistance = 1f;
    //float recoilRestitutionSharpness = 10f;

    private void Start()
    {
        Debug.Log("this should be called twice");
        bulletLeft = magzineSize;
        ready = true;
        shooting = false;
        reloading = false;
        recoil = GameObject.Find("CameraHolder/CameraRecoil").GetComponent<CameraRecoil>();
        anime = GetComponent<Animator>();
        originalPosition = recoilPosition.localPosition;
        reloadUI.SetActive(false);
        magazine[1].SetValue(gameObject);
        //SetBulletUI();
        //m_WeaponRecoilLocalPosition = transform.localPosition;
        //m_AccumulatedRecoil = Vector3.zero;
    }

    private void Update()
    {
        MyInput();
    }

    private void LateUpdate()
    {
        rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, rotationReturnSpeed * Time.deltaTime);
        positionRecoil = Vector3.Lerp(positionRecoil, originalPosition, positionReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionRecoil, positionRecoilSpeed * Time.deltaTime);
        rot = Vector3.Slerp(rot, rotationRecoil, rotationRecoilSpeed * Time.deltaTime);
        rotationPoint.localRotation = Quaternion.Euler(rot);
    }


    private void MyInput()
    {
        if(automatic)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magzineSize && !reloading)
        {
            Reload();
        }
        if (ready && shooting && !reloading && bulletLeft > 0)
        {
            bulletShot = bulletPerShot;
            shootEvent.Post(gameObject);
            Shoot();
        }
        else if (ready && Input.GetKeyDown(KeyCode.Mouse0) && !reloading && bulletLeft <= 0)
        {
            magazine[0].SetValue(gameObject);
            shootEvent.Post(gameObject);
        }
        // Debug.Log(ready + " " + shooting + " " + reloading + " " + bulletLeft);
        //if (bulletLeft <= 0 && !reloadUIOn) ShowReloadUI();
        //else HideReloadUI();
    }


    private void Shoot()
    {
        //Debug.Log("!");
        ready = false;

        //Debug.Log(ready + " " + shooting + " " + reloading + " " + bulletLeft);
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);

        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, ~triggerLayer))
        {
            //Debug.Log(rayHit.collider.gameObject);

            if (rayHit.collider.CompareTag("enemy"))
            {
                //Debug.Log("Hit Enemy");
                rayHit.collider.GetComponent<EnemyAI>().TakeDamage(damage * damageMultiply);
            }
        }
        //StartCoroutine(camShake.Shake(camShakeDuration, camShakeStrength));
        recoil.FireRecoil();
        if (rayHit.collider != null && !rayHit.collider.CompareTag("enemy"))
        {
            
            Vector3 gunMarkDirection = rayHit.point - attackPoint.position;
            float angle = Vector3.Angle(rayHit.point, attackPoint.position);
            //Debug.Log(angle);
            Instantiate(bulletHole, rayHit.point, Quaternion.LookRotation(rayHit.normal, rayHit.transform.up));
        }

        //Debug.Log(rayHit.point);
        anime.SetTrigger("WeaponFire");
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity, cam.transform);

        rotationRecoil += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
        positionRecoil += new Vector3(Random.Range(-recoilKickBack.x, recoilKickBack.x), Random.Range(-recoilKickBack.y, recoilKickBack.y), -recoilKickBack.z);

        //m_AccumulatedRecoil += Vector3.back * recoilForce;
        //m_AccumulatedRecoil = Vector3.ClampMagnitude(m_AccumulatedRecoil, maxRecoilDistance);
        ////

        //Debug.Log(m_AccumulatedRecoil);
        //Debug.Log(m_WeaponRecoilLocalPosition);

        bulletLeft--;
        bulletShot--;
        if (bulletLeft <= 0) ShowReloadUI();
        SetBulletUI();
        
        
        Invoke(nameof(ResetShooting), readyShootTime);

        if (bulletLeft > 0 && bulletShot > 0)
            Invoke(nameof(Shoot), timeBetween);
    }

    private void ResetShooting()
    {
        ready = true;
    }
    private void Reload()
    {
        reloading = true;
        anime.SetTrigger("WeaponReload");
        //if (realoadBarUI.activeSelf == false)
        //realoadBarUI.SetActive(true);
        ShowReloadUI();
        reloadEvent.Post(gameObject);
        StartCoroutine(ReloadBar());
        Invoke(nameof(FinishReload), reloadTime);
        //anime.
    }

    private void FinishReload()
    {
        bulletLeft = magzineSize;
        reloadDoneEvent.Post(gameObject);
        HideReloadUI();
        SetBulletUI();
        magazine[1].SetValue(gameObject);
        reloading = false;
    }

    public void SetBulletUI()
    {
        bulletUI.SetText(bulletLeft / bulletPerShot + "/" + magzineSize / bulletPerShot);
    }

    public void ShowReloadUI()
    {
        reloadUI.SetActive(true);
        realoadBarUI.SetActive(true);
    }

    public void HideReloadUI()
    {
        reloadBar.SetValue(0f);
        reloadUI.SetActive(false);
        realoadBarUI.SetActive(false);
    }

    private IEnumerator ReloadBar()
    {
        float value = 0f;
        reloadBar.SetMax(this.reloadTime);
        
        while (value < this.reloadTime)
        {
            value += Time.fixedDeltaTime;
            reloadBar.SetValue(value);
            yield return new WaitForFixedUpdate();
        }
    }



    //void UpdateWeaponRecoil()
    //{
    //    if (m_WeaponRecoilLocalPosition.z >= m_AccumulatedRecoil.z * 0.99f)
    //    {
    //        m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, m_AccumulatedRecoil, recoilSharpness * Time.deltaTime);

    //    }


    //    else
    //    {
    //        //Debug.Log("Go back");
    //        m_WeaponRecoilLocalPosition = Vector3.Lerp(m_WeaponRecoilLocalPosition, Vector3.zero, recoilRestitutionSharpness * Time.deltaTime);
    //        m_AccumulatedRecoil = m_WeaponRecoilLocalPosition;
    //    }
    //}

    //Vector3 UpdateWeaponBob()
    //{
    //    Vector3 pos = Vector3.zero;
    //    if (pm.isGrounded && pm.rb.velocity.magnitude > 0)
    //    {
    //        pos.y += Mathf.Sin(Time.time * bobFreq) * bobAmp;
    //        pos.x += Mathf.Cos(Time.time * bobFreq / 2) * bobAmp * 2;
    //    }
    //    else pos = Vector3.zero;

    //    return pos;
    //}
}
