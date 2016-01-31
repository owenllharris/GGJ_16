using UnityEngine;
using System.Collections;

public class EnemyStartSpawning : MonoBehaviour {

    private EnemySpawner[] es;

	void Start()
    {
        es = GetComponentsInChildren<EnemySpawner>();
        
    }

    void OnEnable()
    {
        GameManager.StartGame += BeginSpawning;
        GameManager.EndGame += StopSpawning;
    }

    void OnDisable()
    {
        GameManager.StartGame -= BeginSpawning;
        GameManager.EndGame -= StopSpawning;
    }


    void BeginSpawning()
    {
        foreach (EnemySpawner enemy in es)
        {
            enemy.enabled = true;
        }
        
    }

    void StopSpawning()
    {
        foreach (EnemySpawner enemy in es)
        {
            //enemy.enabled = false;
        }
    }

    

}
