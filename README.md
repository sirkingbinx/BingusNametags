# BingusNametags ![download badge thing (its not a virus i promise)](https://img.shields.io/github/downloads/sirkingbinx/BingusNametags/total)
BingusNametags is a customizable name mod for Gorilla Tag. It's easily extendable by anyone with a little bit of C# knowledge.

<img width=350 height=350 src="https://github.com/user-attachments/assets/ad530b19-5795-40d8-95cb-3a697340e041">
<img width=350 height=350 src="https://github.com/user-attachments/assets/cb126c2f-3cf4-4b40-a405-a0288445a7c6">

## Configuration
- You can choose what and how to show nametags in `BepInEx/config/bingus.nametags.cfg`.
- You can place any `.ttf (font file)` in your plugins folder named `BingusNametagsFont.ttf` to use that font instead of the default. I recommend [JetBrains Mono](https://www.jetbrains.com/lp/mono/).

## For Developers
### Properties
You can choose to show or hide the recognized mods nametags for the player by setting `bingusnametags_ignoreme` in your player properties.
```cs
// Mod checker only shows [Private] on top of your rig
PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
    { { "bingusnametags_ignoreme", "showPrivate" } });

// Mod checker just doesn't show anything
// This can be anything BUT "showPrivate"
PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable()
    { { "bingusnametags_ignoreme", "abaisuhghauyyus" } });
```


## Dependencies
- .NET
- [GorillaFriends](https://github.com/not-a-bird-09/GorillaFriends) (only needed for building)

## Credits
- Mod checker list from [GorillaNametags](https://github.com/HanSolo1000Falcon/GorillaNametags) by [HanSolo1000Falcon](https://github.com/HanSolo1000Falcon)
- Verified users, friending system, and recent player checks from [GorillaFriends](https://github.com/rusjj/gorillafriends) by [RusJJ](https://github.com/rusjj)
