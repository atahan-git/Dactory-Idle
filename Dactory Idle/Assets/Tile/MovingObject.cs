using UnityEngine;
using System.Collections;

public class MovingObject : MonoBehaviour {

	public int type = 0;

	public Vector3 placeToBe = Vector3.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = Vector3.Lerp (transform.position, placeToBe, 10f * Time.deltaTime);
		transform.position = Vector3.MoveTowards (transform.position, placeToBe, 2f * Time.deltaTime);
	}

	public void SelfDestruct(Vector3 offset){
		placeToBe = placeToBe + offset;
		Invoke ("DestroySelf", 0.2f);
	}

	void DestroySelf(){
		Destroy (gameObject);
	}
}
