using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public GameBoard gameBoard;

	// Use this for initialization
	void Start () {
		#if !UNITY_EDITOR
		gameBoard.gameObject.SetActive (true);
		gameBoard.OnMove ();
		#endif

	}
	
	// Update is called once per frame
	void Update () {
		//WIN/LOSE CONDITIONS
	}
}
