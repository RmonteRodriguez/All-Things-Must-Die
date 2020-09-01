using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerInfo
{
    public ProfileData profile;
    public int actor;
    public short kills;
    public short deaths;


    public PlayerInfo (ProfileData p, int a, short k, short d)
    {
        this.profile = p;
        this.actor = a;
        this.kills = k;
        this.deaths = d;
    }
}

public class Manager : MonoBehaviourPunCallbacks
{
    //Player
    public string player_prefab;
    public Transform[] spawn_points;
    public float playerCount;

    public GameObject mapCam;

    public float elapsedTime = 0f;
    public float repeatTime = 10f;

    //Ball Grab
    public string ball_prefab;
    public Transform ball_spawn;

    //King
    public string crown_prefab;
    public Transform crown_spawn;

    public void Start()
    {
        mapCam.SetActive(false);
        ValidateConnection();
        Spawn();
    }

    void Update()
    {
        if (playerCount == 2f)
        {
            CrownSpawn();
        }
    }

    public void Spawn()
    {
        Transform t_spawn = spawn_points[Random.Range(0, spawn_points.Length)];
        PhotonNetwork.Instantiate(player_prefab, t_spawn.position, t_spawn.rotation);
        playerCount = playerCount + 1f;
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    public void BallSpawn()
    {
        PhotonNetwork.Instantiate(ball_prefab, ball_spawn.position, ball_spawn.rotation);
    }

    public void CrownSpawn()
    {
        PhotonNetwork.Instantiate(crown_prefab, crown_spawn.position, crown_spawn.rotation);
    }

    private void ValidateConnection()
    {
        if (PhotonNetwork.IsConnected) return;
        SceneManager.LoadScene(0);
    }
}