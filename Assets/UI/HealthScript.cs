using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// This class is everything related to computer health
/// 
/// determines when a player dies
/// determines how much health player has
/// determines how many health container player has
/// shows the health images
/// plays decreasing health animation
/// 
/// </summary>
public class HealthScript : MonoBehaviour
{
    // event variable for player losing
    public static event Action OnPlayerDeath;
    // how much health player has
    public int Health { get; private set; }
    // how many containers of health player has
    public int NumOfBatts { get; private set; }

    // health container images in array
    [SerializeField] Image[] Batteries;

    // sprite for full health
    [SerializeField] Sprite FullBatt;

    // sprite for empty container (NOT NEEDED because the animation just ends with empty batt)
    [SerializeField] Sprite EmptyBatt;

    // animations for each health container
    [SerializeField] Animator Animator;
    [SerializeField] Animator Animator1;
    [SerializeField] Animator Animator2;
    [SerializeField] Animator Animator3;
    [SerializeField] Animator Animator4;
    [SerializeField] Animator Animator5;

    // this bool is so that it does not keep invoking every frame
    private bool invokeOnce = false;

    public void Start()
    {
        // start with 3 health
        Health = 3;
        NumOfBatts = 3;
        invokeOnce = false;
    }
    private void Update()
    {
        // if health is 0 or lower, player dies
        if (Health <= 0 && !invokeOnce)
        {
            Health = 0;
            OnPlayerDeath?.Invoke();
            invokeOnce = true;
        }

        // if the health is more than the number of containers, health will equal number of containers, can't exceed num of containers
        if (Health > NumOfBatts)
        {
            Health = NumOfBatts;
        }

        // loop through every battery containers in the array and count
        for (int i = 0; i < Batteries.Length; i++)
        {

            // when counting up, if the number is less than the amount of health, then that means you can fill that container with health
            if (i < Health)
            {
                Batteries[i].sprite = FullBatt;
            }

            // if your health is less than the number of containers that exist in the array then don't fill it up with battery
            else
            {
                // if number of containers is bigger, then show the sprite
                if (i < NumOfBatts)
                {
                    Batteries[i].enabled = true;
                }

                // don't show the sprite if number of containers is less than i
                else
                {
                    Batteries[i].enabled = false;
                }

                // animations for each battery when losing health
                switch (i)
                {
                    case 0:
                        Animator.SetBool("lostHealth", true);
                        break;
                    case 1:
                        Animator1.SetBool("lostHealth", true);
                        break;
                    case 2:
                        Animator2.SetBool("lostHealth", true);
                        break;
                    case 3:
                        Animator3.SetBool("lostHealth", true);
                        break;
                    case 4:
                        Animator4.SetBool("lostHealth", true);
                        break;
                    case 5:
                        Animator5.SetBool("lostHealth", true);
                        break;
                }
            }
        }
    }

    // method that decreases health used when ad is clicked or ad virus downloads
    public void DecreaseHealth()
    {
        if (Health > 0)
        {
            Health--;
        }
    }
}
