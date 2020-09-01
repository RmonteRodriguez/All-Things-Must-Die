using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponChanger : MonoBehaviourPunCallbacks
{
    public GameObject[] weaponsPool;
    private int currentIndex;

    /*public float elapsedTime = 0f;
    public float repeatTime = 10f;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= repeatTime)
        {
            photonView.RPC("NewRandomWeapon", RpcTarget.All);

            elapsedTime -= repeatTime;
        }
    }*/

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
}
