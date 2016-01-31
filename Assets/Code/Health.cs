using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public int health;
	public int maxHealth;

    public Transform healthBar;

    private float healthBarSize;
    private Vector3 healthBarV3;

    




	// Use this for initialization
	void Awake () 
	{
		health = maxHealth;
	}

    void Start()
    {
        healthBarSize = healthBar.localScale.z;
    }
	
	public void TakeDamage(int damage)
	{
		health = Mathf.Clamp (health - damage, 0, maxHealth);

        healthBarV3 = healthBar.localScale;
        healthBarV3.z = (healthBarSize / 100) * health;
        healthBar.localScale = healthBarV3;

		if (health == 0) 
		{
			Die ();
		}
	}

  



	public void Die()
	{

        GameManager.instance.GameOver();
        //GetComponent<IKillable> ().Kill ();
	}
}