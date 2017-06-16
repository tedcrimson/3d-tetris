using UnityEngine;

public class Block {
	public GameObject Object;
	public Vector3 Position;


	public Block(GameObject Object){
		this.Object = Object;
		this.Position = Object.transform.localPosition;
	}

	public Block(GameObject Object, Vector3 Dir){
		this.Object = Object;
		this.Position = Object.transform.localPosition;
	}

	public Block(GameObject Object, Vector3 Position, Vector3 NextPosition){
		this.Object = Object;
		this.Position = Position;
	}

	public void Move(Vector3 dir){
		Object.transform.Translate(dir);
		Position = Object.transform.localPosition;
	}
}
