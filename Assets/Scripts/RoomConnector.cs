using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class will create and place the doors between each room
//along with locking and unlocking the doors
public class RoomConnector : MonoBehaviour
{

    [SerializeField] private GameObject leftDoorPrefab;
    [SerializeField] private GameObject rightDoorPrefab;
    [SerializeField] private GameObject bottomDoorPrefab;
    [SerializeField] private GameObject topDoorPrefab;
    private RoomManagement manage;

    // Start is called before the first frame update
    void Start()
    {
        manage = FindObjectOfType<RoomManagement>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!manage)
        {
            manage = FindObjectOfType<RoomManagement>();
        }
    }

    public void connectRooms(Vector3 room1Center, Vector3 room2Center)
    {
        // Calculate the hallway or connection points considering roomSpacing
        Vector3 startPoint = room1Center + new Vector3(manage.roomSizeX / 2 + manage.roomSpacing / 2, 0, 0);
        Vector3 endPoint = room2Center - new Vector3(manage.roomSizeX / 2 + manage.roomSpacing / 2, 0, 0);

        // Instantiate hallway or adjust doors accordingly
    }

}
