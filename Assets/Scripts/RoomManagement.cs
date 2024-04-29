using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


// this class will call the others to keep everything modular
public class RoomManagement : MonoBehaviour
{
    [SerializeField] public int roomAmount = 10;
    [SerializeField] public int floorNum;
    
    public bool[,] roomPositions;
    public int startXPos;
    public int startYPos;
    public RoomCreator creator;
    public GameObject[] roomHolder;
    [SerializeField] private GameObject playerPrefab;
    public GameObject player;

    private void Start()
    {
        floorNum = 1;
        roomPositions = new bool[roomAmount,roomAmount];
        SetArrayFalse();
        GenerateRoomPlacement();
        creator = GetComponent<RoomCreator>();
        roomHolder = creator.createRoom(roomPositions);
        player = creator.SpawnPlayer(playerPrefab, roomAmount/2, roomAmount/2);
    }

    private void Update()
    {
        
    }

    private void travelToNextFloor()
    {
        floorNum++;
        if (floorNum > 10)
            Congrats();
        else
        {
            //need to play an animation of the player going to the new floor
            // playAnimation();
            
            //reconstruct rooms and add enemies depending on floorNum
            roomPositions = new bool[roomAmount,roomAmount];
            SetArrayFalse();
            GenerateRoomPlacement();
            roomHolder = creator.createRoom(roomPositions);
            player = creator.SpawnPlayer(player, roomAmount/2, roomAmount/2);
        }
    }

    private void Congrats()
    {
        
    }

    private void SetArrayFalse()
    {
        for (int i = 0; i < roomPositions.GetLength(0); i++)
        {
            for (int j = 0; j < roomPositions.GetLength(1); j++)
            {
                roomPositions[i, j] = false;
            }
        }
    }

    private void GenerateRoomPlacement()
    {
        int startXPos = roomAmount / 2;
        int startYPos = roomAmount / 2;
        roomPositions[startXPos, startYPos] = true;
        int previousDirection = -1; // Stores the last used direction
        int currentX = startXPos;
        int currentY = startYPos;

        // Generate additional rooms
        for (int i = 0; i < roomAmount;)
        {
            int direction;
            if (previousDirection == -1 || Random.Range(0, 100) < 60)
            {
                // 60% chance to change direction
                direction = Random.Range(0, 4); // Choose a new random direction
            }
            else
            {
                direction = previousDirection; // 40% chance to continue in the same direction
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
            
            if (newX >= 0 && newX < roomPositions.GetLength(0) && newY >= 0 && newY < roomPositions.GetLength(1) &&
                !roomPositions[newX, newY])
            {
                // make sure room doesn't have connections on all sides
                if (CanConnectToExistingRoom(newX, newY) < 4)
                {
                    roomPositions[newX, newY] = true;
                    Debug.Log($"Generating room position {i + 1} at coordinate: {newX}, {newY}");
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
    }
    
    // count surrounding rooms
    private int CanConnectToExistingRoom(int x, int y)
    {
        int numConnected = 0;
        if ((x > 0 && roomPositions[x - 1, y]))
        {
            numConnected++;
        }
        if ((x < roomPositions.GetLength(0) - 1 && roomPositions[x + 1, y]))
        {
            numConnected++;
        }
        if ((y > 0 && roomPositions[x, y - 1]) )
        {
            numConnected++;
        } 
        if ((y < roomPositions.GetLength(1) - 1 && roomPositions[x, y + 1]))
        {
            numConnected++;
        }
        return numConnected;
    }
}