using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fyp
{
    [CreateAssetMenu(fileName = "RoomType", menuName = "Dungeon/RoomType")]
    public class RoomType : ScriptableObject
    {
        public string roomName;
        public GameObject prefab; // Prefab to instantiate for this room type
        public Color roomColor; // Color can be used for debugging or visual distinction
        public bool isSpecialRoom; // Indicates if the room has a special significance, like a boss or treasure room
        public int minSize; // Minimum size of the room
        public int maxSize; // Maximum size of the room
    }
}
