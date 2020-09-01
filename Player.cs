using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 5f;
    public float maxHealth = 100;
    public float currentHealth;
    public float points;
    public float storedPoints;

    public Rigidbody2D rb;

    public GameObject cameraParent;

    //UI
    private Transform UIHealthbar;
    private Text healthbarText;
    private Text UIUsername;
    private Text UIPoints;

    public ProfileData playerProfile;
    public TextMeshPro playerUsername;

    Vector2 movement;

    private Manager manager;

    //Swapping Weapons
    public GameObject[] weaponsPool;
    private int currentIndex;
    public GameObject primaryWeapon;
    public GameObject secondaryWeapon;

    //King Gamemode
    public string crownPrefab;
    private bool isKing = false;
    private float pointsIncreasedPerSecond;

    void Start()
    {
        if(!photonView.IsMine) return;

        manager = GameObject.Find("Manager").GetComponent<Manager>();

        currentHealth = maxHealth;

        cameraParent.SetActive(photonView.IsMine);

        if (photonView.IsMine)
        {
            UIHealthbar = GameObject.Find("HUD/Health/Bar").transform;
            healthbarText = GameObject.Find("HUD/Health/Text").GetComponent<Text>();
            UIUsername = GameObject.Find("HUD/Username/Text").GetComponent<Text>();
            UIPoints = GameObject.Find("HUD/Stats/Kills/Text").GetComponent<Text>();

            RefreshHealthBar();
            UIUsername.text = Launcher.myProfile.username;

            photonView.RPC("SyncProfile", RpcTarget.All, Launcher.myProfile.username, Launcher.myProfile.level, Launcher.myProfile.xp);

            //King Gamemode
            isKing = false;
            pointsIncreasedPerSecond = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        if (photonView.IsMine)
        {
            healthbarText.text = currentHealth.ToString();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                photonView.RPC("PrimaryWeaponSwap", RpcTarget.All);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                photonView.RPC("SecondaryWeaponSwap", RpcTarget.All);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                manager.EndGame();
            }
        }
    
        //Controls
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        bool pause = Input.GetKeyDown(KeyCode.Escape);

        //Pause
        if (pause)
        {
            GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
        }

        if (Pause.paused)
        {
            movement.x = 0f;
            movement.y = 0f;
        }

        Death();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            photonView.RPC("PistolDamage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "BR69Bullet")
        {
            photonView.RPC("BR69Damage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "PewPew")
        {
            photonView.RPC("PewPewDamage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "Long")
        {
            photonView.RPC("L0N9Damage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "Kah")
        {
            photonView.RPC("K4HDamage", RpcTarget.All);
        }

        if (collision.gameObject.tag == "Crown")
        {
            //crown.SetActive(true);
            isKing = true;
            Debug.Log("Is King!");
        }
    }

    public void Death()
    {
        if (photonView.IsMine)
        {
            if (currentHealth <= 0)
            {
                manager.Spawn();
                PhotonNetwork.Destroy(gameObject);

                if (isKing == true)
                {
                    PhotonNetwork.Instantiate(crownPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
    }

    void RefreshHealthBar()
    {
        float t_health_ratio = currentHealth / maxHealth;
        UIHealthbar.localScale = new Vector3(t_health_ratio, 1, 1);
    }

    [PunRPC]
    private void SyncProfile(string p_username, int p_level, int p_xp)
    {
        playerProfile = new ProfileData(p_username, p_level, p_xp);
        playerUsername.text = playerProfile.username;
    }

    //Guns Damage
    [PunRPC]
    public void PistolDamage()
    {
        currentHealth = currentHealth - 2f;

        RefreshHealthBar();
    }

    [PunRPC]
    public void BR69Damage()
    {
        currentHealth = currentHealth - 10f;

        RefreshHealthBar();
    }

    [PunRPC]
    public void PewPewDamage()
    {
        currentHealth = currentHealth - 7f;

        RefreshHealthBar();
    }

    [PunRPC]
    public void L0N9Damage()
    {
        currentHealth = currentHealth - 40f;

        RefreshHealthBar();
    }

    [PunRPC]
    public void K4HDamage()
    {
        currentHealth = currentHealth - 3f;

        RefreshHealthBar();
    }

    //Weapons Equip
    [PunRPC]
    public void PrimaryWeaponSwap()
    {
        primaryWeapon.SetActive(true);
        secondaryWeapon.SetActive(false);
    }

    [PunRPC]
    public void SecondaryWeaponSwap()
    {
        primaryWeapon.SetActive(false);
        secondaryWeapon.SetActive(true);
    }

    [PunRPC]
    public void NewRandomWeapon()
    {
        int newIndex = Random.Range(0, weaponsPool.Length);
        //Deactivate old weapon
        weaponsPool[currentIndex].SetActive(false);
        //Activate new weapon
        currentIndex = newIndex;
        weaponsPool[currentIndex].SetActive(true);
    }

    //Points
    [PunRPC]
    public void PointCounter()
    {
        UIPoints.text = (int)points + " Seconds";
        points += pointsIncreasedPerSecond * Time.fixedDeltaTime;
    }
}