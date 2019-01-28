using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCharacter : MonoBehaviour {
	public bool canMove;
	Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!canMove)
			return;
		float x = Input.GetAxis ("Horizontal");
		if (x < 0)
			transform.eulerAngles= new Vector3(0, 180, 0);
		else if (x > 0)
			transform.eulerAngles= new Vector3(0, 0, 0);
		
		anim.SetFloat ("Speed", Mathf.Abs(x));
	}
}
