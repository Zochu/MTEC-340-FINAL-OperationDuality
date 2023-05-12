using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaracterFadeToMain : FadeToScene
{
    [SerializeField] GameObject[] toEnable;

    private void Awake()
    {
        this.GetComponent<Animator>().SetTrigger("fadeIn");
    }

    public void ToEnable()
    {
        foreach (GameObject gb in toEnable)
        {
            gb.SetActive(true);
        }
    }

    public override void ToMain()
    {
        SceneManager.LoadSceneAsync("MainLevel", LoadSceneMode.Single);
    }

}
