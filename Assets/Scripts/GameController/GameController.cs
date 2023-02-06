using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public enum states
    {
        mainMenu,
        ingame,
        gameOver
    }
    public states gameStates;

    [Header("Menu UI")]
    public GameObject menuUI;
    public Text homeHighScoreText;

    [Header("Ingame UI")]
    public GameObject ingameUI;
    public Text ingameScoreText;
    public Text coinText;

    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public Text scoreText;
    public Text highScoreText;

    [Header("Other UI")]
    public GameObject settingsUI;
    public GameObject levelCompleteUI;

    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public bool restartLevel=false;
    [HideInInspector]
    public int collectedCoins=0;

    [HideInInspector]public int infiHighScore,infiCurScore;

    [HideInInspector]public int musicIndex,sfxIndex,vibrationIndex;

    public enum mode
    {
        levels,
        infinite
    }
    public mode gameMode;
    [HideInInspector]
    public int modeIndex;

    //
    [Header("Settings")]
    public SettingsHandler settings;
    [HideInInspector]public bool isTutorialActive=false;

    [Header("Tutorial")]
    public Transform tutorialGesture;

    void Awake()
    {
        if(instance!=null)
        {
            return;
        }
        else
        {
            instance=this;
        }
    }

    void Start()
    {
        Application.targetFrameRate=60;
        menuUI.SetActive(true);
        ingameUI.SetActive(false);
        settingsUI.SetActive(false);
        gameOverUI.SetActive(false);
        levelCompleteUI.SetActive(false);
        if(restartLevel)
        {
            Play();
            UpdateRestart(false);
        }
        updateModeIndex();
    }

    #region ui
    public void Play()
    {
        menuUI.SetActive(false);
        ingameUI.SetActive(true);
        gameStates=states.ingame;
        if(!restartLevel)
        {
            isTutorialActive=true;
            tutorialGesture.gameObject.SetActive(true);
        }
        else
        {
            isTutorialActive=false;
        }
        PlayerController.instance.EnablePlayer();
        //lets change game mode from here
        if((mode)modeIndex==mode.levels)
        {
            LevelManager.instance.LoadLevels(currentLevel);
        }
        else
        {
            LevelManager.instance.LoadInfinteMode();
        }
        HandleUIAudio();
    }

    //settings
    public void GoToSettings()
    {
        menuUI.SetActive(false);
        settingsUI.SetActive(true);
        HandleUIAudio();
    }

    public void CloseSettings()
    {
        menuUI.SetActive(true);
        settingsUI.SetActive(false);
        HandleUIAudio();
    }
    #endregion

    #region tutorial
    public void DisableTutorial()
    {
        if(tutorialGesture!=null)
        {
            tutorialGesture.GetComponent<TutorialHandler>().HideTutorial();
        }
        isTutorialActive=false;
    }
    #endregion

    #region gameMode
    public void PlayLevelMode()
    {
        gameMode=mode.levels;
        updateModeIndex();
        //save data
    }

    public void PlayInfiniteMode()
    {
        gameMode=mode.infinite;
        updateModeIndex();
        //save data
    }

    void updateModeIndex()
    {
        modeIndex=(int)gameMode;
    }
    #endregion

    #region game data
    public void UpdateGameData()
    {
        if(settings!=null)
        {
            settings.UpdateSettings();
        }
        homeHighScoreText.text=infiHighScore.ToString();
        UpdateCoins();
    }
    #endregion

    #region score
    public void AddScore(int amount)
    {
        infiCurScore=amount;
        ingameScoreText.text=infiCurScore.ToString();
    }
    #endregion

    #region coins
    public void UpdateCoins()
    {
        if(collectedCoins>1000)
        {
            coinText.text=(collectedCoins/1000).ToString()+"k";   
        }
        else
        {
           coinText.text=collectedCoins.ToString();
        }
    }
    #endregion

    #region gameOver
    public void GameOver()
    {
        if(infiCurScore>infiHighScore)
        {
            infiHighScore=infiCurScore;
            SaveSystemHandler.instance.UpdateData();
        }

        scoreText.text=infiCurScore.ToString();
        highScoreText.text="High Score : "+infiHighScore.ToString();

        gameStates=states.gameOver;
        ingameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void Home()
    {
        HandleUIAudio();
        UpdateRestart(false);
        StartCoroutine(DelayLevelLoad());
    }

    public void Restart()
    {
        HandleUIAudio();
        UpdateRestart(true);
        StartCoroutine(DelayLevelLoad());
    }

    IEnumerator DelayLevelLoad()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateRestart(bool value)
    {
        restartLevel=value;
        SaveSystemHandler.instance.UpdateData();
    }
    #endregion

    #region level complete
    public void LevelComplete()
    {
        ingameUI.SetActive(false);
        levelCompleteUI.SetActive(true);
    }
    #endregion

    #region audio
    void HandleUIAudio()
    {
        if(GameAudioHandler.instance!=null)
        {
            GameAudioHandler.instance.PlayButtonClickSound();
        }
    }
    #endregion
}
