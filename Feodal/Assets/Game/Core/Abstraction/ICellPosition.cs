﻿using System;
using Game.Core.Cells;
using HexEngine;
using UnityEngine;

namespace Game.Core.Abstraction
{
    public interface ICellPosition: 
        IEquatable<Vector3>,
        IEquatable<int>,
        IEquatable<Transform>,
        IEquatable<ICellPosition>
    {
        public int Identifier { get; set; }
        public Vector3 Global { get; set; }
        public HexPosition CellHexPosition { get; set; }
        public HexCoords CellHexCoord { get; set; }
        public Transform Root { get; set; }
    }
}