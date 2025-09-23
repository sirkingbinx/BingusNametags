using BepInEx.Bootstrap;

namespace BingusNametags
{
    internal class GFriendsIntegration
    {
        public static bool Enabled = true;

        private static bool Installed(string uuid) =>
            Chainloader.PluginInfos.ContainsKey(uuid);

        // oopsies, shoulda been && instead of & (my bad lmao)
        public static bool Friend(NetPlayer player) =>
            (Installed("net.rusjj.gorillafriends") & Enabled) && GorillaFriends.Main.IsFriend(player.UserId);

        public static bool RecentlyPlayedWith(NetPlayer player) =>
            (Installed("net.rusjj.gorillafriends") & Enabled) && GorillaFriends.Main.HasPlayedWithUsRecently(player.UserId) == GorillaFriends.Main.eRecentlyPlayed.Before;
        
        public static bool Verified(NetPlayer player) =>
            (Installed("net.rusjj.gorillafriends") & Enabled) && GorillaFriends.Main.IsVerified(player.UserId);
    }
}
