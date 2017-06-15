using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject brickPrefab;

	// Use this for initialization
	IEnumerator Start () {
		while(true){
			Instantiate(brickPrefab, new Vector3(9,9,0), Quaternion.identity);
			yield return new WaitForSeconds(10);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
