using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class is everything related to the close button and the closing of the ad by itself (downloading virus)
/// 
/// virus will download after a certain amount of seconds
/// sound effect when virus is downloading
/// lose health when virus is downloading
/// animation when virus is downloading
/// ad will close when button is pressed
/// hovering color change over the button
/// hovering size change over the button
/// disable button when on lose/win screen
/// 
/// </summary>
[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(AdSpawnerScript))]
[RequireComponent(typeof(SoundScript))]
public class CloseButtonScript : MonoBehaviour
{
    // Hitbox of the close button
    [SerializeField] BoxCollider2D MyBoxCollider2D;
    // Hitbox of the ad button
    [SerializeField] BoxCollider2D AdBoxCollider2D;
    // for the close button hovering
    private SpriteRenderer _spriteRenderer;

    // scripts to use
    [SerializeField] HealthScript HealthScript;
    [SerializeField] SoundScript SoundScript;
    [SerializeField] AdSpawnerScript AdSpawnerScript;

    // Original color of the close button
    private Color _originalColor;
    // The color the sprite will brighten to when hovering
    [SerializeField] Color HoverColor;

    // Original size of the close button
    private Vector3 _originalSize;
    // the hover size when user is on the close button
    private Vector3 _hoverSize = new Vector3(0.95f, 1.1f, 1f);

    // death rate is 6 seconds
    [SerializeField] float DownloadRate = 6;
    // The health of the ad is 1
    [SerializeField] int AdHealth = 1;
    // enable close button to work
    private bool _enable = true;

    // animation for not clicking the close button fast enough
    public Animator Animator;

    void Start()
    {
        // start the timer of ad download
        StartCoroutine(DestroyAfterDelay(DownloadRate));

        // ad button activated so it can be clicked and hovered
        _enable = true;

        // health script to decrease health when ad downloads
        HealthScript = GameObject.FindGameObjectWithTag("healthScript").GetComponent<HealthScript>();
        SoundScript = GameObject.FindGameObjectWithTag("clickingScript").GetComponent<SoundScript>();

        // ad spawner script to decrease the amount of ads on screen when ad downloads
        AdSpawnerScript = GameObject.FindGameObjectWithTag("adSpawnerScript").GetComponent<AdSpawnerScript>();
        // get the original color and size of the close button and sprite of close button
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _originalSize = _spriteRenderer.transform.localScale;
    }

    private void Update()
    {
        // if the close button is disabled, stop every download timer
        if (!_enable)
        {
            StopAllCoroutines();
        }
    }

    // When mouse enters the close button collider
    private void OnMouseEnter()
    {
        // if the closed button is disabled then return
        if (!_enable)
        {
            return; 
        }
        // change the color and hover size when mouse is on the close button
        _spriteRenderer.color = HoverColor;
        ChangeSpriteSize(_hoverSize);

        // change cursor
        MouseControlScript.instance.Clickable();
    }

    // when mouse leaves the close button collider
    private void OnMouseExit()
    {
        // if the closed button is disabled then return
        if (!_enable)
        {
            return;
        }
        // change back the color and button size when mouse is no longer on close button
        _spriteRenderer.color = _originalColor;
        ChangeSpriteSize(_originalSize);

        // change cursor
        MouseControlScript.instance.Default();
    }

    // method to change the sprite size
    private void ChangeSpriteSize(Vector3 newScale)
    {
        _spriteRenderer.transform.localScale = newScale;
    }

    // when user clicks close button, the game object gets destroyed
    void OnMouseDown()
    {
        // if the closed button is disabled then return
        if (!_enable)
        {
            return;
        }
        // change cursor to default after closing the ad
        MouseControlScript.instance.Default();

        // play clicking sound
        SoundScript.PlayClick();

        // Destroy the parent game object if ad health is 1 or less, decrease the ad health when it is greater than 1
        if(AdHealth > 1)
        {
            AdHealth--;
        }
        else if(AdHealth <= 1)
        {
            Destroy(GetParentObject());
            // when destroyed, decrease amount of ads on screen by one
            AdSpawnerScript.DecreaseOnScreen();
        }
    }

    // Method to destroy after delay of death rate
    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // play sound effect
        SoundScript.PlayDownload();

        // If ad gets deleted after delay, lose health
        if (GetParentObject() != null)
        {
            HealthScript.DecreaseHealth();
        }

        // hide the close button and ad button
        _spriteRenderer.enabled = false;
        MyBoxCollider2D.enabled = false;
        AdBoxCollider2D.enabled = false;

        // play the corrupted animation for the ad
        Animator.SetBool("isCorrupt", true);

        // Destroy the parent game object after 0.75 seconds to let animation play
        Destroy(GetParentObject(), 0.75f);

        // decrease the amount of ads on screen
        AdSpawnerScript.DecreaseOnScreen();
    }

    // get the parent object
    public GameObject GetParentObject()
    {
        return transform.parent.gameObject;
    }

    // make the close button stop working
    void DisableCloseButton()
    {
        _enable = false;
    }

    // on player death susbscribe to disabling close button
    private void OnEnable()
    {
        HealthScript.OnPlayerDeath += DisableCloseButton;
    }
    // not on player death unsusbscribe to disabling close button
    private void OnDisable()
    {
        HealthScript.OnPlayerDeath -= DisableCloseButton;
    }
}
