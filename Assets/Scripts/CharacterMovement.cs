using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
class C_Audio
{
    public AudioSource AS;
    public AudioClip ShootAudio;

    public void PlayAudio(AudioClip AC)
    {
        AS.PlayOneShot(AC);   
    }
}



public class CharacterMovement : MonoBehaviour
{
    [SerializeField] C_Audio C_Audio;

    [Header("GAMEOBJECTS")]
    public GameObject player;
    public GameObject MuzzleEffect;
    public GameObject camPivot;
    public GameObject Camera;
    public GameObject Bullet;
    public GameObject Muzzle;
    public GameObject MuzzleGrahical;
    public GameObject Canvas;
    public GameObject Target;

    [Header("LINKS")]
    public FixedJoystick FJ1;
    public FixedJoystick FJ2;
    public Animator anim;
    public PhotonView PV;
    public ParticleSystem DustParticle;
    private ObjectPooling _objectPool;

    [Header("VALUES")]
    public float rotatespeed;
    public float movespeed;
    public float delayToDeactivate;
    public float ShootDelay;
    public float Force;
    public int BulletToSpawn;
    public float timer;
    public float timerG;
    public float AngleInDegrees;
    public float Y = -10f;
    public float Ydefault = -10f; 
    public float adderY = 4f;

    [Header("TRANSFORMS")]
    public Transform TargetTransform;

    [Header("VECTORS_&_QUATERNIONS")]
    public Vector2 deltaRot1;
    public Vector3 currentPositionForCam;
    public Quaternion quat;

    [Header("BOOLS")]
    public bool DiffY;
    public bool DiffX;
    public bool IsMine;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        IsMine = PV.IsMine;
        if (!IsMine)
        {            
            transform.parent.Find("Canvas").gameObject.SetActive(false);
            transform.parent.Find("Main Camera").gameObject.SetActive(false);
            GetComponent<CharacterMovement>().enabled = false;
        }
        _objectPool = FindObjectOfType<ObjectPooling>();
        StartCoroutine(Tick());
    }



    void FixedUpdate()
    {
        Moving();
    }

    

    void Shoot()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {

            for (int i = 0; i < BulletToSpawn; i++)
            {
                quat = Quaternion.Euler(0, Y, 0);
                Muzzle.transform.localRotation = quat;

                GameObject bul = Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
                bul.GetComponent<Bullet>().force = Force;

                Y += adderY;
            }
            GameObject Eff = Instantiate(MuzzleEffect, Muzzle.transform.position, Muzzle.transform.rotation);
            Y = Ydefault;
            timer = ShootDelay;

            C_Audio.PlayAudio(C_Audio.ShootAudio);
        }
    }

    public void Moving()
    {
        Animations();
        Camera_AND_Input();
        Shooting();
        Particles();
    }
    void Shooting()
    {
        if (!FJ1.IsPressed)
        {
            if (Target != null)
            {
                Shoot();
                var lookPos = Target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotatespeed * 2);
            }
        }
        else
        {
            timer = ShootDelay / 2f;
        }
    }

    void Camera_AND_Input()
    {
        camPivot.transform.position = new Vector3(player.transform.position.x - currentPositionForCam.x,
            camPivot.transform.position.y - currentPositionForCam.y,
            player.transform.position.z - currentPositionForCam.z);
        deltaRot1 = new Vector2(FJ1.Horizontal, FJ1.Vertical);
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        Vector3 direction = Vector3.forward * deltaRot1.y + Vector3.right * deltaRot1.x;
        player.transform.position += direction * Time.deltaTime * movespeed;
        if (Mathf.Abs(deltaRot1.x) > 0.5f || Mathf.Abs(deltaRot1.y) > 0.5f)
        {
            Vector3 relativePos = new Vector3(deltaRot1.x, 0f, deltaRot1.y);
            Quaternion targetRot = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotatespeed);
        }
    }

    void Animations()
    {

        if (deltaRot1.y != 0f || deltaRot1.x != 0f)
        {
            if (DiffX == false && DiffY == false)
            {
                anim.SetFloat("WalkFloat", 0.5f);
            }
            if (DiffX == true || DiffY == true)
            {
                anim.SetFloat("WalkFloat", 1f);
            }
        }
        else
        {
            anim.SetFloat("WalkFloat", 0f);
        }
    }

    void Particles()
    {
        if(Mathf.Abs((deltaRot1.y + deltaRot1.x)) > 0.1f)
        {
            if(!DustParticle.isPlaying)
                DustParticle.Play();
        }
        else
        {
            if(DustParticle.isStopped == false)
                DustParticle.Stop();
        }
    }


    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            if (IsMine)
            {
                Billboard[] billboards = FindObjectsOfType<Billboard>();
                foreach (Billboard i in billboards)
                {
                    i.cam = Camera.GetComponent<Camera>();
                }
            }
        }
    }
}
