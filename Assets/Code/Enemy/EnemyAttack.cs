﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyAttack : EnemyBehaviour
{
	private Transform target;
	private Vector3 destination;

	public EnemyAttack(Enemy enemy, Transform target) : base(enemy)
	{
		this.target = target;
		Begin();
	}

	public override void Begin ()
	{
		enemy.StartCoroutine (Sidestepping ());
	}

	void Attack()
	{
		EnemyFireball fireball = GameObject.Instantiate<EnemyFireball>(enemy.fireBall);
		fireball.transform.position = enemy.transform.position + enemy.transform.forward;
		fireball.Fire (enemy.transform, target);
		enemy.Attacked ();
	}

	IEnumerator Sidestepping()
	{
		yield return new WaitForSeconds (0.25f);
		Attack ();
		yield return new WaitForSeconds (1f);
		Attack ();
		yield return new WaitForSeconds (1f);
		Attack ();
		yield return new WaitForSeconds (0.25f);
		enemy.ChangeBehaviour(new EnemyTakeCover(enemy));

//		while (true) 
//		{
//			destination = enemy.transform + Vector3.left;
//			enemy.navMeshAgent.SetDestination (destination);
//			yield return null;
//		}
	}

	public override void Run ()
	{
//		enemy.ChangeBehaviour(new EnemyAggro(enemy, GameManager.player.transform));
//		enemy.ChangeBehaviour(new EnemyTakeCover(enemy));

//		enemy.navMeshAgent.Move ();
	}


}