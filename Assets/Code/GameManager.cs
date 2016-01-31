using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private static GameManager _instance;
	public static GameManager instance {
		get {
			return _instance;
		}
	}

    public delegate void GamePlayEvent();
    public static event GamePlayEvent StartGame;
    public static event GamePlayEvent EndGame;

    public Text scoreBoard;

    public int health = 100;
    public int score = 0;


    public static GameObject player;
    public GameObject healthBar;

	void Awake()
	{
		if (_instance != null)
		{
			Destroy (gameObject);
			return;
		}
		player = GameObject.FindWithTag("Player");
	}

	void OnEnable()
	{
		if (_instance == null) 
		{
			_instance = this;
		}  
	}

    public void UpdateScore(int value)
    {
        score += value;
        scoreBoard.text = "000" + score;
    }

    public void StartTheGame()
    {
        if (StartGame != null)
            StartGame();
    }

    public void GameOver()
    {
        if (EndGame != null)
            EndGame();
    }


    
}