using UnityEngine;
using System.Collections.Generic;
using fyp;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject roomPrefab;
    public List<RoomType> roomTypes; // List of room types
    public int dungeonWidth = 30;
    public int dungeonHeight = 30;
    public int roomCount = 10;
    private List<Rect> rooms = new List<Rect>(); // List to store room data

    float cellWidth = 4.3f; // The width of each grid cell (along the x-axis)
    float cellHeight = 4.3f; // The height of each grid cell (along the z-axis)

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
                // Log dimensions of the room
            }
            else
            {
                Debug.Log("Failed to place room " + i);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (rooms.Count == 0)
            return;

        // Define the grid cell size based on the desired room dimensions
        // Since you want the grid to have a ratio of 4:3 and cover the entire dungeon, we'll use these dimensions for the cells.
        float cellWidth = 4.3f; // The width of each grid cell (along the x-axis)
        float cellHeight = 4.3f; // The height of each grid cell (along the z-axis), maintaining a 4:3 ratio

        Gizmos.color = Color.red;

        // Draw a common grid for the entire dungeon floor
        for (float x = 0; x < dungeonWidth; x += cellWidth)
        {
            for (float z = 0; z < dungeonHeight; z += cellHeight)
            {
                // Draw grid lines for each cell
                for (float i = 0; i <= cellWidth; i += cellWidth)
                {
                    Vector3 startV = new Vector3(x + i, 0, z);
                    Vector3 endV = startV + new Vector3(0, 0, dungeonHeight);
                    Gizmos.DrawLine(startV, endV);
                }
                for (float j = 0; j <= cellHeight; j += cellHeight)
                {
                    Vector3 startH = new Vector3(x, 0, z + j);
                    Vector3 endH = startH + new Vector3(dungeonWidth, 0, 0);
                    Gizmos.DrawLine(startH, endH);
                }
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
        int gridWidth = 3; // The width of the grid cell
        int gridHeight = 4; // The height of the grid cell
        int maxTries = 100;

        Bounds prefabBounds = CalculatePrefabBounds(type.prefab);

        // Ensure the room dimensions are multiples of the grid dimensions
        int roomGridWidth = Mathf.CeilToInt(prefabBounds.size.x / gridWidth) * gridWidth;
        int roomGridHeight = Mathf.CeilToInt(prefabBounds.size.z / gridHeight) * gridHeight;

        for (int i = 0; i < maxTries; i++)
        {
            // Snap room position to the grid
            float roomX = Mathf.FloorToInt(Random.Range(0, dungeonWidth / cellWidth)) * cellWidth;
            float roomY = Mathf.FloorToInt(Random.Range(0, dungeonHeight / cellHeight)) * cellHeight;
            Rect newRoom = new Rect(roomX, roomY, roomGridWidth, roomGridHeight);

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
                // Snap room position to the grid, and adjust for the room's size
                rooms.Add(newRoom);
                //Vector3 roomPositionOffset = new Vector3(2.2f, 1.8f, -8.65f);
                Vector3 roomPosition = new Vector3(roomX, 0, roomY) + new Vector3(cellWidth / 2, 0, cellHeight / 2) - new Vector3(prefabBounds.extents.x, 0, prefabBounds.extents.z);
                GameObject roomObj = Instantiate(type.prefab, roomPosition, Quaternion.identity);
                LogDimensions(roomObj);
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


    Bounds CalculatePrefabBounds(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogError("No renderers found in prefab.");
            return new Bounds();
        }

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        return bounds;
    }

}
