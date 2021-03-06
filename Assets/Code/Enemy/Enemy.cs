﻿using UnityEngine;
using System.Collections;

public enum EnemyType
{
	Arc = 0,
	Straight = 1
}
public class Enemy : MonoBehaviour 
{
	private NavMeshAgent _navMeshAgent;
	public NavMeshAgent navMeshAgent {
		get {
			if (_navMeshAgent == null)
				_navMeshAgent = GetComponent<NavMeshAgent> ();
			return _navMeshAgent;
		}
	}

	public EnemyBehaviour currentBehaviour;

	public EnemyFireball fireBall;

	public float hideTime = 3f;
	public float shootDistance = 3f;

	public float cooldown = 3f;
	private float coolingDown = 0f;

	public EnemyType enemyType;

	public bool isCooledDown {
		get {
			return Mathf.Approximately(coolingDown,cooldown);
		}
	}

	public void Attacked()
	{
		coolingDown = 0f;
	}

	void Awake()
	{
//		enemyType = (EnemyType)Random.Range (0, 2);
	}

	void Start()
	{
		ChangeBehaviour(new EnemyAggro (this, GameManager.player.transform));
	}

	public void ChangeBehaviour(EnemyBehaviour behaviour)
	{
		if (currentBehaviour != null)
			currentBehaviour.End ();
		currentBehaviour = behaviour; 
	}

	void Update () 
	{
		currentBehaviour.Run ();
		coolingDown = Mathf.Clamp(coolingDown + Time.deltaTime, 0f, cooldown);
	}
}