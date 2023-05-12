using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float healingCount;
    public float healingSpeedUp;
    [SerializeField] float healingAmount;
    [SerializeField] float healingtimeBetween;
    [SerializeField] float healingCD;

    [SerializeField] CharacterLight c_light;
    [SerializeField] CharacterShadow c_shadow;

    bool isHealing;
    bool shadowDamage = false;

    [SerializeField] public MovingBar bar;

    [Space(20)]
    public bool playerOnDeath;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event takeDamageEvent;
    [SerializeField] AK.Wwise.Event recoverEvent;

    private void Start()
    {
        c_light = GetComponent<CharacterLight>();
        c_shadow = GetComponent<CharacterShadow>();
        health = maxHealth;
        healingCount = healingCD;
        bar = GameObject.Find("Canvas/Health").GetComponent<MovingBar>();
        bar.SetValue(health);
    }

    private void Update()
    {
        //Debug.Log(isHealing);
        if (c_light.isStrong)
            healingCount = -1;

        if (c_shadow.isSkillActive2)
        {
            if (!shadowDamage)
            {
                healingCount = healingCD;
                shadowDamage = true;
            }
        }
        if (healingCount >= 0 && !c_light.isStrong)
            healingCount = healingCount - Time.deltaTime;
        if (health > maxHealth) health = maxHealth;
        if ((health < maxHealth) && !isHealing && healingCount < 0)
        {
            isHealing = true;
            recoverEvent.Post(gameObject);
            StartCoroutine(StartHealing());
        }

        else if ( health >= maxHealth)
        {
            StopCoroutine(StartHealing());
            isHealing = false;
        }
        

        if (health < 0) PlayerDie();
    }

    private IEnumerator StartHealing()
    {
        //Debug.Log("healing: " + health);
        health++;
        bar.SetValue(health);
        yield return new WaitForSeconds(healingtimeBetween * healingSpeedUp);

        if (health < maxHealth && isHealing)
        {
            StartCoroutine(StartHealing());
        }
    }


    public void TakeDamage(float damage)
    {
        Debug.Log(damage);
        //Debug.Log("health: "+ health);
        c_shadow.DamageRecover();
        health -= damage;
        //takeDamageEvent.Post(gameObject);
        bar.SetValue(health);
        if (!c_light.isStrong)
        {
            StopCoroutine(StartHealing());
            healingCount = healingCD;
            isHealing = false;
        }
        if (health < 0) PlayerDie();

    }

    private void PlayerDie()
    {
        health = 0;
        GameBehaviour.Instance.PlayerDead();
    }
}
