# BingusNametags ![download badge thing (its not a virus i promise)](https://img.shields.io/github/downloads/sirkingbinx/BingusNametags/total)
BingusNametags is a customizable name mod for Gorilla Tag. It's easily extendable by anyone with a little bit of C# knowledge.

<img width=350 height=350 src="https://github.com/user-attachments/assets/ad530b19-5795-40d8-95cb-3a697340e041">
<img width=350 height=350 src="https://github.com/user-attachments/assets/cb126c2f-3cf4-4b40-a405-a0288445a7c6">

## Dependencies
- .NET
- [GorillaFriends](https://github.com/not-a-bird-09/GorillaFriends) (only needed for building)

## Configuration
- You can choose what and how to show nametags in `BepInEx/config/bingus.nametags.cfg`.
- You can place any `.ttf (font file)` in your plugins folder named `BingusNametagsFont.ttf` to use that font instead of the default. I recommend [JetBrains Mono](https://www.jetbrains.com/lp/mono/).

## Plugins
> [!WARNING]
> Plugins are still a work in progress and are not expected to work for a while.
You can create your own nametags with the plugin system. While it's technically not a real plugin system, you can still extend the nametag system and that's all I need.

```
using BingusNametags.Plugins;
using UnityEngine;
using TMPro;

public class MyNametag : MonoBehaviour
{
    void Start() =>
        PluginManager.AddPluginUpdate(NametagUpdate);

    void NametagUpdate(VRRig rig, TextMeshPro textObject) {
        // do nametag stuff here. textObject represents the tag TextMeshPro object
    }
}
```

## Credits
- Mod checker list from [GorillaNametags](https://github.com/HanSolo1000Falcon/GorillaNametags) by [HanSolo1000Falcon](https://github.com/HanSolo1000Falcon)
- Verified users, friending system, and recent player checks from [GorillaFriends](https://github.com/rusjj/gorillafriends) by [RusJJ](https://github.com/rusjj)
