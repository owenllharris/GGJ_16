using UnityEngine;
using System.Collections;

public class EnemyHide : EnemyBehaviour 
{
	protected Cover cover;
	public EnemyHide(Enemy enemy, Cover cover) : base(enemy)
	{
		this.cover = cover;
		Begin ();
	}

	public override void Begin ()
	{
		cover.beingUsed = true;
		enemy.StartCoroutine (Wait ());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (enemy.hideTime);
		enemy.ChangeBehaviour (new EnemyAggro (enemy, GameManager.player.transform));
	}

	public override void End ()
	{
		cover.beingUsed = false;
	}
}