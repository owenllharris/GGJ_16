using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyBehaviour
{
	protected Enemy enemy;
	protected string name = "";

	public EnemyBehaviour(Enemy enemy)
	{
		name = this.GetType ().Name;
		this.enemy = enemy;
		Debug.Log (name, enemy.gameObject);
	}
	public virtual void Begin()
	{
		
	}
	public virtual void Run()
	{
		
	}

	public virtual void End()
	{

	}
}