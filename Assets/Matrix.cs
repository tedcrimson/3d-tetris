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
					int l = length - 1;
					if(i == 0 || j == 0 || k == 0 || i == l || j == l || k == l){
						GameObject g = Instantiate(prefab);
						g.transform.parent = this.transform;
						g.transform.localPosition = new Vector3(i,j,k);
						matrix[i][j].Add(g);
					}
				}
			}
		}

		this.transform.Translate((Vector3.back + Vector3.down + Vector3.left)*(length-1)/2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
