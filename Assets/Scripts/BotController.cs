using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BotController : MonoBehaviour
{
    public enum ShootMode
    {
        shotgun,
        semi,
        burst,
        auto
    }
    public ShootMode SM;
    private NavMeshAgent NMA;
    private GameObject Player;
    private GameObject[] hidePoses;
    private Animator anim;
    Quaternion quat;
    public ParticleSystem[] emojis;
    public GameObject Muzzle;
    public AudioSource AS;
    public AudioClip ShootAudio;
    public GameObject MuzzleEffect;
    public GameObject Bullet;

    public float Y = -10f;
    public float Ydefault;
    public float Yadder;
    public float Force;
    public float timer;
    public float ShootDelay;
    public int bulletToSpawn;
    public float d;
    public float HP;
    public Slider slider;
    public float distToAttack;

    float t = 7f;

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        hidePoses = GameObject.FindGameObjectsWithTag("HidePoint");
    }

    
    void Update()
    {
        slider.value = HP;
        Seek();
    }

    public void Stay()
    {
        NMA.SetDestination(gameObject.transform.position);
        anim.SetBool("Walk", false);
        Debug.Log("2");
    }
    public void Seek()
    {
        NMA.SetDestination(Player.transform.position);
        anim.SetBool("Walk", true);
        Detect();
        Debug.Log("3");
    }
    public void Detect()
    {
        if(Vector3.Distance(Player.transform.position, gameObject.transform.position) <= 30f)
        {
            RaycastHit hit;
            if(Physics.Raycast(gameObject.transform.position, (Player.transform.position - gameObject.transform.position), out hit, distToAttack))
            {
               if(hit.collider.gameObject.tag == "Player")
               {
                    Attack();
               }
            }
        }
    }
    public void Attack()
    {
        Stay();
        gameObject.transform.LookAt(Player.transform.position);
        switch (SM)
        {
            case ShootMode.shotgun:
                Shotgun();
                break;
            case ShootMode.semi:
                Debug.Log("semi");
                break;
            case ShootMode.burst:
                Debug.Log("brust");
                break;
            case ShootMode.auto:
                Debug.Log("auto");
                break;
        }
    }
    
    public void Emoji()
    {
        emojis[Random.Range(0, emojis.Length)].Play();
    }

    public void Shotgun()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {

            for (int i = 0; i < bulletToSpawn; i++)
            {
                quat = Quaternion.Euler(0, Y, 0);
                Muzzle.transform.localRotation = quat;

                GameObject bul = Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
                bul.GetComponent<Bullet>().force = Force;

                Y += Yadder;
            }
            AS.PlayOneShot(ShootAudio);
            GameObject Eff = Instantiate(MuzzleEffect, Muzzle.transform.position, Muzzle.transform.rotation);
            Y = Ydefault;
            timer = ShootDelay;
        }
    }
   
}
