using UnityEngine;
using System.Collections;

public class EnemyFireball : MonoBehaviour 
{
	public float speed = 10f;
	public float arcDistanceMod = 2f;
	public GameObject gibs;

	IEnumerator Start()
	{
		yield return new WaitForSeconds (10f);
		Remove ();
	}

	public void Fire(Transform from, Transform to)
	{
		Fire (from.position, to.position);
	}

	public void Fire(Vector3 from, Vector3 to)
	{
		StartCoroutine (Firing (from, to));
	}

	public void FireArc(Transform from, Transform to)
	{
		FireArc (from.position, to.position);
	}

	public void FireArc(Vector3 from, Vector3 to)
	{
		StartCoroutine (FiringArc (from, to));
	}

	IEnumerator Firing(Vector3 from, Vector3 to)
	{
		float elapsed = 0f;
		Vector3 dir = (new Vector3(to.x, 0, to.z) - new Vector3(from.x, 0, from.z)).normalized;
		Vector3 start = from;
		Vector3 end = to + (dir * 10f);
		float distance = Vector3.Distance (from, end);
		float duration = distance / speed;

		while(elapsed <= duration)
		{
			elapsed += Time.deltaTime; 
			float percentageComplete = elapsed / duration;
			transform.position = Vector3.Lerp(start, end, percentageComplete);
			yield return null;
		}
		Destroy (gameObject);
	}

	IEnumerator FiringArc(Vector3 from, Vector3 to)
	{
		float elapsed = 0f;
		float distance = Vector3.Distance (from, to);
		float duration = distance / speed;
		Vector3 dir = (new Vector3(to.x, 0, to.z) - new Vector3(from.x, 0, from.z)).normalized;
		Vector3 vectorLerp = Vector3.zero;
		Vector3 start = from;
		Vector3 end = to + dir + new Vector3(0,-2,0f);

		Vector3 heightBezier = new Vector3 (0f, distance / arcDistanceMod, 0f);
		while(elapsed <= duration)
		{
			elapsed += Time.deltaTime; 
			float percentageComplete = elapsed / duration;
//			vectorLerp = CalculateBezierPoint (Mathf.Lerp (0, 1, percentageComplete), start, ((start + end) + heightBezier) / 2, end);
			vectorLerp = CalculateBezierPoint (Mathf.Lerp (0, 1, percentageComplete), start, start + heightBezier, end + heightBezier, end);
			transform.position = vectorLerp;
			yield return null;
		}
	}

	Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float u1 = 1f - t;
		float tt = t * t;
		float uu = u1*u1;
		float uuu = uu * u1;
		float ttt = tt * t;

		Vector3 p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u1 * tt * p2; //third term
		p += ttt * p3; //fourth term

		return p;
	}

	Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u1 = 1f - t;
		float tt = t * t;
		float uu = u1*u1;
		float uuu = uu * u1;

		Vector3 p = uuu * p0; //first term
		p += 3 * uu * t * p1; //second term
		p += 3 * u1 * tt * p2; //third term

		return p;
	}

	void OnTriggerEnter(Collider col)
	{
//		if(col.gameObject.tag == "Player")
//		{
//			col.gameObject.BroadcastMessage("Death");    
//		}
		if (col.transform.gameObject.layer != 12 && col.transform.gameObject.layer != 15)
		{
			Health health = col.GetComponent<Health> ();
			if (health != null)
				health.TakeDamage (1);
			gibs.SetActive (true);
			gibs.transform.parent = null;
			Remove ();
		}
	}

	void Remove()
	{
		Destroy(gameObject);
	}
}