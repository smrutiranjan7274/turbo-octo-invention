using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _warningText;
    [SerializeField] private TMP_InputField _roomId;
    [SerializeField] private Button _createButton;


    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void FixedUpdate()
    {
        if (!string.IsNullOrEmpty(_roomId.text) && _roomId.text.Length == 5)
            _warningText.text = "";
        else
            _warningText.text = "Enter a valid room code!";

    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        // JoinOrCreateRoom
        RoomOptions options = new()
        {
            MaxPlayers = 2
        };

        string roomId = _roomId.text.ToUpper();

        if (!string.IsNullOrEmpty(roomId))
        {
            PhotonNetwork.JoinOrCreateRoom(roomId, options, TypedLobby.Default);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room Successfully!");
        _roomsCanvases.CurrentRoomCanvas.Show();
        _roomsCanvases.CreateOrJoinRoomCanvas.Hide();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _roomsCanvases.CurrentRoomCanvas.Show();
        _roomsCanvases.CreateOrJoinRoomCanvas.Hide();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room creation failed: " + message);
        _warningText.text = "Room creation failed: " + message;
    }
}
