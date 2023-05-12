using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUIset : MonoBehaviour
{
    [SerializeField] protected Image skill1;
    [SerializeField] protected Image skill2;

    [SerializeField] protected GameObject bar1;
    [SerializeField] protected GameObject bar2;

    [SerializeField] protected MovingBar cdValue1;
    [SerializeField] protected MovingBar cdValue2;

    [SerializeField] float skill1Time;
    [SerializeField] protected float skill1CD;
    [SerializeField] float skill2Time;
    [SerializeField] float skill2CD;

    protected Color original;
    protected Color faded;
    protected float bar1Value = 0;
    protected float bar2Value = 0;

    protected virtual void Start()
    {
        //cdValue1 = GameObject.Find("Canvas/LightSkill/Skill1_Icon/Skill1_Bar").GetComponent<MovingBar>();
        //cdValue2 = GameObject.Find("Canvas/LightSkill/Skill2_Icon/Skill2_Bar").GetComponent<MovingBar>();

        bar1.SetActive(false);
        bar2.SetActive(false);

        original = skill1.color;

        faded = skill1.color;

        faded.a = 0.2f;
    }

    public virtual void SetTransparency(int skill, bool state)
    {
        if (skill == 1)
        {
            if (state == true)
            {
                skill1.color = original;
                bar1.SetActive(false);
            }
            else
            {
                skill1.color = faded;
                bar1.SetActive(true);
                bar1Value = 10;
                StartCoroutine(SetBar1());
            }
        }
        else if (skill == 2)
        {
            if (state == true)
            {
                skill2.color = original;
                bar2.SetActive(false);
            }
            else
            {
                skill2.color = faded;
                bar2.SetActive(true);
                bar2Value = 10;
                StartCoroutine(SetBar2());
            }
        }
    }

    protected virtual IEnumerator SetBar1()
    {
        while (bar1Value > 0)
        {
            bar1Value -= Time.deltaTime * 1.5f;
            cdValue1.SetValue(bar1Value);
            //Debug.Log(bar1Value);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(0.5f);

        while (bar1Value < skill1CD)
        {
            bar1Value += Time.deltaTime * 1.5f;
            cdValue1.SetValue(bar1Value);
            //Debug.Log(bar1Value);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    

    protected virtual IEnumerator SetBar2()
    {
        while (bar2Value > 0)
        {
            bar2Value -= Time.deltaTime * 5.5f;
            cdValue2.SetValue(bar2Value);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(0.5f);

        while (bar2Value < skill1CD)
        {
            bar2Value += Time.deltaTime;
            cdValue2.SetValue(bar2Value);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    
}
