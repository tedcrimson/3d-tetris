using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

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
	private bool MoveOn;

	void Start(){
		MoveOn = CanMove;
		endRotation = new GameObject();
		endRotation.name ="rotato";
		ChangeMode(MoveOn);
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
		}else if(Input.GetKeyDown(modeKey) && CanMove){
			ChangeMode(false);
		}
		if(vec != Vector3.zero){
			this.transform.Translate(vec, Space.World);
			Vector3 smoothDir = vec;
			if(!CheckMove()){
				smoothDir = Vector3.zero;
				this.transform.Translate(-vec, Space.World);
				Stick();
				Debug.LogError("Cant Move there");
			}
			LevelManager.Instance.MoveCamera(smoothDir);
		}
	}
	public bool CheckMove(){
		List<Block> blocks =LevelManager.Instance.blocks;
		Transform core = LevelManager.Instance.CoreObject.transform;
		int bound = LevelManager.Instance.MaxLevel;

		
		foreach(Transform c in this.transform){
			if(blocks.Any(x=>x.m_GameObject.transform.position == RoundVector(c.position,1)))
				return false;
			if(!LevelManager.Instance.CheckBounds(this.transform.position-core.position)&&this.transform.position.x < 10)
				return false;
		}
		return true;
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
		}else if(Input.GetKeyDown(modeKey) && CanMove){
			ChangeMode(true);
			this.transform.rotation = endRotation.transform.rotation;
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

	private void Stick(){
		if(IsCore) return;
		IsCore = true;

		LevelManager.Instance.IsFalling = false;
		this.transform.eulerAngles = RoundVector(this.transform.eulerAngles, 90);
		this.transform.position = RoundVector(this.transform.position, 1);
		
		Transform[] children = this.transform.GetComponentsInChildren<Transform>();
		foreach(Transform c in children){
			if(c == this.transform)continue;
			c.parent = LevelManager.Instance.CoreObject.transform;
			// Debug.Log(c.localPosition);
			c.localPosition = RoundVector(c.localPosition,1);
			c.localEulerAngles = Vector3.zero;
			LevelManager.Instance.SetLevel(c.localPosition);
			LevelManager.Instance.blocks.Add(new Block(c.gameObject));
		}
		// other.contacts[0].thisCollider.GetComponent<BoxCollider>().enabled = false;
		LevelManager.Instance.Blow();
		Destroy(endRotation);
		Destroy(this.gameObject);
	}

	private void ChangeMode(bool mode){
		MoveOn = mode;
		GameObject modeParent = GameObject.FindGameObjectWithTag("AxisMode");
		GameObject Rotate = modeParent.transform.GetChild(0).gameObject;
		GameObject Move = modeParent.transform.GetChild(1).gameObject;
		Rotate.SetActive(!mode);
		Move.SetActive(mode);
	}

	/// <summary>
	/// OnCollisionStay is called once per frame for every collider/rigidbody
	/// that is touching rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		LevelManager.Instance.MoveCamera(Vector3.zero);
		if(other.tag == "Kratos"){
			Destroy(this.gameObject);
			LevelManager.Instance.IsFalling = false;
		}else
			Stick();
	}


	public static Vector3 RoundVector(Vector3 vector, int r){
		var vec = vector;
		vec.x = Mathf.Round(vec.x / r) * r;
		vec.y = Mathf.Round(vec.y / r) * r;
		vec.z = Mathf.Round(vec.z / r) * r;
		return vec;
	}

}
