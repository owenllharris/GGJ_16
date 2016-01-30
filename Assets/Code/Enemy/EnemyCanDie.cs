using UnityEngine;
using System.Collections;

public class EnemyCanDie : MonoBehaviour {

    GameObject gibs;

    void Start()
    {
        gibs = transform.FindChild("Gibs").gameObject;
    }


    void Death()
    {
        gibs.SetActive(true);
        gibs.transform.parent = null;
        Destroy(gameObject);
    }	
}