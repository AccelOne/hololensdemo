using UnityEngine;
using System.Collections;

public class BlueCharController : MonoBehaviour {

	public Transform head;
	public Transform rangeQuad;
	public float rotationSpeed= 4.0f;
	public float viewRange= 20.0f;
	public float viewAngle = 90.0f;
	public float runDistance = 5.0f;

	public float followAngle = 20;

	Animator anim;
	Transform target;
	TextMesh text;
	PlaceableObject placeable;

	enum CharacterState {none, idle, patrol, follow};
	CharacterState state = CharacterState.idle;

	// Use this for initialization
	void Start () {
		placeable = gameObject.GetComponent<PlaceableObject> ();
		anim = gameObject.GetComponentInChildren<Animator> ();
		text = gameObject.GetComponentInChildren<TextMesh> ();
		target = Camera.main.transform;

		text.gameObject.SetActive (false);
		rangeQuad.gameObject.SetActive (false);

		#if UNITY_EDITOR
		//state = CharacterState.patrol;
		BlueCharacterConsts.debug = true;
		#endif
	}

	void Update () {
		if (placeable.placing) {
			return;
		}

		switch (state) {
		case CharacterState.idle: Idle (); break;
		case CharacterState.patrol: Patrol (); break;
		case CharacterState.follow: Following (); break;
		default: break;
		}

		if (BlueCharacterConsts.debug) {
			text.gameObject.SetActive (true);
			text.text = "";
			if(target) {
				Vector3 fixedPos = target.position;
				fixedPos.y = head.position.y;
				string val = string.Format ("{0:0.#}", Vector3.Distance (target.position, head.position));
				text.text = "dist: "+val+"\n";
			}
			text.text += "speed: "+BlueCharacterConsts.speed.ToString();

			//9.7f = 0.5f
			//19.4 = 1
			rangeQuad.gameObject.SetActive (true);
			rangeQuad.localScale = Vector3.one * (BlueCharacterConsts.followRange * 19.4f);
			//DebugExtension.DebugCircle(head.position, transform.up, Color.red, BlueCharacterConsts.followRange);
		} else {
			text.gameObject.SetActive (false);
			rangeQuad.gameObject.SetActive (false);
		}

	}

	void Idle () {
		//anim.Play ("idle");
		anim.SetTrigger ("rest");
	}

	void Patrol () {
		//anim.Play ("idle");
		anim.SetTrigger ("rest");
		if (CanSeeTarget ()){
			state = CharacterState.follow;
		}
	}

	void Following () {
		if (CanSeeTarget()) 
		{
			Vector3 fixedPos = target.position;
			fixedPos.y = head.position.y;

			// if no player (maybe dead) stop following
			if (target == null){
				state = CharacterState.idle;
			}

			float distance = Vector3.Distance(head.position, fixedPos);
			// player is too far away, then give up
			if (distance > viewRange){
				state = CharacterState.patrol;
			}
				
			if (distance > BlueCharacterConsts.followRange){
				//if distance is greater than the attack range, move to last visible pos
				MoveTowards (fixedPos);

			} else{
				//anim.Play ("idle");
				anim.SetTrigger ("rest");
				//else rotate towards target
				RotateTowards(fixedPos);

			}

			if (IsInRange (BlueCharacterConsts.followRange, followAngle)) {
				state = CharacterState.patrol;
			}
		}
		else 
		{
			state = CharacterState.patrol;
		}
	}

	bool IsInRange (float range, float minAngle) {
		Vector3 fixedPos = target.position;
		fixedPos.y = head.position.y;

		float distance = Vector3.Distance(head.position, fixedPos);
		Vector3 targetDirection = fixedPos - head.position;
		targetDirection.y = 0;

		float angle = Vector3.Angle(targetDirection, transform.forward);

		// start the attack if the target is close and inside angle
		if (distance < range && angle < minAngle)
		{
			return true;
		}

		return false;
	}

	bool CanSeeTarget ()
	{
		Vector3 fixedPos = target.position;
		fixedPos.y = head.position.y;

		//First, check for the player inside range
		if (Vector3.Distance(head.position, fixedPos) > viewRange) {
			return false;
		}

		Vector3 forward= transform.TransformDirection(Vector3.forward);
		Vector3 targetDirection= fixedPos - head.position;
		targetDirection.y = 0;
		float angle = Vector3.Angle(targetDirection, forward);

		if(angle>viewAngle)
		{
			return false;
		}
			
		RaycastHit hit;

		//Using a layerMask we can ignore colliders
		LayerMask layerMask = 1 << LayerMask.NameToLayer ("Character") |
		                      1 << LayerMask.NameToLayer ("Ground");

		//here we'll have all layers but the shifted
		layerMask = ~layerMask;

		if (Physics.Linecast (head.position, fixedPos, out hit,layerMask))
		{
			//with a line checks if no object is blocking the view
			return false;
		}

		return true;
	}

	Vector3 RotateTowards (Vector3 position)
	{
		Vector3 direction = position - head.position;
		direction.y = 0;

		if (direction.magnitude < 0.1f)
		{
			return direction;
		}

		// rotate towards target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.smoothDeltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

		return direction;
	}

	void  MoveTowards (Vector3 position)
	{
		Vector3 direction = RotateTowards (position);

		// modifies speed to slowdown when is close to target
		Vector3 forward= transform.TransformDirection(Vector3.forward);
		float speedModifier= Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);

		// moves character
		if(Vector3.Distance(head.position, position) > runDistance){
			//anim.Play ("run");
			anim.SetTrigger ("run");
			direction = forward * (BlueCharacterConsts.speed*2) * speedModifier;
		}
		else{
			//anim.Play ("walk");
			anim.SetTrigger ("walk");
			direction = forward * BlueCharacterConsts.speed * speedModifier;
		}
		direction.y=0;

		transform.Translate(direction*Time.smoothDeltaTime, Space.World);
	}


	//add follow events
	void OnEnable(){
		BlueCharacterConsts.onFollow += OnFollow;
	}

	//remove new device event
	void OnDisable() {
		BlueCharacterConsts.onFollow -= OnFollow;
	}

	void OnFollow (bool follow){
		state = follow ? CharacterState.follow : CharacterState.idle;
	}
}
