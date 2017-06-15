using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix : MonoBehaviour {

	public GameObject prefab;

	public List<List<List<GameObject>>> matrix;
	// Use this for initialization
	void Start () {
		int length = 3;
		matrix = new List<List<List<GameObject>>>();
		for (int i = 0; i < length; i++)
		{
			matrix.Add(new List<List<GameObject>>());
			for (int j = 0; j < length; j++)
			{
				matrix[i].Add(new List<GameObject>());
				for (int k = 0; k < length; k++)
				{
					GameObject g = Instantiate(prefab);
					g.transform.parent = this.transform;
					g.transform.localPosition = new Vector3(i,j,k);
					matrix[i][j].Add(g);
				}
			}
		}


		for (int i = 1; i < length - 1; i++)
		{
			for (int j = 1; j < length - 1; j++)
			{
				for (int k = 1; k < length - 1; k++)
				{
					// if( i == 0 || j == 0 || k == 0 || i == max || j == max || k == max) continue;
					Destroy(matrix[i][j][k].gameObject);
					// Debug.Log("Deleted " + i + " " + j + " " +k );
				}
			}
		}
		this.transform.Translate((Vector3.back + Vector3.down + Vector3.left));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
