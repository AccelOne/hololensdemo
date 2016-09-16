using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {

	public bool autoPilot = false;
	public BarController health;
	public BarController resources;
	public GameObject unitPref;
	public Transform[] spawners;
	public GameObject cooldownClock;
	public Transform unitsContainer;

	private bool cooling = false;
	private float cooldownTime = 5;
	private float resourcesSpeed = 0.1f;
	private float clockRotationZ = 360;

	// Use this for initialization
	void Start () {
		health.current = health.max = 100;
		resources.current = resources.max = 100;

		#if !UNITY_EDITOR
		if (gameObject.tag == "PlayerTower") {
			autoPilot = false;
		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameplayController.CanUpdate()) {
			return;
		}

		resources.current += resourcesSpeed*Time.deltaTime;
		UpdateCooldown ();
		AutoSpawn ();
	}

	void UpdateCooldown () {
		if (cooling) {
			cooldownClock.SetActive (true);
			clockRotationZ -= (360/5) * Time.deltaTime;
			if (clockRotationZ <= 0) {
				clockRotationZ = 360;
				cooling = false;
			}
			cooldownClock.transform.localRotation = Quaternion.Euler (0, 0, clockRotationZ);
		} else {
			cooldownClock.SetActive (false);
		}
	}

	void ModifyResources (float val) {
		if (GameplayController.CanUpdate()) {
			resources.current += val;
			resources.current = Mathf.Clamp (resources.current, 0, resources.max);
		}
	}

	void ModifyHealth (float val) {
		if (GameplayController.CanUpdate()) {
			health.current += val;
			health.current = Mathf.Clamp (health.current, 0, health.max);
		}
	}

	public void Spawn () {
		if (!GameplayController.CanUpdate() || cooling || resources.current < 10) {
			return;
		}

		//pick a spawner
		int idx = Random.Range(0,3);
		Vector3 spawnPos = spawners [idx].position;
		GameObject newUnit = Instantiate (unitPref) as GameObject;
		newUnit.transform.SetParent (unitsContainer);
		newUnit.transform.position = spawnPos;
		newUnit.transform.localScale = Vector3.one;

		//if autopilot on, pick an order
		if (autoPilot) {
			int order = resources.current <= 15 ? UnitController.COLLECT : Random.Range (0, 3);
			newUnit.SendMessage ("SetOrder", order);
		}

		cooling = true;
		resources.current -= 10;
	}

	float timer = 0;
	void AutoSpawn () {
		if (autoPilot) {
			timer += Time.deltaTime;
			if (timer >= 2) {
				Spawn ();
				timer = 0;
			}
		}
	}
}
