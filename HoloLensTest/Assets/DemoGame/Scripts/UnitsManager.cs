using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class UnitsManager : MonoBehaviour {

	public void OnCollect()
	{
		UnitController unit = GetGazedUnit ();
		if (unit) {
			unit.SetOrder (UnitController.COLLECT);
		}
	}

	public void OnDefend()
	{
		UnitController unit = GetGazedUnit ();
		if (unit) {
			unit.SetOrder (UnitController.DEFEND);
		}
	}

	public void OnAttack()
	{
		UnitController unit = GetGazedUnit ();
		if (unit) {
			unit.SetOrder (UnitController.ATTACK);
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
