# Plugins API (for v1.2.0+)
```cs
void UpdateNametag(TextMeshPro tmProText, VRRig playerRig)
{
    string text = "";

    // Without color tags, the nametag color will either be:
    //  - white (no accent color)
    //  - accent color
    // PluginManager.AddPluginUpdate() allows you to toggle accent colors.
    // You can define your own colors with <color=*> to override this.
    text = $"{playerRig.OwningNetPlayer.NickName}";
    
    return playerRig.OwningNetPlayer.NickName;
}

// .. somewhere in your initialization ..
//                             <update nametag> <offset>  <use accent color>
PluginManager.AddPluginUpdate(  UpdateNametag,    0.6f,          true       );
```

You can use any TextMeshPro styling tricks (including `<color=*>`) for the nametag.