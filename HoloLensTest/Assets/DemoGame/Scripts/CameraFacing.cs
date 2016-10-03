using UnityEngine;
using System.Collections;

public class CameraFacing : MonoBehaviour
{
	public Camera m_Camera;

	void Awake()
	{
		m_Camera = Camera.main;
	}
	void FixedUpdate()
	{
		Vector3 dir = m_Camera.transform.position - transform.position;
		transform.forward = dir;
		//transform.forward = -m_Camera.transform.forward;
	}
}