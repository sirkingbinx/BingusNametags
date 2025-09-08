using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace BingusNametags.Plugins
{
    internal class PluginSupport
    {
        internal static bool PluginsEnabled = true;
        internal static Dictionary<string, Type> Plugins = new List<Type>();

        public static void Register<T>()
        {
            if (!PluginsEnabled) return;

            Type type = typeof(T);

            if (Plugins.TryGetValue(type.FullName, out Type _)) {
                Debug.LogWarning($"BingusNametagsPlugin: Already loaded {type.FullName}, ignoring")
                return;
            } else {
                Debug.Log($"BingusNametagsPlugin: Loading nametag plugin: {type.FullName}");

                MethodInfo tryUpdate = type.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance);

                if (tryUpdate == null)
                    Debug.LogError($"BingusNametagsPlugin: Error - TYPE: ${type.FullName}\nNo update method found, try adding a public void Update().");
                else
                    Main.UpdateTags += delegate { tryUpdate.Invoke(type, null); };

                Plugins.Add(type.FullName, type)
                Debug.Log($"BingusNametagsPlugin: Loaded nametag plugin: ${type.FullName}");
            }
        }
    }
}
