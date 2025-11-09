namespace BingusNametags.Plugins
{
	public interface INametag
	{
		public bool Enabled => true;
		public string Update(VRRig tagOwner);
	}
}