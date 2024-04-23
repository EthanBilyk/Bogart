using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject topDoor;
    [SerializeField] GameObject bottomDoor;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;

    [SerializeField] GameObject topDoorBlocker;
    [SerializeField] GameObject bottomDoorBlocker;
    [SerializeField] GameObject leftDoorBlocker;
    [SerializeField] GameObject rightDoorBlocker;

    [SerializeField] private Vector3 roomPosition;

    public UnityEvent OnPlayerEnterDoor;
    
    public Vector2Int RoomIndex { get; set; }

    private void Start()
    {
        OnPlayerEnterDoor = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger the event when the player enters the room
            OnPlayerEnterDoor.Invoke();
        }
    }

    // Handle door triggers
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the direction of the door
            Vector2Int direction = GetDoorDirection(other.GetComponent<Rigidbody2D>().transform.position);

            // If a valid door direction is found, open the door and move the player
            if (direction != Vector2Int.zero)
            {
                OpenDoor(direction);
                MovePlayerToAdjacentRoom(other.GetComponent<Rigidbody2D>(), direction);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void OpenDoor(Vector2Int direction)
    {
        GameObject door = null;
        GameObject doorBlocker = null;

        // Determine which door to open based on the direction
        if (direction == Vector2Int.up)
        {
            door = topDoor;
            doorBlocker = topDoorBlocker;
        }
        else if (direction == Vector2Int.down)
        {
            door = bottomDoor;
            doorBlocker = bottomDoorBlocker;
        }
        else if (direction == Vector2Int.left)
        {
            door = leftDoor;
            doorBlocker = leftDoorBlocker;
        }
        else if (direction == Vector2Int.right)
        {
            door = rightDoor;
            doorBlocker = rightDoorBlocker;
        }

        // Activate the door
        if (door != null)
        {
            door.SetActive(true);

            // Remove Rigidbody2D component from the door
            Collider2D doorCollider = door.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }

            if (doorBlocker != null)
            {
                doorBlocker.SetActive(false);
            }
        }
    }

    private Vector2Int GetDoorDirection(Vector3 playerPosition)
    {
        // Get the position of the player relative to the room
        Vector3 relativePosition = playerPosition - transform.position;

        // Determine the direction of the door based on the player's position
        if (relativePosition.y > 0)
        {
            return Vector2Int.up;
        }
        else if (relativePosition.y < 0)
        {
            return Vector2Int.down;
        }
        else if (relativePosition.x < 0)
        {
            return Vector2Int.left;
        }
        else if (relativePosition.x > 0)
        {
            return Vector2Int.right;
        }

        // If no valid door direction is found, return zero vector
        return Vector2Int.zero;
    }

    private void MovePlayerToAdjacentRoom(Rigidbody2D playerRigidbody, Vector2Int direction)
    {
        // Get the adjacent room position
        Vector3 adjacentRoomPosition = transform.position + new Vector3(direction.x, direction.y, 0);

        // Move the player to the adjacent room
        playerRigidbody.MovePosition(adjacentRoomPosition);
    }
}
