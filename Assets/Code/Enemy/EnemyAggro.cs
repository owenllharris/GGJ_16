using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyAggro : EnemyBehaviour
{
	private Transform target;
	public EnemyAggro(Enemy enemy, Transform target) : base(enemy)
	{
		this.target = target;
		enemy.navMeshAgent.SetDestination (target.position);
		enemy.navMeshAgent.Resume ();
		Begin();
	}

	public override void Run ()
	{
		float distance = Vector3.Distance (target.position, enemy.transform.position);

		if (distance <= enemy.shootDistance && enemy.isCooledDown)
		{
			enemy.ChangeBehaviour(new EnemyAttack (enemy, target));
			return;
		}

		if (distance > enemy.shootDistance-1) 
		{
			enemy.navMeshAgent.SetDestination (target.position);
			Vector3 dir = (enemy.navMeshAgent.nextPosition - enemy.transform.position).normalized;
			enemy.navMeshAgent.Move (dir * enemy.navMeshAgent.speed * Time.deltaTime);
		}
	}

	public override void End ()
	{
		enemy.navMeshAgent.Stop ();
	}
}