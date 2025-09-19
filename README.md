# BingusNametags ![download badge thing (its not a virus i promise)](https://img.shields.io/github/downloads/sirkingbinx/BingusNametags/total)
BingusNametags is a customizable name mod for Gorilla Tag. It's easily extendable by anyone with a little bit of C# knowledge.

> <img width=350 height=350 src="https://github.com/user-attachments/assets/ad530b19-5795-40d8-95cb-3a697340e041">
> <img width=350 height=350 src="https://github.com/user-attachments/assets/cb126c2f-3cf4-4b40-a405-a0288445a7c6">
>
> <b>Note</b>: Both of these mods <a href="#custom-font">use a custom font</a> called <a href="https://www.jetbrains.com/lp/mono/">JetBrains Mono</a>.

## Dependencies
- .NET
- [GorillaFriends](https://github.com/not-a-bird-09/GorillaFriends) (only needed for building)

## Configuration
### File
You can choose what and how to show nametags in `BepInEx/config/bingus.nametags.cfg`.
### Custom Font
You can place any `.ttf (font file)` in the same folder as BingusNametags named `BingusNametagsFont.ttf` to use that font instead of the default.
If installing via MonkeModManager, place the font in the `BingusNametags` folder.

## Plugins
The plugin system is an easy way for other mods to add their own nametags to BingusNametags. It does not provide a whole ton of control over positioning but is a quick and easy way to display information.
```cs
using BepInEx;
using BingusNametags.Plugins;
using TMPro;
using UnityEngine

public class MyNametag : MonoBehaviour
{
    // New updates should automatically manage nametag offsets.
    void Start() =>
        PluginManager.AddPluginUpdate(NametagUpdate);

    void NametagUpdate(TextMeshPro textObject, VRRig playerRig) {
        // do nametag stuff here. textObject represents the tag TextMeshPro object
        textObject.Text = playerRig.OwningNetPlayer.NickName;
    }
}

// See below if you already have a mod put together and nametags are only an optional feature.
[BepInDependency("bingus.nametags", DependencyFlags.HardDependency)]
[BepInPlugin("myname.mynametag", "NametagThing", "1.0.0")]
public class NametagLoader : BaseUnityPlugin
{
    void Start() => new GameObject(Info.Metadata.GUID, typeof(MyNametag));
}
```
If your mod does other stuff besides add nametags, you can append this somewhere during mod initialization.
```cs
using BepInEx.Chainloader;
```
```cs
Chainloader.PluginInfos.ContainsKey(_UUID) ?? new GameObject(Info.Metadata.GUID, typeof(MyNametag));
```

## Credits
- Mod checker list from [GorillaNametags](https://github.com/HanSolo1000Falcon/GorillaNametags) by [HanSolo1000Falcon](https://github.com/HanSolo1000Falcon)
- Verified users, friending system, and recent player checks from [GorillaFriends](https://github.com/rusjj/gorillafriends) by [RusJJ](https://github.com/rusjj)
