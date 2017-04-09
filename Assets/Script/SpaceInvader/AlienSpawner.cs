using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlienSpawner : MonoBehaviour {
	//public int row = 5;
	public int col = 5;
	public float insetX = 0.5f;
	public float insetY = 0.5f;
	public GameObject spawner;
    public float DistDivider = 6;
    public bool shootable = false;
    public int rowShootable = 0;
    public int numShootable = 3;
    public bool startMoving = false;
	public static int currentShootable = 0;
	public static Dictionary<int, bool> rsMap = new Dictionary<int, bool> (); // rowshootable map
    public string AlienType;
    public Alien[] aliens;
    private Dictionary<int, int> pickedAlienMap = new Dictionary<int, int>();
    public int deads = 0;
    

    Alien CreateAlien(GameObject spawner, float spawnX, float spawnY, float lbx, float rbx, float distDivider, string alienName, bool startMoving = false){
        GameObject go = Instantiate (spawner,new Vector3(spawnX,spawnY,0.0f),Quaternion.identity);
        go.name = alienName;
        Alien goAlien = go.GetComponent<Alien> ();
        goAlien.Init ();
        goAlien.LEFT_BOUND_X = lbx;
        goAlien.RIGHT_BOUND_X = rbx;//spawnX + goAlien.RIGHT_BOUND_X - (col - j) * (sr.bounds.size.x + insetX);
        goAlien.distDivider = DistDivider;
        goAlien.startMoving = startMoving;
        //Ent2D.CreateCirc(new Vector3(goAlien.RIGHT_BOUND_X,spawnY, transform.position.z),Color.yellow);
        return goAlien;
    }

    public void StartSpawn(){
        rsMap.Add (rowShootable, false);
        aliens = new Alien[col];
		SpriteRenderer sr = spawner.GetComponent<SpriteRenderer> ();
        float RBX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f)).x /*- sr.bounds.extents.x*/;
        float LBX = -RBX;
        float alienOffsetX = sr.bounds.size.x + insetX; // + or - 
        Debug.Log("RBX:" + RBX);
        Debug.Log("LBX:" + LBX);
        // Debug.Log("sz:" + sr.bounds.size);
        // Debug.Log("alienBoundx:" + sr.bounds.extents.x);
        // Debug.Log("alienOffsetX:" + alienOffsetX);
        //float width = RBX*2;
        float rbx = RBX - (col-1)*alienOffsetX;
        float lbx = LBX;
        Debug.Log("rbx:" + rbx);
        Debug.Log("lbx:" + lbx);
		//float lastPos = transform.position.x + col * (sr.bounds.size.x + insetX);
		//float distX = RBX - lastPos;
        // Ent2D.CreateCirc(new Vector3(rbx,transform.position.y, transform.position.z),Color.magenta, "RBX", 1.0f);
        //Ent2D.CreateCirc(new Vector3(lbx,transform.position.y, transform.position.z),Color.blue, "LBX", 1.0f);
        //Ent2D.CreateCirc(new Vector3(lastPos,transform.position.y, transform.position.z),Color.yellow);
        if(col > 0){
            float spawnX = lbx;
            float spawnY = transform.position.y;
            aliens[0] = CreateAlien(spawner, spawnX, spawnY, lbx, rbx, DistDivider, "alien-" + 0, startMoving);
            // Ent2D.CreateCirc(new Vector3(rbx,spawnY, transform.position.z),Color.red, "alien-rbx-" + 0, 1.0f);
            // Ent2D.CreateCirc(new Vector3(lbx,spawnY, transform.position.z),Color.green, "alien-lbx-" + 0, 1.0f);
            for (int j = 1; j < col; j++) {
                lbx += alienOffsetX;
                rbx += alienOffsetX;
                //lbx = aliens[j-1].LEFT_BOUND_X + j*alienOffsetX; 
                //rbx = aliens[j-1].RIGHT_BOUND_X - j*alienOffsetX;
                spawnX += alienOffsetX;
                aliens[j] = CreateAlien(spawner, spawnX, spawnY, lbx, rbx, DistDivider, "alien-" + j, startMoving);
                // Ent2D.CreateCirc(new Vector3(rbx,spawnY, transform.position.z),Color.red, "alien-rbx-" + j, 1.0f);
                // Ent2D.CreateCirc(new Vector3(lbx,spawnY, transform.position.z),Color.green, "alien-lbx-" + j, 1.0f);
            }
            //pick the one who get to shoot
            if (shootable)
            {
                pickRandomAliensShootableNotDead(numShootable);
            }
        }
    }

    
    //picked = isZombie || isInMap
    bool isAlienNotPickable(Alien alien){
        int index;
        bool isZ = alien.isZombie;
        //Debug.Log("isZombie:"+ isZ);
        bool isPrevPicked = pickedAlienMap.TryGetValue(alien.GetHashCode(), out index);
        //Debug.Log("isPrevPicked:"+ isPrevPicked);
        return isZ || isPrevPicked;
    }

    int pickRandomAlienIndexNotDead()
    {
        int randomIndex = Random.Range(0, aliens.Length);
        //Debug.Log("how about index:" + randomIndex);
        //Debug.Log("hashcodeb4:" + aliens[randomIndex].GetHashCode());
		//last one and it is already picked?
		int nAvails = aliens.Length - deads;
        while (isAlienNotPickable(aliens[randomIndex])) {
			if (nAvails == 1) {
				//isAlready picked break
				break;
			}
			randomIndex = Random.Range(0, aliens.Length); 
		}
        return randomIndex;
    }

    void pickRandomAlienShootableNotDead(){
        int randomIndex = pickRandomAlienIndexNotDead();
        //Debug.Log("randomIndex:" + randomIndex);
        aliens[randomIndex].shootable = true;
        pickedAlienMap.Add(aliens[randomIndex].GetHashCode(), randomIndex);
    }

    void pickRandomAliensShootableNotDead(int nums){
		int nAvails = aliens.Length - deads; // BEWARE of nums > available
		int picks = (nums <= nAvails) ? nums : nAvails;
//		Debug.Log("available:" + nAvails);
//		Debug.Log ("want:" + nums);
//		Debug.Log ("pick:" + picks);
        for (int i = 0; i < picks; i++){
            pickRandomAlienShootableNotDead(); 
        }
    }

    public void OnAlienDie(Alien alien)
    {   
        //does it belong to my spawner?
        if(spawner && spawner.GetComponent<Alien>().AlienType != alien.AlienType){
            Debug.Log("not in this spawner");
            return;
        }
        //register dead
        deads++;
        if(deads == aliens.Length){//safely remove all aliens from scene
            //Debug.Log("shootableRowDestroyed!!!");
            foreach(Alien a in aliens){
                Destroy(a.gameObject);
            }
			bool isDead = false;
			if (rsMap.TryGetValue (rowShootable, out isDead)) {
				rsMap [rowShootable] = true;
			}

			if (currentShootable == rowShootable){//currentshootable ddead{
				if (currentShootable == 2) {
					Debug.Log ("last row");
				}
				//mark currentindex dead

				//get next one not dead
				int i = currentShootable + 1;
				for (; i < rsMap.Count; i++) {
					isDead = false;
					if (rsMap.TryGetValue (i, out isDead)) {
						//Debug.Log ("row:" + i + ":" + isDead);
						if(!isDead){
							
							break;
						}
					}
				}
				//Debug.Log ("next row:" + i);
				EventDispatcher.Instance.ShootableRowEradicateEvent.Invoke (i); // next one responsible for the shootable
            	
			}
			
			Destroy(gameObject);//Destroy this so event wont get to here
            return;
        }
        int index;
        if(pickedAlienMap.TryGetValue(alien.GetHashCode(),out index)){ //armed one dead, pick new shootable
            pickedAlienMap.Remove(alien.GetHashCode());
            pickRandomAlienShootableNotDead();
        }
        //unarmed alien dead ?
    }

    public void OnNextShootableRow(int row){
        if(rowShootable == row){
			currentShootable = rowShootable;
            shootable = true;
			pickRandomAliensShootableNotDead(numShootable);
        }
    }

    void ClearAliens(){
        foreach(Alien a in aliens){
            Destroy(a.gameObject);
        }
    }
    public void OnGameOver(){
        ClearAliens();
    }
    
}
