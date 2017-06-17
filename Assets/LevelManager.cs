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
		levels = new List<List<Vector3>>();
		blocks = new List<Block>();

		for(int level = 0; level < 5; level++){
			levels.Add(new List<Vector3>());
			int length = 2*level + 3;
			GameObject pivot = new GameObject();
			pivot.name ="Pivot";
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
								blocks.Add(new Block(g, BlockType.Core));
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
		InitBrick();

	}
	public bool[] Remove(){
		bool[] removeLevels = new bool[5];
		for(int l = 1; l < 5; l++){
			int z = 0;
			foreach(var x in levels[l]){
				foreach(var y in blocks){
					if(x == y.m_Position){
						z++;
						break;
					}
					Vector3 co = Spin.RoundVector((CoreObject.transform.position - y.m_GameObject.transform.position).normalized, 1);
					Debug.DrawRay(y.m_GameObject.transform.position, co);
				}
			}

			float count = Mathf.Pow(2*l + 3, 3f) - Mathf.Pow(2*(l-1) + 3, 3f);
			int f = (int)count/4;
			Debug.Log("Level " + l + " " + z + "/" + f);
			// int z = levels[1].Count(x=> blocks.Contains(x) == true);
			if(z > f){
				removeLevels[l] = true;
				foreach(var x in levels[l]){
					for (int i = blocks.Count - 1; i >= 0; i--)
					{
						if(x == blocks[i].m_Position && blocks[i].m_Type == BlockType.Default){
							Destroy(blocks[i].m_GameObject);
							blocks.RemoveAt(i);
						}
					}
				}
			}
		}
		return removeLevels;
	}
	
	void Update () {
		
		
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

	public void Blow(){
		Debug.ClearDeveloperConsole();

		bool[] removeArray = Remove();
		if(removeArray.All(x=>!x))return;
		int m = 0;
		for(int l=0; l < 5; l++){
			if(l+1 == 5)continue;
			if(removeArray[l])m++;
			foreach(var x in levels[l+1]){
				for (int i = blocks.Count - 1; i >= 0; i--)
				{
					if(x == blocks[i].m_Position){
						Vector3 co = m * Spin.RoundVector((CoreObject.transform.position - blocks[i].m_GameObject.transform.position).normalized, 1);
						Debug.DrawRay(blocks[i].m_GameObject.transform.position, co, Color.red);
						
						blocks[i].Move(blocks, co);
						break;
					}
				}
			}
		}
	}

	IEnumerator BrickFall(){
		Debug.LogError("Brickfall");		
		while(IsFalling){
			currentBrick.transform.Translate(Vector3.left/5, Space.World);
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(.5f);
		InitBrick();
	}
}
