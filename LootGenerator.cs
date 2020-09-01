using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LootGenerator : MonoBehaviourPunCallbacks
{
    public GameObject[] lootPool;
    private int currentIndex;

    public float elapsedTime = 0f;
    public float repeatTime = 10f;

    void Start()
    {
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime = elapsedTime + 1;

        if(elapsedTime >= repeatTime)
        {
            RandomLoot();
            elapsedTime = 0;
        }
    }

    public void RandomLoot()
    {
        int newIndex = Random.Range(0, lootPool.Length);
        lootPool[currentIndex].SetActive(false);
        currentIndex = newIndex;
        lootPool[currentIndex].SetActive(true);
    }
}
