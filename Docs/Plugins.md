# Plugins API (for v1.2+)
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

# Plugins API (for 1.3+)
> If you are using a release version of BingusNametags, this will not work for you.
> You must build BingusNametags from source to get these features (until they are released).

```cs
// Note: not implemented yet, i'm working on it though :)
//                   (name        offset)
[BingusNametagsPlugin("Plugin Name", 1.2f)]
public class MyNametag : INametag
{
    // Called everytime the nametags are updated (which is every 4 frames)
    public override string Update(VRRig NametagOwner) 
    {
        // All nametags (besides the name itself) are colored to the user's accent color.
        // Please respect this where you can.
        // The accent color is applied to all text that is not in <color> brackets.
        return $"{NametagOwner.OwningNetPlayer.UserId}"
    }
    
    // (Note: this is not required)
    // Called when the player joins. This is much more efficient if your nametag
    // does not include data that changes.
    public override string OnJoin(VRRig NametagOwner) 
    {
        // After player cosmetics are initialized, this is called.
        // Nametag remains empty.
    }
}
```