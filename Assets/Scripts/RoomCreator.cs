using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//this class is responsible for making the rooms and placing the doors to the other rooms
public class RoomCreator : MonoBehaviour
{
    [SerializeField] private GameObject leftWallPrefab;
    [SerializeField] private GameObject rightWallPrefab;
    [SerializeField] private GameObject bottomWallPrefab;
    [SerializeField] private GameObject topWallPrefab;
    [SerializeField] private GameObject leftDoorPrefab;
    [SerializeField] private GameObject rightDoorPrefab;
    [SerializeField] private GameObject bottomDoorPrefab;
    [SerializeField] private GameObject topDoorPrefab;
    [SerializeField] private GameObject tavernPrefab;
    [SerializeField] private GameObject challegeRoom1Prefab;
    [SerializeField] private GameObject eventRoom1Prefab;
    [SerializeField] private GameObject itemRoom1Prefab;
    [SerializeField] public int roomSizeX;
    [SerializeField] public int roomSizeY;
    public float roomSpacing = 30f;
    private RoomManagement manager;
    private GameObject[] rooms;

    private void Start()
    {
        manager = FindObjectOfType<RoomManagement>();
        rooms = new GameObject[manager.roomAmount];
    }

    private void Update()
    {
        
    }

    public GameObject SpawnPlayer(GameObject player, int x, int y)
    {
        Vector3 spawnPos = new Vector3(roomSpacing * x + roomSizeX * x + roomSizeX/2, roomSpacing * y + roomSizeY * y + roomSizeY/2, 0);
        GameObject SpawnedPlayer = Instantiate(player, spawnPos, Quaternion.identity);
        return SpawnedPlayer;
    }

    public GameObject[] createRoom(bool[,] roomPositions)
    {
        float roomPlacementX;
        float roomPlacementY;
        for (int x = 0; x < roomPositions.GetLength(0); x++)
        {
            for (int y = 0; y < roomPositions.GetLength(1); y++)
            {
                if (roomPositions[x, y])
                {
                    // check for doors
                    List<int> directions = checkDoors(roomPositions, x, y);
                    roomPlacementY = roomSpacing*y + roomSizeY*y;
                    roomPlacementX = roomSpacing*x + roomSizeX*x;
                    for (int i = 0; i < roomSizeX; i++)
                        for (int j = 0; j < roomSizeY; j++)
                        {
                                if (i == 0 && j != roomSizeY/2)
                                {
                                    // Place left wall except for the middle where the door goes
                                    Instantiate(leftWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (directions.Contains(0) && i == 0)
                                {
                                    // Place left door at the middle of the left wall
                                    Instantiate(leftDoorPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (!directions.Contains(0) && i == 0)
                                {
                                    // Place left door at the middle of the left wall
                                    Instantiate(leftWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (j == 0 && i != roomSizeX / 2)
                                {
                                    // Place bottom wall except for the middle where the door goes
                                    Instantiate(bottomWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (directions.Contains(3) && j == 0)
                                {
                                    // Place bottom door at the middle of the bottom wall
                                    Instantiate(bottomDoorPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (!directions.Contains(3) && j == 0)
                                {
                                    // Place bottom door at the middle of the bottom wall
                                    Instantiate(bottomWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                 else if (i == roomSizeX - 1 && j != roomSizeY / 2)
                                {
                                    // Place right wall except for the middle where the door goes
                                    Instantiate(rightWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (directions.Contains(1) && i == roomSizeX - 1)
                                {
                                     // Place right door at the middle of the right wall
                                    Instantiate(rightDoorPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (!directions.Contains(1) && i == roomSizeX - 1)
                                {
                                    // Place right door at the middle of the right wall
                                    Instantiate(rightWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0),
                                        Quaternion.identity);
                                }
                                else if (j == roomSizeY - 1 && i != roomSizeX / 2)
                                {
                                    // Place top wall except for the middle where the door goes
                                    Instantiate(topWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (directions.Contains(2) && j == roomSizeY - 1)
                                {
                                    // Place top door at the middle of the top wall
                                    Instantiate(topDoorPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else if (!directions.Contains(2) && j == roomSizeY - 1)
                                {
                                    // Place top door at the middle of the top wall
                                    Instantiate(topWallPrefab, new Vector3(roomPlacementX + i, roomPlacementY + j, 0), Quaternion.identity);
                                }
                                else
                                {
                                    // Handle other cases or leave it empty if it's an interior part of the room
                                }
                        }
                }
            }
        }
        //list of rooms in an array
        return rooms;
    }
    
    private List<int> checkDoors(bool[,] roomPositions, int x, int y)
    {
        List<int> directions = new List<int>();
        if (x > 0 && roomPositions[x - 1, y]) directions.Add(0); // left
        if (x < roomPositions.GetLength(0) - 1 && roomPositions[x + 1, y]) directions.Add(1); // right
        if (y < roomPositions.GetLength(1) - 1 && roomPositions[x, y + 1]) directions.Add(2); // top
        if (y > 0 && roomPositions[x, y - 1]) directions.Add(3); // bottom

        return directions;
    }

}