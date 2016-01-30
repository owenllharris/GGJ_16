using UnityEngine;
using System.Collections;

public class Gib : MonoBehaviour 
{
	IEnumerator Start () 
	{
		yield return new WaitForSeconds(Random.Range(25f, 35f));
		Material mat = transform.GetComponent<Renderer> ().material;
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed <= duration)
		{
			mat.color = new Color (1, 1, 1, Mathf.Lerp(1, 0, (elapsed / duration)));
			elapsed += Time.deltaTime;
			yield return null;
		}
		Destroy (gameObject);
	}
}