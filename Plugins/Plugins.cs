using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using HarmonyLib;
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
		
		private static BingusNametagsPlugin GetNametagsPluginFromINametag(INametag nametag) => nametag.GetType().GetCustomAttribute<BingusNametagsPlugin>();

		// Plugin management
		private static void SetupPlugin(INametag nametag)
		{
			var pluginData = GetNametagsPluginFromINametag(nametag);
			if (pluginData == null)
				Debug.LogError($"Error in {nametag.GetType().Name}: No BingusNametagsPlugin attribute discovered.");
			
			All.Add(nametag);
			MetadataDictionary.Add(nametag, pluginData);

			Main.UpdateTags += delegate
			{
				if (GorillaParent.hasInstance && pluginData != null)
				{
					List<VRRig> list = new List<VRRig>();

					foreach (KeyValuePair<VRRig, GameObject> keyValuePair in pluginData.Tags)
					{
						if (!GorillaParent.instance.vrrigs.Contains(keyValuePair.Key) || !nametag.Enabled)
						{
							keyValuePair.Value.Destroy();
							list.Add(keyValuePair.Key);
						}
					}

					foreach (var key in list)
						pluginData.Tags.Remove(key);

					foreach (var rig in GorillaParent.instance.vrrigs)
					{
						if (rig != GorillaTagger.Instance.offlineVRRig)
						{
							if (!pluginData.Tags.ContainsKey(rig))
								pluginData.Tags[rig] = NametagCreator.CreateTag(rig, pluginData.Offset, "");

							TextMeshPro component = pluginData.Tags[rig].GetComponent<TextMeshPro>();

							component.text = $"<color={Configuration.AccentColor}>{nametag.Update(rig)}</color>";

							Transform transform = rig.transform.Find("Head") ?? rig.transform;
							pluginData.Tags[rig].transform.position = transform.position + new Vector3(0f, pluginData.Offset, 0f);

							if (Camera.main == null)
								return;
							
							Vector3 forward = Camera.main.transform.forward;
							forward.y = 0f;
							forward.Normalize();
							pluginData.Tags[rig].transform.rotation = Quaternion.LookRotation(forward);
						}
					}

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