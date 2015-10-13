﻿using UnityEngine;
using UnityEngine.UI;

/**
 * 
 * Class which handles player health logic.
 * 
 **/
public class PlayerHealth : MonoBehaviour
{
	public int startingHealth = 100;            // The amount of health the player starts with.
	public int currentHealth;                   // The current health the player has.
    public int lethalLow;
	public Slider healthSlider;					// Slider for player's health.
    private PlayerControl playerControlScript;


    void Awake ()
	{		
		// Setting the current health when the player first spawns.
		currentHealth = startingHealth;
        playerControlScript = GetComponent<PlayerControl>();
    }

    void Update()
    {
        if (gameObject.transform.position.y < lethalLow)
        {
            TakeDamage(startingHealth);
        }
    }

	public void TakeDamage (int amount)
	{		
		// Reduce the current health by the amount of damage sustained.
		currentHealth -= amount;

		// Update health on slider to new value.
		healthSlider.value = currentHealth;
		
		// If the current health is less than or equal to zero...
		if(currentHealth <= 0)
		{
            // ... the player is destroyed.
            playerControlScript.InitiateAnimation("Die");
            Destroy (gameObject, 0.95f);
		}
	}

	public void HealPlayer(int heal){
		currentHealth += heal;
		if (currentHealth > 100) {
			currentHealth = 100;
		}
		healthSlider.value = currentHealth;
	}
}
