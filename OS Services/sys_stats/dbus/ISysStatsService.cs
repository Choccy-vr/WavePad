using Tmds.DBus;
namespace sys_stats
{
    [DBusInterface("org.waveOS.SysStats")]
    public interface ISysStatsService : IDBusObject
    {
        #region WiFi Public Methods
        Task<string> GetWiFiSSIDAsync();
        Task<int> GetWiFiSignalStrengthAsync();
        Task<string> GetWiFiIPAddressAsync();
        Task<string> GetWiFiFrequencyAsync();
        Task<bool> GetWiFiIsConnectedAsync();
        Task<string> GetWiFiProtocolAsync();
        Task<double> GetWiFiLinkSpeedAsync();
        Task<string> GetWiFiSecurityTypeAsync();
        Task<string> GetWiFiConnectionStatusAsync();
        Task<string> GetWiFiNetworkAdapterNameAsync();
        Task<int> GetWiFiChannelAsync();
        Task<double> GetWiFiLatencyAsync();
        Task<bool> GetWiFiIsMeteredAsync();
        #endregion

        #region WiFi Protected Methods
        Task<string> GetWiFiMacAddressAsync();
        Task<string> GetWiFiGatewayAsync();
        Task<string> GetWiFiDNSServersAsync();
        Task<bool> GetWiFiAutoConnectAsync();
        Task<string[]> GetWiFiSavedNetworkNamesAsync();
        Task<string> GetWiFiNetworkVendorAsync();
        Task<string> GetWiFiCountryCodeAsync();
        #endregion

        #region WiFi Restricted Methods
        Task<string> GetWiFiPasswordAsync();
        Task<string> GetWiFiBSSIDAsync();
        Task<(string[], string[])> GetWiFiStoredPasswordsAsync();
        Task<string[]> GetWiFiBSSIDHistoryAsync();
        Task<string[]> GetWiFiGeolocationDataAsync();
        #endregion

        #region Bluetooth Public Methods
        Task<bool> GetBluetoothIsEnabledAsync();
        Task<bool> GetBluetoothIsConnectedAsync();
        Task<string> GetBluetoothAdapterNameAsync();
        Task<string> GetBluetoothVersionAsync();
        Task<bool> GetBluetoothIsDiscoverableAsync();
        Task<int> GetBluetoothConnectedDeviceCountAsync();
        Task<string> GetBluetoothActiveDeviceNameAsync();
        Task<string> GetBluetoothActiveDeviceTypeAsync();
        Task<int> GetBluetoothSignalStrengthAsync();
        Task<int> GetBluetoothBatteryLevelAsync();
        Task<string> GetBluetoothConnectionStatusAsync();
        Task<string> GetBluetoothAudioCodecAsync();
        Task<string> GetBluetoothAudioProfileAsync();
        #endregion

        #region Bluetooth Protected Methods
        Task<string> GetBluetoothAdapterMacAddressAsync();
        Task<string[]> GetBluetoothPairedDevicesAsync();
        Task<string[]> GetBluetoothTrustedDevicesAsync();
        Task<(string[], string[])> GetBluetoothDeviceAddressesAsync();
        Task<string[]> GetBluetoothServiceUUIDsAsync();
        Task<string> GetBluetoothCountryCodeAsync();
        Task<long> GetBluetoothDataTransferredAsync();
        #endregion

        #region Bluetooth Restricted Methods
        Task<(string[], string[])> GetBluetoothStoredKeysAsync();
        Task<string[]> GetBluetoothEncryptionKeysAsync();
        Task<string> GetBluetoothAdapterSerialAsync();
        Task<(string[], string[])> GetBluetoothDeviceFingerprintsAsync();
        Task<string[]> GetBluetoothConnectionLogAsync();
        Task<string[]> GetBluetoothBlockedDevicesAsync();
        Task<string> GetBluetoothControllerFirmwareAsync();
        #endregion

        #region USB Public Methods
        Task<int> GetUSBConnectedDeviceCountAsync();
        Task<string[]> GetUSBDeviceNamesAsync();
        Task<string[]> GetUSBDeviceTypesAsync();
        Task<string> GetUSBActiveDeviceNameAsync();
        Task<string> GetUSBActiveDeviceTypeAsync();
        Task<string> GetUSBVersionAsync();
        Task<double> GetUSBTransferSpeedAsync();
        Task<int> GetUSBPortCountAsync();
        Task<bool> GetUSBHasUSB3Async();
        Task<bool> GetUSBHasUSBTypeCAsync();
        Task<string> GetUSBControllerNameAsync();
        Task<bool> GetUSBIsHubConnectedAsync();
        Task<int> GetUSBPowerUsageAsync();
        #endregion

        #region USB Protected Methods
        Task<string[]> GetUSBDeviceSerialNumbersAsync();
        Task<(string[], string[])> GetUSBVendorIDsAsync();
        Task<(string[], string[])> GetUSBProductIDsAsync();
        Task<string[]> GetUSBDeviceClassesAsync();
        Task<(string[], string[])> GetUSBDevicePathsAsync();
        Task<string[]> GetUSBMountPointsAsync();
        Task<long> GetUSBTotalDataTransferredAsync();
        #endregion

        #region USB Restricted Methods
        Task<(string[], string[])> GetUSBDeviceKeysAsync();
        Task<string[]> GetUSBAccessHistoryAsync();
        Task<(string[], string[])> GetUSBDeviceSecretsAsync();
        Task<string[]> GetUSBSecurityDescriptorsAsync();
        Task<(string[], string[])> GetUSBEncryptionKeysAsync();
        Task<string[]> GetUSBForensicDataAsync();
        Task<string> GetUSBRootHubInfoAsync();
        #endregion

        #region Bulk Data Methods
        Task<string> GetWiFiAllDetailsAsync();
        Task<string> GetWiFiAllProtectedDetailsAsync();
        Task<string> GetWiFiAllRestrictedDetailsAsync();
        Task<string> GetBluetoothAllDetailsAsync();
        Task<string> GetBluetoothAllProtectedDetailsAsync();
        Task<string> GetBluetoothAllRestrictedDetailsAsync();
        Task<string> GetUSBAllDetailsAsync();
        Task<string> GetUSBAllProtectedDetailsAsync();
        Task<string> GetUSBAllRestrictedDetailsAsync();
        #endregion

        #region System Refresh Methods
        Task RefreshWiFiDataAsync();
        Task RefreshBluetoothDataAsync();
        Task RefreshUSBDataAsync();
        Task RefreshAllDataAsync();
        #endregion
    }
}