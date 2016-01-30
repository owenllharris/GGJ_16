using UnityEngine;
using System.Collections;

public class SpawnFireball : MonoBehaviour {

    public GameObject fireBall;

    public SteamVR_TrackedController controller;

    void Start()
    {
        controller.TriggerClicked += new ClickedEventHandler(FireBall);

    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Instantiate(fireBall, transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
        }
	}



    void FireBall(object sender, ClickedEventArgs e)
    {
        Instantiate(fireBall, transform.position + Camera.main.transform.forward, transform.rotation);

    }
}
