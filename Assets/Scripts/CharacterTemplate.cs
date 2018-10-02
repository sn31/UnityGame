using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterTemplate : MonoBehaviour
{
// Character name
	public string charName;
// Variables to determine health.
    public int maxHealth;
    public int currentHealth;  
// Variables to determine armor.
    // Base armor for character
    public int armor; 
// Min damage taken at any attack on character
    public int minDamage;
// Chance to avoid damage by character.
    public int dodgeChance;

    // Sight Radius for enemies
    public float sightRadius;

// Variables for offensive combat.
// Attack damage variable.
    public int attackDamage;
// Attack range variable.
    public float attackRange;
// Launch point for projectiles
	public GameObject launchPoint;
// Projectile prefab
	public GameObject projectile;
// Attack target character character script.
    public CharacterTemplate targetCharScript;

    // Animator variable
    public Animator animator;
    public float damageDelay;

// Portrait variable
    public Texture portrait;


// Turn Manager variable.
    public TurnManagerScript turnManager;

  

// Movement variables.
    // Movement distance of this character.
    public int movementRadius;
    // Number of moves in a turn.
    public int actionPoints = 2;
// Reference to the UI's health bar.
    // public Slider healthSlider;                                 
    // AudioSource playerAudio;                                    // Reference to the AudioSource component.
    // PlayerMovement playerMovement;                              // Reference to the player's movement.
    // PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.

    // Whether the player is dead.
    public bool isDead;                                    
    // True when the player gets damaged.   
    public bool damaged;                                               


    void Start ()
    {
      // Sets booleans to default values
      isDead = false;

      // Grabs the animator component.
      animator = gameObject.GetComponent<Animator>();

      // Grabs the turn manager.
      turnManager = GameObject.Find("TurnManager").GetComponent<TurnManagerScript>();


        // Setting up the references.
        // anim = GetComponent <Animator> ();
        // playerAudio = GetComponent <AudioSource> ();
        // playerMovement = GetComponent <PlayerMovement> ();
        // playerShooting = GetComponentInChildren <PlayerShooting> ();
    }


    void Update ()
    {
      if (damaged)
      {
        damaged = false;
      }
        // // If the player has just been damaged...
        // if(damaged)
        // {
        //     // ... set the colour of the damageImage to the flash colour.
        //     damageImage.color = flashColour;
        // }
        // // Otherwise...
        // else
        // {
        //     // ... transition the colour back to clear.
        //     damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        // }

        // // Reset the damaged flag.
        // damaged = false;
    }


    public void TakeDamage (int amount)
    {
        damaged = true;
        animator.SetTrigger("attacked");

        // Trigger damaged animation here

        int currentDamage = amount;
        currentDamage = currentDamage - armor;

        // First condition. Checks if armor reduces all damage below min threshold. Breaks function if true.
        if (currentDamage <= minDamage) 
        {
            currentDamage = minDamage;
        } 

        int dodgeRoll = Random.Range(1, 101);

        if (dodgeRoll <= dodgeChance && currentDamage > minDamage)
        {
            currentDamage = minDamage;
        }

        currentHealth -= currentDamage;
        FloatingTextController.CreateFloatingText(currentDamage.ToString(), transform);
        // Debug.Log(gameObject.name + " took " + currentDamage + " damage.");
        // Debug.Log("Current Health = " + currentHealth);
        Death();

        // Reduce the current health by the damage amount.
        

        // Set the health bar's value to the current health.
        // healthSlider.value = currentHealth;

        // Play the hurt sound effect.
        // playerAudio.Play ();
    }

    public IEnumerator AttackTarget(GameObject target)
    {
        targetCharScript = target.GetComponent<CharacterTemplate>();

        // Attack animation here
        animator.SetTrigger("attack");
        yield return new WaitForSecondsRealtime(damageDelay);

        if (launchPoint && projectile)
        {
            GameObject newProjectile = Instantiate(projectile, launchPoint.transform.position, transform.rotation);
            yield return StartCoroutine(newProjectile.GetComponent<ProjectilePath>().TargetLocation(target));
        }

        targetCharScript.TakeDamage(attackDamage);
    }


    void Death ()
    {
        if (currentHealth <= 0 && !isDead)
        {
            // Set the death flag so this function won't be called again.
            isDead = true;
            animator.SetTrigger("Death");
            StartCoroutine(turnManager.CheckGameOver());
            

            // Turn off any remaining shooting effects.
            // playerShooting.DisableEffects ();

            // // Tell the animator that the player is dead.
            // anim.SetTrigger ("Die");

            // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
            // playerAudio.clip = deathClip;
            // playerAudio.Play ();

            // // Turn off the movement and shooting scripts.
            // playerMovement.enabled = false;
            // playerShooting.enabled = false;
        }
    }       
}
