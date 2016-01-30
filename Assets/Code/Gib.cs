using UnityEngine;
using System.Collections;

public class Gib : MonoBehaviour 
{

    public AudioClip[] clips;
    AudioSource a;
	IEnumerator Start () 
	{
        a = GetComponent<AudioSource>();
        clips = new AudioClip[6];
		yield return new WaitForSeconds(Random.Range(10f, 20f));
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

    void OnCollisionEnter(Collision col) {
        a.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}