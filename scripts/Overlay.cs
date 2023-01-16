using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    //Composantes fournis par Unity
    public GameObject healthBar;
    public GameObject gameOverCam;
    public Camera cam;
    public Text scoreTxt;
    public Text HighScore;
    public Slider slider;
    private AudioSource audio;
    public AudioClip scoreSound;
    private Image image;

    //variables à chiffres décimaux
    private float health=100;
    public float score;
    private float restartTimer;
    private float restartCountdown;
    private bool timerStarted;
    public float hitBuffer;
    private float colorChangeTimer;

    //variables static à accédés et modifié dans d'autres scripts
    public static bool canStartTimer;
    public static bool scored;
    public static bool hit;
    public static bool inGame;

    //exécuté une seule fois des que le programme est activé
    void Start()
    {
        //initialiser les variables
        audio = GetComponent<AudioSource>();
        image = GetComponent<Image>();
        scored = false;
        hit = false;
        inGame = true;
        canStartTimer = true;
        timerStarted = false;
        restartCountdown = 4f;
        health = 100f;
        score = 0f;
        HighScore.enabled = false;
        gameOverCam.SetActive(false);
  
    }

    //exécuté à chaque "frame" executé par unity
    void Update()
    {
        scoreTxt.text = "Score: " + score;
        //code pour recommencer le jeu lorsque la partie est fini
        if (health < 0f)
        {
            inGame = false;
        }
       
        if (!inGame)
        {
            if (!timerStarted)
            {
                HighScore.enabled = true;
                PlayerPrefs.SetFloat("HighScore", score);
                HighScore.text = "HighScore: " + PlayerPrefs.GetFloat("HighScore");
                restartTimer = Time.time;
                timerStarted = true;
            }
            hit = false;
            gameOverCam.SetActive(true);

            if (Time.time > restartTimer + restartCountdown)
            {
                SceneManager.LoadScene(0);
            }
        }


        //code pour jouer le son lorsqu'un point est marqué ainsi que l'animation lorsque le joueur perd des points de vie
        if (scored)
        {
            audio.PlayOneShot(scoreSound, 1f);
            scored = false;
        }
        if (hit)
        {
            if (canStartTimer)
            {
                colorChangeTimer = Time.time;
                canStartTimer = false; ;
            }
            var color = image.color;
            color.a = 0.2f;
            color.r = 1f;
            image.color = color;
            Debug.Log("hit");
           
            if (colorChangeTimer + hitBuffer < Time.time)
            {
                hit = false;
                Debug.Log("not hit");
                colorChangeTimer = Time.time;
                color.a = 0f;
                color.r = 1f;
                image.color = color;
            }
        }

    }

    //méthode pour réduire la vie
    public void ReduceHp(float f)
    {
        slider.value -= f / 100f;
        health -= f;
    }

}
