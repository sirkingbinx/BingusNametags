using System;
using System.Collections.Generic;
using System.Linq;

namespace BingusNametags
{
	public static class Extensions
	{
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Func<T, T> action)
		{
			var all = new List<T>();
			
			foreach (var element in source)
				all.Add(action(element));

			return all;
		}
	}
}