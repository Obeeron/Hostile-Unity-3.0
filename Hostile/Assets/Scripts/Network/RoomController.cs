using UnityEngine;
using Photon.Pun;

public class RoomController : MonoBehaviourPunCallbacks
{
#pragma warning disable 649
    [SerializeField] private int multiSceneIndex;
#pragma warning restore 649

    public override void OnJoinedRoom()
    {
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("Starting Game..");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(multiSceneIndex);
        }
    }
}
