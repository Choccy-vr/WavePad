using System.Diagnostics;
using System.Text.RegularExpressions;

namespace sys_stats.stats
{
    #region Public
    public class Bluetooth_Details
    {
        public static bool IsEnabled { get; set; } = false;
        public static bool IsConnected { get; set; } = false;
        public static string AdapterName { get; set; } = string.Empty;
        public static string Version { get; set; } = string.Empty;
        public static bool IsDiscoverable { get; set; } = false;
        public static int ConnectedDeviceCount { get; set; } = 0;
        public static string ActiveDeviceName { get; set; } = string.Empty;
        public static string ActiveDeviceType { get; set; } = string.Empty;
        public static int SignalStrength { get; set; } = 0;
        public static int BatteryLevel { get; set; } = -1;
        public static string ConnectionStatus { get; set; } = string.Empty;
        public static string AudioCodec { get; set; } = string.Empty;
        public static string AudioProfile { get; set; } = string.Empty;
    }

    public class Bluetooth()
    {
        public static Bluetooth_Details Bluetooth_Details { get; set; } = new Bluetooth_Details();
        
        public static void RefreshPublicDetails()
        {
            UpdateIsEnabled();
            UpdateIsConnected();
            UpdateAdapterName();
            UpdateVersion();
            UpdateIsDiscoverable();
            UpdateConnectedDeviceCount();
            UpdateActiveDeviceName();
            UpdateActiveDeviceType();
            UpdateSignalStrength();
            UpdateBatteryLevel();
            UpdateConnectionStatus();
            UpdateAudioCodec();
            UpdateAudioProfile();
        }

        #region Get Methods
        public static Bluetooth_Details GetAllDetails()
        {
            RefreshPublicDetails();
            return Bluetooth_Details;
        }

        public static bool GetIsEnabled()
        {
            return Bluetooth_Details.IsEnabled;
        }

        public static bool GetIsConnected()
        {
            return Bluetooth_Details.IsConnected;
        }

        public static string GetAdapterName()
        {
            return Bluetooth_Details.AdapterName;
        }

        public static string GetVersion()
        {
            return Bluetooth_Details.Version;
        }

        public static bool GetIsDiscoverable()
        {
            return Bluetooth_Details.IsDiscoverable;
        }

        public static int GetConnectedDeviceCount()
        {
            return Bluetooth_Details.ConnectedDeviceCount;
        }

        public static string GetActiveDeviceName()
        {
            return Bluetooth_Details.ActiveDeviceName;
        }

        public static string GetActiveDeviceType()
        {
            return Bluetooth_Details.ActiveDeviceType;
        }

        public static int GetSignalStrength()
        {
            return Bluetooth_Details.SignalStrength;
        }

        public static int GetBatteryLevel()
        {
            return Bluetooth_Details.BatteryLevel;
        }

        public static string GetConnectionStatus()
        {
            return Bluetooth_Details.ConnectionStatus;
        }

        public static string GetAudioCodec()
        {
            return Bluetooth_Details.AudioCodec;
        }

        public static string GetAudioProfile()
        {
            return Bluetooth_Details.AudioProfile;
        }
        #endregion

        #region Update Methods
        public static void UpdateIsEnabled()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "rfkill",
                    Arguments = "list bluetooth",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Bluetooth_Details.IsEnabled = !output.Contains("Soft blocked: yes") && !output.Contains("Hard blocked: yes");
                    return;
                }
            }
            catch { }

            Bluetooth_Details.IsEnabled = false;
        }

        public static void UpdateIsConnected()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "show",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Bluetooth_Details.IsConnected = output.Contains("Powered: yes") && Bluetooth_Details.ConnectedDeviceCount > 0;
                    return;
                }
            }
            catch { }

            Bluetooth_Details.IsConnected = false;
        }

        public static void UpdateAdapterName()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "hciconfig",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        Bluetooth_Details.AdapterName = lines[0].Split(':')[0].Trim();
                        return;
                    }
                }
            }
            catch { }

            Bluetooth_Details.AdapterName = "Unknown";
        }

        public static void UpdateVersion()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "hciconfig",
                    Arguments = "version",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = Regex.Match(output, @"HCI Version: (\d+\.\d+)");
                    if (match.Success)
                    {
                        Bluetooth_Details.Version = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            Bluetooth_Details.Version = "Unknown";
        }

        public static void UpdateIsDiscoverable()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "show",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    Bluetooth_Details.IsDiscoverable = output.Contains("Discoverable: yes");
                    return;
                }
            }
            catch { }

            Bluetooth_Details.IsDiscoverable = false;
        }

        public static void UpdateConnectedDeviceCount()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "devices Connected",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    Bluetooth_Details.ConnectedDeviceCount = lines.Length;
                    return;
                }
            }
            catch { }

            Bluetooth_Details.ConnectedDeviceCount = 0;
        }

        public static void UpdateActiveDeviceName()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "devices Connected",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    if (lines.Length > 0)
                    {
                        var match = Regex.Match(lines[0], @"Device [A-F0-9:]{17} (.+)");
                        if (match.Success)
                        {
                            Bluetooth_Details.ActiveDeviceName = match.Groups[1].Value.Trim();
                            return;
                        }
                    }
                }
            }
            catch { }

            Bluetooth_Details.ActiveDeviceName = string.Empty;
        }

        public static void UpdateActiveDeviceType()
        {
            if (!string.IsNullOrEmpty(Bluetooth_Details.ActiveDeviceName))
            {
                try
                {
                    var deviceAddress = GetDeviceAddress(Bluetooth_Details.ActiveDeviceName);
                    if (!string.IsNullOrEmpty(deviceAddress))
                    {
                        var process = Process.Start(new ProcessStartInfo
                        {
                            FileName = "bluetoothctl",
                            Arguments = $"info {deviceAddress}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });

                        if (process != null)
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit();

                            if (output.Contains("audio-headphones") || output.Contains("audio-headset"))
                                Bluetooth_Details.ActiveDeviceType = "Audio";
                            else if (output.Contains("input-mouse"))
                                Bluetooth_Details.ActiveDeviceType = "Mouse";
                            else if (output.Contains("input-keyboard"))
                                Bluetooth_Details.ActiveDeviceType = "Keyboard";
                            else if (output.Contains("phone"))
                                Bluetooth_Details.ActiveDeviceType = "Phone";
                            else
                                Bluetooth_Details.ActiveDeviceType = "Unknown";
                            return;
                        }
                    }
                }
                catch { }
            }

            Bluetooth_Details.ActiveDeviceType = string.Empty;
        }

        public static void UpdateSignalStrength()
        {
            if (!string.IsNullOrEmpty(Bluetooth_Details.ActiveDeviceName))
            {
                try
                {
                    var deviceAddress = GetDeviceAddress(Bluetooth_Details.ActiveDeviceName);
                    if (!string.IsNullOrEmpty(deviceAddress))
                    {
                        var process = Process.Start(new ProcessStartInfo
                        {
                            FileName = "bluetoothctl",
                            Arguments = $"info {deviceAddress}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });

                        if (process != null)
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit();

                            var match = Regex.Match(output, @"RSSI: (-?\d+)");
                            if (match.Success)
                            {
                                Bluetooth_Details.SignalStrength = int.Parse(match.Groups[1].Value);
                                return;
                            }
                        }
                    }
                }
                catch { }
            }

            Bluetooth_Details.SignalStrength = -70;
        }

        public static void UpdateBatteryLevel()
        {
            if (!string.IsNullOrEmpty(Bluetooth_Details.ActiveDeviceName))
            {
                try
                {
                    var deviceAddress = GetDeviceAddress(Bluetooth_Details.ActiveDeviceName);
                    if (!string.IsNullOrEmpty(deviceAddress))
                    {
                        var process = Process.Start(new ProcessStartInfo
                        {
                            FileName = "bluetoothctl",
                            Arguments = $"info {deviceAddress}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });

                        if (process != null)
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit();

                            var match = Regex.Match(output, @"Battery Percentage: \(0x\w+\) (\d+)");
                            if (match.Success)
                            {
                                Bluetooth_Details.BatteryLevel = int.Parse(match.Groups[1].Value);
                                return;
                            }
                        }
                    }
                }
                catch { }
            }

            Bluetooth_Details.BatteryLevel = -1;
        }

        public static void UpdateConnectionStatus()
        {
            if (Bluetooth_Details.IsEnabled)
            {
                if (Bluetooth_Details.ConnectedDeviceCount > 0)
                    Bluetooth_Details.ConnectionStatus = "Connected";
                else
                    Bluetooth_Details.ConnectionStatus = "Disconnected";
            }
            else
            {
                Bluetooth_Details.ConnectionStatus = "Disabled";
            }
        }

        public static void UpdateAudioCodec()
        {
            if (Bluetooth_Details.ActiveDeviceType == "Audio")
            {
                Bluetooth_Details.AudioCodec = "SBC";
            }
            else
            {
                Bluetooth_Details.AudioCodec = string.Empty;
            }
        }

        public static void UpdateAudioProfile()
        {
            if (Bluetooth_Details.ActiveDeviceType == "Audio")
            {
                Bluetooth_Details.AudioProfile = "A2DP";
            }
            else
            {
                Bluetooth_Details.AudioProfile = string.Empty;
            }
        }

        private static string GetDeviceAddress(string deviceName)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.Contains(deviceName))
                        {
                            var match = Regex.Match(line, @"Device ([A-F0-9:]{17})");
                            if (match.Success)
                                return match.Groups[1].Value;
                        }
                    }
                }
            }
            catch { }
            return string.Empty;
        }
        #endregion
    }
    #endregion

    #region Protected
    public class Bluetooth_Details_Protected
    {
        public static string AdapterMacAddress { get; set; } = string.Empty;
        public static List<string> PairedDevices { get; set; } = new();
        public static List<string> TrustedDevices { get; set; } = new();
        public static Dictionary<string, string> DeviceAddresses { get; set; } = new();
        public static List<string> ServiceUUIDs { get; set; } = new();
        public static string CountryCode { get; set; } = string.Empty;
        public static long DataTransferred { get; set; } = 0;
    }

    public class Bluetooth_Protected()
    {
        public static Bluetooth_Details_Protected Bluetooth_Details_Protected { get; set; } = new Bluetooth_Details_Protected();

        public static void RefreshProtectedDetails()
        {
            UpdateAdapterMacAddress();
            UpdatePairedDevices();
            UpdateTrustedDevices();
            UpdateDeviceAddresses();
            UpdateServiceUUIDs();
            UpdateCountryCode();
            UpdateDataTransferred();
        }

        #region Get Methods
        public static Bluetooth_Details_Protected GetAllProtectedDetails()
        {
            RefreshProtectedDetails();
            return Bluetooth_Details_Protected;
        }

        public static string GetAdapterMacAddress()
        {
            return Bluetooth_Details_Protected.AdapterMacAddress;
        }

        public static List<string> GetPairedDevices()
        {
            return Bluetooth_Details_Protected.PairedDevices;
        }

        public static List<string> GetTrustedDevices()
        {
            return Bluetooth_Details_Protected.TrustedDevices;
        }

        public static Dictionary<string, string> GetDeviceAddresses()
        {
            return Bluetooth_Details_Protected.DeviceAddresses;
        }

        public static List<string> GetServiceUUIDs()
        {
            return Bluetooth_Details_Protected.ServiceUUIDs;
        }

        public static string GetCountryCode()
        {
            return Bluetooth_Details_Protected.CountryCode;
        }

        public static long GetDataTransferred()
        {
            return Bluetooth_Details_Protected.DataTransferred;
        }
        #endregion

        #region Update Methods
        public static void UpdateAdapterMacAddress()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "hciconfig",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = Regex.Match(output, @"BD Address: ([A-F0-9:]{17})");
                    if (match.Success)
                    {
                        Bluetooth_Details_Protected.AdapterMacAddress = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            Bluetooth_Details_Protected.AdapterMacAddress = string.Empty;
        }

        public static void UpdatePairedDevices()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "paired-devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var devices = new List<string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"Device [A-F0-9:]{17} (.+)");
                        if (match.Success)
                        {
                            devices.Add(match.Groups[1].Value.Trim());
                        }
                    }
                    Bluetooth_Details_Protected.PairedDevices = devices;
                    return;
                }
            }
            catch { }

            Bluetooth_Details_Protected.PairedDevices = new List<string>();
        }

        public static void UpdateTrustedDevices()
        {
            try
            {
                var trustedDevices = new List<string>();
                foreach (var device in Bluetooth_Details_Protected.PairedDevices)
                {
                    var deviceAddress = GetDeviceAddress(device);
                    if (!string.IsNullOrEmpty(deviceAddress))
                    {
                        var process = Process.Start(new ProcessStartInfo
                        {
                            FileName = "bluetoothctl",
                            Arguments = $"info {deviceAddress}",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        });

                        if (process != null)
                        {
                            string output = process.StandardOutput.ReadToEnd();
                            process.WaitForExit();

                            if (output.Contains("Trusted: yes"))
                            {
                                trustedDevices.Add(device);
                            }
                        }
                    }
                }
                Bluetooth_Details_Protected.TrustedDevices = trustedDevices;
            }
            catch
            {
                Bluetooth_Details_Protected.TrustedDevices = new List<string>();
            }
        }

        public static void UpdateDeviceAddresses()
        {
            try
            {
                var deviceAddresses = new Dictionary<string, string>();
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"Device ([A-F0-9:]{17}) (.+)");
                        if (match.Success)
                        {
                            deviceAddresses[match.Groups[2].Value.Trim()] = match.Groups[1].Value;
                        }
                    }
                }

                Bluetooth_Details_Protected.DeviceAddresses = deviceAddresses;
            }
            catch
            {
                Bluetooth_Details_Protected.DeviceAddresses = new Dictionary<string, string>();
            }
        }

        public static void UpdateServiceUUIDs()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "bluetoothctl",
                    Arguments = "show",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var uuids = new List<string>();
                    var matches = Regex.Matches(output, @"UUID: ([a-f0-9-]+)");
                    foreach (Match match in matches)
                    {
                        uuids.Add(match.Groups[1].Value);
                    }

                    Bluetooth_Details_Protected.ServiceUUIDs = uuids;
                    return;
                }
            }
            catch { }

            Bluetooth_Details_Protected.ServiceUUIDs = new List<string>();
        }

        public static void UpdateCountryCode()
        {
            Bluetooth_Details_Protected.CountryCode = "US";
        }

        public static void UpdateDataTransferred()
        {
            Bluetooth_Details_Protected.DataTransferred = 0;
        }

        private static string GetDeviceAddress(string deviceName)
        {
            if (Bluetooth_Details_Protected.DeviceAddresses.ContainsKey(deviceName))
            {
                return Bluetooth_Details_Protected.DeviceAddresses[deviceName];
            }
            return string.Empty;
        }
        #endregion
    }
    #endregion

    #region Restricted
    public class Bluetooth_Details_Restricted
    {
        public static Dictionary<string, string> StoredKeys { get; set; } = new();
        public static List<string> EncryptionKeys { get; set; } = new();
        public static string AdapterSerial { get; set; } = string.Empty;
        public static Dictionary<string, string> DeviceFingerprints { get; set; } = new();
        public static List<string> ConnectionLog { get; set; } = new();
        public static List<string> BlockedDevices { get; set; } = new();
        public static string ControllerFirmware { get; set; } = string.Empty;
    }

    public class Bluetooth_Restricted()
    {
        public static Bluetooth_Details_Restricted Bluetooth_Details_Restricted { get; set; } = new Bluetooth_Details_Restricted();

        public static void RefreshRestrictedDetails()
        {
            UpdateStoredKeys();
            UpdateEncryptionKeys();
            UpdateAdapterSerial();
            UpdateDeviceFingerprints();
            UpdateConnectionLog();
            UpdateBlockedDevices();
            UpdateControllerFirmware();
        }

        #region Get Methods
        public static Bluetooth_Details_Restricted GetAllRestrictedDetails()
        {
            RefreshRestrictedDetails();
            return Bluetooth_Details_Restricted;
        }

        public static Dictionary<string, string> GetStoredKeys()
        {
            return Bluetooth_Details_Restricted.StoredKeys;
        }

        public static List<string> GetEncryptionKeys()
        {
            return Bluetooth_Details_Restricted.EncryptionKeys;
        }

        public static string GetAdapterSerial()
        {
            return Bluetooth_Details_Restricted.AdapterSerial;
        }

        public static Dictionary<string, string> GetDeviceFingerprints()
        {
            return Bluetooth_Details_Restricted.DeviceFingerprints;
        }

        public static List<string> GetConnectionLog()
        {
            return Bluetooth_Details_Restricted.ConnectionLog;
        }

        public static List<string> GetBlockedDevices()
        {
            return Bluetooth_Details_Restricted.BlockedDevices;
        }

        public static string GetControllerFirmware()
        {
            return Bluetooth_Details_Restricted.ControllerFirmware;
        }
        #endregion

        #region Update Methods
        public static void UpdateStoredKeys()
        {
            try
            {
                var keys = new Dictionary<string, string>();
                var bluetoothDir = "/var/lib/bluetooth";
                if (Directory.Exists(bluetoothDir))
                {
                    var adapterDirs = Directory.GetDirectories(bluetoothDir);
                    foreach (var adapterDir in adapterDirs)
                    {
                        var deviceDirs = Directory.GetDirectories(adapterDir);
                        foreach (var deviceDir in deviceDirs)
                        {
                            var deviceAddress = Path.GetFileName(deviceDir);
                            var infoFile = Path.Combine(deviceDir, "info");
                            if (File.Exists(infoFile))
                            {
                                var content = File.ReadAllText(infoFile);
                                var match = Regex.Match(content, @"Key=([A-F0-9]+)");
                                if (match.Success)
                                {
                                    keys[deviceAddress] = match.Groups[1].Value;
                                }
                            }
                        }
                    }
                }
                Bluetooth_Details_Restricted.StoredKeys = keys;
            }
            catch
            {
                Bluetooth_Details_Restricted.StoredKeys = new Dictionary<string, string>();
            }
        }

        public static void UpdateEncryptionKeys()
        {
            try
            {
                var encryptionKeys = new List<string>();
                foreach (var key in Bluetooth_Details_Restricted.StoredKeys.Values)
                {
                    encryptionKeys.Add(key);
                }
                Bluetooth_Details_Restricted.EncryptionKeys = encryptionKeys;
            }
            catch
            {
                Bluetooth_Details_Restricted.EncryptionKeys = new List<string>();
            }
        }

        public static void UpdateAdapterSerial()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "hciconfig",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = Regex.Match(output, @"HCI Ver: .+ \(0x([a-f0-9]+)\)");
                    if (match.Success)
                    {
                        Bluetooth_Details_Restricted.AdapterSerial = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            Bluetooth_Details_Restricted.AdapterSerial = string.Empty;
        }

        public static void UpdateDeviceFingerprints()
        {
            try
            {
                var fingerprints = new Dictionary<string, string>();
                foreach (var kvp in Bluetooth_Details_Protected.DeviceAddresses)
                {
                    fingerprints[kvp.Key] = kvp.Value;
                }
                Bluetooth_Details_Restricted.DeviceFingerprints = fingerprints;
            }
            catch
            {
                Bluetooth_Details_Restricted.DeviceFingerprints = new Dictionary<string, string>();
            }
        }

        public static void UpdateConnectionLog()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "journalctl",
                    Arguments = "-u bluetooth --no-pager | tail -50",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    Bluetooth_Details_Restricted.ConnectionLog = lines.ToList();
                    return;
                }
            }
            catch { }

            Bluetooth_Details_Restricted.ConnectionLog = new List<string>();
        }

        public static void UpdateBlockedDevices()
        {
            try
            {
                var blockedFile = "/etc/bluetooth/blocked_devices";
                if (File.Exists(blockedFile))
                {
                    var blockedDevices = File.ReadAllLines(blockedFile).ToList();
                    Bluetooth_Details_Restricted.BlockedDevices = blockedDevices;
                    return;
                }
            }
            catch { }

            Bluetooth_Details_Restricted.BlockedDevices = new List<string>();
        }

        public static void UpdateControllerFirmware()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "hciconfig",
                    Arguments = "-a",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = Regex.Match(output, @"LMP Ver: (\d+\.\d+)");
                    if (match.Success)
                    {
                        Bluetooth_Details_Restricted.ControllerFirmware = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            Bluetooth_Details_Restricted.ControllerFirmware = string.Empty;
        }
        #endregion
    }
    #endregion
}
