using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
using UnityEngine.UI;

// TODO maybe add a new class for the download reset button and the logic for chance of download error because it's not related to ad spawning really
// TODO finish adding comments to this class.

/// <summary>
/// This class is everything related to ad spawning
/// 
/// determines when a player wins
/// spawn any type of ad randomly from a list of ads in that particular level.
/// spawn any number of ads for an ad type from a pre determined provided number.
/// determine how fast the spawning is per wave.
/// determine at how many enemies the other wave begins.
/// calculates how much to add in progress bar per ad spawn
/// randomly makes download error where the ads will spawn infinitely unless player resets download
/// 
/// </summary>
/// 

public class AdSpawnerScript : MonoBehaviour
{
    // Below are variables for winning
    public static event Action OnPlayerWin;
    private bool _enableSpawning = true;

    // Below are variables for spawn times and amount
    // timer for counting up to the speed of wave, starting at 0
    private double _spawnTimer;
    // spawning how many ads on this level
    private float _totalAmount;
    // Below is the ad we are spawning, the ads are chosen on the Unity screen
    [SerializeField] List<GameObject> Ads = new();
    // The ad itself
    private GameObject Ad;
    // list of amounts of each ad
    [SerializeField] List<int> adAmounts = new();
    // how many ads are on screen, start with 1 because initially spawns 1 ad
    private int _onScreen = 0;
    // this variable is so you don't win instantly since no ad exists at the start and you would win instantly
    private bool canWin = false;
    // this bool is so that it does not keep invoking every frame
    private bool invokeOnce = false;

    // Below are variables for the ad spawn location
    // the z axis of the ads
    private float _zAxis = 0;
    // margin so the batteries health don't get covered up
    const float MARGIN = 3.5f;

    // Below are variables to set the wave benchmarks
    // wave 2 happens on the specified enemy spawned
    [SerializeField] int WaveTwo;
    // wave 3 happens on the specified enemy spawned
    [SerializeField] int WaveThree;

    // Below are the variables to set how much faster each wave will be
    [SerializeField] double WaveOneSpeed;
    [SerializeField] double WaveTwoSpeed;
    [SerializeField] double WaveThreeSpeed;

    public float SpeedMultiplier;

    // Below are other scripts
    [SerializeField] HealthScript HealthScript;
    [SerializeField] ProgressBarScript ProgressBarScript;

    void Start()
    {
        // health script for determining that player has more health than 0 so they can win
        HealthScript = GameObject.FindGameObjectWithTag("healthScript").GetComponent<HealthScript>();
        // to use increment progress method
        ProgressBarScript = GameObject.FindGameObjectWithTag("progressBarScript").GetComponent<ProgressBarScript>();

        // spawning is enabled
        _enableSpawning = true;
        _totalAmount = 0f;
        canWin = false;

        // Calculate the total spawn amount by summing all the values in the list
        foreach (int amount in adAmounts)
        {
            _totalAmount += amount;
        }

        if(_totalAmount > 0)
        {
            // how much per ad download the bar should progress each spawn
            ProgressBarScript._progressAmount = 1f/_totalAmount;
        }

        invokeOnce = false;
        SpeedMultiplier = 1f;
}

    void Update()
    {
        // when there's nothing else to spawn and there's no enemy on screen, win the level
        if (_totalAmount <= 0 && _onScreen <= 0 && HealthScript.Health > 0 && canWin && !invokeOnce && !ProgressBarScript.Error)
        {
            OnPlayerWin?.Invoke();
            invokeOnce = true;
        }

       
        // normal speed when there are above a specified amount of ads not yet spawned
        // WAVE 1
        else if (_totalAmount > WaveTwo)
        {
            SpawnAdRate(WaveOneSpeed / SpeedMultiplier);
        }

        // faster speed when there are between the wave three and wave two numbers of ads spawned
        // WAVE 2
        else if (_totalAmount <= WaveTwo && _totalAmount > WaveThree)
        {
            SpawnAdRate(WaveTwoSpeed / SpeedMultiplier);
        }

        // on the specified wave three number, the speed gets even faster
        // WAVE 3
        else
        {
            SpawnAdRate(WaveThreeSpeed / SpeedMultiplier);
        }
    }

    void SpawnAdRate(double spawnRate)
    {
        // when spawning is not enabled, because of losing, stop spawning
        if (!_enableSpawning)
        {
            return;
        }

        // if timer is less than the spawn rate, add time to it
        else if (_spawnTimer < spawnRate)
        {
            _spawnTimer += Time.deltaTime;
            
        }

        // if the timer is the spawn rate, spawn ad and reset the timer to 0
        else if (_spawnTimer >= spawnRate && _totalAmount > 0)
        {
          
            if (Ads.Count > 0)
            {
                // spawn the ad
                SpawnAd();
            }
            // increase the amount of ads on screen
            _onScreen++;
            // reset timer
            _spawnTimer = 0;
        }
    }

    void SpawnAd()
    {
        if (!ProgressBarScript.Error)
        {
            // decrease the number of spawns for the level if there is no error
            _totalAmount--;
        }
        
        // change the z axis so that the ads don't overlap eachother
        _zAxis = _zAxis + 1f;

        // randomly pick an ad from the list to spawn
        Ad = Ads[UnityEngine.Random.Range(0, Ads.Count)];
        string AdName = Ad.name;

        // switch statements, if the ad name matches, call the method to check in the list
        switch (AdName)
        {
            case "BasAD":
                CheckAdList(Ad);
                break;
            case "Batt":
                CheckAdList(Ad);
                break;
            default:
                Debug.Log("None of tese");
                break;

        }
        // spawn the ad at the random position 
        Instantiate(Ad, Location(), transform.rotation);
        
        if (!ProgressBarScript.Error)
        {
            // Increase the progress bar download if there's no error
            ProgressBarScript.IncrementProgress();
        }
        canWin = true;

    }

    public void CheckAdList(GameObject Ad)
    {
        // decrease the amount of that ad if it exists in the list and the download bar is not error
        if (Ads.Contains(Ad) && !ProgressBarScript.Error)
        {
            adAmounts[Ads.IndexOf(Ad)] -= 1;
        }
        // at 0 amount of that ad, remove that ad from list
        if (adAmounts[Ads.IndexOf(Ad)] <= 0 && !ProgressBarScript.Error)
        {
            Ads.Remove(Ad);
        }
    }

    public Vector3 Location()
    {
        // get the screen boundaries, add margin for the bottom because of the health bar
        float screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x + 0.5f;
        float screenRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - 0.5f;
        float screenBottom = Camera.main.ScreenToWorldPoint(Vector3.zero).y + MARGIN;
        float screenTop = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - 0.5f;

        float adHeight = Ad.GetComponent<SpriteRenderer>().bounds.size.y;
        float adWidth = Ad.GetComponent<SpriteRenderer>().bounds.size.x;

        // generate random positions within the adjusted screen boundaries
        float randomX = UnityEngine.Random.Range(screenLeft + adWidth / 2, screenRight - adWidth / 2);
        float randomY = UnityEngine.Random.Range(screenBottom + adHeight / 2, screenTop - adHeight / 2);
        Vector3 randomPosition = new Vector3(randomX, randomY, _zAxis);

        return randomPosition;
    }

    // decrease the amount of ads on screen
    public void DecreaseOnScreen()
    {
        _onScreen--;
    }

    // on player death, subscribe to disable spawning
    private void OnEnable()
    {
        HealthScript.OnPlayerDeath += DisableSpawning;
    }

    // not on player death, unsubscribe to disable spawning
    private void OnDisable()
    {
        HealthScript.OnPlayerDeath -= DisableSpawning;
    }

    // disable spawning when getting lose/win screen
    void DisableSpawning()
    {
        _enableSpawning = false;
    }
}
