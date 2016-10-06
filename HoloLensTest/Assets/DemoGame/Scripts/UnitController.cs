using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour {

	public const int COLLECT = 0;
	public const int DEFEND = 1;
	public const int ATTACK = 2;

	public bool enemy = true;
	public float speed = 0.1f;
	public float rotSpeed= 4.0f;
	public float viewRange= 0.9f;
	public float attackRange = 0.2f;

	public float health = 5;
	public float damage = 1;
	public GameObject actions;
	public AudioSource audioSrc;
	public AudioClip attackSnd;
	public AudioClip collectSnd;
	public Transform mainTarget = null;
	public Transform target = null;
	private int order = -1;
	private Animator anim;
	private GameObject myTower;
	private GameObject enemyTower;
	private ResourceController curResource = null;
	private float originalSoundMaxDistance;

	public enum UnitState {none, idle, walk, attack, dead};
	public UnitState state = UnitState.idle;

	// Use this for initialization
	void Start () {
		originalSoundMaxDistance = audioSrc.maxDistance;
		anim = gameObject.GetComponentInChildren<Animator> ();
		anim.Play ("idle");
		GetTowers ();
	}

	void GetTowers () {
		if (myTower == null || enemyTower == null) {
			myTower = enemy ? GameplayController.enemy.gameObject : GameplayController.player.gameObject;
			enemyTower = enemy ? GameplayController.player.gameObject : GameplayController.enemy.gameObject;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!GameplayController.CanUpdate()) {
			return;
		}

		audioSrc.maxDistance = originalSoundMaxDistance * GameBoard.scale;

		//check for near enemies while target is the main target
		if (target == mainTarget) {
			target = CheckForEnemies ();
		}

		//enemy killed look for new target
		/*if (target == null && (order == DEFEND || order == ATTACK)) {
			state = UnitState.idle;
			target = CheckForEnemies ();
		}*/

		//resource is empty, look for a new one
		if (curResource != null && curResource.health <= 0 && order == COLLECT) {
			state = UnitState.idle;
			target = mainTarget = GetClosestResource ();
		}

		//no targets at all, go back to main target and idle
		if (target == null) {
			target = mainTarget;
			state = UnitState.idle;
		}

		switch (state) {
		case UnitState.idle: Idle (); break;
		case UnitState.walk: Walk (); break;
		case UnitState.attack: Attack (); break;
		case UnitState.dead: Die (); break;
		default: break;
		}

	}

	void Idle () {
		anim.Play ("idle");
		if (target != null) {
			state = UnitState.walk;
			return;
		}
	}

	void Walk () {
		float distance = Vector3.Distance(transform.position, target.position);

		if (distance > attackRange*GameBoard.scale){
			anim.Play ("walk");
			MoveTowards (target.position);

		} else{
			if (target == myTower.transform) {
				state = UnitState.idle;
			} else {
				state = UnitState.attack;
			}
		}

	}

	private float timer=0;
	void Attack () {
		float distance = Vector3.Distance(transform.position, target.position);
		if (distance > attackRange*GameBoard.scale){
			state = UnitState.walk;
			timer = 0;
			return;
		}

		RotateTowards (target.position);

		anim.Play ("attack");
		timer += Time.deltaTime;
		if (timer >= 0.9f) {
			target.SendMessage("ModifyHealth",-damage);
			if (target.gameObject.tag == "Resource") {
				target.SendMessage ("ExtractResource", myTower, SendMessageOptions.DontRequireReceiver);
				audioSrc.PlayOneShot (collectSnd);
			} else {
				audioSrc.PlayOneShot (attackSnd);
			}

			timer = 0;
		}
	}

	void Die () {
		Destroy (gameObject);
	}

	public void SetOrder (int val) {
		GetTowers ();
		order = val;
		if (order == COLLECT) {
			target = mainTarget = GetClosestResource ();
		} else if (order == DEFEND) {
			target = mainTarget = myTower.transform;
		} else if (order == ATTACK) {
			target = mainTarget = enemyTower.transform;
		} else {
			target = mainTarget = null;
			order = -1;
		}
		showActions = false;
		actions.SetActive (showActions);
	}

	Transform GetClosestResource ()
	{
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(GameObject potentialTarget in GameplayController.resources)
		{
			ResourceController resource = potentialTarget.GetComponent<ResourceController> ();
			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr && resource.health > 0)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget.transform;
			}
		}

		if (bestTarget != null) {
			curResource = bestTarget.gameObject.GetComponent<ResourceController> ();
		} else {
			curResource = null;
		}

		return bestTarget;
	}

	Transform CheckForEnemies () {
		int layer = enemy ? LayerMask.NameToLayer ("Knight") : LayerMask.NameToLayer ("Goblin");
		LayerMask layerMask = 1 << layer;

		Collider[] cols = Physics.OverlapSphere (transform.position, viewRange * GameBoard.scale, layerMask);
		Transform bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach (Collider col in cols) {
			Vector3 directionToTarget = col.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = col.transform;
			}
		}

		if (bestTarget != null) {
			return bestTarget;
		} else {
			return mainTarget;
		}
	}

	void  MoveTowards (Vector3 position)
	{
		Vector3 direction = RotateTowards (position);

		// modifies speed to slowdown when is close to target
		Vector3 forward= transform.TransformDirection(Vector3.forward);
		float speedModifier= Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);

		// moves character
		direction = forward * speed * GameBoard.scale * speedModifier;
		direction.y=0;

		transform.Translate(direction*Time.smoothDeltaTime, Space.World);
	}

	Vector3 RotateTowards (Vector3 position)
	{
		Vector3 direction = position - transform.position;
		direction.y = 0;

		/*if (direction.magnitude < 0.1f)
		{
			return direction;
		}*/

		// rotate towards target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.smoothDeltaTime);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

		return direction;
	}

	void ModifyHealth (float val) {
		if (GameplayController.CanUpdate()) {
			health += val;
			health = Mathf.Clamp (health, 0, 5);

			if (health <= 0) {
				state = UnitState.dead;
			}
		}
	}

	private bool showActions = false;
	void OnSelect () {
		if (!enemy) {
			showActions = !showActions;
			actions.SetActive (showActions);
		}
	}
}
