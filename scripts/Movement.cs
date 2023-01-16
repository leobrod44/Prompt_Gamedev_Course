using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    //Composantes fournis par Unity
    private Camera cam;
    public Transform yRef;
    public ParticleSystem gameOverParticles;
    private Rigidbody rb;
    //Variables
    public float rotation;
    private float mouseX;
    private float mouseY;
    public float movementSpeed;
    private Vector3 keyX;
    private Vector3 keyY;
    private bool aiming = false;
    public float aimSlow;
    public float yCamSpeed;
    private float normalCamSpeed =1f;
    private float camSpeed;
    private bool gameOverAnimationAllowed = true;

    //exécuté une seule fois des que le programme est activé
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        camSpeed = normalCamSpeed;
    }

    //exécuté à chaque "frame" executé par unity
    void Update()
    {
        //bouger et tourner delon le mouvement de la souris
        mouseX = rotation * Input.GetAxis("Mouse X")*Time.deltaTime;
        mouseY = -rotation * Input.GetAxis("Mouse Y") * Time.deltaTime;
        keyX = transform.forward * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        keyY = transform.right * Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        cam.transform.LookAt(yRef);
        yRef.transform.Translate(0f, camSpeed * yCamSpeed * -mouseY, 0f);
        transform.Rotate(0f, camSpeed * mouseX, 0f);
        rb.MovePosition(transform.position + keyX + keyY);
        Debug.Log(aiming);

            //viser lorsque le boutton de souris est pesé ou relaché
        if (Input.GetMouseButtonDown(1))
        {
            aiming = true;
            camSpeed = aimSlow;
        }
        if (Input.GetMouseButtonUp(1))
        {
            aiming = false;
            camSpeed = normalCamSpeed;
        }
        if (aiming)
        {
            if (cam.fieldOfView > 30f)
            {
                cam.fieldOfView--;
            }
        }
        else
        {
            if(cam.fieldOfView < 60f)
            {
                cam.fieldOfView++;
            }
        }

        //animation et événements gameover
        if (!Interface.inGame)
        {
            if (gameOverAnimationAllowed)
            {
                Instantiate(gameOverParticles, transform.position, Quaternion.identity);
                gameOverAnimationAllowed = false;
            }
          
            foreach(GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                if(o.gameObject.tag=="Gun")
                {
                    Destroy(o);
                }
            }
        }
    }
}
