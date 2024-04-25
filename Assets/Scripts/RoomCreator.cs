using System;
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
    
    private Vector2 nextRoomStartPos;
    private int roomSpacing = 20;

    private void Start()
    {
        
    }
    
    public void createSpecialRoom(int roomSizeX, int roomSizeY, int roomType, int newX, int newY) {
        GameObject roomPrefab = null;

        // Determine which prefab to use based on the roomType parameter
        switch (roomType) {
            case 0:
                roomPrefab = tavernPrefab;
                break;
            case 1:
                roomPrefab = challegeRoom1Prefab;
                break;
            case 2:
                roomPrefab = eventRoom1Prefab;
                break;
            case 3:
                roomPrefab = itemRoom1Prefab;
                break;
            default:
                Debug.LogError("Invalid room type provided: " + roomType);
                return;
        }

        // Calculate the position to spawn the special room
        Vector3 roomPosition = new Vector3(roomSizeX * newX, roomSizeY * newY, 0);

        // Instantiate the special room at the calculated position
        if (roomPrefab != null) {
            Instantiate(roomPrefab, roomPosition, Quaternion.identity);
            Debug.Log($"Special room of type {roomType} created at {roomPosition}.");
        }
    }

    


    // public void createRoom(int roomSizeX, int roomSizeY, int direction, int newX, int newY)
    // {
    //     nextRoomStartPos.x = roomSizeX * newX;
    //     nextRoomStartPos.y = roomSizeY * newY;
    //
    //     // Instantiate walls around the room
    //     for (int x = 0; x < roomSizeX; x++)
    //     {
    //         for (int y = 0; y <= roomSizeY - 1; y++)
    //         {
    //             if (x == 0)
    //             {
    //                 Instantiate(leftWallPrefab, new Vector3(nextRoomStartPos.x + x, nextRoomStartPos.y + y,0), Quaternion.identity);
    //             }
    //             else if (x == roomSizeX - 1)
    //             {
    //                 Instantiate(rightWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
    //             }
    //             else if (y == 0)
    //             {
    //                 Instantiate(bottomWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
    //             }
    //             else if (y == roomSizeY -1)
    //             {
    //                 Instantiate(topWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
    //             }
    //         }
    //     }
    //     
    // }
    
    public void createRoom(int roomSizeX, int roomSizeY, int direction, int newX, int newY)
    {
        // Calculate the starting position of the room considering the spacing
        Vector3 roomPosition = new Vector3(
            newX * (roomSizeX + roomSpacing), 
            newY * (roomSizeY + roomSpacing), 
            0
        );

        // Instantiate walls and other components around the room
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y < roomSizeY; y++)
            {
                // Position calculation for each wall segment
                Vector3 position = roomPosition + new Vector3(x, y, 0);
            
                // Example: Placing walls, checking edges to place different components
                if (x == 0) {
                    Instantiate(leftWallPrefab, position, Quaternion.identity);
                } else if (x == roomSizeX - 1) {
                    Instantiate(rightWallPrefab, position, Quaternion.identity);
                }
                if (y == 0) {
                    Instantiate(bottomWallPrefab, position, Quaternion.identity);
                } else if (y == roomSizeY - 1) {
                    Instantiate(topWallPrefab, position, Quaternion.identity);
                }
            }
        }

        // If there is special handling for doors or other elements based on `direction`
        // Adjust those placements here.
    }


}