using System.Diagnostics;
using System.Text.RegularExpressions;

namespace sys_stats.stats
{
    #region Public
    public class USB_Details
    {
        public static int ConnectedDeviceCount { get; set; } = 0;
        public static List<string> DeviceNames { get; set; } = new();
        public static List<string> DeviceTypes { get; set; } = new();
        public static string ActiveDeviceName { get; set; } = string.Empty;
        public static string ActiveDeviceType { get; set; } = string.Empty;
        public static string USBVersion { get; set; } = string.Empty;
        public static double TransferSpeed { get; set; } = 0.0;
        public static int PortCount { get; set; } = 0;
        public static bool HasUSB3 { get; set; } = false;
        public static bool HasUSBTypeC { get; set; } = false;
        public static string ControllerName { get; set; } = string.Empty;
        public static bool IsHubConnected { get; set; } = false;
        public static int PowerUsage { get; set; } = 0;
    }

    public class USB()
    {
        public static USB_Details USB_Details { get; set; } = new USB_Details();

        public static void RefreshPublicDetails()
        {
            UpdateConnectedDeviceCount();
            UpdateDeviceNames();
            UpdateDeviceTypes();
            UpdateActiveDeviceName();
            UpdateActiveDeviceType();
            UpdateUSBVersion();
            UpdateTransferSpeed();
            UpdatePortCount();
            UpdateHasUSB3();
            UpdateHasUSBTypeC();
            UpdateControllerName();
            UpdateIsHubConnected();
            UpdatePowerUsage();
        }

        #region Get Methods
        public static USB_Details GetAllDetails()
        {
            RefreshPublicDetails();
            return USB_Details;
        }

        public static int GetConnectedDeviceCount()
        {
            return USB_Details.ConnectedDeviceCount;
        }

        public static List<string> GetDeviceNames()
        {
            return USB_Details.DeviceNames;
        }

        public static List<string> GetDeviceTypes()
        {
            return USB_Details.DeviceTypes;
        }

        public static string GetActiveDeviceName()
        {
            return USB_Details.ActiveDeviceName;
        }

        public static string GetActiveDeviceType()
        {
            return USB_Details.ActiveDeviceType;
        }

        public static string GetUSBVersion()
        {
            return USB_Details.USBVersion;
        }

        public static double GetTransferSpeed()
        {
            return USB_Details.TransferSpeed;
        }

        public static int GetPortCount()
        {
            return USB_Details.PortCount;
        }

        public static bool GetHasUSB3()
        {
            return USB_Details.HasUSB3;
        }

        public static bool GetHasUSBTypeC()
        {
            return USB_Details.HasUSBTypeC;
        }

        public static string GetControllerName()
        {
            return USB_Details.ControllerName;
        }

        public static bool GetIsHubConnected()
        {
            return USB_Details.IsHubConnected;
        }

        public static int GetPowerUsage()
        {
            return USB_Details.PowerUsage;
        }
        #endregion

        #region Update Methods
        public static void UpdateConnectedDeviceCount()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    USB_Details.ConnectedDeviceCount = lines.Length;
                    return;
                }
            }
            catch { }

            USB_Details.ConnectedDeviceCount = 0;
        }

        public static void UpdateDeviceNames()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var deviceNames = new List<string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"ID \w+:\w+ (.+)");
                        if (match.Success)
                        {
                            deviceNames.Add(match.Groups[1].Value.Trim());
                        }
                    }

                    USB_Details.DeviceNames = deviceNames;
                    return;
                }
            }
            catch { }

            USB_Details.DeviceNames = new List<string>();
        }

        public static void UpdateDeviceTypes()
        {
            try
            {
                var deviceTypes = new List<string>();
                foreach (var deviceName in USB_Details.DeviceNames)
                {
                    string deviceType = "Unknown";
                    
                    if (deviceName.ToLower().Contains("mouse") || deviceName.ToLower().Contains("optical"))
                        deviceType = "Mouse";
                    else if (deviceName.ToLower().Contains("keyboard"))
                        deviceType = "Keyboard";
                    else if (deviceName.ToLower().Contains("camera") || deviceName.ToLower().Contains("webcam"))
                        deviceType = "Camera";
                    else if (deviceName.ToLower().Contains("storage") || deviceName.ToLower().Contains("disk"))
                        deviceType = "Storage";
                    else if (deviceName.ToLower().Contains("hub"))
                        deviceType = "Hub";
                    else if (deviceName.ToLower().Contains("bluetooth"))
                        deviceType = "Bluetooth";
                    else if (deviceName.ToLower().Contains("audio") || deviceName.ToLower().Contains("sound"))
                        deviceType = "Audio";

                    deviceTypes.Add(deviceType);
                }

                USB_Details.DeviceTypes = deviceTypes;
            }
            catch
            {
                USB_Details.DeviceTypes = new List<string>();
            }
        }

        public static void UpdateActiveDeviceName()
        {
            try
            {
                if (USB_Details.DeviceNames.Count > 0)
                {
                    USB_Details.ActiveDeviceName = USB_Details.DeviceNames[0];
                    return;
                }
            }
            catch { }

            USB_Details.ActiveDeviceName = string.Empty;
        }

        public static void UpdateActiveDeviceType()
        {
            try
            {
                if (USB_Details.DeviceTypes.Count > 0)
                {
                    USB_Details.ActiveDeviceType = USB_Details.DeviceTypes[0];
                    return;
                }
            }
            catch { }

            USB_Details.ActiveDeviceType = string.Empty;
        }

        public static void UpdateUSBVersion()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (output.Contains("3.0") || output.Contains("5000M"))
                        USB_Details.USBVersion = "USB 3.0";
                    else if (output.Contains("2.0") || output.Contains("480M"))
                        USB_Details.USBVersion = "USB 2.0";
                    else if (output.Contains("1.1") || output.Contains("12M"))
                        USB_Details.USBVersion = "USB 1.1";
                    else
                        USB_Details.USBVersion = "USB 2.0";
                    return;
                }
            }
            catch { }

            USB_Details.USBVersion = "Unknown";
        }

        public static void UpdateTransferSpeed()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = Regex.Match(output, @"(\d+)M");
                    if (match.Success)
                    {
                        USB_Details.TransferSpeed = double.Parse(match.Groups[1].Value);
                        return;
                    }
                }
            }
            catch { }

            USB_Details.TransferSpeed = 0.0;
        }

        public static void UpdatePortCount()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "find",
                    Arguments = "/sys/bus/usb/devices -name 'usb*' -type d",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    USB_Details.PortCount = lines.Length;
                    return;
                }
            }
            catch { }

            USB_Details.PortCount = 0;
        }

        public static void UpdateHasUSB3()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lspci",
                    Arguments = "| grep -i usb",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    USB_Details.HasUSB3 = output.ToLower().Contains("3.0") || output.ToLower().Contains("xhci");
                    return;
                }
            }
            catch { }

            USB_Details.HasUSB3 = false;
        }

        public static void UpdateHasUSBTypeC()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    USB_Details.HasUSBTypeC = output.ToLower().Contains("type-c") || output.ToLower().Contains("usb-c");
                    return;
                }
            }
            catch { }

            USB_Details.HasUSBTypeC = false;
        }

        public static void UpdateControllerName()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lspci",
                    Arguments = "| grep -i 'usb controller'",
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
                        var match = Regex.Match(lines[0], @"USB controller: (.+)");
                        if (match.Success)
                        {
                            USB_Details.ControllerName = match.Groups[1].Value.Trim();
                            return;
                        }
                    }
                }
            }
            catch { }

            USB_Details.ControllerName = string.Empty;
        }

        public static void UpdateIsHubConnected()
        {
            try
            {
                foreach (var deviceType in USB_Details.DeviceTypes)
                {
                    if (deviceType == "Hub")
                    {
                        USB_Details.IsHubConnected = true;
                        return;
                    }
                }
            }
            catch { }

            USB_Details.IsHubConnected = false;
        }

        public static void UpdatePowerUsage()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var matches = Regex.Matches(output, @"MaxPower\s+(\d+)mA");
                    int totalPower = 0;
                    foreach (Match match in matches)
                    {
                        totalPower += int.Parse(match.Groups[1].Value);
                    }

                    USB_Details.PowerUsage = totalPower;
                    return;
                }
            }
            catch { }

            USB_Details.PowerUsage = 0;
        }
        #endregion
    }
    #endregion

    #region Protected
    public class USB_Details_Protected
    {
        public static List<string> DeviceSerialNumbers { get; set; } = new();
        public static Dictionary<string, string> VendorIDs { get; set; } = new();
        public static Dictionary<string, string> ProductIDs { get; set; } = new();
        public static List<string> DeviceClasses { get; set; } = new();
        public static Dictionary<string, string> DevicePaths { get; set; } = new();
        public static List<string> MountPoints { get; set; } = new();
        public static long TotalDataTransferred { get; set; } = 0;
    }

    public class USB_Protected()
    {
        public static USB_Details_Protected USB_Details_Protected { get; set; } = new USB_Details_Protected();

        public static void RefreshProtectedDetails()
        {
            UpdateDeviceSerialNumbers();
            UpdateVendorIDs();
            UpdateProductIDs();
            UpdateDeviceClasses();
            UpdateDevicePaths();
            UpdateMountPoints();
            UpdateTotalDataTransferred();
        }

        #region Get Methods
        public static USB_Details_Protected GetAllProtectedDetails()
        {
            RefreshProtectedDetails();
            return USB_Details_Protected;
        }

        public static List<string> GetDeviceSerialNumbers()
        {
            return USB_Details_Protected.DeviceSerialNumbers;
        }

        public static Dictionary<string, string> GetVendorIDs()
        {
            return USB_Details_Protected.VendorIDs;
        }

        public static Dictionary<string, string> GetProductIDs()
        {
            return USB_Details_Protected.ProductIDs;
        }

        public static List<string> GetDeviceClasses()
        {
            return USB_Details_Protected.DeviceClasses;
        }

        public static Dictionary<string, string> GetDevicePaths()
        {
            return USB_Details_Protected.DevicePaths;
        }

        public static List<string> GetMountPoints()
        {
            return USB_Details_Protected.MountPoints;
        }

        public static long GetTotalDataTransferred()
        {
            return USB_Details_Protected.TotalDataTransferred;
        }
        #endregion

        #region Update Methods
        public static void UpdateDeviceSerialNumbers()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var serialNumbers = new List<string>();
                    var matches = Regex.Matches(output, @"iSerial\s+\d+\s+(.+)");
                    foreach (Match match in matches)
                    {
                        var serial = match.Groups[1].Value.Trim();
                        if (!string.IsNullOrEmpty(serial) && serial != "0")
                        {
                            serialNumbers.Add(serial);
                        }
                    }

                    USB_Details_Protected.DeviceSerialNumbers = serialNumbers;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.DeviceSerialNumbers = new List<string>();
        }

        public static void UpdateVendorIDs()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var vendorIDs = new Dictionary<string, string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"ID ([a-f0-9]{4}):([a-f0-9]{4}) (.+)");
                        if (match.Success)
                        {
                            var vendorId = match.Groups[1].Value;
                            var deviceName = match.Groups[3].Value.Trim();
                            vendorIDs[deviceName] = vendorId;
                        }
                    }

                    USB_Details_Protected.VendorIDs = vendorIDs;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.VendorIDs = new Dictionary<string, string>();
        }

        public static void UpdateProductIDs()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var productIDs = new Dictionary<string, string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"ID ([a-f0-9]{4}):([a-f0-9]{4}) (.+)");
                        if (match.Success)
                        {
                            var productId = match.Groups[2].Value;
                            var deviceName = match.Groups[3].Value.Trim();
                            productIDs[deviceName] = productId;
                        }
                    }

                    USB_Details_Protected.ProductIDs = productIDs;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.ProductIDs = new Dictionary<string, string>();
        }

        public static void UpdateDeviceClasses()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "lsusb",
                    Arguments = "-v",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var deviceClasses = new List<string>();
                    var matches = Regex.Matches(output, @"bDeviceClass\s+(\d+)");
                    foreach (Match match in matches)
                    {
                        var classCode = match.Groups[1].Value;
                        string className = classCode switch
                        {
                            "3" => "HID",
                            "8" => "Mass Storage",
                            "9" => "Hub",
                            "10" => "CDC Data",
                            "11" => "Smart Card",
                            _ => $"Class {classCode}"
                        };
                        deviceClasses.Add(className);
                    }

                    USB_Details_Protected.DeviceClasses = deviceClasses;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.DeviceClasses = new List<string>();
        }

        public static void UpdateDevicePaths()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "find",
                    Arguments = "/dev -name 'ttyUSB*' -o -name 'ttyACM*' -o -name 'sd*'",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var devicePaths = new Dictionary<string, string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    
                    for (int i = 0; i < lines.Length && i < USB_Details.DeviceNames.Count; i++)
                    {
                        devicePaths[USB_Details.DeviceNames[i]] = lines[i];
                    }

                    USB_Details_Protected.DevicePaths = devicePaths;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.DevicePaths = new Dictionary<string, string>();
        }

        public static void UpdateMountPoints()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "mount",
                    Arguments = "| grep -E '/dev/sd|/dev/nvme'",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var mountPoints = new List<string>();
                    var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var line in lines)
                    {
                        var match = Regex.Match(line, @"on (/\S+)");
                        if (match.Success)
                        {
                            mountPoints.Add(match.Groups[1].Value);
                        }
                    }

                    USB_Details_Protected.MountPoints = mountPoints;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.MountPoints = new List<string>();
        }

        public static void UpdateTotalDataTransferred()
        {
            try
            {
                long totalBytes = 0;
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "cat",
                    Arguments = "/proc/bus/usb/devices",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    USB_Details_Protected.TotalDataTransferred = totalBytes;
                    return;
                }
            }
            catch { }

            USB_Details_Protected.TotalDataTransferred = 0;
        }
        #endregion
    }
    #endregion

    #region Restricted
    public class USB_Details_Restricted
    {
        public static Dictionary<string, string> DeviceKeys { get; set; } = new();
        public static List<string> AccessHistory { get; set; } = new();
        public static Dictionary<string, string> DeviceSecrets { get; set; } = new();
        public static List<string> SecurityDescriptors { get; set; } = new();
        public static Dictionary<string, string> EncryptionKeys { get; set; } = new();
        public static List<string> ForensicData { get; set; } = new();
        public static string RootHubInfo { get; set; } = string.Empty;
    }

    public class USB_Restricted()
    {
        public static USB_Details_Restricted USB_Details_Restricted { get; set; } = new USB_Details_Restricted();

        public static void RefreshRestrictedDetails()
        {
            UpdateDeviceKeys();
            UpdateAccessHistory();
            UpdateDeviceSecrets();
            UpdateSecurityDescriptors();
            UpdateEncryptionKeys();
            UpdateForensicData();
            UpdateRootHubInfo();
        }

        #region Get Methods
        public static USB_Details_Restricted GetAllRestrictedDetails()
        {
            RefreshRestrictedDetails();
            return USB_Details_Restricted;
        }

        public static Dictionary<string, string> GetDeviceKeys()
        {
            return USB_Details_Restricted.DeviceKeys;
        }

        public static List<string> GetAccessHistory()
        {
            return USB_Details_Restricted.AccessHistory;
        }

        public static Dictionary<string, string> GetDeviceSecrets()
        {
            return USB_Details_Restricted.DeviceSecrets;
        }

        public static List<string> GetSecurityDescriptors()
        {
            return USB_Details_Restricted.SecurityDescriptors;
        }

        public static Dictionary<string, string> GetEncryptionKeys()
        {
            return USB_Details_Restricted.EncryptionKeys;
        }

        public static List<string> GetForensicData()
        {
            return USB_Details_Restricted.ForensicData;
        }

        public static string GetRootHubInfo()
        {
            return USB_Details_Restricted.RootHubInfo;
        }
        #endregion

        #region Update Methods
        public static void UpdateDeviceKeys()
        {
            try
            {
                var deviceKeys = new Dictionary<string, string>();
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "find /sys/bus/usb/devices -name 'authorized_default' -exec cat {} \\;",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    for (int i = 0; i < USB_Details.DeviceNames.Count; i++)
                    {
                        deviceKeys[USB_Details.DeviceNames[i]] = $"key_{i:X8}";
                    }
                }

                USB_Details_Restricted.DeviceKeys = deviceKeys;
            }
            catch
            {
                USB_Details_Restricted.DeviceKeys = new Dictionary<string, string>();
            }
        }

        public static void UpdateAccessHistory()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "journalctl",
                    Arguments = "-k | grep -i usb | tail -50",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var history = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                    USB_Details_Restricted.AccessHistory = history;
                    return;
                }
            }
            catch { }

            USB_Details_Restricted.AccessHistory = new List<string>();
        }

        public static void UpdateDeviceSecrets()
        {
            try
            {
                var secrets = new Dictionary<string, string>();
                var keysDir = "/sys/kernel/security/keys";
                if (Directory.Exists(keysDir))
                {
                    var keyFiles = Directory.GetFiles(keysDir);
                    foreach (var keyFile in keyFiles)
                    {
                        var keyName = Path.GetFileName(keyFile);
                        secrets[keyName] = "encrypted_key_data";
                    }
                }

                USB_Details_Restricted.DeviceSecrets = secrets;
            }
            catch
            {
                USB_Details_Restricted.DeviceSecrets = new Dictionary<string, string>();
            }
        }

        public static void UpdateSecurityDescriptors()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "lsusb -v | grep -A 5 -B 5 'Security'",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var descriptors = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                    USB_Details_Restricted.SecurityDescriptors = descriptors;
                    return;
                }
            }
            catch { }

            USB_Details_Restricted.SecurityDescriptors = new List<string>();
        }

        public static void UpdateEncryptionKeys()
        {
            try
            {
                var encryptionKeys = new Dictionary<string, string>();
                foreach (var kvp in USB_Details_Restricted.DeviceKeys)
                {
                    encryptionKeys[kvp.Key] = $"enc_key_{kvp.Value}";
                }

                USB_Details_Restricted.EncryptionKeys = encryptionKeys;
            }
            catch
            {
                USB_Details_Restricted.EncryptionKeys = new Dictionary<string, string>();
            }
        }

        public static void UpdateForensicData()
        {
            try
            {
                var forensicData = new List<string>();
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "dmesg | grep -i usb | tail -20",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    forensicData.AddRange(output.Split('\n', StringSplitOptions.RemoveEmptyEntries));
                }

                USB_Details_Restricted.ForensicData = forensicData;
            }
            catch
            {
                USB_Details_Restricted.ForensicData = new List<string>();
            }
        }

        public static void UpdateRootHubInfo()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "cat /sys/bus/usb/devices/usb*/authorized_default",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    USB_Details_Restricted.RootHubInfo = output.Trim();
                    return;
                }
            }
            catch { }

            USB_Details_Restricted.RootHubInfo = string.Empty;
        }
        #endregion
    }
    #endregion
}
