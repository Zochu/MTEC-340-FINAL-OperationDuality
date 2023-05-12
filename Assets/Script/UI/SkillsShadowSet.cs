using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using static UnityEngine.UI.Image;

public class SkillsShadowSet : SkillsUIset
{
    [SerializeField] PlayerBehaviour pb;
    private bool isCoolingDown;
    private float _startTime;
    public float _currentTime;
    private float _changedCDTime;
    protected override void Start()
    {
        //pb = GameObject.Find("PlayerController/Player").GetComponent<PlayerBehaviour>();
        //cdValue1 = GameObject.Find("Canvas/ShadowSkill/Shadow_Skill1_Icon/Shadow_Skill1_Bar").GetComponent<MovingBar>();
        //cdValue2 = GameObject.Find("Canvas/ShadowSkill/Shadow_Skill2_Icon/Shadow_Skill2_Bar").GetComponent<MovingBar>();

        bar1.SetActive(false);
        bar2.SetActive(false);

        original = skill1.color;

        faded = skill1.color;

        faded.a = 0.2f;
    }

    private void Update()
    {
        if(isCoolingDown)
        {
            if (_currentTime - _startTime < _changedCDTime)
            {
                cdValue1.SetValue(Remap(_currentTime - _startTime, 0, _changedCDTime, 0, 10f));
                _currentTime += Time.deltaTime;
            }
            else
            {
                isCoolingDown = false;
            }
        }
    }
    public override void SetTransparency(int skill, bool state)
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
                bar1Value = 0;
                isCoolingDown = true;
                _currentTime = Time.time;
                _startTime = Time.time;
                cdValue1.SetValue(0);
                _changedCDTime = 10f - (10f * (1 - pb.health / pb.maxHealth));
                Debug.Log(_changedCDTime);
                //StartCoroutine(SetBar1());
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

    //protected override IEnumerator SetBar1()
    //{
    //    float delta = 1.5f + (1.5f * (1 - pb.health / pb.maxHealth) * 2.1f);
    //    Debug.Log("From SkillUISet    " + pb.health / pb.maxHealth);
    //    Debug.Log("Bar1Value : " + bar1Value + "   skill1CD : "  + skill1CD);
    //    while (bar1Value < skill1CD)
    //    {
    //        bar1Value += Time.deltaTime * delta;
    //        cdValue1.SetValue(bar1Value);
    //        yield return new WaitForSeconds(Time.deltaTime);
    //    }
    //}

    protected override IEnumerator SetBar2()
    {
        while (bar2Value > 0)
        {
            bar2Value -= Time.deltaTime * 2.1f;
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


    float Remap(float source, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
    {
        return targetFrom + (source - sourceFrom) * (targetTo - targetFrom) / (sourceTo - sourceFrom);
    }

}
