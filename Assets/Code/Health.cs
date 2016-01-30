using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	private int health;
	public int maxHealth;

	// Use this for initialization
	void Awake () 
	{
		health = maxHealth;
	}
	
	public void TakeDamage(int damage)
	{
		health = Mathf.Clamp (health - damage, 0, maxHealth);

		if (health == 0) 
		{
			Die ();
		}
	}

	public void Die()
	{
		GetComponent<IKillable> ().Kill ();
	}
}