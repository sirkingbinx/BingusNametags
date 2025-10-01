using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BingusNametags.Tags
{
    internal class Name
    {
        internal static string UpdateNametag(TextMeshPro component, VRRig rig)
        {
            if (rig.OwningNetPlayer.UserId == "DEFC9810769F1F55")
                return $"<color=#5865f2>{rig.OwningNetPlayer.NickName}</color>"; 
            if (GFriendsIntegration.Verified(rig.OwningNetPlayer))
                return $"<color={Configuration.GFriendsVerifiedColor}>{rig.OwningNetPlayer.NickName}</color>";
            else if (GFriendsIntegration.Friend(rig.OwningNetPlayer))
                return $"<color={Configuration.GFriendsFriendColor}>{rig.OwningNetPlayer.NickName}</color>";
            else if (GFriendsIntegration.RecentlyPlayedWith(rig.OwningNetPlayer))
                return $"<color={Configuration.GFriendsRecentColor}>{rig.OwningNetPlayer.NickName}</color>"; 
            else
                return rig.OwningNetPlayer.NickName;
        }
    }
}
