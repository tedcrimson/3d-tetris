using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.SerializableAttribute]
public struct Brick{
	public GameObject prefab;
	public Sprite sprite;
	[HideInInspector]
	public Material material;
}

public class LevelManager : MonoBehaviour {

	public List<Brick> bricks;
	public Material BlockMaterial;
	public GameObject CorePrefab;
	public GameObject CoreObject;
	public GameObject MatrixObject;
	public GameObject Destroyer;
	public ParticleSystem Blast;

	[HeaderAttribute("Cameras")]
	public Camera MainCamera;
	public Camera SideCamera;

	[HeaderAttribute("UI")]
	public EventManager eventManager;
	public UnityEngine.UI.Image NextBrickImage;
	public UnityEngine.UI.Text GameSceneScoreText;
	public GameObject GameOverPanel;
	public UnityEngine.UI.Text GameOverPanelScoreText;
	public UnityEngine.UI.Text GameOverPanelHighScoreText;
	
	[SpaceAttribute]
	public int MaxLevel = 5;
	public float FallSpeed = 5f;

	internal float normalSpeed;
	internal float boostedSpeed = 2f;
	internal int currentLevel = 0;
	internal bool IsFalling = false;
	internal List<Block> blocks;

	private int score = 0;
	private int highScore;

	private List<List<Vector3>> levels; 
	private GameObject currentBrick;
	private Brick nextBrick;
	private Vector3 cameraStartPosition;

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

		normalSpeed = FallSpeed;

		cameraStartPosition = MainCamera.transform.position;

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
								GameObject g = Instantiate(CorePrefab);
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

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space)){
			FallSpeed = boostedSpeed;
		}
		// MainCamera.transform.LookAt(CoreObject.transform);
	}


	public bool[] Remove(){
		bool[] removeLevels = new bool[MaxLevel];
		List<Block> nonCore = blocks.Where(x=>x.m_Type == BlockType.Default).ToList();

		
		for(int l = 1; l < MaxLevel; l++){
			int z = nonCore.Count(x=>levels[l].Contains(x.m_Position));

			float count = Mathf.Pow(2*l + 3, 3f) - Mathf.Pow(2*(l-1) + 3, 3f);
			int f = (int)count/2;
			// Debug.Log("Level " + l + " " + z + "/" + f);
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


	public void SetLevel(Vector3 pos){
		int level = (int)pos.magnitude;
		// Debug.Log(level);
		if(level > currentLevel){
			currentLevel = level;
			SetLevelScene(level);
		}
		bool inBounds = CheckBounds(pos);
		if(!inBounds){
			GameOver();
			// UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
		}
	}

	public void GameOver(){
		GameOverPanel.SetActive(true);
		StopAllCoroutines();

		if(score > PlayerPrefsManager.HighScore){
			highScore = score;
			PlayerPrefsManager.HighScore = highScore;
		}

		GameOverPanelScoreText.text = "Score " + score;
		GameOverPanelHighScoreText.text = "HighScore " + PlayerPrefsManager.HighScore;
	}

	private void SetLevelScene(int level){
		SideCamera.orthographicSize = 1 + level * 1.2f;

		Vector3 z = Destroyer.transform.position;
		z.x = -level - 3;
		Destroyer.transform.position = z;
	}

	public bool CheckBounds(Vector3 pos){
		int level = (int)pos.magnitude;
		if(level > MaxLevel){
			// UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
			return false;
		}
		return true;
	}

	public void MoveCamera(Vector3 dir){
		Smooth sm = MainCamera.GetComponent<Smooth>();
		if(dir == Vector3.zero){
			sm.Reset();
		}else
			sm.MoveDir(dir);
	}


	public void InitBrick(){
		MainCamera.transform.position = cameraStartPosition;

		GameObject newBrickPrefab;
		Material newBrickColor = new Material(BlockMaterial);
		if(nextBrick.prefab == null){
			Debug.Log("NewBrick");
			newBrickPrefab = bricks[Random.Range(0, bricks.Count)].prefab;
			newBrickColor.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(1/3f, 1f), 0.8f));
		}else{
			newBrickPrefab = nextBrick.prefab;
			newBrickColor = nextBrick.material;
		}
		nextBrick = bricks[Random.Range(0, bricks.Count)];
		nextBrick.material = new Material(BlockMaterial);
		nextBrick.material.SetColor("_Color", Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(1/3f, 1f), 0.8f));
		NextBrickImage.sprite = nextBrick.sprite;
		NextBrickImage.material = nextBrick.material;

		currentBrick = Instantiate(newBrickPrefab, new Vector3(15,9,0), Quaternion.identity);
		currentBrick.name = "opala" + Random.Range(0, 100);
		MeshRenderer[] meshes = currentBrick.GetComponentsInChildren<MeshRenderer>();
		foreach(var me in meshes)
			me.sharedMaterial = newBrickColor;
		// currentBrick = g.GetComponent<Spin>();
		IsFalling = true;
		FallSpeed = normalSpeed;
		StartCoroutine(BrickFall());
	}

	public void Blow(){
		// Debug.ClearDeveloperConsole();
		AddScore(10);//block stick
		bool[] removeArray = Remove();
		if(removeArray.All(x=>!x))return;
		int m = 0;
		for(int l=0; l < MaxLevel; l++){
			if(l+1 == MaxLevel)continue;
			if(removeArray[l])
				m++;
			foreach(var x in levels[l+1]){
				for (int i = blocks.Count - 1; i >= 0; i--)
				{
					if(x == blocks[i].m_Position){
						Vector3 co = m * Spinner.RoundVector((CoreObject.transform.position - blocks[i].m_GameObject.transform.position).normalized, 1);
						// Debug.DrawRay(blocks[i].m_GameObject.transform.position, co, Color.red);
						
						blocks[i].Move(blocks, co);
						break;
					}
				}
			}
		}
		Blast.Play();
		currentLevel -= m;
		AddScore(m*100); // level blow
		SetLevelScene(currentLevel);
	}

	public void AddScore(int points){
		normalSpeed /= 1.0001f;
		Debug.Log("AddScore" + points);
		score += points;
		GameSceneScoreText.text = score +"";
	}


	IEnumerator BrickFall(){
		while(IsFalling){
			currentBrick.transform.Translate(Vector3.left/FallSpeed, Space.World);
			yield return new WaitForSeconds(.005f);
		}
		yield return new WaitForSeconds(.5f);
		InitBrick();
	}
}
