﻿using Game.RepositoryEngine.TechnologyRepositories;

namespace Game.DataStructures.Technologies.Abstraction
{
    public interface ITechnology
    {
        public string Title  { get; set; }
        public TechnologyTemp Temp { get; set; }
        TechnologyRepository Repository { get; set; }
        public bool CurrentStatus { get; set; }
        public bool Status();
    }
}