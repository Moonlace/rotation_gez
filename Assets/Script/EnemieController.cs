using UnityEngine;
using System.Collections;

public class EnemieController : MonoBehaviour {

	public bool isColliding;
	public int enemieTag;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void enteredArea () {
		isColliding = true;
	}

	void leftArea () {
		isColliding = false;
	}
}
