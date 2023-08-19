using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is for the download bar and download errors
/// </summary>
public class ProgressBarScript : MonoBehaviour
{
    // slider is for how much progress
    [SerializeField] Slider progressSlider;
    // image is to get and set colors for it
    [SerializeField] Image progressFill;

    // how much progress is added each progress
    public float _progressAmount;
    private int percentChance;

    // color and text for downloading
    private Color _downloadingFillColor;
    [SerializeField] GameObject downloadingText;

    // color and text for error
    private Color _errorColor;
    // hex code for the error color
    private string _hexErrorColor;
    [SerializeField] GameObject errorText;

    // text representing float and float value for percent
    [SerializeField] TMP_Text percentText;
    [SerializeField] float percentValue;

    // button for fixing error
    [SerializeField] Button errorFixButton;
    // true if there's error, false if no error
    public bool Error { get; private set; }

    private const int ONE_HUNDRED = 100;
    private const int ONE = 1;
    private const int TWO = 2;

    private double speedTimer;
    private float speedRate = 3f;

    // other scripts

    // sound script to play when downloading/error/fixing
    public SoundScript SoundScript;
    // adspawner script for adding multiplier when error not fixed fast enough
    public AdSpawnerScript AdSpawnerScript;

    void Start()
    {
        // Start the progress at 0%
        progressSlider.value = 0f;

        // Don't start with an error
        Error = false;

        _downloadingFillColor = progressFill.color;

        percentChance = TWO;
        percentText.text = "0.00%";
        
        _hexErrorColor = "#362f63";

        SoundScript = GameObject.FindGameObjectWithTag("clickingScript").GetComponent<SoundScript>();
        AdSpawnerScript = GameObject.FindGameObjectWithTag("adSpawnerScript").GetComponent<AdSpawnerScript>();
        speedTimer = 0f;
        percentChance = TWO;
        // TODO reset class method that resets all values

    }

    private void Update()
    {
        if (Error)
        {
            if (speedTimer < speedRate)
            {
                speedTimer += Time.deltaTime;
            }
            else if (speedTimer > speedRate)
            {
                AdSpawnerScript.SpeedMultiplier += 0.1f;
                speedTimer = 0f;
            }

        }
    }

    // increment progress in adspawnerscript when an ad gets spawned
    public void IncrementProgress()
    {
        progressSlider.value += _progressAmount;

        percentValue = Mathf.RoundToInt(progressSlider.value * ONE_HUNDRED);
        if (percentValue >= 100)
        {
            percentText.text = "99.99%";
        }
        else
        {
            int decimalPlace = Random.Range(11, ONE_HUNDRED);
            percentText.text = percentValue.ToString() + "." + decimalPlace.ToString() + "%";
        }
        
        int chance = Random.Range(ONE, ONE_HUNDRED + ONE);
        if (chance <= percentChance)
        {
            DownloadError();
            percentChance = TWO;
        }
        else
        {
            percentChance += TWO;
        }
    }

    private void DownloadError()
    {
        Error = true;
        errorText.SetActive(true);
        downloadingText.SetActive(false);


        SoundScript.PlayError();
        SoundScript.StopGoodDownload();
        // Try parsing the hexadecimal color code into a Color object
        if (ColorUtility.TryParseHtmlString(_hexErrorColor, out _errorColor))
        {
            progressFill.color = _errorColor;
        }
    }

    public void DownloadFix()
    {
        SoundScript.PlayFix();
        SoundScript.PlayGoodDownload();
        Error = false;
        progressFill.color = _downloadingFillColor;
        errorText.SetActive(false);
        downloadingText.SetActive(true);
    }
    // TODO change color to red of the progress bar when download error
    // TODO add percent on dopwnload bar
    // Add error sound and make the button change color
    // TODO keep spawning ads after download complete to do the illusion
}
