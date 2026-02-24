using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Keys;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using System;
using System.Linq;
using System.Numerics;

namespace KeepAfk
{
    // Inherit from Dalamud's Window class
    public class ConfigWindow : Window, IDisposable
    {
        private Configuration Configuration;
        private VirtualKey selectedKeyToAdd = VirtualKey.NO_KEY;

        public ConfigWindow(Configuration configuration)
            : base("KeepAfk Settings")
        {
            Configuration = configuration;
        }

        public void Dispose() { }

        public override void Draw()
        {
            // Master Enable/Disable Toggle
            bool enabled = Configuration.PluginEnabled;
            if (ImGui.Checkbox("Enable KeepAfk", ref enabled))
            {
                Configuration.PluginEnabled = enabled;
                Configuration.Save();
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (Configuration.PluginEnabled)
            {
                bool isBlacklist = Configuration.IsBlacklistMode;
                if (ImGui.Checkbox("Use Blacklist Mode", ref isBlacklist))
                {
                    Configuration.IsBlacklistMode = isBlacklist;
                    Configuration.Save();
                }

                ImGui.Text(Configuration.IsBlacklistMode ?
                    "Currently: BLACKLIST (Blocking these keys, everything else goes through)" :
                    "Currently: WHITELIST (Allowing these keys, everything else is blocked)");

                var currentActiveList = Configuration.IsBlacklistMode ?
                                        Configuration.FilteredKeysBlackList :
                                        Configuration.FilteredKeys;

                if (ImGui.BeginCombo("##KeySelect", selectedKeyToAdd.ToString()))
                {
                    foreach (VirtualKey key in Enum.GetValues(typeof(VirtualKey)))
                    {
                        if (ImGui.Selectable(key.ToString(), key == selectedKeyToAdd))
                        {
                            selectedKeyToAdd = key;
                        }
                    }
                    ImGui.EndCombo();
                }

                ImGui.SameLine();

                if (ImGui.Button("Add Key") && selectedKeyToAdd != VirtualKey.NO_KEY)
                {
                    if (currentActiveList.Add(selectedKeyToAdd))
                    {
                        Configuration.Save();
                    }
                }

                ImGui.BeginChild("KeyList", new Vector2(300, 150), true);
                foreach (var key in currentActiveList.ToList())
                {
                    ImGui.Text(key.ToString());
                    ImGui.SameLine(200);
                    if (ImGui.Button($"Remove##{key}"))
                    {
                        currentActiveList.Remove(key);
                        Configuration.Save();
                    }
                }
                ImGui.EndChild();
            }
        }
    }
}
