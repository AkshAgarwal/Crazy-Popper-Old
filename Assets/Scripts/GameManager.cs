using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject levelCompletePanel, levelFailedPanel, pausePanel, levelSelectionPanel;
    public Button levelBtn, pauseBtn, undoBtn;
    public Text scoreTxt, attemptTxt;
    RaycastHit2D hit;
    public GameObject explosion, projectileObj;
    public Sprite yellow, blue, purple;
    public int totalPlayerInLevel, numOfTapInLevel;
    int actualScore = 30, attempt;
    static int TOTALTAPS;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void OnEnable()
    {
        totalPlayerInLevel = 0;
        attempt = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        // SET ALL GAME PREFS
        if (PlayerPrefs.GetInt(ResourceManager.ISGameLoadedFirst) == 0)
        {
            PlayerPrefs.SetInt(ResourceManager.ISGameLoadedFirst, 1);
            PlayerPrefs.SetInt(ResourceManager.Level, 1);
            PlayerPrefs.SetInt(ResourceManager.Score, 0);
            PlayerPrefs.SetInt(ResourceManager.TotalUnlockLevel, 1);
        }
        else
        {
            PlayerPrefs.SetInt(ResourceManager.ISGameLoadedFirst, PlayerPrefs.GetInt(ResourceManager.ISGameLoadedFirst));
            PlayerPrefs.SetInt(ResourceManager.Level, PlayerPrefs.GetInt(ResourceManager.Level));
            PlayerPrefs.SetInt(ResourceManager.Score, PlayerPrefs.GetInt(ResourceManager.Score));
            PlayerPrefs.SetInt(ResourceManager.TotalUnlockLevel, PlayerPrefs.GetInt(ResourceManager.TotalUnlockLevel));
        }

        //Open LevelSelectionPanel
        OnLevelBtnClk();
        levelCompletePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnNextLevelBtnClk());
        levelFailedPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnRetryBtnClk());
        pausePanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => OnGamePause());
        pauseBtn.onClick.AddListener(() => OnGamePause());
        levelBtn.onClick.AddListener(() => OnLevelBtnClk());
    }

    public void LoadLevel()
    {
        GetComponent<GridLevel>().LevelMap(PlayerPrefs.GetInt(ResourceManager.Level));
        attemptTxt.text = "# " + numOfTapInLevel;
        scoreTxt.text = "Score: " + PlayerPrefs.GetInt(ResourceManager.Score).ToString();
        TOTALTAPS = numOfTapInLevel;
        if (levelSelectionPanel.activeInHierarchy)
            levelSelectionPanel.SetActive(false);
    }

    private void OnLevelBtnClk()
    {// SHOW LEVEL SELECTION PANEL
        if (!levelSelectionPanel.activeInHierarchy)
            levelSelectionPanel.SetActive(true);
    }

    public void OnLevelComplete()
    {//ADD SCORE ON LEVEL COMPLETE
        SoundManager.instance.Play("Winner");
        if (!levelCompletePanel.activeInHierarchy)
            levelCompletePanel.SetActive(true);
        //INCREASE SCORE
        int currentTotalScore = (actualScore * PlayerPrefs.GetInt(ResourceManager.Level)) - ((attempt) == 1 ? 0 : attempt * 2);
        PlayerPrefs.SetInt(ResourceManager.Score, PlayerPrefs.GetInt(ResourceManager.Score) + currentTotalScore);
        scoreTxt.text = "Score: " + PlayerPrefs.GetInt(ResourceManager.Score).ToString();

    }
    void OnNextLevelBtnClk()
    {
        //INCREASE LEVEL
        if (PlayerPrefs.GetInt(ResourceManager.Level) < 5)
        {
            PlayerPrefs.SetInt(ResourceManager.Level, PlayerPrefs.GetInt(ResourceManager.Level) + 1);
            PlayerPrefs.SetInt(ResourceManager.TotalUnlockLevel, PlayerPrefs.GetInt(ResourceManager.TotalUnlockLevel) + 1);
        }
        else
            PlayerPrefs.SetInt(ResourceManager.Level, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void OnRetryBtnClk()
    {// RELOAD LEVEL ON RETRY 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLevelFailed()
    {// FAILED TO COMPLETE THE LEVEL
        SoundManager.instance.Play("GameOver");
        if (!levelFailedPanel.activeInHierarchy)
            levelFailedPanel.SetActive(true);
        foreach (Player player in GameObject.FindObjectsOfType<Player>())
            Destroy(player.gameObject);
    }
    void OnGamePause()
    {// PAUSE GAME 
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            GetComponent<GridLevel>().parentHolder.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            GetComponent<GridLevel>().parentHolder.SetActive(true);
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
    // Update is called once per frame
    void Update()
    {//CHECK THE USER INPUT AND DO STUFF ACCORDINGLY

        if (Input.GetMouseButtonDown(0) && numOfTapInLevel > 0)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.001f);
            if (this.hit.collider != null)
            {
                if (hit.collider.CompareTag("purple"))
                {
                    ToDoOnTap();
                    hit.collider.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Purple);
                }
                if (hit.collider.CompareTag("blue"))
                {
                    ToDoOnTap();
                    hit.collider.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Blue);
                }
                if (hit.collider.CompareTag("yellow"))
                {
                    ToDoOnTap();
                    hit.collider.GetComponent<Player>().CheckPlayerState(Player.PLAYERCOLOR.Yellow);
                }

            }
        }

    }
    void ToDoOnTap()
    {
        numOfTapInLevel -= 1;
        attemptTxt.text = "# " + numOfTapInLevel;
        attempt++;
        if (numOfTapInLevel < TOTALTAPS)
            undoBtn.interactable = true;
    }
    public void OnUndo()
    {
        int tempVal = numOfTapInLevel + 1;
        tempVal = tempVal < TOTALTAPS ? tempVal : TOTALTAPS;
        //attemptTxt.text = "# " + numOfTapInLevel;
        if (tempVal == TOTALTAPS)
            undoBtn.interactable = false;
    }
}
