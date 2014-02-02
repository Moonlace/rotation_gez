using UnityEngine;
using System.Collections;

public class EnemyMoveAround : MonoBehaviour {

	public GameObject projectile;
	public float moveSpeed;
	public float originDistance;
	public int numberOfEnemies;


	private int currentEnemies;
	private Hashtable ht;
	private GameObject nextEnemy;

	void Awake(){

	}

	// we have 0 enemies displaying
	// set the enemy position
	// spawn a enemy
	void Start () {
		currentEnemies = 0;
		this.setEnemyPath ();
		this.spawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	// based on the origin position creates a new enemy path
	void setEnemyPath () {
		transform.position = new Vector3 (originDistance, originDistance, 0);
		Vector3 topRight = new Vector3 (originDistance, originDistance, 0);
		Vector3 topLeft = new Vector3 (-originDistance, originDistance, 0);
		Vector3 botLeft = new Vector3 (-originDistance, -originDistance, 0);
		Vector3 botRight = new Vector3 (originDistance, -originDistance, 0);
		Vector3[] path = {topRight, topLeft, botLeft, botRight, topRight};

		ht = new Hashtable();
		ht.Add("path",path);
		ht.Add("time",moveSpeed);
		ht.Add("delay",1);
		ht.Add("looptype",iTween.LoopType.none);
		ht.Add("onstart", "CanSendNextEnemy");
		ht.Add ("onstarttarget", this.gameObject);
	}
		
	// spawns a new anemie
	void spawnEnemy () {
		nextEnemy = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
		nextEnemy.tag = "Enemy";
		nextEnemy.transform.position = new Vector3(originDistance, originDistance, 0);
		iTween.MoveTo (nextEnemy, ht);
	}

	// called after an enemy completes its path
	// if the enemy limit was not reached a new enemy is spawed
	// otherwise if the originDistace is not to near from the player seends another wave of enemies
	// when a new wave is created 

	void CanSendNextEnemy () {
		if (numberOfEnemies == currentEnemies && originDistance > 2) {
			originDistance -= 1.0f;
			GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject obj in allEnemies) {
				DestroyObject(obj);
			}
			this.Start();
		} else if (currentEnemies < numberOfEnemies) {
			currentEnemies++;
			this.spawnEnemy ();
		}
	}

}
