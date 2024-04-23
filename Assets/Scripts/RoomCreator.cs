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
    private Vector2 nextRoomStartPos;

    private void Start()
    {

    }

    public void createRoom(int roomSizeX, int roomSizeY, int direction)
    {
        switch (direction)
        {
            case 0: // Up
                nextRoomStartPos.x += roomSizeX;
                break;
            case 1: // Down
                nextRoomStartPos.y -= roomSizeY;
                break;
            case 2: // Left
                nextRoomStartPos.x -= roomSizeX;
                break;
            case 3: // Right
                nextRoomStartPos.y += roomSizeX;
                break;
            case 4:
                break;
        }

        // Instantiate walls around the room
        for (int x = 0; x < roomSizeX; x++)
        {
            for (int y = 0; y <= roomSizeY - 1; y++)
            {
                if (x == 0)
                {
                    Instantiate(leftWallPrefab, new Vector3(nextRoomStartPos.x +x, nextRoomStartPos.y + y,0), Quaternion.identity);
                }
                else if (x == roomSizeX - 1)
                {
                    Instantiate(rightWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
                }
                else if (y == 0)
                {
                    Instantiate(bottomWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
                }
                else if (y == roomSizeY -1)
                {
                    Instantiate(topWallPrefab, new Vector3(nextRoomStartPos.x + x,nextRoomStartPos.y + y,0), Quaternion.identity);
                }
            }
        }
    }

}