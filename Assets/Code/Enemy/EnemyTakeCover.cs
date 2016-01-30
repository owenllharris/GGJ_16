using UnityEngine;
using System.Collections;

public class EnemyTakeCover : EnemyBehaviour
{
	private Cover myCover;
	private GameObject[] covers;

	public EnemyTakeCover(Enemy enemy) : base(enemy)
	{
		Begin ();
		enemy.navMeshAgent.Resume ();
	}

	public override void Begin ()
	{
		covers = GameObject.FindGameObjectsWithTag("EnemyCover");
		if (FindCover () == false)
		{
			enemy.ChangeBehaviour(new EnemyAggro(enemy, GameManager.player.transform)); //bail
		}
	}

	bool FindCover()
	{
		for (int i = 0; i < covers.Length; i++) 
		{
			Cover cover = covers [i].GetComponent<Cover> ();
			if (cover == null)
				cover = covers [i].AddComponent<Cover> ();
			if (cover.beingUsed == false)
			{
				myCover = cover;
				return true;
			}
		}
		return false;
	}

	public override void Run ()
	{
		if (myCover == null)
			return;
		if (myCover.beingUsed)
		{
			if(FindCover () == false)
				enemy.ChangeBehaviour(new EnemyAggro(enemy, GameManager.player.transform));
			return;
		}
		enemy.navMeshAgent.SetDestination (myCover.transform.position);
		Vector3 dir =  (enemy.navMeshAgent.nextPosition - enemy.transform.position).normalized;
		enemy.navMeshAgent.Move(dir * enemy.navMeshAgent.speed * Time.deltaTime);
		if (Vector3.Distance (myCover.transform.position, enemy.transform.position) < 0.5f)
		{
			enemy.ChangeBehaviour(new EnemyHide(enemy, myCover));
		}
	}

	public override void End ()
	{
		enemy.navMeshAgent.Stop ();
	}
}