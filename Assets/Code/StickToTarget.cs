using UnityEngine;
using System.Collections;

public class StickToTarget : MonoBehaviour {


    public GameObject target;
    public Vector3 offset = Vector3.zero;

    
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + offset;
        transform.rotation = target.transform.rotation;
	}
}
