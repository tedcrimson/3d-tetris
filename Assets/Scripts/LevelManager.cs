using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	public List<GameObject> brickPrefabs;
	public GameObject corePrefab;
	public Material mat;
	public GameObject CoreObject;
	public GameObject MatrixObject;
	public GameObject Destroyer;
	public Camera SideCamera;
	public int MaxLevel = 5;


	internal int currentLevel = 0;
	internal bool IsFalling = false;
	internal List<Block> blocks;

	private List<List<Vector3>> levels; 
	private GameObject currentBrick;

	private static LevelManager instance;
	public static LevelManager Instance{get{return instance;} set{instance = value;}}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Instance = this;

		levels = new List<List<Vector3>>();
		blocks = new List<Block>();

		MatrixObject.transform.Translate(-Vector3.one);

		for(int level = 0; level < MaxLevel; level++){
			levels.Add(new List<Vector3>());
			int length = 2*level + 3;
			GameObject pivot = new GameObject();
			pivot.name ="Pivot " + level;
			pivot.transform.parent = CoreObject.transform;
			pivot.transform.localPosition = Vector3.zero;
			pivot.transform.Translate(-Vector3.one*(level+1));
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
								g.transform.position = new Vector3(i,j,k)+MatrixObject.transform.localPosition;
								blocks.Add(new Block(g, BlockType.Core));
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


		InitBrick();
	}


	public bool[] Remove(){
		bool[] removeLevels = new bool[MaxLevel];
		List<Block> nonCore = blocks.Where(x=>x.m_Type == BlockType.Default).ToList();

		
		for(int l = 1; l < MaxLevel; l++){
			int z = nonCore.Count(x=>levels[l].Contains(x.m_Position));

			float count = Mathf.Pow(2*l + 3, 3f) - Mathf.Pow(2*(l-1) + 3, 3f);
			int f = (int)count/2;
			Debug.Log("Level " + l + " " + z + "/" + f);
			// int z = levels[1].Count(x=> blocks.Contains(x) == true);
			if(z > f){
				removeLevels[l] = true;
				foreach(var x in levels[l]){
					for (int i = nonCore.Count - 1; i >= 0; i--)
					{
						if(x == nonCore[i].m_Position){
							Destroy(nonCore[i].m_GameObject);
							blocks.Remove(nonCore[i]);
							nonCore.RemoveAt(i);
						}
					}
				}
			}
		}
		return removeLevels;
	}


	public void CheckBounds(Vector3 pos){
		int level = (int)pos.magnitude;
		if(level > currentLevel){
			currentLevel = level;
			SideCamera.orthographicSize = 1 + level * 1.2f;

			Vector3 z = Destroyer.transform.position;
			z.x = -level - 3;
			Destroyer.transform.position = z;
		}
		if(level > MaxLevel){
			UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
		}
	}


	public void InitBrick(){
		GameObject brickPrefab = brickPrefabs[Random.Range(0, brickPrefabs.Count)];
		currentBrick = Instantiate(brickPrefab, new Vector3(15,9,0), Quaternion.identity);
		currentBrick.name = "opala" + Random.Range(0, 100);
		Material m = new Material(mat);
		m.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(1/3f, 1f), 0.8f));
		MeshRenderer[] meshes = currentBrick.GetComponentsInChildren<MeshRenderer>();
		foreach(var me in meshes)
			me.sharedMaterial = m;
		// currentBrick = g.GetComponent<Spin>();
		IsFalling = true;
		StartCoroutine(BrickFall());
	}

	public void Blow(){
		// Debug.ClearDeveloperConsole();

		bool[] removeArray = Remove();
		if(removeArray.All(x=>!x))return;
		int m = 0;
		for(int l=0; l < MaxLevel; l++){
			if(l+1 == MaxLevel)continue;
			if(removeArray[l])m++;
			foreach(var x in levels[l+1]){
				for (int i = blocks.Count - 1; i >= 0; i--)
				{
					if(x == blocks[i].m_Position){
						Vector3 co = m * Spin.RoundVector((CoreObject.transform.position - blocks[i].m_GameObject.transform.position).normalized, 1);
						// Debug.DrawRay(blocks[i].m_GameObject.transform.position, co, Color.red);
						
						blocks[i].Move(blocks, co);
						break;
					}
				}
			}
		}
	}


	IEnumerator BrickFall(){
		while(IsFalling){
			currentBrick.transform.Translate(Vector3.left/5, Space.World);
			yield return new WaitForSeconds(.05f);
		}
		yield return new WaitForSeconds(.5f);
		InitBrick();
	}
}
