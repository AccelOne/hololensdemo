using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class GameBoard : MonoBehaviour {

	public static float scale = 0.1f;
	public static bool placing = false;

	public AudioSource musicSrc;

	private float originalSoundMaxDistance;
	float height = 0;

	void Start () {
		Medium ();
		placing = false;
		originalSoundMaxDistance = musicSrc.maxDistance;
	}

	//preset sizes
	public void Small () {
		scale = 0.05f;
		transform.localScale = Vector3.one * scale;
	}
	public void Medium () {
		scale = 0.1f;
		transform.localScale = Vector3.one * scale;
	}
	public void Big () {
		scale = 0.5f;
		transform.localScale = Vector3.one * scale;
	}

	//manual size
	public void IncreaseSize () {
		if (scale >= 0.1f) {
			scale += 0.1f;
		} else {
			scale += 0.01f;
		}
		transform.localScale = Vector3.one * scale;
	}
	public void DecreaseSize () {

		if (scale <= 0.1f) {
			scale -= 0.01f;
		} else {
			scale -= 0.1f;
		}

		if (scale <= 0.01f) {
			scale = 0.01f;
		}
		transform.localScale = Vector3.one * scale;
	}

	//manual height
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
		musicSrc.maxDistance = originalSoundMaxDistance * GameBoard.scale;

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
