# Plugins API (for 1.3+)
## Quick Start
```cs
// Set the nametag name and offset here.
[BingusNametagsPlugin("Plugin Name", 1.2f)]
public class MyNametag : INametag
{
    // Called everytime the nametags are updated (which is every 4 frames)
    public override string Update(VRRig NametagOwner) 
    {
        // All nametags (besides the name itself) are colored to the user's accent color.
        // Please respect this where you can.
        // The accent color is applied to all text that is not in <color> brackets.
        return $"{NametagOwner.OwningNetPlayer.UserId}";
    }
}
```

## Disable built-in nametags
If your plugin has the need to override the default nametags, each nametag is stored in BingusNametags.Tags and has an instance property to allow you to disable it when needed.
```cs
// Here's some convienent methods I borrowed from my grandma
void BGToggleBuiltin(bool all) {
    BingusNametags.Tags.Name.instance.Enabled = all;
    BingusNametags.Tags.Platform.instance.Enabled = all;
}

void BGToggleBuiltin(bool name, bool platform) {
    BingusNametags.Tags.Name.instance.Enabled = name;
    BingusNametags.Tags.Platform.instance.Enabled = platform;
}
```

## Finding certain nametags
For searching for nametags, my dad has given you these two functions that you can paste for free without crediting him. Isn't he nice?
> *PS:* The reason this wasn't implemented in the BingusNametags plugin system itself is because I forgot about adding them on release.

```cs
using BingusNametags.Plugins;
using System.Collections.Generic;
using System.Linq;

// Here's some convienent methods I borrowed from my dad
bool TryGetNametag(string pluginName, out KeyValuePair<INametag, BingusNametagsPlugin> val) {
    val = Plugins.All.Where(pair => pair.Value.Name == pluginName).FirstOrDefault();
    return (val != null);
}

KeyValuePair<INametag, BingusNametagsPlugin>? TryGetNametag(string pluginName) {
    return Plugins.All.Where(pair => pair.Value.Name == pluginName).FirstOrDefault();
} 
```
