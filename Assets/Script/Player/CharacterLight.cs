using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLight : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] PlayerBehaviour pb;
    [SerializeField] SkillsUIset skillUI;
    [SerializeField] LightShieldAnime shieldAnme;
    [SerializeField] KeyCode skillInput1;
    [SerializeField] KeyCode skillInput2;
    [SerializeField] Transform player;
    [SerializeField] PlayerCamera cam;

    //active skill 1
    [Header("Skill 1")]
    [SerializeField] GameObject shield;
    [SerializeField] float shieldTime;
    [SerializeField] float skillCD1;
    bool isSkillActive1;
    GameObject newShield;

    //active skill 2
    [Header("Skill 2")]
    [SerializeField] float strongTime;
    [SerializeField] float skillCD2;
    [SerializeField] bool isSkillActive2;
    [SerializeField] float increasedMaxHealth;
    [SerializeField] float normalHealth;
    float healingTimer;
    public bool isStrong;

    //passive skill
    [Header("Passive")]
    [SerializeField] float strongerAmount;

    //[Header("SetHUD")]
    //[SerializeField] GameObject lightHUD;
    //[SerializeField] GameObject shadowHUD;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event skill1UpEvent;
    [SerializeField] AK.Wwise.Event skill1DownEvent;
    [SerializeField] AK.Wwise.Event buffUpEvent;
    [SerializeField] AK.Wwise.Event buffDownEvent;

    private void Start()
    {
        pb = GetComponent<PlayerBehaviour>();
        //lightHUD.SetActive(true);
        //shadowHUD.SetActive(false);
        //skillUI = GameObject.Find("Canvas/LightSkill").GetComponent<SkillsUIset>();
        isSkillActive1 = false;
        isSkillActive2 = false;
        pb.healingSpeedUp = strongerAmount;
        normalHealth = pb.maxHealth;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(skillInput1) && !isSkillActive1)
        {
            ShieldUp();
            Invoke(nameof(ShieldDown), shieldTime);
        }

        if (Input.GetKeyDown(skillInput2) && (pb.health < pb.maxHealth))
        {
            StrongBody();
            Invoke(nameof(NormalBody), strongTime);
        }

        if (isStrong) pb.healingCount = -1; //Should be overwriting player's healingCount, so player is always healing
    }


    //Skill 1
    private void ShieldUp()
    {
        isSkillActive1 = true;
        newShield = Instantiate(shield, player.position + player.forward*3, player.rotation);
        skill1UpEvent.Post(gameObject);
        skillUI.SetTransparency(1, false);
        shieldAnme = newShield.GetComponent<LightShieldAnime>();
        shieldAnme.ShieldUp();
    }


    private void ShieldDown()
    {
        shieldAnme.ShieldDown();
        Destroy(newShield, 0.2f);
        skill1DownEvent.Post(gameObject);
        Invoke(nameof(ShieldCD), skillCD1);
    }

    private void ShieldCD()
    {
        isSkillActive1 = false;
        skillUI.SetTransparency(1, true);
    }



    //Skill2
    private void StrongBody()
    {
        //Debug.Log("I am strong");
        isSkillActive2 = true;
        isStrong = true;
        pb.maxHealth = increasedMaxHealth;
        pb.healingSpeedUp = strongerAmount * 0.5f;
        buffUpEvent.Post(gameObject);
        skillUI.SetTransparency(2, false);
        cam.Fov(75);
    }

    private void NormalBody()
    {
        isStrong = false;
        pb.healingSpeedUp = strongerAmount;
        pb.maxHealth = normalHealth;
        //buffDownEvent.Post(gameObject);
        cam.Fov(65);
        Invoke(nameof(StrongReady), skillCD2);
    }

    private void StrongReady()
    {
        isSkillActive2 = false;
        skillUI.SetTransparency(2, true);
    }
}
