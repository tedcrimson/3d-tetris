using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance;
	public List<GameObject> brickPrefabs;
	public GameObject corePrefab;
	public Material mat;
	public GameObject CoreObject;
	public GameObject MatrixObject;
	public GameObject currentBrick;

	public bool IsFalling = false;

	public List<Block> blocks;
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
		levels = new List<List<Vector3>>();
		blocks = new List<Block>();

		for(int level = 0; level < 3; level++){
			levels.Add(new List<Vector3>());
			int length = 2*level + 3;
			GameObject pivot = new GameObject();
			pivot.transform.parent = CoreObject.transform;
			pivot.transform.localPosition = Vector3.zero;
			pivot.transform.Translate((Vector3.back + Vector3.down + Vector3.left)*(level+1));
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					for (int k = 0; k < length; k++)
					{

						int l = length - 1;
						if(i == 0 || j == 0 || k == 0 || i == l || j == l || k == l){
							if(level == 0){
								GameObject g = Instantiate(corePrefab);
								g.transform.parent = MatrixObject.transform;
								g.transform.localPosition = new Vector3(i,j,k);
							}
							Vector3 v = new Vector3(i, j, k)+pivot.transform.localPosition;
							levels[level].Add(v);
						}
					}
				}
			}

			Destroy(pivot);
		}


		MatrixObject.transform.Translate((Vector3.back + Vector3.down + Vector3.left)*(3-1)/2f);
		Debug.Log(levels[1].Count);

	}

	
	void Update () {
		int z = 0;
		foreach(var x in levels[1]){
			foreach(var y in blocks){
				if(x == y.Position){
					z++;
					break;
				}
				Vector3 co = Spin.RoundVector((CoreObject.transform.position - y.Object.transform.position).normalized, 1);
				Debug.DrawRay(y.Object.transform.position, co);
			}
		}

		

		// int z = levels[1].Count(x=> blocks.Contains(x) == true);
		if(z > 3){
			foreach(var x in levels[1]){
				for (int i = blocks.Count - 1; i >= 0; i--)
				{
					if(x == blocks[i].Position){
						Destroy(blocks[i].Object);
						blocks.RemoveAt(i);
					}
				}
			}

			foreach(var x in levels[2]){
				for (int i = blocks.Count - 1; i >= 0; i--)
				{
					if(x == blocks[i].Position){
						Vector3 co = Spin.RoundVector((CoreObject.transform.position - blocks[i].Object.transform.position).normalized, 1);
						Debug.DrawRay(blocks[i].Object.transform.position, co, Color.red);
						blocks[i].Move(co);
						Debug.LogError("WAT");
						break;
					}
				}
			}
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
