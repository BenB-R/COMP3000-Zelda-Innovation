using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    public enum RoomTypeEnum
    {
        Start,
        Boss,
        Other
    }

    [CreateAssetMenu(fileName = "RoomType", menuName = "Dungeon/RoomType")]
    public class RoomType : ScriptableObject
    {
        public GameObject prefab; // Assign prefab for each room type
        public RoomTypeEnum roomCategory; // To specify what type of room this is
        public Vector2 size; // Size of the room based on tiles
    }

}
