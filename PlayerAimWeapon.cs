using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAimWeapon : MonoBehaviourPunCallbacks
{
    public float angle;

    public Transform playerTransform;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;

    public Vector3 mousePos;
    public Vector3 gunPos;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        if (Pause.paused) return;

        mousePos = Input.mousePosition;
        gunPos = cam.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.position = playerTransform.position - offset;
    }
}
