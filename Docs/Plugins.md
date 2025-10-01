# Plugins API (for v1.2+)
- [Overview](#overview)
- [Classes](#classes)
    - [BingusNametagsPlugin](#bingusnametagsplugin)

# Overview
```cs
string UpdateNametag(VRRig playerRig, string accentColorHex)
{
    // For version 1.2.0+, the plugin manager expects you to return a string for the nametag text.
    // It is configurable with color (via TextMeshPro syntax) as you can see below

    bool useAccentColor = true;

    if (useAccentColor)
        return $"<color={accentColorHex}>{playerRig.OwningNetPlayer.NickName}</color>";
    else
        // You can include no color tags if you want the color to be white, as it is by default
        return $"<color=#ffffff>{playerRig.OwningNetPlayer.NickName}</color>";
    
    return playerRig.OwningNetPlayer.NickName;
}

// .. somewhere in your initialization ..
void Start() {
    // The default nametags take up 0.8f and 1f offsets
    // The arguments: (Update function, enabled, nametag offset)
    BingusNametagsPlugin myNametag = new BingusNametagsPlugin(UpdateNametag);

    // You can enable / disable it as well
    myNametag.Enabled = false;
}
```

# Classes
## BingusNametagsPlugin
`BingusNametagsPlugin` lets you hook your own nametags to BingusNametags. It is used internally to provide the default nametags, and can be used externally to add your own nametags as you please.

**Constructor:** `BingusNametagsPlugin(Func<VRRig, string, string> _NametagUpdate, bool __Enabled, float _NametagOffset) => BingusNametagsPlugin`
### NametagUpdate
> **Note:** You provide this method in the constructor and cannot set it yourself.

BingusNametags provides the VRRig of the player whose nametag is being updated, as well as the hex code for the accent color set by the user, which should be used by all non-essential nametags.
```cs
string NametagUpdate(VRRig target, string hex) {
    // you can now do what you want here
    if (target.OwningNetPlayer.UserId == "Completely real UserId")
        return "Hi ita me!";
    else
        return "that isnt me lmao";
}
```

You can use any TextMeshPro styling tricks (including `<color=*>`) for the nametag.