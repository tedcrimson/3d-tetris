using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halu : MonoBehaviour {

	public Material shade;

	public Color[] colors;
         
	public int currentIndex = 0;
	private int nextIndex;
	
	public float changeColourTime = 2.0f;
	
	private float lastChange = 0.0f;
	private float timer = 0.0f;
     
	void Start() {
		if (colors == null || colors.Length < 2)
			Debug.Log ("Need to setup colors array in inspector");
		
		nextIndex = (currentIndex + 1) % colors.Length;
    
	}
     
	void Update() {
		float offset = Time.time * 1;
        shade.SetTextureOffset("_MainTex", new Vector2(offset, 0));
         
		timer += Time.deltaTime;
		
		if (timer > changeColourTime) {
			currentIndex = (currentIndex + 1) % colors.Length;
			nextIndex = (currentIndex + 1) % colors.Length;
			timer = 0.0f;
			
		}
		shade.SetColor("_TintColor", Color.Lerp (colors[currentIndex], colors[nextIndex], timer / changeColourTime ))	;
	}
}
