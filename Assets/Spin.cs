using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

	public KeyCode submitKey;
	public KeyCode upkey;
	public KeyCode downKey;
	public KeyCode leftKey;
	public KeyCode rightKey;	
	public KeyCode zleftKey;
	public KeyCode zrightKey; 
	public KeyCode modeKey;

	[SpaceAttribute]
	public float speed = 10;

	public bool CanMove = false;
	private GameObject endRotation;
	private bool MoveOn = false;

	void Start(){
		endRotation = new GameObject();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(submitKey)){
			GetComponent<Rigidbody>().AddForce(Vector3.left*150);
			return;
		}

		if(MoveOn)
			Move();
		else
			Rotate();
	}

	private void Move(){
		Vector3 vec = Vector3.zero;
		if(Input.GetKeyDown(upkey)){
			vec = Vector3.up;
		}else if(Input.GetKeyDown(downKey)){
			vec = Vector3.down;
		}else if(Input.GetKeyDown(leftKey)){
			vec = Vector3.back;
		}else if(Input.GetKeyDown(rightKey)){
			vec = Vector3.forward;
		}else if(Input.GetKeyDown(modeKey)){
			MoveOn = false;
		}
		if(vec != Vector3.zero){
			this.transform.Translate(vec, Space.World);
		}

	}

	private void Rotate(){
		Vector3 axis = Vector3.zero;
		if(Input.GetKeyDown(upkey)){
			axis = Vector3.forward;
		}else if(Input.GetKeyDown(downKey)){
			axis = Vector3.back;
		}else if(Input.GetKeyDown(leftKey)){
			axis = Vector3.up;
		}else if(Input.GetKeyDown(rightKey)){
			axis = Vector3.down;
		}else if(Input.GetKeyDown(zleftKey)){
			axis = Vector3.left;
		}else if(Input.GetKeyDown(zrightKey)){
			axis = Vector3.right;
		}else if(Input.GetKeyDown(modeKey)){
			MoveOn = true;
		}
		if(axis != Vector3.zero){
			endRotation.transform.Rotate(axis * 90, Space.World);
		}

		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, endRotation.transform.rotation, Time.deltaTime*speed);
	}

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other)
	{
		this.transform.parent = other.collider.transform;
		enabled = false;
	}
}
