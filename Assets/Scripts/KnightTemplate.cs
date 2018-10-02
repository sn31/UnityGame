using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightTemplate : MonoBehaviour {

	CharacterTemplate healthScript;

// Character name
	private string name = "Footman";
// Base character health.
	private int maxHealth = 100;
// Base character armor.
	private int armor = 10;
	private int minDamage = 5;
	private int dodgeChance = 20;
// Base character movement.
	private int movementRadius = 8;
// Base character damage.
	private int attackDamage = 20;

	private float damageDelay = 0.5f;

	// Sight radius for detection;
	private float sightRadius = 13f;
	
	private float attackRange = 3.7f;

	public Texture portrait;

	// Launch point for projectiles
	public GameObject launchPoint;

	// Projectile prefab
	public GameObject projectile;

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
