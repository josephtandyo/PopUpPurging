using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is for everything UI related.
/// 
/// Health bar
/// Download bar
/// Download error color changes
/// Win screen
/// Lose screen
/// 
/// </summary>
public class UIManagerScript : MonoBehaviour
{
    // Restart level STUFF
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] GameObject GO_lastBatt, GO_retryButton, GO_mainMenuButton, GO_youLoseText;

    // Next Level Menu STUFF
    [SerializeField] GameObject NextLevelMenu;
    [SerializeField] GameObject NL_downloadArea, NL_nextLevelButton, NL_levelText, NL_percent, NL_99percentText, NL_downloadingText, NL_mainMenuButton, NL_downloadCompleteText;

    [SerializeField] TMP_Text PercentageText;

    public SoundScript SoundScript;
    public void Start()
    {

        SoundScript = GameObject.FindGameObjectWithTag("clickingScript").GetComponent<SoundScript>();
    }
    // using events, on player death: subscribe to game over menu
    // on player win: subscribe to next level menu
    private void OnEnable()
    {
        HealthScript.OnPlayerDeath += EnableGameOverMenu;
        AdSpawnerScript.OnPlayerWin += EnableNextLevelMenu;
    }

    // not on player death: unsub
    // not on player win: unsub
    private void OnDisable()
    {
        HealthScript.OnPlayerDeath -= EnableGameOverMenu;
        AdSpawnerScript.OnPlayerWin -= EnableNextLevelMenu;
    }

    public void EnableGameOverMenu()
    {
        // activate the GO menu
        GameOverMenu.SetActive(true);
        
        // change cursor to default
        MouseControlScript.instance.Default();

      /*  // move the last battery to the middle
        LeanTween.moveLocal(GO_lastBatt, new Vector3(-90f, 285f, 2f), 1f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc);
        // scale the last battery to be bigger
        LeanTween.scale(GO_lastBatt, new Vector3(2f, 2f, 2f), 1f).setDelay(0.1f).setEase(LeanTweenType.easeInSine).setOnComplete(LevelFailed);*/

        // after scaling, call LevelFailed()
    }

    public void LevelFailed()
    {
      /*  // make the you lose text bigger (from size 0)
        LeanTween.scale(GO_youLoseText, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0f).setEase(LeanTweenType.easeOutElastic);

        // make the two buttons bigger (from size 0)
        LeanTween.scale(GO_retryButton, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0.25f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(GO_mainMenuButton, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);*/

        // TODO settings button ?
    }

    // restarts the level when restart button is pressed
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        MouseControlScript.instance.Default();
    }

    public void EnableNextLevelMenu()
    {

        SoundScript.StopGoodDownload();
        // activate the NL menu
        NextLevelMenu.SetActive(true);

        // default cursor
        MouseControlScript.instance.Default();
/*
        // move the entire download area to the middle
        LeanTween.moveLocal(NL_downloadArea, new Vector3(-300f, 70f, 2f), 1f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc);
        // scale the entire download area to be bigger
        LeanTween.scale(NL_downloadArea, new Vector3(3f, 3f, 3f), 1f).setDelay(0.1f).setEase(LeanTweenType.easeInSine);

        // move the level text towards the middle of download bar
        LeanTween.moveLocal(NL_levelText, new Vector3(65f, 11.5f, 2f), 2f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc);
        // scale the next level text to be bigger
        LeanTween.scale(NL_levelText, new Vector3(0.5f, 0.5f, 0.5f), 2f).setDelay(0f).setEase(LeanTweenType.easeInSine).setOnComplete(LevelComplete);*/

        // after scaling call LevelComplete()
    }

    public void LevelComplete()
    {
        // deactivate the downloading... text
        NL_downloadingText.SetActive(false);

        /*LeanTween.moveLocal(NL_99percentText, new Vector3(10f, -2f, 2f), 1f).setDelay(0.1f).setEase(LeanTweenType.easeOutCirc);*/

        PercentageText.text = "100%";

        // make the download complete text bigger (from size 0)
      /*  LeanTween.scale(NL_downloadCompleteText, new Vector3(0.5f, 0.5f, 0.5f), 2f).setDelay(0f).setEase(LeanTweenType.easeOutElastic);

        // make the you two buttons bigger (from size 0)
        LeanTween.scale(NL_nextLevelButton, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0.25f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(NL_mainMenuButton, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(0.5f).setEase(LeanTweenType.easeOutElastic);*/

        // TODO subscription button, check if build index is 5. ONLY when winning
        // TODO settings button?
    }

    // goes up one in the build index when next level button is pressed
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

