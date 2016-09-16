using UnityEngine;
using System.Collections;

public class ResourceController : MonoBehaviour {

	public float health = 15;
	public GameObject rock;
	public GameObject destroyed;

	private float timer = 0;
	
	// Update is called once per frame
	void Update () {
		if (health <= 0 && GameplayController.CanUpdate()) {
			rock.SetActive (false);
			destroyed.SetActive (true);
			timer += Time.deltaTime;
			if(timer>=15){
				rock.SetActive (true);
				destroyed.SetActive (false);
				timer = 0;
				health = 15;
			}
		}
	}

	void ModifyHealth (float val) {
		if (GameplayController.CanUpdate()) {
			health += val;
			health = Mathf.Clamp (health, 0, 15);

			if (health <= 0) {
				health = 0;
				timer = 0;
			}
		}
	}

	void ExtractResource (GameObject tower) {
		if (GameplayController.CanUpdate () && health > 0) {
			tower.SendMessage ("ModifyResources", 2);
		}
	}
}
