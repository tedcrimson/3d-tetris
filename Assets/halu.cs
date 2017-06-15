using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halu : MonoBehaviour {

	Material shade;
	// Use this for initialization
	void Start () {
		shade = GetComponent<MeshRenderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update() {
        float offset = Time.time * 1;
        shade.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
