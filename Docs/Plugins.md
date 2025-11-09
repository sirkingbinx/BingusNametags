# Plugins API (for 1.3+)
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
        return $"{NametagOwner.OwningNetPlayer.UserId}"
    }
}
```