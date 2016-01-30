using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameObject player;



	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
