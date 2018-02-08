// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AssistidCollector2.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

        private const string SettingsToken = "settings_token";
        private const string SettingsName = "settings_name";
        private const string SettingsId = "settings_id";

        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsToken, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsToken, value);
            }
        }

        public static string AppName
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsName, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsName, value);
            }
        }

        public static string AppId
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsId, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsId, value);
            }
        }
	}
}