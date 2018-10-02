using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightTemplate : MonoBehaviour {

	CharacterTemplate healthScript;

// Base character health.
	public int maxHealth = 10000;
// Base character armor.
	public int armor = 10;
	public int minDamage = 5;
	public int dodgeChance = 20;
// Base character movement.
	public int movementRadius = 6;
// Base character damage.
	public int attackDamage = 20;

	public float damageDelay = 1.0f;

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
	}
}
