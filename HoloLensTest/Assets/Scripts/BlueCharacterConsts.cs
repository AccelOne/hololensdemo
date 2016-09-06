using UnityEngine;
using System.Collections;
using System;

public class BlueCharacterConsts : MonoBehaviour {

	public static Action<bool> onFollow = delegate {};

	public static float speed = 0.1f;
	public static float followRange = 0.3f;
	public static bool debug = false;

	private bool following;

	public void StartFollowing () {
		//state = CharacterState.patrol;
		following = true;
		BlueCharacterConsts.onFollow (following);
	}

	public void StopFollowing () {
		//state = CharacterState.idle;
		following = false;
		BlueCharacterConsts.onFollow (following);
	}

	public void Closer () {
		followRange -= 0.1f;
		followRange = Mathf.Clamp (followRange, 0.0f, followRange);
	}

	public void Farther () {
		followRange += 0.1f;
	}

	public void Faster () {
		speed += 0.1f;
	}

	public void Slower () {
		speed -= 0.1f;
		speed = Mathf.Clamp (followRange, 0.1f, followRange);
	}

	public void ShowDebug () {
		debug = true;
	}

	public void HideDebug () {
		debug = false;
	}

	void Update () {
		#if UNITY_EDITOR
		if (Input.GetMouseButtonDown (2)) {
			following = !following;
			BlueCharacterConsts.onFollow (following);
		}
		#endif
	}
}
