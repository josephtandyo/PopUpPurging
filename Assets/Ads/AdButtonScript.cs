using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This class is everything related to the ad body itself
/// 
/// sound effect when clicking the ad
/// click color when clicking the ad
/// lose health when clicking the ad
/// hovering color change over the ad
/// disable and enable ad functions on win/lose screens
/// 
/// </summary>
[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(SoundScript))]
public class AdButtonScript : MonoBehaviour
{
    // Below are variables for the ads
    public BoxCollider2D MyBoxColldier2d;
    private SpriteRenderer _spriteRenderer;

    // variables for colors of hovering over ad and clicking on the ad
    private Color _originalColor;
    [SerializeField] Color HoverColor;
    [SerializeField] Color DownloadColor;

    // Variables for scripts
    [SerializeField] HealthScript HealthScript;
    [SerializeField] SoundScript SoundScript;

    // The duration (in seconds) the sprite remains clicked color.
    private float _colorChangeDuration = 1f;
    private bool _isClicked;

    // allow ads to be clicked and change hover color
    private bool _enable = true;

    void Start()
    {
        // health script is for decreasing health
        HealthScript = GameObject.FindGameObjectWithTag("healthScript").GetComponent<HealthScript>();
        SoundScript = GameObject.FindGameObjectWithTag("clickingScript").GetComponent <SoundScript>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        // get the original color of sprite
        _originalColor = _spriteRenderer.color;

        // allow ads to be clicked and change hover color (for game over/win screen)
        _enable = true;
    }

    private void OnMouseEnter()
    {
        // when ads are disabled return
        if (!_enable)
        {
            return;
        }
        // when mouse hovers and ad has not been clicked
        if (!_isClicked && _spriteRenderer != null)
        {
            // change color to hover color
            _spriteRenderer.color = HoverColor;
        }

    }

    private void OnMouseExit()
    {
        // when ads are disabled return
        if (!_enable)
        {
            return;
        }

        // when mouse gets off the ad and has not been clicked
        if (!_isClicked)
        {
            // change color back to original color
            _spriteRenderer.color = _originalColor;
        }
        
    }

    // when user clicks on the ad
    void OnMouseDown() 
    {
        // when ads are disabled return
        if (!_enable)
        {
            return;
        }

        // clicking ad decreases health and plays sound effect
        HealthScript.DecreaseHealth();
        SoundScript.PlayDownload();
     
        // begin to count how long to change color of clicked ad
        StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        // is clicked is true so that hovering won't change color again
        _isClicked = true;
        _spriteRenderer.color = DownloadColor;
        yield return new WaitForSeconds(_colorChangeDuration);

        // after timer ends, change back to original color
        _spriteRenderer.color = _originalColor;
        _isClicked = false;
    }

    // disable functions on the ad
    void DisableAds()
    {
        _enable = false;
    }

    // on player death, disable the ads
    private void OnEnable()
    {
        HealthScript.OnPlayerDeath += DisableAds;
    }

    // not on player death, unsub from disabling ad
    private void OnDisable()
    {
        HealthScript.OnPlayerDeath -= DisableAds;
    }
}
