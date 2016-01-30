using UnityEngine;
using System.Collections;

public class StickToTarget : MonoBehaviour {


    public GameObject target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position + (target.transform.right * offsetX) + (target.transform.up * offsetY) + (target.transform.forward * offsetZ);
        transform.rotation = target.transform.rotation;
	}


}
