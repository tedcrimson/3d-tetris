using System.Collections.Generic;
using UnityEngine;

public class JigoloJigolo : MonoBehaviour {

	public List<Transform> objects;

	void Start()
	{
		Time.timeScale = 1.0f;
	}

	// Update is called once per frame
	void Update () {
		float s = Mathf.PingPong(Time.time*0.2f, 0.2f) + 1f;
		foreach(var o in objects)
			o.localScale = s * Vector3.one;
	}
}
