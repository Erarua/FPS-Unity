using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField RoomName;
    public bool connectedToMaster;
    public bool joinedRoom;

    public string PlayerPrefabName = "Player";

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        connectedToMaster = true;
    }

    public void CreateRoom()
    {
        if (!connectedToMaster || joinedRoom) return;
        PhotonNetwork.CreateRoom(RoomName.text, 
            new RoomOptions() { MaxPlayers = 16 }, 
            TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        joinedRoom = true;
    }

    public void JoinRoom()
    {
        if (!connectedToMaster || joinedRoom) return;
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        joinedRoom = true;
        Debug.Log("Joined Room");
        PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
    }
}
