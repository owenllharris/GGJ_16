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

		if (distance <= 6f && enemy.isCooledDown)
		{
			enemy.ChangeBehaviour(new EnemyAttack (enemy, target));
		}
//
//		if (distance < 5f) 
//		{
//			return;
//		}
		
		enemy.navMeshAgent.SetDestination (target.position);
		Vector3 dir =  (enemy.navMeshAgent.nextPosition - enemy.transform.position).normalized;
		enemy.navMeshAgent.Move(dir * enemy.navMeshAgent.speed * Time.deltaTime);
	}

	public override void End ()
	{

	}
}