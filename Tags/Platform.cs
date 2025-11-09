using BingusNametags.Plugins;

namespace BingusNametags.Tags
{
    [BingusNametagsPlugin("Name", 0.8f)]
    internal class Platform : INametag
    {
        internal static bool UseOculusName = false;

        public string Update(VRRig rig)
        {
            var concatStringOfCosmeticsAllowed = rig.concatStringOfCosmeticsAllowed;

            if (concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                return "[Steam]";
            if (concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") | rig.Creator.GetPlayerRef().CustomProperties.Count > 1)
                return $"[{(UseOculusName ? "Oculus Rift" : "Oculus PCVR")}]";

            return $"[{(UseOculusName ? "Oculus Quest" : "Meta")}]";
        }
    }
}
