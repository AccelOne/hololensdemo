using UnityEngine;
using System.Collections;

public class BarController : MonoBehaviour {

	public float current = 0;
	public float max = 0;
	public Transform bar;

	private Vector3 pos;
	private Vector3 scale;

	// Use this for initialization
	void Start () {
		pos = bar.localPosition;
		scale = bar.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		float sizeX = (current / max);
		float posX = (1 - sizeX) / -2;

		scale.x = sizeX;
		pos.x = posX;

		bar.localScale = scale;
		bar.localPosition = pos;
	}
}
