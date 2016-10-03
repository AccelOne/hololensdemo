using UnityEngine;
using System.Collections;

public class ActionByGesture : MonoBehaviour {

	public UnitController unit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnSelect () {
		int order;
		if (gameObject.name == "Attack") {
			order = UnitController.ATTACK;
		} else if (gameObject.name == "Collect") {
			order = UnitController.COLLECT;
		} else {
			order = UnitController.DEFEND;
		}

		unit.SetOrder (order);
	}
}
