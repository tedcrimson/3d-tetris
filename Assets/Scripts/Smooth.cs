using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smooth : MonoBehaviour {
    public Vector3 targetPosition = Vector3.zero;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
	private Vector3 startPos;

	void Awake()
	{
		startPos = transform.position;	
	}

    void Update() {
		if(targetPosition != Vector3.zero){
			// Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));
			transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		}
    }
	public void MoveDir(Vector3 dir){
		smoothTime = 0.3f;
		targetPosition = transform.position + dir;
	}

	public void Reset(){
		smoothTime /=2f;
		targetPosition = startPos;
	}
}
