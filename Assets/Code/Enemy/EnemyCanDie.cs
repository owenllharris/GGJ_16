using UnityEngine;
using System.Collections;

public class EnemyCanDie : MonoBehaviour {

    GameObject gibs;

    void OnEnable()
    {
        GameManager.EndGame += RemoveMe;
    }

    void OnDisable()
    {
        GameManager.EndGame -= RemoveMe;
    }


    void Start()
    {
        gibs = transform.FindChild("Gibs").gameObject;
    }


    void Death()
    {
        gibs.SetActive(true);
        gibs.transform.parent = null;

        // add to score
        
        GameManager.instance.UpdateScore(50);

        Debug.Log("Score: " +GameManager.instance.score);

        Destroy(gameObject);
    }
    
    void RemoveMe()
    {
        Destroy(gameObject);
    }	
}