using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum BlockType{
	Default, Core, Bomb
}

public class Block {
	public GameObject m_GameObject;
	public Vector3 m_Position;
	public BlockType m_Type;

	public Block(GameObject Object){
		this.m_GameObject = Object;
		this.m_Position = Object.transform.localPosition;
	}

	public Block(GameObject Object, BlockType type){
		this.m_GameObject = Object;
		this.m_Position = Object.transform.localPosition;
		this.m_Type = type;
	}

	public Block(GameObject Object, Vector3 Dir){
		this.m_GameObject = Object;
		this.m_Position = Object.transform.localPosition;
	}

	public Block(GameObject Object, Vector3 Position, Vector3 NextPosition){
		this.m_GameObject = Object;
		this.m_Position = Position;
	}

	public void Move(List<Block> blocks, Vector3 dir){
		Transform temp = new GameObject().transform;
		temp.name = m_GameObject.name + "Temp";
		temp.parent = m_GameObject.transform.parent;
		temp.position = m_GameObject.transform.position;

		temp.Translate(dir, Space.World);
		Vector3 newPos = temp.localPosition;
		
		if(blocks.Any(b=>b.m_Position == newPos)){
			Debug.Log("Cant Move"+m_GameObject.name +" to " + newPos);
			return;
		}else{
			m_Position = newPos;
			m_GameObject.transform.localPosition = newPos;
			if(!HasNeighboars(blocks)){
				blocks.Remove(this);
				Object.Destroy(m_GameObject);
				Debug.LogError("Remove Single" + m_GameObject.name);
			}
		}
		Object.Destroy(temp.gameObject);
	}

	public bool HasNeighboars(List<Block> blocks){
		Vector3[] dirs = new Vector3[]{
			Vector3.up, Vector3.down, 
			Vector3.left, Vector3.right, 
			Vector3.forward, Vector3.back
		};
		foreach(var dir in dirs){
			var n = m_GameObject.transform.localPosition + dir;
			if(blocks.Any(x=> x.m_GameObject.transform.localPosition == n))
				return true;
		}
		return false;
	}
}
