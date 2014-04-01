using UnityEngine;
using System.Collections;

public class EnemyMoveAround : MonoBehaviour {

	public GameObject projectile;
	public float moveSpeed;
	public float originDistance;
	public int numberOfEnemies;
	public int numberOfSpawnedEnemies;
	public int spawnedEnemiesCount;
	public int compledEnemiesCount;
	public int killedEnemies;
	public GameObject nextHUD1;
	public GameObject nextHUD2;
	public GameObject nextHUD3;
	public GameObject nextHUD4;
	public GameObject nextHUD5;
	public GameObject nextHUD6;
	public GameObject nextHUD7;
	public GameObject nextHUD8;
	public GameObject nextHUD9;
	public GameObject nextHUD10;
	public GameObject nextHUD11;
	public GameObject nextHUD12;
	public GameObject nextHUD13;
	public GameObject nextHUD14;
	public GameObject nextHUD15;
	public GameObject nextHUD16;
	public GameObject nextHUD17;
	public GameObject nextHUD18;
	public GameObject nextHUD19;
	public GameObject nextHUD20;

	private GameObject [] nextEnemies;
	private GameObject[] createdEnemies;
	private Hashtable p1;
	private Hashtable p2;
	private Hashtable p3;
	private Hashtable p4;
	private GameObject nextEnemy; 
	public bool blackNextEnemieHUD;

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
		numberOfSpawnedEnemies = 0;
		spawnedEnemiesCount = 0;
		compledEnemiesCount = 0;

		// get the next enemie HUD objects
		blackNextEnemieHUD = false;
		nextEnemies = new GameObject [20]{	nextHUD1,
		                             		nextHUD2,
		                             		nextHUD3,
		                             		nextHUD4,
		                             		nextHUD5,
		                             		nextHUD6,
		                             		nextHUD7,
		                            		nextHUD8,
		                             		nextHUD9,
		                            	 	nextHUD10,
		                           	  		nextHUD11,
		                             		nextHUD12,
		                             		nextHUD13,
		                             		nextHUD14,
		                             		nextHUD15,
		                             		nextHUD16,
		                             		nextHUD17,
		                             		nextHUD18,
		                             		nextHUD19,
											nextHUD20
										};

		// create the path for the enemies
		this.setEnemyPath ();

		// create enemies
		createdEnemies = new GameObject[numberOfEnemies];
		for (int i = 0; i < numberOfEnemies; i ++) {
			createdEnemies[i] = this.createEnemyWithTag(i);
		}

		// start the spawning
		this.spawnEnemy ();
	}
	
	// Update is called once per frame
	void Update () {

			int j = numberOfSpawnedEnemies-1;
			Debug.Log (numberOfSpawnedEnemies);
			for (int i = 0; i < nextEnemies.Length; i++) {
				while (j != createdEnemies.Length 
			    	   && createdEnemies[j] == null) {
					j++;
				}
				if (j != createdEnemies.Length 
			    && createdEnemies[j] != null && !blackNextEnemieHUD) { 
					((SpriteRenderer)nextEnemies[i].renderer).color = createdEnemies[j].renderer.material.color;
					j++;
				}
				else {
					((SpriteRenderer)nextEnemies[i].renderer).color = Color.black;
				}
			}

	}

	void NextWave () {

		// set counters to zero
		killedEnemies = 0;
		numberOfSpawnedEnemies = 0;
		spawnedEnemiesCount = 0;
		compledEnemiesCount = 0;
		blackNextEnemieHUD = false;
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
		while (spawnedEnemiesCount != createdEnemies.Length 
		       && createdEnemies[spawnedEnemiesCount] == null) {
			spawnedEnemiesCount++;
		}
		if (spawnedEnemiesCount != createdEnemies.Length) { 
			obj = createdEnemies [spawnedEnemiesCount];
			spawnedEnemiesCount++;
			numberOfSpawnedEnemies++;
			iTween.MoveTo (obj, p1);
			iTween.MoveTo (obj, p2);
			iTween.MoveTo (obj, p3);
			iTween.MoveTo (obj, p4);
		}
	}
	
	// called after an enemy leaves the first square
	void CanSendNextEnemy () {

		// checks if we reach the limite of enemies
		if (numberOfSpawnedEnemies < numberOfEnemies) {
			//spawns an enemy
			this.spawnEnemy ();
		} else {
			// in order to for the next enemies hud to go go full black
			blackNextEnemieHUD = true;
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
		if (compledEnemiesCount >= numberOfSpawnedEnemies && numberOfEnemies == numberOfSpawnedEnemies) {

			//reduces the number of enemies by the amount we killed
			numberOfEnemies -= killedEnemies;
			compledEnemiesCount = 0;
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
		// 6 special colors :)
		int pickedColor = Random.Range(0, 10); // creates a number between 0 and 9
		Color specialColor = this.getSpecialColor();
		switch (pickedColor)
		{
		case 0:
		case 1:
		case 2:
			return new Color(172.0f/255.0f,24.0f/255.0f,24.0f/255.0f,1.0f); // return a red color
		case 3:
		case 4:
		case 5:
			return new Color(55.0f/255.0f,120.0f/255.0f,13.0f/255.0f,1.0f); // return a green color
		case 6:
		case 7:
		case 8:
			return new Color(40.0f/255.0f,80.0f/255.0f,156.0f/255.0f,1.0f); // return a blue color
		case 9:
			return specialColor;
		default:
			return new Color(172.0f/255.0f,24.0f/255.0f,24.0f/255.0f,1.0f); // return a red color
		}
	}
			
	Color getSpecialColor() {
		int specialColor = Random.Range(0, 6); // creates a number between 0 and 5
		switch (specialColor) 
		{
		case 0: 
			return new Color(227.0f/255.0f,144.0f/255.0f,37.0f/255.0f,1.0f); // return a red + green
		case 1: 
			return new Color(212.0f/255.0f,104.0f/255.0f,180.0f/255.0f,1.0f); // return red + blue
		case 2: 
			return new Color(95.0f/255.0f,200.0f/255.0f,169.0f/255.0f,1.0f); // return green + blue
		case 3: 
			return new Color(344.0f/255.0f,48.0f/255.0f,48.0f/255.0f,1.0f); // return red + red
		case 4: 
			return new Color(110.0f/255.0f,240.0f/255.0f,26.0f/255.0f,1.0f); // return green + green
		case 5: 
			return new Color(80.0f/255.0f,160.0f/255.0f,312.0f/255.0f,1.0f); // return blue + blue
		default:
			return new Color(80.0f/255.0f,160.0f/255.0f,312.0f/255.0f,1.0f); // return blue + blue
		}
	}
}
