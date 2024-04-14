using UnityEngine;
using System.Collections.Generic;
using fyp;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab; // Assume a generic prefab; replace with actual prefab
    public List<RoomType> roomTypes; // List of room types
    public int dungeonWidth = 30;
    public int dungeonHeight = 30;
    public int roomCount = 10;
    private List<Rect> rooms = new List<Rect>(); // List to store room data

    void Start()
    {
        Debug.Log("Starting Dungeon Generation");
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        if (roomTypes.Count == 0)
        {
            Debug.LogError("No room types assigned!");
            return;
        }

        for (int i = 0; i < roomCount; i++)
        {
            Debug.Log("Generating room " + i);
            RoomType chosenType = ChooseRoomType(); // Ensure this is not returning null
            Rect room = PlaceRoom(chosenType);
            if (room.width != 0) // Checks if room placement was successful
            {
                Debug.Log("Placing room at: " + room.position);
                GameObject roomObj = Instantiate(chosenType.prefab, new Vector3(room.x + room.width / 2, 0.5f, room.y + room.height / 2), Quaternion.identity);
                roomObj.transform.localScale = new Vector3(room.width, 1, room.height); // Set scale based on room size
                LogDimensions(roomObj); // Log dimensions of the room
            }
            else
            {
                Debug.Log("Failed to place room " + i);
            }
        }
    }


    RoomType ChooseRoomType()
    {
        // Simple random pick for demonstration; replace with your own logic as needed
        return roomTypes[Random.Range(0, roomTypes.Count)];
    }

    Rect PlaceRoom(RoomType type)
    {
        int maxTries = 50;
        for (int i = 0; i < maxTries; i++)
        {
            int roomWidth = Random.Range(type.minSize, type.maxSize + 1);
            int roomHeight = Random.Range(type.minSize, type.maxSize + 1);
            int roomX = Random.Range(0, dungeonWidth - roomWidth);
            int roomY = Random.Range(0, dungeonHeight - roomHeight);
            Rect newRoom = new Rect(roomX, roomY, roomWidth, roomHeight);
            bool overlaps = false;
            foreach (Rect room in rooms)
            {
                if (newRoom.Overlaps(room))
                {
                    overlaps = true;
                    break;
                }
            }
            if (!overlaps)
            {
                rooms.Add(newRoom); // Add room to the list if it doesn't overlap
                return newRoom;
            }
        }
        return new Rect(); // Return an empty rectangle if no placement succeeds
    }

    // Log dimensions of a room
    void LogDimensions(GameObject roomObj)
    {
        Renderer[] renderers = roomObj.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            Vector3 worldDimensions = bounds.size;
            Debug.Log(roomObj.name + " Dimensions - Width: " + worldDimensions.x + ", Height: " + worldDimensions.y + ", Depth: " + worldDimensions.z);
        }
        else
        {
            Debug.LogError("No Renderer components found on the object or its children! Ensure your room prefab is set up correctly.");
        }
    }

}
