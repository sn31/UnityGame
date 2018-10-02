using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonTemplate : MonoBehaviour {

	CharacterTemplate healthScript;

// Character name
	private string name = "Skeleton";
// Base character health.
	private int maxHealth = 60;
// Base character armor.
	private int armor = 5;
	private int minDamage = 5;
	private int dodgeChance = 10;
// Base character movement.
	private int movementRadius = 6;
// Base character damage.
	private int attackDamage = 15;

	// Seconds to wait before dealing damage
	private float damageDelay = 1.0f;

	// Sight radius for detection;
	private float sightRadius = 12f;

	// Attack range
	private float attackRange = 2.35f;

	// Launch point for projectiles
	public GameObject launchPoint;

	// Projectile prefab
	public GameObject projectile;

	public Texture portrait;

	// Use this for initialization
	void Awake () {
		healthScript = GetComponent<CharacterTemplate>();

		healthScript.maxHealth = maxHealth;
		healthScript.currentHealth = maxHealth;

		healthScript.armor = armor;
		healthScript.dodgeChance = dodgeChance;
		healthScript.minDamage = minDamage;

		healthScript.movementRadius = movementRadius;

		healthScript.attackDamage = attackDamage;
		
		healthScript.damageDelay = damageDelay;

		healthScript.sightRadius = sightRadius;
		healthScript.attackRange = attackRange;

		healthScript.name = name;

		healthScript.portrait = portrait;

		if (launchPoint && projectile)
		{
			healthScript.launchPoint = launchPoint;
			healthScript.projectile = projectile;
		}
	}
}

