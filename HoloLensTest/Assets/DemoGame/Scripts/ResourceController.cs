using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour {

	public bool empty = false;
	private float health = 15;
	private float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (empty && GameplayController.CanUpdate()) {
			gameObject.SetActive (false);
			timer += Time.deltaTime;
			if(timer>=15){
				gameObject.SetActive (true);
				timer = 0;
				empty = false;
			}
		}
	}

	void ModifyHealth (float val) {
		if (GameplayController.CanUpdate()) {
			health += val;
			health = Mathf.Clamp (health, 0, 15);

			if (health <= 0) {
				empty = true;
				timer = 0;
			}
		}
	}

	void ExtractResource (GameObject tower) {
		tower.SendMessage("ModifyResources",2);
	}
}
