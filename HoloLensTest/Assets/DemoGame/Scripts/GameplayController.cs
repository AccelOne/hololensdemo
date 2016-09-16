using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour {

	public static bool Won = false;
	public static bool Lost = false;
	public static bool Paused = false;

	public static TowerController player;
	public static TowerController enemy;
	public static GameObject[] resources;

	public GameSpeech speech;
	public GameBoard gameBoard;
	public TextMesh finalMessage;
	private bool gameEnded = false;

	// Use this for initialization
	void Start () {
		#if !UNITY_EDITOR
		gameBoard.gameObject.SetActive (true);
		gameBoard.OnMove ();
		#endif

		Won = false;
		Lost = false;
		Paused = false;

		player = GameObject.FindWithTag("PlayerTower").GetComponent<TowerController>();
		enemy = GameObject.FindWithTag("EnemyTower").GetComponent<TowerController>();
		resources = GameObject.FindGameObjectsWithTag ("Resource");
		finalMessage.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameEnded) {
			return;
		}

		if (enemy.health.current <= 0) {
			finalMessage.gameObject.SetActive (true);
			finalMessage.text = "Victory!";
			Won = gameEnded = true;
			speech.OnWin ();
		} else if (player.health.current <= 0) {
			finalMessage.gameObject.SetActive (true);
			finalMessage.text = "You Lost";
			Lost = gameEnded = true;
			speech.OnLose ();
		}
	}

	public void OnPause () {
		Paused = !Paused;
	}

	public void OnAutopilotOn () {
		player.autoPilot = true;
	}
	public void OnAutopilotOff () {
		player.autoPilot = false;
	}

	public void OnCreate () {
		player.Spawn ();
	}

	public void OnReset () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public static bool CanUpdate () {
		return (!GameBoard.placing && !Won && !Lost && !Paused);
	}
}
