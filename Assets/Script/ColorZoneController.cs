using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

	public Color sectionColor;

	private EnemyMoveAround emaScript;
	private GameObject enemyPreFab;

	// Use this for initialization
	void Start () {
		enemyPreFab = GameObject.FindGameObjectWithTag("Enemie");
		emaScript = enemyPreFab.GetComponent<EnemyMoveAround>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown() {
		if (Input.GetMouseButtonDown (0)) {
			GameObject[] taggedGameObjects = GameObject.FindGameObjectsWithTag("Enemy"); 
			
			//find first object in the clicked area that are of the same color
			foreach (GameObject obj in taggedGameObjects) {
				
				if (obj.renderer.material.color != sectionColor) {
					continue;
				}

				Debug.Log(isObjectInArea(obj));
				if (!this.isObjectInArea(obj)) {
					continue;
				}
				Debug.Log("Passed");

				Destroy(obj);
				emaScript.killedAnEnemy();
				break;
			}
		}
	}

	bool isObjectInArea(GameObject obj) {
		EnemieController ec = obj.GetComponent<EnemieController>();
		return ec.isColliding;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.renderer.material.color == sectionColor) {
			other.gameObject.SendMessage("enteredArea", null);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.renderer.material.color == sectionColor) {
			other.gameObject.SendMessage("leftArea", null);
		}
	}
}