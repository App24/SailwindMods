using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace TweaksAndFixes
{
    public static class SaveFileHelper
    {
		public static T Load<T>(this string modName) where T : new()
		{
			string s;
			if (GameState.modData != null && GameState.modData.TryGetValue(modName, out s))
			{
				Debug.Log("Proceeding to parse save data for " + modName);
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				using (StringReader stringReader = new StringReader(s))
				{
					return (T)((object)xmlSerializer.Deserialize(stringReader));
				}
			}
			Debug.Log("Cannot load data from save file. Using defaults for " + modName);
			return Activator.CreateInstance<T>();
		}

		public static void Save<T>(this T toSerialize, string modName)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			using (StringWriter stringWriter = new StringWriter())
			{
				xmlSerializer.Serialize(stringWriter, toSerialize);
				GameState.modData[modName] = stringWriter.ToString();
				Debug.Log("Packed save data for " + modName);
			}
		}
	}
}
