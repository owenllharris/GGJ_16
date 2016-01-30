using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private static GameManager _instance;
	public static GameManager instance {
		get {
			return _instance;
		}
	}

    public static GameObject player;

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

	void Start () 
	{
	}
}