using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //Composantes fournis par Unity
    public Transform yRef;
    private Camera cam;
    public ParticleSystem splash;
    public GameObject paintGunMuzzle;
    public AudioListener audio;
    private AudioSource splashSound;
    public AudioClip clip;
    public GameObject bullet;
    public Transform firePoint;

    //variables
    private Vector3 lookDir;
    public float bulletSpeed;
    private bool firing;
    public float cooldown;
    private float time;

    void Start()
    {
        //initialiser les variables
        cam = Camera.main;
        time = Time.time;
        splashSound = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        //tirer lorsque le boutton de souris est pesé ou relaché
        if(Input.GetMouseButtonDown(0))
        {
            firing = true;
            splashSound.volume = 1f;
            splashSound.Play(); 
        }
        if (Input.GetMouseButtonUp(0))
        {
            splashSound.Stop();
            firing = false;
        }
        if (firing)
        {
            if (Time.time > time + cooldown)
            {
                Fire();
                time = Time.time;
            }
        }

        lookDir = cam.transform.forward;
    }

    //méthode pour tirer un projectile
    void Fire()
    {
        GameObject clone = Instantiate(bullet, firePoint.position, Quaternion.Euler(90,0,0));
        bullet.transform.rotation = Quaternion.LookRotation(this.transform.forward);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        rb.AddForce(bulletSpeed*lookDir, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(clone, 2f));
        Instantiate(splash, paintGunMuzzle.transform.position, Quaternion.identity);
    }

    //méthode pour détruire le projectile apres un délais avec une coroutine
     public IEnumerator DestroyBullet(GameObject obj, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(obj);
    }
}

