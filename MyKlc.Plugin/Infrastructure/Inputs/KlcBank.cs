using System;
using System.Collections.Generic;

namespace MyKlc.Plugin.Infrastructure.Inputs
{
    [Serializable]
    public class KlcBank
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Number { get; set; }
        public bool Active { get; set; }
        public IEnumerable<KlcGraph> Graphs { get; set; }
    }
}
