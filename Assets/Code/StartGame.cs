using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

    void OnEnable()
    {
        GameManager.StartGame += RemoveMe;
    }

    void OnDisable()
    {
        GameManager.StartGame -= RemoveMe;
    }


    void OnCollisionEnter(Collision col)
    {
        GameManager.instance.StartTheGame();
        
    }

    void RemoveMe()
    {
        Destroy(gameObject);
    }
}
