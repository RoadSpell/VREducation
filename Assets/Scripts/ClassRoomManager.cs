using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ClassRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject recordingCamera;
    [SerializeField] private GameObject teacherPrefab;
    [SerializeField] private Transform teacherSpawnTransform;
    [SerializeField] private GameObject studentPrefab;
    [SerializeField] private Transform studentSpawnTransform;

    private void SpawnClients()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var teacher = PhotonNetwork.Instantiate(teacherPrefab.name, teacherSpawnTransform.position,
                    Quaternion.identity);
                Debug.Log("Teacher is: " + teacher.name);
            }

            else
            {
                var student = PhotonNetwork.Instantiate(studentPrefab.name, studentSpawnTransform.position,
                    Quaternion.identity);
                Debug.Log("Student is: " + student.name);
            }
        }
    }

    void Start()
    {
        SpawnClients();
        recordingCamera.SetActive(true);
    }
}