using UnityEngine;
using System.Collections;

public class MoveTowardsPlayer : MonoBehaviour
{
    private GameObject target;

    public float speed;

	// Use this for initialization
	void Start ()
	{
        target = GameManager.player;
	}
	
	// Update is called once per frame
	void Update ()
    {
		Vector3 dir =  (target.transform.position - transform.position).normalized;

        transform.Translate(dir * speed * Time.deltaTime);
	}
}
