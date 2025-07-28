using Tmds.DBus;
namespace sys_stats
{
    public class SysStatsService : ISysStatsService
    {
        public ObjectPath ObjectPath => "/org/waveOS/SysStats";

        public Task<string> GetBluetoothActiveDeviceNameAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetActiveDeviceName());
        }

        public Task<string> GetBluetoothActiveDeviceTypeAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetActiveDeviceType());
        }

        public Task<string> GetBluetoothAdapterMacAddressAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetAdapterMacAddress());
        }

        public Task<string> GetBluetoothAdapterNameAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetAdapterName());
        }

        public Task<string> GetBluetoothAdapterSerialAsync()
        {
            return Task.FromResult(stats.Bluetooth_Restricted.GetAdapterSerial());
        }

        public Task<string> GetBluetoothAllDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.Bluetooth.GetAllDetails()));
        }

        public Task<string> GetBluetoothAllProtectedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.Bluetooth_Protected.GetAllProtectedDetails()));
        }

        public Task<string> GetBluetoothAllRestrictedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.Bluetooth_Restricted.GetAllRestrictedDetails()));
        }

        public Task<string> GetBluetoothAudioCodecAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetAudioCodec());
        }

        public Task<string> GetBluetoothAudioProfileAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetAudioProfile());
        }

        public Task<int> GetBluetoothBatteryLevelAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetBatteryLevel());
        }

        public Task<string[]> GetBluetoothBlockedDevicesAsync()
        {
            return Task.FromResult(stats.Bluetooth_Restricted.GetBlockedDevices().ToArray());
        }

        public Task<int> GetBluetoothConnectedDeviceCountAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetConnectedDeviceCount());
        }

        public Task<string[]> GetBluetoothConnectionLogAsync()
        {
            return Task.FromResult(stats.Bluetooth_Restricted.GetConnectionLog().ToArray());
        }

        public Task<string> GetBluetoothConnectionStatusAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetConnectionStatus());
        }

        public Task<string> GetBluetoothControllerFirmwareAsync()
        {
            return Task.FromResult(stats.Bluetooth_Restricted.GetControllerFirmware());
        }

        public Task<string> GetBluetoothCountryCodeAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetCountryCode());
        }

        public Task<long> GetBluetoothDataTransferredAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetDataTransferred());
        }

        public Task<(string[], string[])> GetBluetoothDeviceAddressesAsync()
        {
            var addresses = stats.Bluetooth_Protected.GetDeviceAddresses();
            return Task.FromResult((addresses.Keys.ToArray(), addresses.Values.ToArray()));
        }

        public Task<(string[], string[])> GetBluetoothDeviceFingerprintsAsync()
        {
            var fingerprints = stats.Bluetooth_Restricted.GetDeviceFingerprints();
            return Task.FromResult((fingerprints.Keys.ToArray(), fingerprints.Values.ToArray()));
        }

        public Task<string[]> GetBluetoothEncryptionKeysAsync()
        {
            return Task.FromResult(stats.Bluetooth_Restricted.GetEncryptionKeys().ToArray());
        }

        public Task<bool> GetBluetoothIsConnectedAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetIsConnected());
        }

        public Task<bool> GetBluetoothIsDiscoverableAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetIsDiscoverable());
        }

        public Task<bool> GetBluetoothIsEnabledAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetIsEnabled());
        }

        public Task<string[]> GetBluetoothPairedDevicesAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetPairedDevices().ToArray());
        }

        public Task<string[]> GetBluetoothServiceUUIDsAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetServiceUUIDs().ToArray());
        }

        public Task<int> GetBluetoothSignalStrengthAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetSignalStrength());
        }

        public Task<(string[], string[])> GetBluetoothStoredKeysAsync()
        {
            var keys = stats.Bluetooth_Restricted.GetStoredKeys();
            return Task.FromResult((keys.Keys.ToArray(), keys.Values.ToArray()));
        }

        public Task<string[]> GetBluetoothTrustedDevicesAsync()
        {
            return Task.FromResult(stats.Bluetooth_Protected.GetTrustedDevices().ToArray());
        }

        public Task<string> GetBluetoothVersionAsync()
        {
            return Task.FromResult(stats.Bluetooth.GetVersion());
        }

        public Task<string[]> GetUSBAccessHistoryAsync()
        {
            return Task.FromResult(stats.USB_Restricted.GetAccessHistory().ToArray());
        }

        public Task<string> GetUSBActiveDeviceNameAsync()
        {
            return Task.FromResult(stats.USB.GetActiveDeviceName());
        }

        public Task<string> GetUSBActiveDeviceTypeAsync()
        {
            return Task.FromResult(stats.USB.GetActiveDeviceType());
        }

        public Task<string> GetUSBAllDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.USB.GetAllDetails()));
        }

        public Task<string> GetUSBAllProtectedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.USB_Protected.GetAllProtectedDetails()));
        }

        public Task<string> GetUSBAllRestrictedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.USB_Restricted.GetAllRestrictedDetails()));
        }

        public Task<int> GetUSBConnectedDeviceCountAsync()
        {
            return Task.FromResult(stats.USB.GetConnectedDeviceCount());
        }

        public Task<string> GetUSBControllerNameAsync()
        {
            return Task.FromResult(stats.USB.GetControllerName());
        }

        public Task<string[]> GetUSBDeviceClassesAsync()
        {
            return Task.FromResult(stats.USB_Protected.GetDeviceClasses().ToArray());
        }

        public Task<(string[], string[])> GetUSBDeviceKeysAsync()
        {
            var keys = stats.USB_Restricted.GetDeviceKeys();
            return Task.FromResult((keys.Keys.ToArray(), keys.Values.ToArray()));
        }

        public Task<string[]> GetUSBDeviceNamesAsync()
        {
            return Task.FromResult(stats.USB.GetDeviceNames().ToArray());
        }

        public Task<(string[], string[])> GetUSBDevicePathsAsync()
        {
            var paths = stats.USB_Protected.GetDevicePaths();
            return Task.FromResult((paths.Keys.ToArray(), paths.Values.ToArray()));
        }

        public Task<(string[], string[])> GetUSBDeviceSecretsAsync()
        {
            var secrets = stats.USB_Restricted.GetDeviceSecrets();
            return Task.FromResult((secrets.Keys.ToArray(), secrets.Values.ToArray()));
        }

        public Task<string[]> GetUSBDeviceSerialNumbersAsync()
        {
            return Task.FromResult(stats.USB_Protected.GetDeviceSerialNumbers().ToArray());
        }

        public Task<string[]> GetUSBDeviceTypesAsync()
        {
            return Task.FromResult(stats.USB.GetDeviceTypes().ToArray());
        }

        public Task<(string[], string[])> GetUSBEncryptionKeysAsync()
        {
            var keys = stats.USB_Restricted.GetEncryptionKeys();
            return Task.FromResult((keys.Keys.ToArray(), keys.Values.ToArray()));
        }

        public Task<string[]> GetUSBForensicDataAsync()
        {
            return Task.FromResult(stats.USB_Restricted.GetForensicData().ToArray());
        }

        public Task<bool> GetUSBHasUSB3Async()
        {
            return Task.FromResult(stats.USB.GetHasUSB3());
        }

        public Task<bool> GetUSBHasUSBTypeCAsync()
        {
            return Task.FromResult(stats.USB.GetHasUSBTypeC());
        }

        public Task<bool> GetUSBIsHubConnectedAsync()
        {
            return Task.FromResult(stats.USB.GetIsHubConnected());
        }

        public Task<string[]> GetUSBMountPointsAsync()
        {
            return Task.FromResult(stats.USB_Protected.GetMountPoints().ToArray());
        }

        public Task<int> GetUSBPortCountAsync()
        {
            return Task.FromResult(stats.USB.GetPortCount());
        }

        public Task<int> GetUSBPowerUsageAsync()
        {
            return Task.FromResult(stats.USB.GetPowerUsage());
        }

        public Task<(string[], string[])> GetUSBProductIDsAsync()
        {
            var ids = stats.USB_Protected.GetProductIDs();
            return Task.FromResult((ids.Keys.ToArray(), ids.Values.ToArray()));
        }

        public Task<string> GetUSBRootHubInfoAsync()
        {
            return Task.FromResult(stats.USB_Restricted.GetRootHubInfo());
        }

        public Task<string[]> GetUSBSecurityDescriptorsAsync()
        {
            return Task.FromResult(stats.USB_Restricted.GetSecurityDescriptors().ToArray());
        }

        public Task<long> GetUSBTotalDataTransferredAsync()
        {
            return Task.FromResult(stats.USB_Protected.GetTotalDataTransferred());
        }

        public Task<double> GetUSBTransferSpeedAsync()
        {
            return Task.FromResult(stats.USB.GetTransferSpeed());
        }

        public Task<(string[], string[])> GetUSBVendorIDsAsync()
        {
            var ids = stats.USB_Protected.GetVendorIDs();
            return Task.FromResult((ids.Keys.ToArray(), ids.Values.ToArray()));
        }

        public Task<string> GetUSBVersionAsync()
        {
            return Task.FromResult(stats.USB.GetUSBVersion());
        }

        public Task<string> GetWiFiAllDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.Wifi.GetAllDetails()));
        }

        public Task<string> GetWiFiAllProtectedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.WiFi_Protected.GetAllProtectedDetails()));
        }

        public Task<string> GetWiFiAllRestrictedDetailsAsync()
        {
            return Task.FromResult(System.Text.Json.JsonSerializer.Serialize(stats.WiFi_Restricted.GetAllRestrictedDetails()));
        }

        public Task<bool> GetWiFiAutoConnectAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetAutoConnect());
        }

        public Task<string> GetWiFiBSSIDAsync()
        {
            return Task.FromResult(stats.WiFi_Restricted.GetBSSID());
        }

        public Task<string[]> GetWiFiBSSIDHistoryAsync()
        {
            return Task.FromResult(stats.WiFi_Restricted.GetBSSIDHistory().ToArray());
        }

        public Task<int> GetWiFiChannelAsync()
        {
            return Task.FromResult(stats.Wifi.GetChannel());
        }

        public Task<string> GetWiFiConnectionStatusAsync()
        {
            return Task.FromResult(stats.Wifi.GetConnectionStatus());
        }

        public Task<string> GetWiFiCountryCodeAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetCountryCode());
        }

        public Task<string> GetWiFiDNSServersAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetDNSServers());
        }

        public Task<string> GetWiFiFrequencyAsync()
        {
            return Task.FromResult(stats.Wifi.GetWiFiFrequency());
        }

        public Task<string> GetWiFiGatewayAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetGateway());
        }

        public Task<string[]> GetWiFiGeolocationDataAsync()
        {
            return Task.FromResult(stats.WiFi_Restricted.GetGeolocationData().ToArray());
        }

        public Task<string> GetWiFiIPAddressAsync()
        {
            return Task.FromResult(stats.Wifi.GetIPAddress());
        }

        public Task<bool> GetWiFiIsConnectedAsync()
        {
            return Task.FromResult(stats.Wifi.GetIsConnected());
        }

        public Task<bool> GetWiFiIsMeteredAsync()
        {
            return Task.FromResult(stats.Wifi.GetIsMetered());
        }

        public Task<double> GetWiFiLatencyAsync()
        {
            return Task.FromResult(stats.Wifi.GetLatency());
        }

        public Task<double> GetWiFiLinkSpeedAsync()
        {
            return Task.FromResult(stats.Wifi.GetLinkSpeed());
        }

        public Task<string> GetWiFiMacAddressAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetMacAddress());
        }

        public Task<string> GetWiFiNetworkAdapterNameAsync()
        {
            return Task.FromResult(stats.Wifi.GetNetworkAdapterName());
        }

        public Task<string> GetWiFiNetworkVendorAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetNetworkVendor());
        }

        public Task<string> GetWiFiPasswordAsync()
        {
            return Task.FromResult(stats.WiFi_Restricted.GetPassword());
        }

        public Task<string> GetWiFiProtocolAsync()
        {
            return Task.FromResult(stats.Wifi.GetProtocol());
        }

        public Task<string[]> GetWiFiSavedNetworkNamesAsync()
        {
            return Task.FromResult(stats.WiFi_Protected.GetSavedNetworkNames().ToArray());
        }

        public Task<string> GetWiFiSecurityTypeAsync()
        {
            return Task.FromResult(stats.Wifi.GetSecurityType());
        }

        public Task<int> GetWiFiSignalStrengthAsync()
        {
            return Task.FromResult(stats.Wifi.GetSignalStrength());
        }

        public Task<string> GetWiFiSSIDAsync()
        {
            return Task.FromResult(stats.Wifi.GetSSID());
        }

        public Task<(string[], string[])> GetWiFiStoredPasswordsAsync()
        {
            var passwords = stats.WiFi_Restricted.GetStoredPasswords();
            return Task.FromResult((passwords.Keys.ToArray(), passwords.Values.ToArray()));
        }

        public Task RefreshAllDataAsync()
        {
            stats.Wifi.RefreshPublicDetails();
            stats.WiFi_Protected.RefreshProtectedDetails();
            stats.WiFi_Restricted.RefreshRestrictedDetails();
            stats.Bluetooth.RefreshPublicDetails();
            stats.Bluetooth_Protected.RefreshProtectedDetails();
            stats.Bluetooth_Restricted.RefreshRestrictedDetails();
            stats.USB.RefreshPublicDetails();
            stats.USB_Protected.RefreshProtectedDetails();
            stats.USB_Restricted.RefreshRestrictedDetails();
            return Task.CompletedTask;
        }

        public Task RefreshBluetoothDataAsync()
        {
            stats.Bluetooth.RefreshPublicDetails();
            stats.Bluetooth_Protected.RefreshProtectedDetails();
            stats.Bluetooth_Restricted.RefreshRestrictedDetails();
            return Task.CompletedTask;
        }

        public Task RefreshUSBDataAsync()
        {
            stats.USB.RefreshPublicDetails();
            stats.USB_Protected.RefreshProtectedDetails();
            stats.USB_Restricted.RefreshRestrictedDetails();
            return Task.CompletedTask;
        }

        public Task RefreshWiFiDataAsync()
        {
            stats.Wifi.RefreshPublicDetails();
            stats.WiFi_Protected.RefreshProtectedDetails();
            stats.WiFi_Restricted.RefreshRestrictedDetails();
            return Task.CompletedTask;
        }
    }
}