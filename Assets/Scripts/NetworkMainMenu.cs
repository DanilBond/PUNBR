using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEditor;

public class NetworkMainMenu : MonoBehaviourPunCallbacks
{
    public InputField IF;
    public string nickName;

    public Button[] Buttons;
    void Start()
    {
        IF.onValueChanged.AddListener(delegate { HandleValueChange(); });
        Buttons[0].interactable = false;
        Buttons[1].interactable = false;
        PhotonSettings();
        SetNickNameOnStart();

    }

    
    
    void Update()
    {
       
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    

    void PhotonSettings()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    void SetNickNameOnStart()
    {
        if (PlayerPrefs.HasKey("NickName"))
        {
            nickName = PlayerPrefs.GetString("NickName");
            IF.text = nickName;
            PhotonNetwork.NickName = nickName;
        }
    }


    void HandleValueChange()
    {
        nickName = IF.text;
        PhotonNetwork.NickName = nickName;
        PlayerPrefs.SetString("NickName", nickName);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {

        Buttons[0].interactable = true;
        Buttons[1].interactable = true;
    }
}
