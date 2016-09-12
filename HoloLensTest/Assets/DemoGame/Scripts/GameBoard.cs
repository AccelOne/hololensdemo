﻿using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class GameBoard : MonoBehaviour {

	public static float scale = 1;
	public static bool placing = true;

	float height = 0;

	public void IncreaseHeight () {
		height += 0.1f;
	}

	public void DecreaseHeight () {
		height -= 0.1f;
	}

	public void OnMove () {
		placing = true;
		SpatialMappingManager.Instance.DrawVisualMeshes = true;
	}

	public void OnDone () {
		placing = false;
		SpatialMappingManager.Instance.DrawVisualMeshes = false;
	}

	public void OnSelect()
	{
		placing = !placing;

		if (placing) {
			// If the user is in placing mode, display the spatial mapping mesh.
			SpatialMappingManager.Instance.DrawVisualMeshes = true;

		} else {
			// If the user is not in placing mode, hide the spatial mapping mesh.
			SpatialMappingManager.Instance.DrawVisualMeshes = false;
		}
	}
	
	void Update()
	{
		// If the user is in placing mode,
		// update the placement to match the user's gaze.

		if (placing) {
			// Do a raycast into the world that will only hit the Spatial Mapping mesh.
			var headPosition = Camera.main.transform.position;
			var gazeDirection = Camera.main.transform.forward;

			RaycastHit hitInfo;
			if (Physics.Raycast (headPosition, gazeDirection, out hitInfo,
				    30.0f, SpatialMappingManager.PhysicsRaycastMask)) {
				// Move this object to
				// where the raycast hit the Spatial Mapping mesh.
				this.transform.position = hitInfo.point + (Vector3.up * height);
				//this.transform.position = hitInfo.point;

				Vector3 camPos = Camera.main.transform.position;
				camPos.y = this.transform.position.y;
				Vector3 direction = camPos - this.transform.position;
				transform.rotation = Quaternion.LookRotation (direction);
				//transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			}

		} 
	}
}