using System;
using System.Collections.Generic;

namespace GameRuntime.Room
{
    public class RoomNode
    {
        public RoomType roomType = RoomType.Normal;
        public List<RoomNode> children = new List<RoomNode>();

        public RoomNode(RoomType roomType = RoomType.Normal)
        {
            this.roomType = roomType;
        }

        public RoomNode AddChildren(RoomType roomType, Action<RoomNode> addBranch = null)
        {
            RoomNode roomNode = new RoomNode(roomType);
            children.Add(roomNode);
            addBranch?.Invoke(roomNode);
            return roomNode;
        }
    }

}