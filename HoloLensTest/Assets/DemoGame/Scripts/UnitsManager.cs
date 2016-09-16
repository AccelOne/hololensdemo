using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class UnitsManager : MonoBehaviour {

	public GameSpeech speech;

	public void OnCollect()
	{
		UnitController unit = GetGazedUnit ();
		if (unit && !unit.enemy) {
			unit.SetOrder (UnitController.COLLECT);
			speech.OnTalk(unit.gameObject, "Yes sir!", false);
		}
	}

	public void OnDefend()
	{
		UnitController unit = GetGazedUnit ();
		if (unit && !unit.enemy) {
			unit.SetOrder (UnitController.DEFEND);
			speech.OnTalk(unit.gameObject, "Yes sir!", false);
		}
	}

	public void OnAttack()
	{
		UnitController unit = GetGazedUnit ();
		if (unit && !unit.enemy) {
			unit.SetOrder (UnitController.ATTACK);
			speech.OnTalk(unit.gameObject, "Yes sir!", false);
		}
	}

	UnitController GetGazedUnit () {
		GazeManager gm = GazeManager.Instance;
		if (gm.FocusedObject)
		{
			// Get the target object
			GameObject obj = gm.FocusedObject;
			UnitController unit = obj.GetComponent<UnitController>();

			return unit;
		} 

		return null;
	}
}
