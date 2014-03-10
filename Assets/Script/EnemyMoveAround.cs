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

	private GameObject[] createdEnemies;
	private Hashtable p1;
	private Hashtable p2;
	private Hashtable p3;
	private Hashtable p4;
	private GameObject nextEnemy; 

	public void killedAnEnemy(GameObject obj) {
		EnemieController ec = obj.GetComponent<EnemieController>();
		Destroy(obj);
		createdEnemies [ec.enemieTag] = null;
		killedEnemies ++;
		compledEnemiesCount ++;
		this.shoudStartNextWave();
	}

	void Awake(){

	}

	//start game
	void Start () {

		// set counters to zero
		killedEnemies = 0;
		spawnedEnemiesCount = 0;
		compledEnemiesCount = 0;

		// create the path for the enemies
		this.setEnemyPath ();

		// create enemies
		createdEnemies = new GameObject[numberOfEnemies];
		for (int i = 0; i < numberOfEnemies; i ++) {
			createdEnemies[i] = this.createEnemyWithTag(i);
			Debug.Log (createdEnemies[i].renderer.material.color);
		}

		// start the spawning
		this.spawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void NextWave () {

		Debug.Log("starting next wave!");

		// set counters to zero
		killedEnemies = 0;
		spawnedEnemiesCount = 0;
		compledEnemiesCount = 0;

		// start the spawning
		this.spawnEnemy ();
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
		
	// create a new enemy
	GameObject createEnemyWithTag (int tag) {
		nextEnemy = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
		nextEnemy.tag = "Enemy";
		nextEnemy.renderer.material.color = this.getRandomColor();
		nextEnemy.transform.position = new Vector3(originDistance, originDistance, 0);
		EnemieController ec = nextEnemy.GetComponent<EnemieController>();
		ec.enemieTag = tag;
		return nextEnemy;
	}
	
	// spawn a enemy
	void spawnEnemy() {
		GameObject obj;
		while (createdEnemies[spawnedEnemiesCount] == null 
		       && spawnedEnemiesCount != createdEnemies.Length) {
			spawnedEnemiesCount++;
		}
		Debug.Log("going to spawn");
		obj = createdEnemies [spawnedEnemiesCount];
		spawnedEnemiesCount++;
		iTween.MoveTo (obj, p1);
		iTween.MoveTo (obj, p2);
		iTween.MoveTo (obj, p3);
		iTween.MoveTo (obj, p4);
	}
	
	// called after an enemy leaves the first square
	void CanSendNextEnemy () {

		// checks if we reach the limite of enemies
		if (spawnedEnemiesCount < numberOfEnemies) {
			//spawns an enemy
			this.spawnEnemy ();
		}
	}

	// called after an enemy reaches the end of its path
	void EnemyCompletedPath () {

		//incremente the number of enemies to reach the end of the path
		compledEnemiesCount ++;

		// check if we can start a next wave
		this.shoudStartNextWave();
	} 


	void shoudStartNextWave () {

		// checks if the wave has ended
		if (compledEnemiesCount >= spawnedEnemiesCount && numberOfEnemies == spawnedEnemiesCount) {

			//reduces the number of enemies by the amount we killed
			numberOfEnemies -= killedEnemies;

//			// destroys all the enemies
//			foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
//				DestroyObject (obj);
//			}

			// makes the enemies closer to the origin
			originDistance--;

			// updates the enemy path with the new originDistance value
			this.setEnemyPath();

			// checks if the game should continue
			if (originDistance >= 2) { 
				// starts a new wave
				this.NextWave ();
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
