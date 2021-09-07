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
    public PhotonView photonView;
    [SerializeField] C_Audio C_Audio;    

    [Header("GAMEOBJECTS")]
    public GameObject player;
    public GameObject MuzzleEffect;
    public GameObject camPivot;
    public GameObject Camera;
    public GameObject Bullet;
    public GameObject Muzzle;
    public GameObject MuzzleGraphical;
    public GameObject Canvas;

    [Header("LINKS")]
    public VariableJoystick FJ1;
    public VariableJoystick FJ2;
    public Animator anim;
    public Stats stats;
    

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
    public Vector2 deltaRot2;
    public Vector3 currentPositionForCam;
    public Quaternion quat;

    [Header("BOOLS")]
    public bool DiffY;
    public bool DiffX;
    public bool IsAiming;
    public bool IsShooting;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        
        if (!photonView.IsMine)
        {            
            transform.parent.Find("Canvas").gameObject.SetActive(false);
            transform.parent.Find("Main Camera").gameObject.SetActive(false);
            GetComponent<CapsuleCollider>().radius = 0.8f;
        }
    }



    void FixedUpdate()
    {
        Moving();
        
    }

    

    void Shoot()
    {
        if (photonView.IsMine == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {

                for (int i = 0; i < BulletToSpawn; i++)
                {
                    quat = Quaternion.Euler(0, Y, 0);
                    Muzzle.transform.localRotation = quat;

                    GameObject bul = Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
                    bul.GetComponent<Bullet>().isFake = false;
                    Y += adderY;
                }
                GameObject Eff = Instantiate(MuzzleEffect, Muzzle.transform.position, Muzzle.transform.rotation);
                Destroy(Eff, 1f);
                Y = Ydefault;
                timer = ShootDelay;

                camPivot.GetComponent<Animator>().SetTrigger("Shake");
                C_Audio.PlayAudio(C_Audio.ShootAudio);
            }
        }
        else
        {
            if (IsShooting)
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {

                    for (int i = 0; i < BulletToSpawn; i++)
                    {
                        quat = Quaternion.Euler(0, Y, 0);
                        Muzzle.transform.localRotation = quat;

                        GameObject bul = Instantiate(Bullet, Muzzle.transform.position, Muzzle.transform.rotation);
                        bul.GetComponent<Bullet>().isFake = true;
                        Y += adderY;
                    }
                    GameObject Eff = Instantiate(MuzzleEffect, Muzzle.transform.position, Muzzle.transform.rotation);
                    Destroy(Eff, 1f);
                    Y = Ydefault;
                    timer = ShootDelay;

                    C_Audio.PlayAudio(C_Audio.ShootAudio);
                }
            }
        }
    }

    public void Moving()
    {
        if (photonView.IsMine)
        {
            Animations();
            Camera_AND_Input();
        }
        Shooting();
    }
    void Shooting()
    {
        if (photonView.IsMine)
        {
            if (FJ2.IsPressed)
            {
                Shoot();
                MuzzleGraphical.SetActive(true);
                isShooting = true;
            }
            else
            {
                timer = ShootDelay / 2f;
                MuzzleGraphical.SetActive(false);
                isShooting = false;
            }
        }
        else
        {
            Shoot();
        }
    }

    void Camera_AND_Input()
    {
        if (FJ2.Direction.magnitude > 0.1f)
            IsAiming = true;
        else
            IsAiming = false;
        //CAMERA//
        camPivot.transform.position = new Vector3(player.transform.position.x - currentPositionForCam.x,
            camPivot.transform.position.y - currentPositionForCam.y,
            player.transform.position.z - currentPositionForCam.z);
        //CAMERA//
        deltaRot1 = new Vector2(FJ1.Horizontal, FJ1.Vertical);
        deltaRot2 = new Vector2(FJ2.Horizontal, FJ2.Vertical);
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        Vector3 direction = new Vector3();
        direction = Vector3.forward * deltaRot1.y + Vector3.right * deltaRot1.x;
        player.transform.position += direction * Time.deltaTime * movespeed;

        if (FJ1.Direction.magnitude > 0.35f)
        {
            if (!IsAiming)
            {
                Vector3 relativePos = new Vector3(deltaRot1.x, 0f, deltaRot1.y);
                Quaternion targetRot = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotatespeed);
            }
        }
        if (FJ2.Direction.magnitude > 0.35f)
        {
            if (IsAiming)
            {
                Vector3 relativePos = new Vector3(deltaRot2.x, 0f, deltaRot2.y);
                Quaternion targetRot = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotatespeed);
            }
        }
    }

    public bool isShooting
    {
        get { return IsShooting; }
        set
        {
            if (value == IsShooting)
                return;

            IsShooting = value;
            photonView.RPC(nameof(IsShootingRegister), RpcTarget.All, value);
        }
    }

    [PunRPC]
    void IsShootingRegister(bool v)
    {
        IsShooting = v;
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
}
