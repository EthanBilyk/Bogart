using System.Collections;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject leftWallPrefab;
    [SerializeField] private GameObject rightWallPrefab;
    [SerializeField] private GameObject topWallPrefab;
    [SerializeField] private GameObject bottomWallPrefab;
    [SerializeField] private GameObject leftDoorPrefab;
    [SerializeField] private GameObject rightDoorPrefab;
    [SerializeField] private GameObject topDoorPrefab;
    [SerializeField] private GameObject bottomDoorPrefab;

    [SerializeField] GameObject player;

    [SerializeField] private static int floorNum = 1;
    [SerializeField] private static int roomNum = 1;
    
    private int roomSizeX = (int)(20 * ((floorNum + roomNum)/2));
    private int roomSizeY = (int)(20 * ((floorNum + roomNum)/2));

    private void Start()
    {
        generateRoom();
    } 
    
    private void generateRoom()
{
    // Spawn the player in the middle of the room
    if (roomNum == 1)
    {
        Instantiate(player, new Vector3((roomSizeX/2), (roomSizeY/2), 0), Quaternion.identity);
    }


    // Instantiate walls and doors to form a box around the player
    for (int x = 0; x < roomSizeX; x++)
    {
        for (int y = 0; y < roomSizeY; y++)
        {
            // Check if the current position is on the border of the room
            if (x == 0 || x == roomSizeX - 1 || y == 0 || y == roomSizeY - 1)
            {
                // Spawn walls on the border
                GameObject wallPrefab = null;
                if (x == 0)
                {
                    wallPrefab = leftWallPrefab;
                }
                else if (x == roomSizeX - 1)
                {
                    wallPrefab = rightWallPrefab;
                }

                if (y == 0)
                {
                    wallPrefab = bottomWallPrefab;
                }
                else if (y == roomSizeY - 1)
                {
                    wallPrefab = topWallPrefab;
                }

                if (wallPrefab != null)
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }
        }
    }
}

// Function to destroy the wall segment
private void DestroyWallSegment(Vector3 position, GameObject wallPrefab)
{
    Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f);
    foreach (Collider2D collider in colliders)
    {
        if (collider.gameObject.CompareTag(wallPrefab.tag))
        {
            Destroy(collider.gameObject);
        }
    }
}

}
