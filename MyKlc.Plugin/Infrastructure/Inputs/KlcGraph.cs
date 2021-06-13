using System;

namespace MyKlc.Plugin.Infrastructure.Inputs
{
    [Serializable]
    public class KlcGraph
    {
        public string Id { get; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
