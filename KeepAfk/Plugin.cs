using Dalamud.Game.ClientState.Keys;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;

namespace KeepAfk
{
    public sealed class KeepAfk : IDalamudPlugin
    {
        private IDalamudPluginInterface PluginInterface { get; init; }
        private IKeyState KeyState { get; init; }
        private IFramework Framework { get; init; }
        private IPluginLog Log { get; init; }
        private ICommandManager CommandManager { get; init; }

        private Configuration Configuration { get; init; }

        private WindowSystem WindowSystem = new("KeepAfk");
        private ConfigWindow ConfigWindow { get; init; }

        public KeepAfk(
            IDalamudPluginInterface pluginInterface,
            IKeyState keyState,
            IFramework framework,
            IPluginLog log,
            ICommandManager commandManager)
        {
            PluginInterface = pluginInterface;
            KeyState = keyState;
            Framework = framework;
            Log = log;
            CommandManager = commandManager;

            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(PluginInterface);

            ConfigWindow = new ConfigWindow(Configuration);
            WindowSystem.AddWindow(ConfigWindow);

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            PluginInterface.UiBuilder.OpenMainUi += DrawConfigUI;

            CommandManager.AddHandler("/keepafk", new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens the KeepAfk configuration window."
            });

            Framework.Update += OnFrameworkUpdate;
        }

        private void DrawUI()
        {
            WindowSystem.Draw();
        }

        private void OnCommand(string command, string args)
        {
            DrawConfigUI();
        }

        private void DrawConfigUI()
        {
            ConfigWindow.Toggle();
        }

        private unsafe void OnFrameworkUpdate(IFramework framework)
        {
            // If the master toggle is off, do absolutely nothing
            if (!Configuration.PluginEnabled) return;

            var module = InputTimerModule.Instance();
            if (module == null) return;

            float timer = module->AfkTimer;
            if (timer >= 0) return;

            for (int i = 0; i < 255; i++)
            {
                try
                {
                    var key = (VirtualKey)i;
                    bool shouldClearKey = false;

                    if (Configuration.IsBlacklistMode)
                    {
                        if (Configuration.FilteredKeysBlackList.Contains(key))
                        {
                            shouldClearKey = true;
                        }
                    }
                    else
                    {
                        if (!Configuration.FilteredKeys.Contains(key))
                        {
                            shouldClearKey = true;
                        }
                    }

                    if (shouldClearKey && KeyState[i])
                    {
                        KeyState[i] = false;
                    }
                }
                catch { }
            }
        }

        public void Dispose()
        {
            Framework.Update -= OnFrameworkUpdate;
            PluginInterface.UiBuilder.Draw -= DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi -= DrawConfigUI;
            PluginInterface.UiBuilder.OpenMainUi -= DrawConfigUI;
            CommandManager.RemoveHandler("/keepafk");

            WindowSystem.RemoveAllWindows();
            ConfigWindow.Dispose();
        }
    }
}
