using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public float speed = 10f;
    private Rigidbody rb;
    public AudioClip shootSound, loopSound, impactSound;
    AudioSource a;
    public GameObject gibs;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
a = GetComponent<AudioSource>();
        a.pitch = Random.Range(0.7f, 1.3f);
        a.volume = Random.Range(0.8f, 1f);
        a.PlayOneShot(shootSound);
        StartCoroutine(PlayLoop(shootSound.length));        
		Invoke("Remove", 10f);
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            col.gameObject.BroadcastMessage("Death");
            a.Stop();
            a.PlayOneShot(impactSound);
        }
        gibs.SetActive(true);
        gibs.transform.parent = null;
        StartCoroutine( Remove(impactSound.length) );
    }


    IEnumerator Remove(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    IEnumerator PlayLoop(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        a.clip = loopSound;
        a.loop = true;
        a.Play();
    }
}
