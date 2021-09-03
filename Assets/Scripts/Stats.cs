using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public Vector3 Offset;

    public float Hp;
    public string NickName;

    public Slider HpSlider;
    public Text Text;

    public CharacterMovement Character;
    void Start()
    {
        if (Character.IsMine)
        {
            Destroy(gameObject);
        }
        else
        {
            NickName = PhotonNetwork.NickName;
            Text.text = NickName;
        }

    }

    void Update()
    {
        Vector3 Pos = new Vector3(Character.transform.position.x + Offset.x,
                                  Character.transform.position.y + Offset.y,
                                  Character.transform.position.z + Offset.z);
        transform.position = Pos;
    }
}
