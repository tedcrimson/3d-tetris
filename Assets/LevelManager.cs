using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance;
	public List<GameObject> brickPrefabs;
	public Material mat;
	public GameObject CoreObject;
	public GameObject currentBrick;

	public bool IsFalling = false;

	public List<Vector3> blocks;
	List<List<Vector3>> levels; 

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		InitBrick();
		int length = 4;
		levels = new List<List<Vector3>>();
		blocks = new List<Vector3>();

		for(int level = 0; level < 2; level++){
			levels.Add(new List<Vector3>());
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					for (int k = 0; k < length; k++)
					{
						int l = length - 1;
						if((i > 0 || j > 0 || k > 0) && (i < l || j < l || k < l))continue;
						Debug.Log("ADD");
						levels[level].Add(new Vector3(i, j, k));
					}
				}
			}
		}
		Debug.Log(levels[0].Count);

	}

	
	void Update () {
		bool z = levels[0].All(x=> blocks.Contains(x));
		if(z){
			Debug.LogError("sheivso");
		}


		// if(Input.GetKeyDown(KeyCode.Space)){
		// 	StartCoroutine(currentBrick.Submit()); GetComponent<Rigidbody>().AddForce(Vector3.left*150);
		// 	return;
		// }
	}


	public void InitBrick(){
		GameObject brickPrefab = brickPrefabs[Random.Range(0, brickPrefabs.Count)];
		currentBrick = Instantiate(brickPrefab, new Vector3(15,9,0), Quaternion.identity);
		currentBrick.name = "opala" + Random.Range(0, 100);
		Material m = new Material(mat);
		m.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
		MeshRenderer[] meshes = currentBrick.GetComponentsInChildren<MeshRenderer>();
		foreach(var me in meshes)
			me.sharedMaterial = m;
		// currentBrick = g.GetComponent<Spin>();
		IsFalling = true;
		StartCoroutine(BrickFall());
	}

	IEnumerator BrickFall(){
		while(IsFalling){
			currentBrick.transform.Translate(Vector3.left/5, Space.World);
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(2f);
		InitBrick();
	}
}
