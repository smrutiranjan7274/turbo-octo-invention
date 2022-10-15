using UnityEngine;
using TMPro;
using Photon.Pun;

public class CurrentRoomCanvas : MonoBehaviour
{
    [SerializeField] private PlayerListingsMenu _playerListingsMenu;
    [SerializeField] private LeaveRoomMenu _leaveRoomMenu;
    [SerializeField] private TextMeshProUGUI _joinedRoomText;
    [SerializeField] private TextMeshProUGUI _joinedPlayersText;


    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
        _playerListingsMenu.FirstInitialize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);
    }

    private void FixedUpdate()
    {
        _joinedRoomText.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
        _joinedPlayersText.text = "Joined " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + " Players";
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
