using UnityEngine;
using System.Collections;

public class ShowOnGameOver : MonoBehaviour {

    void OnEnable()
    {
        GameManager.EndGame += ShowMe;
    }

    void OnDisable()
    {
        GameManager.EndGame -= ShowMe;
    }

    void ShowMe()
    {
        GetComponent<Renderer>().enabled = true;

    }
}
