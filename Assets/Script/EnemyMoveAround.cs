using UnityEngine;
using System.Collections;

public class EnemyMoveAround : MonoBehaviour {

	public GameObject projectile;
	public float moveSpeed;
	public float originDistance;
	public int numberOfEnemies;
	public int spawnedEnemiesCount;
	public int compledEnemiesCount;
	public int killedEnemies;

	private Hashtable p1;
	private Hashtable p2;
	private Hashtable p3;
	private Hashtable p4;
	private GameObject nextEnemy;
	public void killedAnEnemy() {
		killedEnemies ++;
		compledEnemiesCount ++;
		this.shoudStartNextWave();
	}

	void Awake(){

	}

	// we have 0 enemies displaying
	// set the enemy position
	// spawn a enemy
	void Start () {
		killedEnemies = 0;
		spawnedEnemiesCount = 0;
		compledEnemiesCount = 0;
		this.setEnemyPath ();
		this.spawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	// based on the origin position creates a new enemy path
	void setEnemyPath () {
		Vector3 topRight = new Vector3 (originDistance, originDistance, 0);
		Vector3 topLeft = new Vector3 (-originDistance, originDistance, 0);
		Vector3 botLeft = new Vector3 (-originDistance, -originDistance, 0);
		Vector3 botRight = new Vector3 (originDistance, -originDistance, 0);
		p1 = createPath (p1, topLeft, 1);
		p2 = createPath (p2, botLeft, 2);
		p3 = createPath (p3, botRight, 3);
		p4 = createPath (p4, topRight, 4);
		p1.Add("onstart", "CanSendNextEnemy");
		p1.Add ("onstarttarget", this.gameObject);
		p4.Add("oncomplete", "EnemyCompletedPath");
		p4.Add ("oncompletetarget", this.gameObject);
	}

	// nextPosition creation config helper
	Hashtable createPath (Hashtable nextPositionOptions, Vector3 nextPosition, double delay) {
		nextPositionOptions = new Hashtable();
		nextPositionOptions.Add("position",nextPosition);
		nextPositionOptions.Add("time",moveSpeed);
		nextPositionOptions.Add("delay",delay);
		nextPositionOptions.Add("looptype",iTween.LoopType.none);
		return nextPositionOptions;
	}
		
	// spawns a new anemie
	void spawnEnemy () {
		nextEnemy = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
		nextEnemy.tag = "Enemy";
		nextEnemy.renderer.material.color = this.getRandomColor();
		nextEnemy.transform.position = new Vector3(originDistance, originDistance, 0);
		iTween.MoveTo (nextEnemy, p1);
		iTween.MoveTo (nextEnemy, p2);
		iTween.MoveTo (nextEnemy, p3);
		iTween.MoveTo (nextEnemy, p4); 
	}

	// called after an enemy completes its path
	// if the enemy limit was not reached a new enemy is spawed
	// otherwise if the originDistace is not to near from the player seends another wave of enemies
	// when a new wave is created 
	void CanSendNextEnemy () {
		spawnedEnemiesCount++;
		if (spawnedEnemiesCount < numberOfEnemies) {
			this.spawnEnemy ();
		}
	}

	void EnemyCompletedPath () {
		//destroy enemy that reached the finishe line :D
		compledEnemiesCount ++;
		this.shoudStartNextWave();
	} 

	void shoudStartNextWave () {
		if (compledEnemiesCount >= spawnedEnemiesCount && numberOfEnemies == spawnedEnemiesCount) {

			numberOfEnemies -= killedEnemies;


			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
				DestroyObject (obj);
			}

			//next cycle of enemies
			originDistance--;
			if (originDistance >= 2) { 
				this.Start ();
			}
		}
	}

	Color getRandomColor() {
		int pickedColor = Random.Range(0, 4); // creates a number between 1 and 4
		switch (pickedColor)
		{
		case 1:
			return new Color(40.0f/255.0f,84.0f/255.0f,153.0f/255.0f,1.0f); // return a blue color
		case 2:
			return new Color(73.0f/255.0f,153.0f/255.0f,56.0f/255.0f,1.0f); // return a green color
		case 3:
			return new Color(84.0f/255.0f,21.0f/255.0f,116.0f/255.0f,1.0f); // return a purple color
		default:
			return new Color(245.0f/255.0f,253.0f/255.0f,57.0f/255.0f,1.0f); // return a yellow color
		}
	}
}
