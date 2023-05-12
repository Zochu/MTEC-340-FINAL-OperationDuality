using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;

    public bool haveKey { get; private set; }
    public bool projecterON { get; private set; }

    public int lives { get; private set; }
    public bool isDead { get; private set; }

    public bool paused { get; private set; }


    [Space(20)]
    [Header("Setting")]
    [SerializeField] KeyCode pauseKey;
    [SerializeField] float deadWaitTime;

    [Space(20)]
    [Header("CheckPoints")]
    [SerializeField] Transform homeCheckpoint;
    [SerializeField] Transform keyCheckpoint;
    [SerializeField] Transform insideCheckpoint;
    [SerializeField] Transform projectorCheckpoint;
    [SerializeField] Transform escapeCheckpoint;
    Transform curretnCheckpoint;

    [Space(20)]
    [Header("Reference")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Transform player;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject[] enemies;
    [SerializeField] PlayerBehaviour pb;
    [SerializeField] PlayerMovement pm;
    [SerializeField] PlayerCamera cam;
    [SerializeField] WayPoint wayPoint;
    [SerializeField] GameObject[] disableOnDeath;
    [SerializeField] TextMeshProUGUI questBar;
    [SerializeField] Animator[] questBarAnim;

    [Space(20)]
    [Header("GameOver")]
    [SerializeField] GameObject gameOverUI;
    [SerializeField] DateTime startTime;
    [SerializeField] TextMeshProUGUI finalState;
    [SerializeField] TextMeshProUGUI timeSpentText;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event ambienceEvent;
    [SerializeField] AK.Wwise.Event playerDeadEvent;
    [SerializeField] AK.Wwise.Event playerResetEvent;
    [SerializeField] AK.Wwise.Event pauseEvent;
    [SerializeField] AK.Wwise.Event unPauseEvent;
    [SerializeField] AK.Wwise.Event stopAllEvents;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        startTime = DateTime.Now;
        lives = 3;
        curretnCheckpoint = homeCheckpoint;
        playerResetEvent.Post(gameObject);
        paused = false;
        pauseMenu.SetActive(false);
        gameOverUI.SetActive(false);
        SetCheckPoint("home");
        questBar.text = "Find the Key";
        ambienceEvent.Post(gameObject);
    }

    private void LateUpdate()
    {
        PauseUnPause();
    }

    public void PauseUnPause()
    {
        if (Input.GetKeyUp(pauseKey))
        {
            if (paused)
            {
                paused = false;
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                unPauseEvent.Post(gameObject);
                pauseMenu.SetActive(false);
            }
            else
            {
                paused = true;
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseEvent.Post(gameObject);
                pauseMenu.SetActive(true);
            }
        }
    }

    public void PlayerGetKey()
    {
        wayPoint.SetToProjector();
        haveKey = true;
        QuestBarFadeOut();
        StartCoroutine(SetQuestBar("Turn On the Projector"));
    }

    public void ProjectorOn()
    {
        wayPoint.SetToEscape();
        projecterON = true;
        QuestBarFadeOut();
        StartCoroutine(SetQuestBar("Escape"));
        Invoke(nameof(EnemieBack), 2f);
    }

    private void QuestBarFadeOut()
    {
        foreach (Animator anim in questBarAnim)
        {
            anim.SetTrigger("FadeOut");
        }
    }

    private IEnumerator SetQuestBar(string content)
    {
        yield return new WaitForSeconds(1f);
        questBar.text = content;
        foreach (Animator anim in questBarAnim)
        {
            anim.SetTrigger("FadeIn");
        }
    }


    public void EnemieBack()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf == false) enemy.SetActive(true);
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            enemyScript.health = 120;
            enemyScript.SetHealthBar();
        }
    }

    public void SetCheckPoint(string checkPoint)
    {
        switch(checkPoint)
        {
            case "home":
                curretnCheckpoint = homeCheckpoint;
                break;

            case "key":
                curretnCheckpoint = keyCheckpoint;
                break;

            case "inside":
                curretnCheckpoint = insideCheckpoint;
                break;

            case "projector":
                curretnCheckpoint = projectorCheckpoint;
                break;

            case "escape":
                curretnCheckpoint = escapeCheckpoint;
                break;

            default:
                curretnCheckpoint = homeCheckpoint;
                break;
        }

        Debug.Log("Current CheckPoint :  " + curretnCheckpoint.name);
    }

    public void PlayerDead()
    {
        pb.playerOnDeath = false;
        playerDeadEvent.Post(gameObject);
        if (lives > 0)
        {
            lives -= 1;
            isDead = true;
            player.gameObject.SetActive(false);
            weaponHolder.SetActive(false);
            foreach(GameObject go in disableOnDeath)
            {
                go.SetActive(false);
            }
            Invoke(nameof(GoBackToCheckPoint), deadWaitTime);
        }
        else if (lives <= 0)
        {
            GameOver();
        }
    }

    public void GoBackToCheckPoint()
    {
        player.position = curretnCheckpoint.position;
        playerResetEvent.Post(gameObject);
        if (player.gameObject.activeSelf == false)
        {
            isDead = false;
            player.gameObject.SetActive(true);
            weaponHolder.SetActive(true);
            foreach (GameObject go in disableOnDeath)
            {
                go.SetActive(true);
            }
            pb.health = pb.maxHealth;
            pb.bar.SetValue(pb.health);
        }
    }

    public void PlayerEscape()
    {
        GameOver();
    }

    public void GameOver()
    {
        stopAllEvents.Post(pb.gameObject);
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.gameObject.SetActive(false);
        finalState.text = lives < 0 ? "MISSION FAILED" : "MISSION ACCOMPLISHED";
        timeSpentText.text = "TIME USED : " + CalculateTime();
    }

    public String CalculateTime()
    {
        TimeSpan timeSpent = DateTime.Now - startTime;
        Debug.Log("time spent: " + timeSpent.ToString(@"hh\:mm\:ss"));
        return timeSpent.ToString(@"hh\:mm\:ss");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }
}
