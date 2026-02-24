# KeepAfk

A Dalamud plugin for Final Fantasy XIV that prevents specific keystrokes and inputs from removing your AFK status. 

By default, the FFXIV client drops the AFK status the moment it detects keyboard input or window focus changes. KeepAfk intercepts local inputs before they reach the game's idle timer, allowing you to use specific keys (like Alt, Tab, or the Windows key) without waking your character.

### Features
* **Input Filtering:** Control exactly which keystrokes affect your AFK timer.
* **Blacklist Mode (Default):** Specify keys that the game should ignore while you are AFK. By default, this blocks the Alt, Tab, and Windows keys so alt-tabbing does not remove your status.
* **Whitelist Mode:** Invert the logic to allow only specific keys to wake the client, while ignoring everything else.

### Commands
* `/keepafk` - Opens and closes the configuration window.
