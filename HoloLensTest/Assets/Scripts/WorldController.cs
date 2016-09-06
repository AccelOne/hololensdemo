using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;

public class WorldController : MonoBehaviour {

	public GameObject charPref;
	public GameObject floor;

	public bool drawMeshes = false;
	public GameObject currChar = null;

	void Start () {
		SpatialMappingManager.Instance.DrawVisualMeshes = drawMeshes;

		#if UNITY_EDITOR
		floor.SetActive (true);
		#else
		floor.SetActive (false);
		#endif
	}

	public void ChangeWireFrame () {
		drawMeshes = !drawMeshes;
		SpatialMappingManager.Instance.DrawVisualMeshes = drawMeshes;
	}

	void CreateCharacter () {
		if (currChar == null) {
			currChar = Instantiate (charPref) as GameObject;
			PlaceableObject po = currChar.GetComponent<PlaceableObject> ();
			po.OnSelect ();
		}
	}

	public void CreationByVoice () {
		if (currChar != null) {
			PlaceableObject po = currChar.GetComponent<PlaceableObject> ();
			if (po.placing) {
				po.OnSelect ();
			}
			currChar = null;
		}
		CreateCharacter ();
	}

	void Update () {
		#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0)){
			CreateCharacter();
		}
		if(Input.GetMouseButtonUp(0)){
			if (currChar != null) {
				PlaceableObject po = currChar.GetComponent<PlaceableObject> ();
				po.OnSelect ();
				currChar = null;
			}
		}
		#endif
	}
}
