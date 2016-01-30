using UnityEngine;
using System.Collections;

public class PlayerDie : MonoBehaviour, IKillable
{
	public void Kill()
	{
		Debug.Log ("Player Dead");
	}
}