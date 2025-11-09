using System;
using System.Collections.Generic;
using UnityEngine;

namespace BingusNametags.Plugins
{
	public class BingusNametagsPlugin : Attribute
	{
		public readonly string Name;
		public readonly float Offset;
        
		internal Dictionary<VRRig, GameObject> Tags = new Dictionary<VRRig, GameObject>();

		public BingusNametagsPlugin(string name, float offset)
		{
			Name = name;
			Offset = offset;
		}
	}
}