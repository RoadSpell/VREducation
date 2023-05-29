using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Person : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    protected void EnableOwnComponents()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            camera.SetActive(true);
        }
    }
}