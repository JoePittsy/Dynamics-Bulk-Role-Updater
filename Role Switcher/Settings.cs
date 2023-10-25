using System.Collections.Generic;

namespace Role_Switcher
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public bool LogsOpen { get; set; } 
        public List<Playlist> Playlists { get; set; }

    }
}