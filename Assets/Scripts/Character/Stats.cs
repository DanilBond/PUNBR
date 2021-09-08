using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public PhotonView photonView;
    public Vector3 Offset;

    public float Hp;
    public float zoneDamage;
    public string NickName;

    public Slider HpSlider;
    public Text Text;

    public CharacterMovement Character;
    public GameObject canvas;

    bool ok = true;
    void Start()
    {
        //NICK
        photonView.RPC(nameof(RegisterNickName), RpcTarget.All, photonView.Owner.NickName);
    }

    void Update()
    {
        Vector3 Pos = new Vector3(Character.transform.position.x + Offset.x,
                                  Character.transform.position.y + Offset.y,
                                  Character.transform.position.z + Offset.z);
        canvas.transform.position = Pos;
        if (ok)
        {
            //HP
            if (Hp <= 0f)
            {
                Die();
                ok = false;
            }
        }
    }
    //DIE
    void Die()
    {
        Character.isDead = true;
        Character.anim.SetBool("IsDead", true);
        Character.rigs[0].weight = 0f;
        Character.rigs[1].weight = 0f;
        Invoke(nameof(Disconnect), 3f);
    }
    //SERVER DIE SYNC
    void Disconnect()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(0);
        if(asyncLoad.isDone)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    //SERVER HP SYNC
    public void RemoveHp(float hp)
    {
        photonView.RPC(nameof(RegisterRemoveHp), RpcTarget.All, hp);
    }
    //SERVER HP SYNC
    [PunRPC]
    void RegisterRemoveHp(float hp)
    {
        Hp -= hp;
        HpSlider.value = Hp;
    }
    //SERVER NICKNAME SYNC
    [PunRPC]
    void RegisterNickName(string name)
    {
        NickName = name;
        Text.text = NickName;
    }

}
