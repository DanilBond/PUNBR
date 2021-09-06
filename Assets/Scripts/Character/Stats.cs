using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public PhotonView photonView;
    public Vector3 Offset;

    public float Hp;
    public string NickName;

    public Slider HpSlider;
    public Text Text;

    public CharacterMovement Character;
    public GameObject canvas;
    void Start()
    {
        NickName = PhotonNetwork.NickName;
        Text.text = NickName;
    }

    void Update()
    {
        Vector3 Pos = new Vector3(Character.transform.position.x + Offset.x,
                                  Character.transform.position.y + Offset.y,
                                  Character.transform.position.z + Offset.z);
        canvas.transform.position = Pos;
    }
    public void RemoveHp(float hp)
    {
        photonView.RPC(nameof(RegisterRemoveHp), RpcTarget.All, hp);
    }

    [PunRPC]
    void RegisterRemoveHp(float hp)
    {
        Hp -= hp;
        HpSlider.value = Hp;
    }

}
