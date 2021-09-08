using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject accountPanel;
    public GameObject loadPanel;
    public GameObject playButton;
    public GameObject windowManagerPanel;
    public GameObject cell;
    public GameObject roomPanel;
    public TextMeshProUGUI text;
    public TextMeshProUGUI playerInfo;

    [Header("DEVELOP VALUES")]
    public bool isConnectedToMaster;
    public bool isConnectedToRoom;
    public bool isAdmin;
    public bool isMaster;
    public float timeSinceRoomCreated;
    public int currentPlayerCountOnTheRoom;

    public void Init()
    {
        NetworkSettings();
        loadPanel.SetActive(true);
        accountPanel.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        isConnectedToMaster = true;
        loadPanel.GetComponent<Animator>().SetTrigger("Close");
        Invoke(nameof(CloseLoadingPanel), 1f);

    }
    void CloseLoadingPanel()
    {
        loadPanel.SetActive(false);
        windowManagerPanel.SetActive(true);
    }
    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined to the room");
        roomPanel.SetActive(true);
        playButton.SetActive(false);

        Player[] players;
        players = PhotonNetwork.PlayerList;
        foreach (Player i in players)
        {
            GameObject Cell = Instantiate(cell, roomPanel.transform.Find("List").transform);
            Cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.NickName;
            Cell.name = i.UserId;
        }
        currentPlayerCountOnTheRoom = players.Length;
        isConnectedToRoom = true;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject Cell = Instantiate(cell, roomPanel.transform.Find("List").transform);
        Cell.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newPlayer.NickName;
        Cell.name = newPlayer.UserId;
        print(newPlayer.UserId);
        currentPlayerCountOnTheRoom++;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(roomPanel.transform.Find("List").transform.Find(otherPlayer.UserId).gameObject);
        currentPlayerCountOnTheRoom--;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomPanel.SetActive(false);
        windowManagerPanel.SetActive(true);

        foreach (Transform i in roomPanel.transform.Find("List").transform)
        {
            Destroy(i.gameObject);
        }
        currentPlayerCountOnTheRoom = 0;
    }
    bool ok = true;
    private void Update()
    {
            if (isConnectedToRoom)
            {
                playerInfo.text = currentPlayerCountOnTheRoom + "/" + "20";
                if (isAdmin)
                {
                    timeSinceRoomCreated += Time.deltaTime;
                    if (timeSinceRoomCreated >= 50 && ok)
                    {
                        PhotonNetwork.LoadLevel(1);
                        ok = false;
                    }
                    if (timeSinceRoomCreated >= 180 && currentPlayerCountOnTheRoom >= 12 && ok)
                    {
                        PhotonNetwork.LoadLevel(1);
                        ok = false;
                    }
                    if (currentPlayerCountOnTheRoom >= 5 && ok)
                    {
                        PhotonNetwork.LoadLevel(1);
                        ok = false;
                    }
                }

            isMaster = PhotonNetwork.IsMasterClient;
            }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        print(message);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 20;
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
        isAdmin = true;
    }


    void NetworkSettings()
    {
        //PhotonNetwork.NickName = "";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0";
        PhotonNetwork.ConnectUsingSettings();
    }
}
