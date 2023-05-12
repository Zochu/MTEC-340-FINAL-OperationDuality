using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeToScene : MonoBehaviour
{

    [SerializeField] GameObject[] toDisable;

    public void ToDisable()
    {
        foreach (GameObject gb in toDisable)
        {
            gb.SetActive(false);
        }
    }
    public virtual void ToMain()
    {
        SceneManager.LoadSceneAsync("CharacterSelection", LoadSceneMode.Single);
    }

    public virtual void ToTutorial()
    {
        SceneManager.LoadSceneAsync("TutorialScene", LoadSceneMode.Single);
    }
}
