using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    //Composantes fournis par Unity
    private Transform bottomRight;
    private Transform bottomLeft;
    private Transform topLeft;
    private Transform topRight;
    private GameObject monster;
    public GameObject monster1;
    public GameObject monster2;
    public GameObject monster3;
    private AudioSource monsterSound;
    private AudioListener audioListener;
    private Animator animator;

    //variables
    private int spawnCount;
    private float randomLocation;
    private float randomRange;
    public float spawnCooldown;
    private float lastSpawn=0f;
    public int numberOfMonsters;
    private Vector3 spawnLocation;

    //exécuté une seule fois des que le programme est activé
    void Start()
    {
        bottomRight = GameObject.Find("bottom right").GetComponent<Transform>();
        bottomLeft = GameObject.Find("bottom left").GetComponent<Transform>();
        topLeft = GameObject.Find("top left").GetComponent<Transform>();
        topRight = GameObject.Find("top right").GetComponent<Transform>();
        audioListener = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        monsterSound = GetComponent<AudioSource>();
        Spawn(topLeft, topRight, true);
        spawnCount = 0;
    }

    //exécuté à chaque "frame" executé par unity
    void Update()
    {
        //faire aparaitre les monstres à une certaines position apres un certain temps
        if(Time.time> lastSpawn + spawnCooldown)
        {
            int rand = Random.Range(0, 4);
            if (rand == 0)
            {
                Spawn(topLeft, topRight, true);
            }
            else if (rand == 1)
            {
                Spawn(bottomLeft, bottomRight, true);
            }
            else if (rand == 2)
            {
                Spawn(topLeft, bottomLeft, false);
            }
            else 
            {
                Spawn(topRight, bottomRight, false);
            }
            lastSpawn = Time.time;
        }

        //acclélérer le temps de réapparition
        if (spawnCount == 3)
        {
            if (spawnCooldown >= 3)
            {
                spawnCooldown--;
            }
           
            spawnCount = 0;
        }
    }
   
    //méthode pour apparaitre les monstres alléatoirement entre certaines contraintes
    void Spawn(Transform loc1,Transform loc2, bool x)
    {
        int rand;
        float position1;
        float position2;
        float opposing;
    
        if (x==true)
        {
            position1 = loc1.position.x;
            position2 = loc2.position.x;
            opposing = loc1.position.z;
        }
        else
        {
            position1 = loc1.position.z;
            position2 = loc2.position.z;
            opposing = loc1.position.x;
        }
        randomRange = Random.Range(position2, position1);


        for (int i = 0; i < numberOfMonsters; i++)
        {
            rand = Random.Range(0, 3);
            if (rand == 0)
            {
                monster = monster1;
            }
            else if (rand == 1)
            {
                monster = monster2;
            }
            else
            {
                monster = monster3;
            }

            randomLocation = Random.Range(randomRange - 10, randomRange + 10);
            if (x == true)
            {
                spawnLocation = new Vector3(randomLocation, loc1.position.y, opposing);
            }
            else
            {
                spawnLocation = new Vector3(opposing, loc1.position.y, randomLocation);
            }
            monster.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            animator = monster.GetComponent<Animator>();
            GameObject currentMonster = Instantiate(monster, spawnLocation, Quaternion.identity);

        }
        spawnCount++;
        monsterSound.Play();
    }
}
