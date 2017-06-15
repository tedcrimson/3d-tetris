using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

	public KeyCode upkey;
	public KeyCode downKey;
	public KeyCode leftKey;
	public KeyCode rightKey;	
	public KeyCode zleftKey;
	public KeyCode zrightKey; 
	public KeyCode modeKey;

	public bool IsCore;

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
			endRotation.transform.eulerAngles = RoundVector(endRotation.transform.eulerAngles, 90);
		}
		if(Quaternion.Angle(endRotation.transform.rotation, this.transform.rotation) < 3)
			this.transform.rotation = endRotation.transform.rotation;
		else
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, endRotation.transform.rotation, Time.deltaTime*speed);
	}

	/// <summary>
	/// OnCollisionStay is called once per frame for every collider/rigidbody
	/// that is touching rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		if(IsCore) return;
		IsCore = true;
		Debug.LogError(this.transform.name);

		LevelManager.Instance.IsFalling = false;
		this.transform.parent = LevelManager.Instance.CoreObject.transform;
		this.transform.localEulerAngles = RoundVector(this.transform.localEulerAngles, 90);
		this.transform.localPosition = RoundVector(this.transform.localPosition, 1);
		
		foreach(Transform c in this.transform){
			c.GetComponent<BoxCollider>().isTrigger = true;
			LevelManager.Instance.blocks.Add(c.position);
		}
		// other.contacts[0].thisCollider.GetComponent<BoxCollider>().enabled = false;

		enabled = false;
	}

	public Vector3 RoundVector(Vector3 vector, int r){
		Debug.Log(vector + " R "+ r);
		var vec = vector;
		vec.x = Mathf.Round(vec.x / r) * r;
		vec.y = Mathf.Round(vec.y / r) * r;
		vec.z = Mathf.Round(vec.z / r) * r;
		Debug.Log(vec);
		return vec;
	}

}
