using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Lobby : MonoBehaviourPunCallbacks
{
    public string player_prefab;
    public Transform spawn_point;

    public int players;
    public int playersReady;

    public bool isReady;

    private Text UIPlayersReady;

    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject playerThree;
    public GameObject playerFour;

    public void Awake()
    {
        ValidateConnection();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        players = players + 1;
        isReady = false;
        UIPlayersReady = GameObject.Find("Readied/Text").GetComponent<Text>();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (players <= 1)
        {
            playerOne.SetActive(true);
        }
        else if (players <= 2)
        {
            playerTwo.SetActive(true);
        }
        else if (players <= 3)
        {
            playerThree.SetActive(true);
        }
        else if (players <= 4)
        {
            playerFour.SetActive(true);
        }

        if (playersReady >= players)
        {
            //PhotonNetwork.LoadLevel(2);
            SceneManager.LoadScene(2);
        }

        UIPlayersReady.text = "Players Ready: " + playersReady + "/" + players;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        players = players + 1;
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate(player_prefab, spawn_point.position, spawn_point.rotation);
    }

    public void Ready()
    {
        isReady = true;
        playersReady = playersReady + 1;
    }

    private void ValidateConnection()
    {
        if (PhotonNetwork.IsConnected) return;
        SceneManager.LoadScene(0);
    }
}
