using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine.SceneManagement;
using TMPro;


public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI gameInfo;

    [SerializeField]
    private TextMeshProUGUI pingInfo;

    private void Awake()
    {
        gameInfo.text = Application.productName + " version: " + Application.version + " | " + Application.platform;
    }

    void Start()
    {

        // One string will be randomly picked 
        string[] nameOptions = { "Daniel", "Meraud", "Henricus", "Albert", "Elder" };
        string prefix = nameOptions[Random.Range(0, nameOptions.Length)];

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = prefix + Random.Range(0, 9999);
    }

    private void FixedUpdate()
    {
        pingInfo.text = PhotonNetwork.GetPing() + "ms | " + PhotonNetwork.CloudRegion;
    }


    public override void OnConnectedToMaster()
    {
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    /*

   public override void OnJoinedLobby()
   {
       PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
   }

   public override void OnJoinedRoom()
   {
       CreatePlayer();
   }


   private void CreatePlayer()
   {
       if (PhotonNetwork.IsMasterClient)
       {
           PhotonNetwork.Instantiate("Player", new Vector2(0, -4f), Quaternion.identity);
       }
       else
       {
           PhotonNetwork.Instantiate("Player", new Vector2(0, 4.0f), Quaternion.identity);
       }
   }
    */


    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene(0);
    }
}
