using BepInEx.Bootstrap;

namespace BingusNametags
{
    // work in progress
    // might work
    internal class GFriendsIntegration
    {
        private static bool Installed(string uuid) =>
            Chainloader.PluginInfos.ContainsKey(uuid);

        public static bool Friend(NetPlayer player) =>
            Installed("net.rusjj.gorillafriends") & GorillaFriends.Main.IsFriend(player.UserId);

        public static bool RecentlyPlayedWith(NetPlayer player) =>
            Installed("net.rusjj.gorillafriends") & GorillaFriends.Main.HasPlayedWithUsRecently(player.UserId) == GorillaFriends.Main.eRecentlyPlayed.Before;
        
        public static bool Verified(NetPlayer player) =>
            Installed("net.rusjj.gorillafriends") & GorillaFriends.Main.IsVerified(player.UserId);
    }
}
