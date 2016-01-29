using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

    public float speed = 10f;
    private Rigidbody rb;

    public GameObject gibs;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Camera.main.transform.forward * speed;
        Invoke("Remove", 10f);
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
