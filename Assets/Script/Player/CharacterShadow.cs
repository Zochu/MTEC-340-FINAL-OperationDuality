using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] PlayerBehaviour pb;
    [SerializeField] PlayerMovement pm;
    [SerializeField] WeaponSystem weapon;
    [SerializeField] SkillsShadowSet skillUI;
    [SerializeField] KeyCode skillInput1;
    [SerializeField] KeyCode skillInput2;
    [SerializeField] Transform player;
    [SerializeField] Transform weaponHolder;
    [SerializeField] PlayerCamera cam;

    //active skill 1
    [Header("Skill 1")]
    [SerializeField] float tpDistance;
    [SerializeField] float tpTimeLimit;
    [SerializeField] float tpDragForce;
    [SerializeField] float skillCD1;
    [SerializeField] float skillCD1Divide = 1f;
    bool isSkillActive1;

    //active skill 2
    [Header("Skill 2")]
    [SerializeField] int healthLoss;
    [SerializeField] float damageIncrease;
    [SerializeField] float shadowTime;
    [SerializeField] float shadowMoveSpeed;
    [SerializeField] float skillCD2;
    float damageAmount = 1;
    public bool isSkillActive2;
    bool isShadowPowerOn;

    //passive skill
    
    [SerializeField] float skillCD1DamageRevocer;
    float skillCD1Counter;


    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event skill1Event;
    [SerializeField] AK.Wwise.Event buffUpEvent;
    [SerializeField] AK.Wwise.Event buffDownEvent;
    //[Header("SetHUD")]
    //[SerializeField] GameObject lightHUD;
    //[SerializeField] GameObject shadowHUD;

    private void Start()
    {
        pb = GetComponent<PlayerBehaviour>();
        pm = GetComponent<PlayerMovement>();
        //skillUI = GameObject.Find("Canvas/ShadowSkill").GetComponent<SkillsShadowSet>();
        isSkillActive1 = false;
        isSkillActive2 = false;
        //lightHUD.SetActive(false);
        //shadowHUD.SetActive(true);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(skillInput1) && !isSkillActive1 && pm.rb.velocity.magnitude > 0)
        {
            StartCoroutine(Teleport());
            //Invoke(nameof(TPReady), skillCD1);
        }
        if (isSkillActive1)
            Skill1CDCount();

        if (Input.GetKeyDown(skillInput2) && !isSkillActive2 && (pb.health > healthLoss))
        {
            ShadowPower();
            Invoke(nameof(BackToNormal), shadowTime);
        }
    }


    //Skill1
    private IEnumerator Teleport()
    {
        isSkillActive1 = true;
        pm.limitSpeed = false;
        float percentageResult = pb.health / pb.maxHealth;
        Debug.Log("From Character    " + percentageResult);
        skillCD1Counter = skillCD1 * percentageResult;
        //transform.position += pm.moveDirection * tpDistance;
        pm.rb.AddForce(pm.moveDirection * tpDistance, ForceMode.Impulse);
        skill1Event.Post(gameObject);
        skillUI.SetTransparency(1, false);
       
        yield return new WaitForSeconds(tpTimeLimit);

        pm.limitSpeed = true;
        if (pm.rb.velocity.magnitude > 10f && !isShadowPowerOn)
            pm.rb.AddForce(-pm.moveDirection * tpDistance * 0.3f, ForceMode.Impulse);
        else if (pm.rb.velocity.magnitude > 10f && isShadowPowerOn)
            pm.rb.AddForce(-pm.moveDirection * tpDistance * 0.5f, ForceMode.Impulse);
    }

    private void Skill1CDCount()
    {
        
        if (skillCD1Counter > 0)
        {
            skillCD1Counter -= Time.deltaTime;
            //Debug.Log(skillCD1Counter);
        }
        if (skillCD1Counter <= 0)
        {
            TPReady();
            skillUI.SetTransparency(1, true);
        }
    }

    public void DamageRecover()
    {
        skillCD1Counter -= skillCD1DamageRevocer;
        skillUI._currentTime += 1f;
    }

    //private void TeleportDone()
    //{
    //    Debug.Log("1");
    //    pm.limitSpeed = true;
    //    if(pm.rb.velocity.magnitude > 10f) pm.rb.AddForce(-pm.moveDirection * tpDistance * 0.7f, ForceMode.Impulse);
    //}

    private void TPReady()
    {
        isSkillActive1 = false;
    }

    //Skill2
    private void ShadowPower()
    {
        isSkillActive2 = true;
        isShadowPowerOn = true;
        pb.TakeDamage(healthLoss);
        skillUI.SetTransparency(2, false);
        damageAmount = damageIncrease;
        pm.moveSpeedMultiply = shadowMoveSpeed;
        buffUpEvent.Post(gameObject);
        if (weaponHolder.childCount > 0)
            weaponHolder.GetChild(0).GetComponent<WeaponSystem>().damageMultiply = 2f;
        cam.Fov(75);
    }

    private void BackToNormal()
    {
        damageAmount = 1;
        isShadowPowerOn = false;
        pm.moveSpeedMultiply = 1;
        buffDownEvent.Post(gameObject);
        if (weaponHolder.childCount > 0)
            weaponHolder.GetChild(0).GetComponent<WeaponSystem>().damageMultiply = 1f;
        cam.Fov(65);
        Invoke(nameof(ShadowReady), skillCD2);
    }

    private void ShadowReady()
    {
        isSkillActive2 = false;
        skillUI.SetTransparency(2, true);
    }

}
