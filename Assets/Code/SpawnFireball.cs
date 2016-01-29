using UnityEngine;
using System.Collections;

public class SpawnFireball : MonoBehaviour {

    public GameObject fireBall;
    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Instantiate(fireBall, transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
        }
	}
}
