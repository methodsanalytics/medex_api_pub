namespace MedicalExaminer.Common.ConnectionSettings
{
    /// <summary>
    /// Connection Settings Extension Methods.
    /// </summary>
    public static class ConnectionSettingsExtensionMethods
    {
        /// <summary>
        /// To Audit Settings.
        /// </summary>
        /// <param name="connectionSetting">Connection Settings.</param>
        /// <returns>Audit Connection Settings.</returns>
        public static IConnectionSettings ToAuditSettings(this IConnectionSettings connectionSetting)
        {
            return new AuditConnectionSetting(
                connectionSetting.EndPointUri,
                connectionSetting.PrimaryKey,
                connectionSetting.DatabaseId,
                connectionSetting.Collection);
        }
    }
}
