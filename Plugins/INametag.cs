namespace BingusNametags.Plugins
{
	public interface INametag
	{
		public bool Enabled { get; set; }
		public string Update(VRRig tagOwner);
	}
}