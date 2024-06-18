using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core.Cells
{
    [System.Serializable]
    public class MapCellTreeNodeHash
    {
        public MapCellTreeNode node;
        public Direction direction;

        public MapCellTreeNodeHash(MapCellTreeNode node, Direction direction)
        {
            this.node = node;
            this.direction = direction;
        }
    }


    [System.Serializable]
    public class MapCellTreeNode
    {
        public float radius;
        public Vector3 position;
        public Cell cell;
        public Dictionary<Direction, MapCellTreeNode> HexagonalVector = new Dictionary<Direction, MapCellTreeNode>();
        public List<MapCellTreeNodeHash> nodes = new List<MapCellTreeNodeHash>();
        internal MapCellTreeNode(MapCellTree map, Cell cell, Vector3 position, float radius)
        {
            this.cell = cell;
            this.position = position;
            this.radius = radius;
            if (cell ==null)
                return;
            else
            {
                HexagonalVector.Add(Direction.Direct0, CreateInstance(map,null,position, Direction.Direct0, radius));
                HexagonalVector.Add(Direction.Direct60, CreateInstance(map,null,position, Direction.Direct60, radius));
                HexagonalVector.Add(Direction.Direct120, CreateInstance(map,null,position, Direction.Direct120, radius));
                HexagonalVector.Add(Direction.Direct180, CreateInstance(map,null,position, Direction.Direct180, radius));
                HexagonalVector.Add(Direction.Direct240, CreateInstance(map,null,position, Direction.Direct240, radius));
                HexagonalVector.Add(Direction.Direct300, CreateInstance(map,null,position, Direction.Direct300, radius));
                foreach (var vHex in HexagonalVector) nodes.Add(new MapCellTreeNodeHash(vHex.Value, vHex.Key));
            }
        }
        public static Vector3 CreatePosition(Vector3 position, Direction direction, float radius)
        {
            return position + DirectionExtended.GetDirectionVector(direction) * radius;
        }
        public static MapCellTreeNode CreateInstance(MapCellTree map, Cell cell, Vector3 position,Direction direction, float radius)
        {
            return new MapCellTreeNode(map,cell, CreatePosition(position,direction, radius), radius);
        }
        internal bool AddNodeRecursive(MapCellTreeNode node,MapCellTree map, HashSet<MapCellTreeNode> visited, Cell nCell, Vector3 newPositionVector3, float newRadius, bool status = false)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);
                foreach (var hNode in node.HexagonalVector)
                {
                    Debug.Log($"{hNode.Value.position.ToString()} == {newPositionVector3.ToString()}");
                    if (hNode.Value.cell == null)
                    {
                        if (hNode.Value.position == newPositionVector3)
                        {

                            HexagonalVector[hNode.Key] = CreateInstance(map,nCell,newPositionVector3,hNode.Key, newRadius);
                            status = true;
                        }
                        return status;
                    }
                    return AddNodeRecursive(hNode.Value, map, visited, nCell, newPositionVector3, newRadius,status);
                }
            }
            return status;
        }

        public List<Vector3> FindAvailablePositions()
        {
            List<Vector3> availablePositions = new List<Vector3>();
            HashSet<MapCellTreeNode> visited = new HashSet<MapCellTreeNode>();
            FindAvailablePositionsRecursive(this, availablePositions, visited);
            return availablePositions;
        }
        private void FindAvailablePositionsRecursive(MapCellTreeNode node, List<Vector3> availablePositions, HashSet<MapCellTreeNode> visited)
        {
            if (!visited.Contains(node))
            {
                visited.Add(node);
                foreach (var hNode in node.HexagonalVector)
                {
                    if (hNode.Value.cell == null)
                    {
                        if (!availablePositions.Contains(hNode.Value.position))
                            availablePositions.Add(hNode.Value.position);
                    }
                    else FindAvailablePositionsRecursive(hNode.Value, availablePositions, visited);
                }
            }
        }
    }
    
    [System.Serializable]
    public class MapCellTree
    {
        public MapCellTreeNode initialNode;
        public MapCellTree(Cell cell,Vector3 position, float radius)
        {
            initialNode = new MapCellTreeNode(this,cell, position, radius);
        }
        public bool AddCell(Cell cell, Vector3 position)
        {
            HashSet<MapCellTreeNode> visited = new HashSet<MapCellTreeNode>();
            initialNode.AddNodeRecursive(initialNode,this,visited, cell, position, initialNode.radius);
            return true;
        }
        public List<Vector3> GetAvailablePositions()
        {
            var n = initialNode.FindAvailablePositions();
            Debug.Log(n.Count);
            return n;
        }
    }
}