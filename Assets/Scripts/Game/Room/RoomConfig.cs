using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameRuntime.Room
{
    public enum RoomType
    {
        Normal,
        Battle,
        Finish
    }

    public class RoomConfig
    {
        public List<string> RoomLines = new();

        public RoomType roomType;

        public int RoomWidth
        {
            get
            {

                int length = 0;
                foreach (var room in RoomLines)
                {
                    length = Mathf.Max(length, room.Length);
                }
                return length;
            }
        }

        public int roomHeight => RoomLines.Count;

        public RoomConfig Type(RoomType type)
        {
            this.roomType = type;
            return this;
        }

        public RoomConfig L(String line)
        {
            RoomLines.Add(line);
            return this;
        }

    }
}