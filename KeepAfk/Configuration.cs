using Dalamud.Configuration;
using Dalamud.Plugin;
using Dalamud.Game.ClientState.Keys;
using System;
using System.Collections.Generic;

namespace KeepAfk
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        // Master Toggle
        public bool PluginEnabled { get; set; } = true;

        public bool IsBlacklistMode { get; set; } = true;

        public HashSet<VirtualKey> FilteredKeys { get; set; } = new()
        {
            VirtualKey.RETURN,   // Enter key
            VirtualKey.W,
            VirtualKey.A,
            VirtualKey.S,
            VirtualKey.D,
            VirtualKey.SPACE,
            VirtualKey.ESCAPE,
        };

        public HashSet<VirtualKey> FilteredKeysBlackList { get; set; } = new()
        {
            VirtualKey.MENU,   // Alt key
            VirtualKey.TAB,    // Tab key
            VirtualKey.LWIN,   // Left Windows key
            VirtualKey.RWIN    // Right Windows key
        };

        [NonSerialized]
        private IDalamudPluginInterface? PluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            PluginInterface = pluginInterface;
        }

        public void Save()
        {
            PluginInterface!.SavePluginConfig(this);
        }
    }
}
