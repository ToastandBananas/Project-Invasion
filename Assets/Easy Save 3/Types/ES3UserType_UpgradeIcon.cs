using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("upgradeUnlocked")]
	public class ES3UserType_UpgradeIcon : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_UpgradeIcon() : base(typeof(UpgradeIcon)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UpgradeIcon)obj;
			
			writer.WriteProperty("upgradeUnlocked", instance.upgradeUnlocked, ES3Type_bool.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UpgradeIcon)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "upgradeUnlocked":
						instance.upgradeUnlocked = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_UpgradeIconArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_UpgradeIconArray() : base(typeof(UpgradeIcon[]), ES3UserType_UpgradeIcon.Instance)
		{
			Instance = this;
		}
	}
}