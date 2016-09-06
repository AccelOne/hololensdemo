using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine.VR.WSA.Input;

public class PlaceableObject : MonoBehaviour {

	[HideInInspector]
	public bool placing = false;

	Rigidbody rb;
	BoxCollider box;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		box = GetComponent<BoxCollider> ();
	}

	// Called by GazeGestureManager when the user performs a Select gesture
	public void OnSelect()
	{
		if (rb == null) {
			rb = GetComponent<Rigidbody> ();
		}

		if (box == null) {
			box = GetComponent<BoxCollider> ();
		}

		// On each Select gesture, toggle whether the user is in placing mode.
		placing = !placing;

		// If the user is in placing mode, display the spatial mapping mesh.
		if (placing)
		{
			box.enabled = true;
			rb.useGravity = false;
			SpatialMappingManager.Instance.DrawVisualMeshes = true;
		}
		// If the user is not in placing mode, hide the spatial mapping mesh.
		else
		{
			box.enabled = false;
			rb.useGravity = true;
			SpatialMappingManager.Instance.DrawVisualMeshes = false;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// If the user is in placing mode,
		// update the placement to match the user's gaze.

		if (placing)
		{
			// Do a raycast into the world that will only hit the Spatial Mapping mesh.
			var headPosition = Camera.main.transform.position;
			var gazeDirection = Camera.main.transform.forward;

			RaycastHit hitInfo;
			if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
				30.0f, SpatialMappingManager.PhysicsRaycastMask))
			{
				// Move this object to
				// where the raycast hit the Spatial Mapping mesh.
				this.transform.position = hitInfo.point + (Vector3.up*0.1f);
				//this.transform.position = hitInfo.point;

				Vector3 camPos = Camera.main.transform.position;
				camPos.y = this.transform.position.y;
				Vector3 direction = camPos - this.transform.position;
				transform.rotation = Quaternion.LookRotation(direction);
				//transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			}
		}
	}
}
