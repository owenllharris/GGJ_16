using UnityEngine;
using System.Collections;

public class Gibs : MonoBehaviour {

    public GameObject gib;

    public int numberOfGibs = 20;
    public float force = 100f;


	// Use this for initialization
	void Start () {
        for (int i = 0; i < numberOfGibs; i++)
        {
            Vector3 loc = Random.onUnitSphere;
            GameObject newGib = Instantiate(gib, transform.position + (loc * 0.2f), gib.transform.rotation) as GameObject;
            newGib.GetComponent<Rigidbody>().AddForce(loc * force);
        }
	}

}
