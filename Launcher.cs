using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[System.Serializable]
public class ProfileData
{
    public string username;
    public int level;
    public int xp;

    public ProfileData()
    {
        this.username = "Default Username";
        this.level = 0;
        this.xp = 0;
    }

    public ProfileData(string u, int l, int x)
    {
        this.username = u;
        this.level = l;
        this.xp = x;
    }
}

public class Launcher : MonoBehaviourPunCallbacks
{
    public InputField userNameField;
    public static ProfileData myProfile = new ProfileData();

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        myProfile = Data.LoadProfile();
        userNameField.text = myProfile.username;

        Connect();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected!");
    }

    public override void OnJoinedRoom()
    {
        StartGame();

        base.OnJoinedRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();

        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("Connecting...");
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom("");
    }

    public void StartGame()
    {
        if (string.IsNullOrEmpty(userNameField.text))
        {
            myProfile.username = "Random_User_" + Random.Range(100,1000);
        }
        else
        {
            myProfile.username = userNameField.text;
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Data.SaveProfile(myProfile);

            PhotonNetwork.LoadLevel(1);
        }
    }
}