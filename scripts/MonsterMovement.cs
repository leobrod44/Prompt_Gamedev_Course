using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterMovement : MonoBehaviour
{
    //Composantes fournis par Unity
    private Transform target;
    private Animator animator;
    private AudioSource audio;
    private AudioClip hitmark;
    private Camera cam;
    private Canvas canvas;
    public Slider slider;
    private Rigidbody rb;
    private Interface interfaceScript;

    //variables
    private Vector3 distanceFromPlayer;
    private Vector3 nextPosition;
    public float monsterSpeed;
    private float health =100f;
    private float currentHealth;
    public float damageTaken;
    public float attackCooldown;
    private float timeElapsed;
    private bool canAttack = false;
    public float attackDamage;


    //exécuté une seule fois des que le programme est activé
    void Start()
    {
        //initialiser les variables
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        interfaceScript = GameObject.Find("Canvas").GetComponent<Interface>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = cam;
        currentHealth = health;
        animator.SetBool("attack", false);
        cam = Camera.main;
        hitmark = audio.clip;
        timeElapsed = 0f;
    }

    //exécuté à chaque "frame" executé par unity
    void Update()
    {
        //mouvement, élimination et attaque du monstre
        transform.LookAt(target);
        distanceFromPlayer = transform.position - target.position;

        if (distanceFromPlayer.magnitude > 4)
        {
            MoveMonster();
        }
        else
        {
            canAttack = true;
        }
        if(currentHealth <= 0)
        {
            interfaceScript.score++;
            Interface.scored = true;
            Destroy(this.gameObject);
        }
        if (canAttack)
        {
            if (Time.time > timeElapsed + attackCooldown)
            {
                Attack();
                timeElapsed = Time.time;
            }
        }
    }

    //méthode pour bouger le monstre
    void MoveMonster()
    {
        nextPosition = transform.forward * Time.deltaTime * monsterSpeed;
        rb.MovePosition(transform.position+nextPosition);
        
    }
    //méthode pour faire le monstre attaquer
    void Attack()
    {
        Interface.hit = true;
        Interface.canStartTimer = true;
        animator.SetBool("attack", true);
        interfaceScript.ReduceHp(attackDamage);
    }

    //réduire la vie du monstre lorsqu'un projectile le touche
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            if (!audio.isPlaying)
            {
                audio.PlayOneShot(hitmark, 0.5f);

            }
        currentHealth -= damageTaken;
        slider.value -= damageTaken / health;
        }
    }
}
