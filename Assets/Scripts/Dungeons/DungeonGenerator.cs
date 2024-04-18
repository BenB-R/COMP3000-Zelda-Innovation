using UnityEngine;
using System.Collections.Generic;
using fyp;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    public RoomType[] roomTypes; // Array of room type configurations
    public int dungeonWidth = 30;
    public int dungeonHeight = 30;
    public int roomCount = 10;

    private RoomType[,] dungeonMap; // 2D array for the dungeon grid

    void Start()
    {
        Debug.Log("Starting Dungeon Generation");
        dungeonMap = new RoomType[dungeonWidth, dungeonHeight];
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Initialize lists to manage placement of mandatory and other rooms
        List<RoomType> mandatoryRooms = roomTypes.Where(r => r.roomCategory == RoomTypeEnum.Start || r.roomCategory == RoomTypeEnum.Boss).ToList();
        List<RoomType> otherRooms = roomTypes.Where(r => r.roomCategory == RoomTypeEnum.Other).ToList();

        // Place mandatory rooms first
        foreach (RoomType roomType in mandatoryRooms)
        {
            PlaceRoom(roomType, true);
        }

        // Place other rooms
        for (int i = 0; i < roomCount - mandatoryRooms.Count; i++)
        {
            if (otherRooms.Count > 0)
            {
                RoomType selectedRoomType = otherRooms[Random.Range(0, otherRooms.Count)];
                PlaceRoom(selectedRoomType, false);
            }
        }
    }

    bool PlaceRoom(RoomType roomType, bool isMandatory)
    {
        bool placed = false;
        for (int attempt = 0; attempt < 100 && !placed; attempt++)
        {
            int x = Random.Range(0, dungeonWidth - (int)roomType.size.x + 1);
            int y = Random.Range(0, dungeonHeight - (int)roomType.size.y + 1);

            if (CanPlaceRoom(x, y, roomType))
            {
                for (int dx = 0; dx < roomType.size.x; dx++)
                {
                    for (int dy = 0; dy < roomType.size.y; dy++)
                    {
                        dungeonMap[x + dx, y + dy] = roomType;
                    }
                }
                placed = true;
            }
        }

        if (!placed && isMandatory)
        {
            Debug.LogError("Failed to place a mandatory room: " + roomType.name);
            return false;
        }

        return placed;
    }

    bool CanPlaceRoom(int x, int y, RoomType roomType)
    {
        // Check if the room fits within the bounds and doesn't overlap with existing rooms
        for (int dx = 0; dx < roomType.size.x; dx++)
        {
            for (int dy = 0; dy < roomType.size.y; dy++)
            {
                if (x + dx >= dungeonWidth || y + dy >= dungeonHeight || dungeonMap[x + dx, y + dy] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void OnDrawGizmos()
    {
        if (dungeonMap == null)
            return;

        Gizmos.color = Color.red;
        float cellSize = 1.0f; // Assuming each cell is a 1x1 square for visualization

        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector3 pos = new Vector3(x * cellSize, 0, y * cellSize);
                if (dungeonMap[x, y] != null)
                {
                    Gizmos.color = GetRoomColor(dungeonMap[x, y].roomCategory);
                    Gizmos.DrawCube(pos, new Vector3(cellSize, 0.1f, cellSize));
                }
                else
                {
                    Gizmos.DrawWireCube(pos, new Vector3(cellSize, 0.1f, cellSize));
                }
            }
        }
    }

    Color GetRoomColor(RoomTypeEnum type)
    {
        switch (type)
        {
            case RoomTypeEnum.Start:
                return Color.green;
            case RoomTypeEnum.Boss:
                return Color.red;
            case RoomTypeEnum.Other:
            default:
                return Color.blue;
        }
    }
}
