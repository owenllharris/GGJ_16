using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public float speed = 10f;
    private Rigidbody rb;

    public GameObject gibs;

	IEnumerator Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Camera.main.transform.forward * speed;
		yield return new WaitForSeconds (5f);
		Remove ();
    }

	public void Fire(Transform target)
	{
		Fire (target.position);
	}

	public void Fire(Vector3 target)
	{
		StartCoroutine (Firing (target));
	}

	IEnumerator Firing(Vector3 pos)
	{
		float elapsed = 0f;
		float duration = Vector3.Distance (transform.position, pos);
		Vector3 vectorLerp = Vector3.zero;
		Vector3 start = transform.position;
		Vector3 end = pos;
		Vector3 height = Vector3.up;
		while(elapsed < duration)
		{
			elapsed += Time.deltaTime; 
			float percentageComplete = elapsed / duration;
			vectorLerp = CalculateBezierPoint(Mathf.Lerp(0, 1, percentageComplete), start, start + height, end + height, end);
			transform.position = transform.position + vectorLerp;
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

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.BroadcastMessage("Death");    
        }
        gibs.SetActive(true);
        gibs.transform.parent = null;
        Remove();
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}