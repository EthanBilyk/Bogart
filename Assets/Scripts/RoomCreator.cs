using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;


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
    [SerializeField] private GameObject[,] floorWallDoorPrefabs;
    [SerializeField] private GameObject[] floorEnemyPrefabs;
    public List<Vector2> roomCenters = new List<Vector2>();


    private void Start()
    {
        manager = FindObjectOfType<RoomManagement>();
        
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
        rooms = new GameObject[roomPositions.GetLength(0)+1];
        float roomPlacementY;
        int roomIndex = 0;
        for (int x = 0; x < roomPositions.GetLength(0); x++)
        {
            for (int y = 0; y < roomPositions.GetLength(1); y++)
            {
                if (roomPositions[x, y])
                {
                    // Creating a new parent GameObject for the room
                    GameObject room = new GameObject($"Room [{x}, {y}]");
                    room.transform.position = new Vector3(roomSpacing * x, roomSpacing * y, 0);
                    Debug.Log($"Room Position: {room.transform.position}" );
                    // check for doors
                    List<int> directions = checkDoors(roomPositions, x, y);
                    roomPlacementY = roomSpacing*y + roomSizeY*y;
                    roomPlacementX = roomSpacing*x + roomSizeX*x;
                    roomCenters.Add(new Vector2(roomPlacementX + roomSizeX/2, roomPlacementY + roomSizeY/2));

                    
                    //place wall unless the room has a door on it
                    for (int i = 0; i < roomSizeX; i++)
                    {
                        for (int j = 0; j < roomSizeY; j++)
                        {
                            GameObject toInstantiate = null;
                            if (i == 0 && j != roomSizeY / 2)
                            {
                                // Place left wall except for the middle where the door goes
                                toInstantiate = leftWallPrefab;
                            }
                            else if (directions.Contains(0) && i == 0)
                            {

                                // Place left door at the middle of the left wall
                                toInstantiate = leftDoorPrefab;
                            }
                            else if (!directions.Contains(0) && i == 0)
                            {
                                // Place left door at the middle of the left wall
                                toInstantiate = leftWallPrefab;

                            }
                            else if (j == 0 && i != roomSizeX / 2)
                            {
                                // Place bottom wall except for the middle where the door goes
                                toInstantiate = bottomWallPrefab;
                            }
                            else if (directions.Contains(3) && j == 0)
                            {
                                // Place bottom door at the middle of the bottom wall
                                toInstantiate = bottomDoorPrefab;
                            }
                            else if (!directions.Contains(3) && j == 0)
                            {
                                // Place bottom door at the middle of the bottom wall
                                toInstantiate = bottomWallPrefab;
                            }
                            else if (i == roomSizeX - 1 && j != roomSizeY / 2)
                            {
                                // Place right wall except for the middle where the door goes
                                toInstantiate = rightWallPrefab;
                            }
                            else if (directions.Contains(1) && i == roomSizeX - 1)
                            {
                                // Place right door at the middle of the right wall
                                toInstantiate = rightDoorPrefab;
                            }
                            else if (!directions.Contains(1) && i == roomSizeX - 1)
                            {
                                // Place right door at the middle of the right wall
                                toInstantiate = rightWallPrefab;
                            }
                            else if (j == roomSizeY - 1 && i != roomSizeX / 2)
                            {
                                // Place top wall except for the middle where the door goes
                                toInstantiate = topWallPrefab;
                            }
                            else if (directions.Contains(2) && j == roomSizeY - 1)
                            {
                                // Place top door at the middle of the top wall
                                toInstantiate = topDoorPrefab;
                            }
                            else if (!directions.Contains(2) && j == roomSizeY - 1)
                            {
                                // Place top door at the middle of the top wall
                                toInstantiate = topWallPrefab;
                            }
                            else
                            {
                                // Handle other cases or leave it empty if it's an interior part of the room
                            }

                            if (toInstantiate)
                            {
                                Vector3 position = new Vector3(roomPlacementX + i, roomPlacementY + j, 0);
                                GameObject instance = Instantiate(toInstantiate, position, Quaternion.identity);
                                instance.transform.SetParent(room.transform);
                            }

                        }
                    }
                    SpawnEnemiesInRoom(room, roomIndex);
                    Debug.Log($"Adding room {roomIndex}");
                    rooms[roomIndex] = room;  // Assign the room to the array
                    roomIndex++;  // Increment the index after adding the room
                }
            }
        }
        //list of rooms in an array
        return rooms;
    }

    private void SpawnEnemiesInRoom(GameObject room, int roomIndex)
    {
        Vector2 spawnPoint = roomCenters[roomIndex];
        Random rand = new Random();
        int numEnemies = rand.Next(1, 5);
        for (int i = 0; i < numEnemies; i++)
        {
            int enemy = rand.Next(0, floorEnemyPrefabs.GetLength(0)-1);
            Vector2 spawnLocation = new Vector2(spawnPoint.x + rand.Next(-roomSizeX/2, roomSizeX/2),
                spawnPoint.y + rand.Next(-roomSizeY/2, roomSizeY/2));
        
            Debug.Log($"Enemy Spawn Location: {spawnLocation}");
            GameObject obj = Instantiate(floorEnemyPrefabs[enemy], spawnLocation, Quaternion.identity);
            obj.transform.SetParent(room.transform);
        }
    }

    
    private List<int> checkDoors(bool[,] roomPositions, int x, int y)
    {
        
        //check directions of surrounding rooms
        List<int> directions = new List<int>();
        if (x > 0 && roomPositions[x - 1, y]) directions.Add(0); // left
        if (x < roomPositions.GetLength(0) - 1 && roomPositions[x + 1, y]) directions.Add(1); // right
        if (y < roomPositions.GetLength(1) - 1 && roomPositions[x, y + 1]) directions.Add(2); // top
        if (y > 0 && roomPositions[x, y - 1]) directions.Add(3); // bottom

        return directions;
    }

}