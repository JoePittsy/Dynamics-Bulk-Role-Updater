using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Role_Switcher.Services
{
    /// <summary>
    /// Provides helper methods to manage playlists, including creation, deletion, and role assignment.
    /// </summary>
    public class PlaylistService
    {
        private readonly BindingList<Playlist> _playlists;
        private readonly Playlist _emptyPlaylist;
        private readonly Action SaveSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistService"/> class.
        /// </summary>
        /// <param name="playlists">The BindingList of playlists to manage.</param>
        /// <param name="emptyPlaylist">The default empty playlist constant.</param>
        /// <param name="saveSettings">An action to trigger saving settings after changes.</param>
        public PlaylistService(BindingList<Playlist> playlists, Playlist emptyPlaylist, Action saveSettings)
        {
            _playlists = playlists;
            _emptyPlaylist = emptyPlaylist;
            SaveSettings = saveSettings;
        }

        /// <summary>
        /// Generates a unique GUID that isn't already used by existing playlists.
        /// </summary>
        /// <returns>A new unique GUID.</returns>
        public Guid GenerateUniqueGuid()
        {
            Guid guid;
            do
            {
                guid = Guid.NewGuid();
            } while (_playlists.Any(p => p.Id == guid));
            return guid;
        }

        /// <summary>
        /// Creates and adds a new blank playlist.
        /// </summary>
        /// <returns>The newly created playlist.</returns>
        public Playlist CreateNewPlaylist()
        {
            var playlist = new Playlist
            {
                Id = GenerateUniqueGuid(),
                Name = "New Playlist",
                Roles = new List<string>(),
                Teams = new List<string>()
            };

            _playlists.Add(playlist);
            SaveSettings?.Invoke();
            return playlist;
        }

        /// <summary>
        /// Deletes the given playlist if it is not the empty playlist.
        /// </summary>
        /// <param name="playlist">The playlist to delete.</param>
        /// <returns>True if deleted; false if not.</returns>
        public bool TryDeletePlaylist(Playlist playlist)
        {
            if (playlist == null || playlist == _emptyPlaylist)
                return false;

            _playlists.Remove(playlist);
            SaveSettings?.Invoke();
            return true;
        }

        /// <summary>
        /// Updates an existing playlist's name and roles.
        /// </summary>
        /// <param name="playlist">The playlist to update.</param>
        /// <param name="name">The new name.</param>
        /// <param name="roles">The new list of roles.</param>
        public void UpdatePlaylist(Playlist playlist, string name, IEnumerable<string> roles, IEnumerable<string> teams)
        {
            if (playlist == null || playlist == _emptyPlaylist)
                return;

            playlist.Name = name;
            playlist.Roles = roles.ToList();
            playlist.Teams = teams.ToList();
            SaveSettings?.Invoke();
        }
    }
}