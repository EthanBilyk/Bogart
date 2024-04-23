using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


// this class will call the others to keep everything modular
public class RoomManagement : MonoBehaviour
{

    [SerializeField] private int roomSizeX = 21;
    [SerializeField] private int roomSizeY = 21;
    
    [SerializeField] private int maxRooms = 10;

    [SerializeField] private int minRooms = 6;

    private RoomCreator creator;

    private RoomConnector connector;

    
    private bool[,] roomPositions; // Store positions of spawned rooms

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        roomPositions = new bool[,]{
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false},
            {false,false,false,false,false,false,false,false,false,false}
        };
        creator = FindObjectOfType<RoomCreator>();
        connector = FindObjectOfType<RoomConnector>();

        GenerateRooms();
    }

    private void GenerateRooms()
    {
        // Start generation from the middle of the grid
        int startX = maxRooms / 2;
        int startY = maxRooms / 2;

        // Create the initial room at the starting position
        roomPositions[startX, startY] = true;
        creator.createRoom(roomSizeX, roomSizeY,4);
        Instantiate(player, new Vector3(startX, startY, 0), Quaternion.identity);
        Vector2Int lastRoomPos = new Vector2Int(startX, startY);

        // Calculate the position for the new room based on the direction
        int newX = startX;
        int newY = startY;
        
        // Generate additional rooms randomly connected to existing rooms
        for (int i = 0; i < maxRooms;)
        {
            // Randomly choose a direction (up, down, left, right)
            int direction = Random.Range(0, 4);

            switch (direction)
            {
                case 0: // Up
                    newY ++;
                    break;
                case 1: // Down
                    newY --;
                    break;
                case 2: // Left
                    newX --;
                    break;
                case 3: // Right
                    newX ++;
                    break;
            }

            // Store the new position if it's within the bounds of the area where rooms can be placed
            if (roomPositions[newX,newY] != true)
            {
                Debug.Log("Generating room " + i + " with newX and newY values: " + newX + ", " + newY);
                roomPositions[newX, newY] = true;
                creator.createRoom(roomSizeX, roomSizeY, direction);
                connector.connectRooms();
                i++;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }
}
