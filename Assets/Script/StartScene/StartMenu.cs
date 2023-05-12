using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject levelMenu;
    [SerializeField] Animator fade;

    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event hover;
    [SerializeField] AK.Wwise.Event click;

    private void Awake()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
    }

    public void OnStart()
    {
        startMenu.SetActive(false);
        levelMenu.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnNewGame()
    {
        fade.SetTrigger("MainLevel");
    }

    public void OnNewTutorial()
    {
        fade.SetTrigger("Tutorial");
    }

    public void OnBack()
    {
        startMenu.SetActive(true);
        levelMenu.SetActive(false);
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
