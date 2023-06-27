using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks 
{
    public TMP_InputField CreateInput;
    public TMP_InputField JoinInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(CreateInput.text); 
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainScene");
    }
}
