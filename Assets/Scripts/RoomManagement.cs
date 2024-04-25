using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


// this class will call the others to keep everything modular
public class RoomManagement : MonoBehaviour
{

    [SerializeField] public int roomSizeX = 21;
    [SerializeField] private int roomSizeY = 21;
    
    [SerializeField] private int maxRooms = 10;

    [SerializeField] private int minRooms = 6;

    private RoomCreator creator;

    private RoomConnector connector;
    public int roomSpacing = 20;

    
    private bool[,] roomPositions; // Store positions of spawned rooms
    private int startX;
    private int startY;

    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        roomPositions = new bool[maxRooms*maxRooms,maxRooms*maxRooms];
        setRoomPosFalse();
        creator = FindObjectOfType<RoomCreator>();
        connector = FindObjectOfType<RoomConnector>();
        // Start generation from the middle of the grid
        startX = maxRooms / 2;
        startY = maxRooms / 2;

        // Create the initial room at the starting position
        roomPositions[startX, startY] = true;
        Debug.Log("Generating room " + 0 + " with newX and newY values: " + startX + ", " + startY);
        creator.createRoom(roomSizeX, roomSizeY,4, startX, startY);

        // Calculate the position for the new room based on the direction
        int newX = startX;
        int newY = startY;


        GenerateRooms(newX, newY);
        //GenerateSpecialRooms();
    }

    private void setRoomPosFalse()
    {
        for (int x = 0; x < roomPositions.GetLength(0); x++)
        {
            for (int y = 0; y < roomPositions.GetLength(1); y++)
            {
                roomPositions[x, y] = false;
            }
        }
    }

    private void GenerateRooms(int startX, int startY)
    {
        int currentX = startX;
        int currentY = startY;
        int previousDirection = -1; // Stores the last used direction

        // Generate additional rooms
        for (int i = 0; i < maxRooms;)
        {
            int direction;
            if (previousDirection == -1 || Random.Range(0, 100) < 30)
            {
                // 30% chance to change direction
                direction = Random.Range(0, 4); // Choose a new random direction
            }
            else
            {
                direction = previousDirection; // 70% chance to continue in the same direction
            }

            // Calculate new position based on direction
            int newX = currentX;
            int newY = currentY;
            switch (direction)
            {
                case 0:
                    newY++;
                    break; // Up
                case 1:
                    newY--;
                    break; // Down
                case 2:
                    newX--;
                    break; // Left
                case 3:
                    newX++;
                    break; // Right
            }

            // Check if the new position is valid and can connect to existing room
            if (newX >= 0 && newX < roomPositions.GetLength(0) && newY >= 0 && newY < roomPositions.GetLength(1) &&
                !roomPositions[newX, newY])
            {
                if (CanConnectToExistingRoom(newX, newY))
                {
                    roomPositions[newX, newY] = true;
                    Debug.Log($"Generating room {i + 1} at position: {newX}, {newY}");
                    creator.createRoom(roomSizeX, roomSizeY, direction, newX, newY);
                    currentX = newX;
                    currentY = newY;
                    previousDirection = direction; // Update the direction
                    i++;
                }
                else
                {
                    previousDirection = -1; // Reset
                }
            }
        }

        Vector3 spawnPosition = new Vector3(
            (startX * roomSizeX) + (roomSizeX / 2), // Center X position of the room
            (startY * roomSizeY) + (roomSizeY / 2), // Center Y position of the room
            0  // Z position (assuming a 2D setup)
        );
        
        Instantiate(player, spawnPosition, quaternion.identity);
    }

    // Check if there's an existing adjacent room
    private bool CanConnectToExistingRoom(int x, int y) {
        return (x > 0 && roomPositions[x - 1, y]) || (x < roomPositions.GetLength(0) - 1 && roomPositions[x + 1, y]) ||
               (y > 0 && roomPositions[x, y - 1]) || (y < roomPositions.GetLength(1) - 1 && roomPositions[x, y + 1]);
    }


    // Helper method to count adjacent rooms at a given position
    private int CountAdjacentRooms(int x, int y)
    {
        int count = 0;

        // Check each adjacent cell
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                // Skip the current cell
                if (i == x && j == y)
                    continue;

                // Check if the cell is within bounds and contains a room
                if (i >= 0 && i < roomPositions.GetLength(0) && j >= 0 && j < roomPositions.GetLength(1))
                {
                    if (roomPositions[i, j])
                        count++;
                }
            }
        }

        return count;
    }
    
    public void GenerateSpecialRooms() {
        List<Vector2Int> edgePositions = GetEdgeRoomPositions();

        // Determine the number of special rooms to generate, between 2 and 4
        int numSpecialRooms = Random.Range(1, Math.Min(5, edgePositions.Count + 1));  // +1 because upper exclusive

        // Shuffle the list to randomize which edge positions are chosen
        Shuffle(edgePositions);

        // Create only the desired number of special rooms
        for (int i = 0; i < numSpecialRooms; i++) {
            Vector2Int pos = edgePositions[i];
            int roomType = Random.Range(0, 3);  // Assuming three types: 0 = Tavern, 1 = Event, 2 = Challenge
            if (CanPlaceSpecialRoom(pos.x, pos.y)) {
                Debug.Log($"Placing special room {roomType} at position: {pos.x}, {pos.y}");
                creator.createSpecialRoom(roomSizeX, roomSizeY, roomType, pos.x, pos.y);
            }
        }
    }

// Method to shuffle a list
    private void Shuffle<T>(List<T> list) {
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            int r = i + Random.Range(0, n - i);
            T temp = list[r];
            list[r] = list[i];
            list[i] = temp;
        }
    }


// Helper method to find edge positions for special rooms
    private List<Vector2Int> GetEdgeRoomPositions() {
        List<Vector2Int> edges = new List<Vector2Int>();

        for (int x = 0; x < roomPositions.GetLength(0); x++) {
            for (int y = 0; y < roomPositions.GetLength(1); y++) {
                if (roomPositions[x, y] && IsEdgeRoom(x, y)) {
                    edges.Add(new Vector2Int(x, y));
                }
            }
        }
        return edges;
    }

// Determine if a room position is on the edge of the currently generated rooms
    private bool IsEdgeRoom(int x, int y) {
        return !roomPositions[x, Math.Max(0, y - 1)] || !roomPositions[x, Math.Min(roomPositions.GetLength(1) - 1, y + 1)]
                                                     || !roomPositions[Math.Max(0, x - 1), y] || !roomPositions[Math.Min(roomPositions.GetLength(0) - 1, x + 1), y];
    }

// Check if a special room can be placed at a given location
    private bool CanPlaceSpecialRoom(int x, int y) {
        // Implement any additional checks needed before placing a special room
        return true;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        int spacing = roomSpacing;  // Ensure this matches the spacing used in room creation

        for (int x = 0; x < maxRooms; x++) {
            for (int y = 0; y < maxRooms; y++) {
                if (roomPositions[x, y]) {
                    // Calculate the correct position including spacing
                    Vector3 pos = new Vector3(
                        x * (roomSizeX + spacing) + roomSizeX / 2,  // Center of the room
                        y * (roomSizeY + spacing) + roomSizeY / 2,
                        0
                    );
                    // Draw the wire cube for the room
                    Gizmos.DrawWireCube(pos, new Vector3(roomSizeX, roomSizeY, 1));
                }
            }
        }
    }


}
