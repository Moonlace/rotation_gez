using UnityEngine;
using System.Collections;

public class ColorZoneController : MonoBehaviour {

	private EnemyMoveAround emaScript;
	private GameObject enemyPreFab;
	private SpriteRenderer mColorZone;
	private GameObject specialColorZone;
	private SpecialZone spcScript;

	// Use this for initialization
	void Start () {
		mColorZone = (SpriteRenderer)this.gameObject.renderer;
		specialColorZone = GameObject.FindGameObjectWithTag("SpecialZone");
		spcScript = specialColorZone.GetComponent<SpecialZone>();
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

				Debug.Log("zone colo");
				Debug.Log(obj.renderer.material.color);
				Debug.Log("object zone");
				Debug.Log(mColorZone.color);

				if (obj.renderer.material.color != mColorZone.color) {
					continue;
				}

				if (!this.isObjectInArea(obj)) {
					continue;
				}

				emaScript.killedAnEnemy(obj);
				if (this.gameObject.name == "LeftButton") {
					mColorZone.color = Color.white;
					spcScript.numberOfMixes = 0;
				} else {
					SpriteRenderer spcRender = (SpriteRenderer)specialColorZone.renderer;
					if (spcScript.numberOfMixes == 0) {
						spcRender.color = obj.renderer.material.color;
						spcScript.numberOfMixes ++;
					}
					else if (spcScript.numberOfMixes == 1) {


						spcRender.color = new Color(spcRender.color.r + obj.renderer.material.color.r,
						                            spcRender.color.g + obj.renderer.material.color.g,
						                            spcRender.color.b + obj.renderer.material.color.b,
						                            1.0f);
						spcScript.numberOfMixes = 0;
					}
				}

				break;
			}
		}
	}

	bool isObjectInArea(GameObject obj) {
		EnemieController ec = obj.GetComponent<EnemieController>();
		return ec.isColliding;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.renderer.material.color == mColorZone.color) {
			other.gameObject.SendMessage("enteredArea", null);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.renderer.material.color == mColorZone.color) {
			other.gameObject.SendMessage("leftArea", null);
		}
	}
}