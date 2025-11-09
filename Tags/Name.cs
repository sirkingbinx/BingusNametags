using BingusNametags.Plugins;

namespace BingusNametags.Tags
{
    [BingusNametagsPlugin("Name", 1f)]
    internal class Name : INametag
    {
        public static bool GFriends = true;
        public string Update(VRRig rig)
        {
            if (!GFriends) 
                return rig.OwningNetPlayer.UserId == "DEFC9810769F1F55" ? $"<color=#7fff7f>{rig.OwningNetPlayer.NickName}</color>" : rig.OwningNetPlayer.NickName;
            
            if (GFriendsIntegration.Verified(rig.OwningNetPlayer) | rig.OwningNetPlayer.UserId == "DEFC9810769F1F55")
                return $"<color=#7fff7f>{rig.OwningNetPlayer.NickName}</color>";
            if (GFriendsIntegration.Friend(rig.OwningNetPlayer))
                return $"<color=#cc7fe5>{rig.OwningNetPlayer.NickName}</color>";
            if (GFriendsIntegration.RecentlyPlayedWith(rig.OwningNetPlayer))
                return $"<color=#ffa0a0>{rig.OwningNetPlayer.NickName}</color>"; 

            return rig.OwningNetPlayer.NickName;
        }
    }
}
