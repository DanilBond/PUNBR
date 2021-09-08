using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviourPunCallbacks
{
    public Item item;

    public void DestroyItem()
    {
        GetComponent<PhotonView>().RPC(nameof(DestroyRegister), RpcTarget.All);
    }

    [PunRPC]
    public void DestroyRegister()
    {
        Destroy(gameObject);
    }

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    base.OnDisconnected(cause);
    //    GetComponent<PhotonView>().OwnershipTransfer();
    //}
}
