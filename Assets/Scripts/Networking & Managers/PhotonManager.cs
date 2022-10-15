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

    private string _region = " ";

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
        string ping = PhotonNetwork.GetPing() + "ms";
        pingInfo.text = ping + " | " + _region;
    }


    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            _region = PhotonNetwork.CloudRegion;
            _region = _region.Substring(0, 2);
        }
            
    }


    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene(0);
    }
}
