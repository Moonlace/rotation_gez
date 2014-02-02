using UnityEngine;
using System.Collections;

public class EnemyMoveAround : MonoBehaviour {

	public GameObject projectile;
	public float moveSpeed;
	public float originDistance;
	public int numberOfEnemies;

	private Hashtable ht = new Hashtable();
	private GameObject nextEnemie;

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		Vector3 topRight = new Vector3 (originDistance, originDistance, 0);
		Vector3 topLeft = new Vector3 (-originDistance, originDistance, 0);
		Vector3 botLeft = new Vector3 (-originDistance, -originDistance, 0);
		Vector3 botRight = new Vector3 (originDistance, -originDistance, 0);
		Vector3[] path = {topRight, topLeft, botLeft, botRight, topRight};
		
		ht.Add("path",path);
		ht.Add("time",moveSpeed);
		ht.Add("delay",1);
		ht.Add("looptype",iTween.LoopType.none);
		ht.Add("onstart", "CanSendNewEnemie");
		ht.Add ("onstarttarget", this.gameObject);
		this.spawnEnemie ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void spawnEnemie () {
		Debug.Log("spawning");
		nextEnemie = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
		nextEnemie.transform.position = new Vector3(originDistance, originDistance, 0);
		iTween.MoveTo (nextEnemie, ht);
	}

	void CanSendNewEnemie () {
		if (numberOfEnemies == 0) {
			return;
		}
		Debug.Log("can send new enemies");
		numberOfEnemies--;
		this.spawnEnemie ();

	}
}
