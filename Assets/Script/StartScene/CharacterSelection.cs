using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] GameObject lightDiscription;
    [SerializeField] GameObject shadowDiscription;
    [SerializeField] GameObject lightConfirm;
    [SerializeField] GameObject shadowConfirm;

    [SerializeField] Animator fade;

    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event hover;
    [SerializeField] AK.Wwise.Event click;

    public void OnLight()
    {
        shadowDiscription.SetActive(false);
        lightDiscription.SetActive(true);
        shadowConfirm.SetActive(false);
        lightConfirm.SetActive(true);
    }

    public void OnShadow()
    {
        lightDiscription.SetActive(false);
        shadowDiscription.SetActive(true);
        lightConfirm.SetActive(false);
        shadowConfirm.SetActive(true);
    }

    public void LightConfirm()
    {
        fade.SetTrigger("fade");
        PlayerPrefs.SetString("PlayerCharacter", "Light");
    }

    public void ShadowConfirm()
    {
        fade.SetTrigger("fade");
        PlayerPrefs.SetString("PlayerCharacter", "Shadow");
    }

    public void OnHover()
    {
        hover.Post(gameObject);
    }

    public void OnClick()
    {
        click.Post(gameObject);
    }
}
