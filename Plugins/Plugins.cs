using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using TMPro;
using UnityEngine;

namespace BingusNametags.Plugins
{
	public class Plugins
	{
		public static List<INametag> All = new List<INametag>();
		public static Dictionary<INametag, BingusNametagsPlugin> MetadataDictionary = new Dictionary<INametag, BingusNametagsPlugin>();
		
		// Locate plugins
		private static List<Type> GetNametagInterfaces()
		{
			var modAssemblies = Chainloader.PluginInfos.Values
				.Select(pluginInfo => pluginInfo.Instance.GetType().Assembly).Distinct();
            var nametagDefinitions = modAssemblies.SelectMany(assembly => assembly.GetTypes())
				.Where(type => typeof(INametag).IsAssignableFrom(type) && type.IsClass && !type.IsInterface);

            return new List<Type>(nametagDefinitions);
		}

		private static bool GetNametagsPluginFromINametag(INametag nametag, out BingusNametagsPlugin pluginData)
		{
			pluginData = nametag.GetType().GetCustomAttribute<BingusNametagsPlugin>();
			return pluginData != null;
		}

		// Plugin management
		private static void SetupPlugin(INametag nametag)
		{
			if (!GetNametagsPluginFromINametag(nametag, out var pluginData))
				Debug.LogError($"Error in {nametag.GetType().Name}: No BingusNametagsPlugin attribute discovered.");
			else
				Debug.Log($"Loading plugin {pluginData.Name}");
			
			All.Add(nametag);
			MetadataDictionary.Add(nametag, pluginData);

			Main.UpdateTags += delegate
			{
				if (!GorillaParent.hasInstance)
					return;
				
				foreach (var rigPair in pluginData.Tags.Where(rigPair => !GorillaParent.instance.vrrigs.Contains(rigPair.Key) && !nametag.Enabled))
				{
					rigPair.Value.Destroy();
					pluginData.Tags.Remove(rigPair.Key);
				}

				foreach (var rig in GorillaParent.instance.vrrigs.Where(rig => rig != GorillaTagger.Instance.offlineVRRig))
				{
					if (!pluginData.Tags.ContainsKey(rig))
						pluginData.Tags[rig] = NametagCreator.CreateTag(rig, pluginData.Offset, "");

					var component = pluginData.Tags[rig].GetComponent<TextMeshPro>();

					component.text = $"<color={Configuration.AccentColor}>{nametag.Update(rig)}</color>";

					var transform = rig.transform.Find("Head") ?? rig.transform;
					pluginData.Tags[rig].transform.position = transform.position + new Vector3(0f, pluginData.Offset, 0f);

					if (Camera.main == null)
						return;
					
					Vector3 forward = Camera.main.transform.forward;
					forward.y = 0f;
					forward.Normalize();
					pluginData.Tags[rig].transform.rotation = Quaternion.LookRotation(forward);
				}
			};
		}

		internal static void PluginStart()
		{
			foreach (var nametag in GetNametagInterfaces())
				SetupPlugin(Activator.CreateInstance(nametag) as INametag);
		}
	}
}