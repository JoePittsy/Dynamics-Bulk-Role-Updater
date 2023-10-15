using System;
using System.Collections.Generic;

namespace Role_Switcher
{
    public class Playlist
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
