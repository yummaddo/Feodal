﻿using System.Collections.Generic;
using Game.DataStructures.Abstraction;
using Game.Typing;
using UnityEngine;

namespace Game.DataStructures
{
    [CreateAssetMenu(menuName = "CellContainer")]
    public class CellContainer : AbstractDataStructure<ICellContainer>, ICellContainer
    {
        public string containerName;
        public int price;
        public CellSeedType seedType;
        public CellState initial;
        public List<CellState> states;
        public GameObject template;
        public Seed seed;
        
        protected override ICellContainer CompareTemplate()
        {
            Initial = initial;
            States = new List<ICellState>();
            foreach (var state in states) { States.Add(state); }
            Price = price;
            SeedType = seedType;
            CellTemplate = template;
            Seed = seed;
            return this;
        }
        
        internal override string DataNamePattern => $"Cell_Container_{containerName}";

        #region ICellContainer
            public GameObject CellTemplate { get; set; }
            public int Price { get; set; }
            public CellSeedType SeedType { get; set; }
            public ICellState Initial { get; set; }
            public List<ICellState> States { get; set; }
            public Seed Seed { get; set; }

        #endregion

    }
}