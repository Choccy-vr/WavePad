using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Sockets;
namespace sys_stats.stats
{
    #region Public
    public class WiFi_Details
    {
        public static string SSID { get; set; } = string.Empty;
        public static int SignalStrength { get; set; } = 0;
        public static string IPAddress { get; set; } = string.Empty;
        public static string WiFiFreq { get; set; } = string.Empty;
        public static bool IsConnected { get; set; } = false;
        public static string Protocol { get; set; } = string.Empty;
        public static double LinkSpeed { get; set; } = 0.0;
        public static string SecurityType { get; set; } = string.Empty;
        public static string ConnectionStatus { get; set; } = string.Empty;
        public static string NetworkAdapterName { get; set; } = string.Empty;
        public static int Channel { get; set; } = 0;
        public static double Latency { get; set; } = 0;
        public static bool IsMetered { get; set; } = false;
    }
    public class Wifi()
    {
        public static WiFi_Details WiFi_Details { get; set; } = new WiFi_Details();
        public static void RefreshPublicDetails()
        {
            UpdateSSID();
            UpdateSignalStrength();
            UpdateIPAddress();
            UpdateWiFiFrequency();
            UpdateIsConnected();
            UpdateProtocol();
            UpdateLinkSpeed();
            UpdateSecurityType();
            UpdateConnectionStatus();
            UpdateNetworkAdapterName();
            UpdateChannel();
            UpdateLatency();
            UpdateIsMetered();
        }
        #region Get Methods
        public static WiFi_Details GetAllDetails()
        {
            RefreshPublicDetails();
            return WiFi_Details;
        }
        public static string GetSSID()
        {
            return WiFi_Details.SSID;
        }
        public static int GetSignalStrength()
        {
            return WiFi_Details.SignalStrength;
        }
        public static string GetIPAddress()
        {
            return WiFi_Details.IPAddress;
        }
        public static string GetWiFiFrequency()
        {
            return WiFi_Details.WiFiFreq;
        }
        public static bool GetIsConnected()
        {
            return WiFi_Details.IsConnected;
        }
        public static string GetProtocol()
        {
            return WiFi_Details.Protocol;
        }
        public static double GetLinkSpeed()
        {
            return WiFi_Details.LinkSpeed;
        }
        public static string GetSecurityType()
        {
            return WiFi_Details.SecurityType;
        }
        public static string GetConnectionStatus()
        {
            return WiFi_Details.ConnectionStatus;
        }
        public static string GetNetworkAdapterName()
        {
            return WiFi_Details.NetworkAdapterName;
        }
        public static int GetChannel()
        {
            return WiFi_Details.Channel;
        }
        public static double GetLatency()
        {
            return WiFi_Details.Latency;
        }
        public static bool GetIsMetered()
        {
            return WiFi_Details.IsMetered;
        }

        #endregion
        #region Update Methods
        public static void UpdateSSID()
        {
            // Method 1: iwgetid 
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwgetid",
                    Arguments = "-r", // Raw output (SSID only)
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();

                    if (process.ExitCode == 0 && !string.IsNullOrEmpty(output))
                    {
                        WiFi_Details.SSID = output;
                        return;
                    }
                }
            }
            catch { }

            // Method 2: nmcli fallback
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "nmcli",
                    Arguments = "-t -f active,ssid dev wifi | grep '^yes:' | cut -d: -f2",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();

                    if (process.ExitCode == 0 && !string.IsNullOrEmpty(output))
                    {
                        WiFi_Details.SSID = output;
                        return;
                    }
                }
            }
            catch { }

            // Final fallback if both methods fail
            WiFi_Details.SSID = string.Empty;
        }
        public static void UpdateSignalStrength()
        {
            // Method 1: iwconfig
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Parse the output to find Signal Level
                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Signal level=(-?\d+) dBm");
                    if (match.Success)
                    {
                        WiFi_Details.SignalStrength = int.Parse(match.Groups[1].Value);
                        return;
                    }
                }
            }
            catch { }

            // Final fallback if iwconfig fails
            WiFi_Details.SignalStrength = 0;
        }
        public static void UpdateIPAddress()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                if (wifiInterface != null)
                {
                    var ipProps = wifiInterface.GetIPProperties();
                    var ipv4Address = ipProps.UnicastAddresses
                        .FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);

                    if (ipv4Address != null)
                    {
                        WiFi_Details.IPAddress = ipv4Address.Address.ToString();
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.IPAddress = string.Empty;
        }

        public static void UpdateWiFiFrequency()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Frequency:(\d+\.\d+) GHz");
                    if (match.Success)
                    {
                        WiFi_Details.WiFiFreq = match.Groups[1].Value + " GHz";
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.WiFiFreq = string.Empty;
        }

        public static void UpdateIsConnected()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                WiFi_Details.IsConnected = wifiInterface != null && !string.IsNullOrEmpty(WiFi_Details.SSID);
            }
            catch
            {
                WiFi_Details.IsConnected = false;
            }
        }

        public static void UpdateProtocol()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"IEEE (802\.11\w+)");
                    if (match.Success)
                    {
                        WiFi_Details.Protocol = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.Protocol = string.Empty;
        }

        public static void UpdateLinkSpeed()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Bit Rate=(\d+\.?\d*) Mb/s");
                    if (match.Success)
                    {
                        WiFi_Details.LinkSpeed = double.Parse(match.Groups[1].Value);
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.LinkSpeed = 0.0;
        }

        public static void UpdateSecurityType()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (output.Contains("Encryption key:off"))
                        WiFi_Details.SecurityType = "Open";
                    else if (output.Contains("WPA"))
                        WiFi_Details.SecurityType = "WPA/WPA2";
                    else if (output.Contains("WEP"))
                        WiFi_Details.SecurityType = "WEP";
                    else
                        WiFi_Details.SecurityType = "Unknown";
                    return;
                }
            }
            catch { }

            WiFi_Details.SecurityType = string.Empty;
        }

        public static void UpdateConnectionStatus()
        {
            try
            {
                if (WiFi_Details.IsConnected)
                    WiFi_Details.ConnectionStatus = "Connected";
                else
                    WiFi_Details.ConnectionStatus = "Disconnected";
            }
            catch
            {
                WiFi_Details.ConnectionStatus = "Unknown";
            }
        }

        public static void UpdateNetworkAdapterName()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                if (wifiInterface != null)
                {
                    WiFi_Details.NetworkAdapterName = wifiInterface.Name;
                    return;
                }
            }
            catch { }

            WiFi_Details.NetworkAdapterName = string.Empty;
        }

        public static void UpdateChannel()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwlist",
                    Arguments = "scan | grep Channel",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Channel (\d+)");
                    if (match.Success)
                    {
                        WiFi_Details.Channel = int.Parse(match.Groups[1].Value);
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.Channel = 0;
        }

        public static void UpdateLatency()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "ping",
                    Arguments = "-c 1 8.8.8.8",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"time=(\d+\.?\d*) ms");
                    if (match.Success)
                    {
                        WiFi_Details.Latency = double.Parse(match.Groups[1].Value);
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details.Latency = 0.0;
        }

        public static void UpdateIsMetered()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "nmcli",
                    Arguments = "-t -f GENERAL.METERED device show",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    WiFi_Details.IsMetered = output.Contains("yes");
                    return;
                }
            }
            catch { }

            WiFi_Details.IsMetered = false;
        }

        #endregion
    }
    #endregion

    #region Protected
    public class WiFi_Details_Protected
    {
        public static string MacAddress { get; set; } = string.Empty;
        public static string Gateway { get; set; } = string.Empty;
        public static string DNSServers { get; set; } = string.Empty;
        public static bool AutoConnect { get; set; } = false;
        public static List<string> SavedNetworkNames { get; set; } = new();
        public static string NetworkVendor { get; set; } = string.Empty;
        public static string CountryCode { get; set; } = string.Empty;
    }
    public class WiFi_Protected()
    {
        public static WiFi_Details_Protected WiFi_Details_Protected { get; set; } = new WiFi_Details_Protected();
        public static void RefreshProtectedDetails()
        {
            UpdateMacAddress();
            UpdateGateway();
            UpdateDNSServers();
            UpdateAutoConnect();
            UpdateSavedNetworkNames();
            UpdateNetworkVendor();
            UpdateCountryCode();
        }

        #region Get Methods
        public static WiFi_Details_Protected GetAllProtectedDetails()
        {
            RefreshProtectedDetails();
            return WiFi_Details_Protected;
        }

        public static string GetMacAddress()
        {
            return WiFi_Details_Protected.MacAddress;
        }

        public static string GetGateway()
        {
            return WiFi_Details_Protected.Gateway;
        }

        public static string GetDNSServers()
        {
            return WiFi_Details_Protected.DNSServers;
        }

        public static bool GetAutoConnect()
        {
            return WiFi_Details_Protected.AutoConnect;
        }

        public static List<string> GetSavedNetworkNames()
        {
            return WiFi_Details_Protected.SavedNetworkNames;
        }

        public static string GetNetworkVendor()
        {
            return WiFi_Details_Protected.NetworkVendor;
        }

        public static string GetCountryCode()
        {
            return WiFi_Details_Protected.CountryCode;
        }
        #endregion

        #region Update Methods
        public static void UpdateMacAddress()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                if (wifiInterface != null)
                {
                    WiFi_Details_Protected.MacAddress = wifiInterface.GetPhysicalAddress().ToString();
                    return;
                }
            }
            catch { }

            WiFi_Details_Protected.MacAddress = string.Empty;
        }

        public static void UpdateGateway()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                if (wifiInterface != null)
                {
                    var ipProps = wifiInterface.GetIPProperties();
                    var gatewayAddress = ipProps.GatewayAddresses
                        .FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork);

                    if (gatewayAddress != null)
                    {
                        WiFi_Details_Protected.Gateway = gatewayAddress.Address.ToString();
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details_Protected.Gateway = string.Empty;
        }

        public static void UpdateDNSServers()
        {
            try
            {
                var wifiInterface = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(ni =>
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                        ni.OperationalStatus == OperationalStatus.Up);

                if (wifiInterface != null)
                {
                    var ipProps = wifiInterface.GetIPProperties();
                    var dnsServers = ipProps.DnsAddresses
                        .Where(addr => addr.AddressFamily == AddressFamily.InterNetwork)
                        .Select(addr => addr.ToString());

                    WiFi_Details_Protected.DNSServers = string.Join(", ", dnsServers);
                    return;
                }
            }
            catch { }

            WiFi_Details_Protected.DNSServers = string.Empty;
        }

        public static void UpdateAutoConnect()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "nmcli",
                    Arguments = $"-t -f GENERAL.AUTOCONNECT connection show \"{WiFi_Details.SSID}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();

                    WiFi_Details_Protected.AutoConnect = output.Contains("yes");
                    return;
                }
            }
            catch { }

            WiFi_Details_Protected.AutoConnect = false;
        }

        public static void UpdateSavedNetworkNames()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "nmcli",
                    Arguments = "-t -f NAME connection show",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var networks = output.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                        .Where(line => !line.Contains("lo") && !line.Contains("Wired"))
                        .ToList();

                    WiFi_Details_Protected.SavedNetworkNames = networks;
                    return;
                }
            }
            catch { }

            WiFi_Details_Protected.SavedNetworkNames = new List<string>();
        }

        public static void UpdateNetworkVendor()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwlist",
                    Arguments = "scan | grep -A 5 -B 5 \"" + WiFi_Details.SSID + "\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Address: ([A-F0-9:]{17})");
                    if (match.Success)
                    {
                        var mac = match.Groups[1].Value;
                        var oui = mac.Substring(0, 8).Replace(":", "").ToUpper();
                        
                        if (oui.StartsWith("00:1A:11"))
                            WiFi_Details_Protected.NetworkVendor = "Google";
                        else if (oui.StartsWith("A4:2B:8C"))
                            WiFi_Details_Protected.NetworkVendor = "Apple";
                        else if (oui.StartsWith("F4:F2:6D"))
                            WiFi_Details_Protected.NetworkVendor = "TP-Link";
                        else
                            WiFi_Details_Protected.NetworkVendor = "Unknown";
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details_Protected.NetworkVendor = string.Empty;
        }

        public static void UpdateCountryCode()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iw",
                    Arguments = "reg get",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"country ([A-Z]{2}):");
                    if (match.Success)
                    {
                        WiFi_Details_Protected.CountryCode = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details_Protected.CountryCode = string.Empty;
        }
        #endregion
    }
    #endregion

    #region Restricted
    public class WiFi_Details_Restricted
    {
        public static string Password { get; set; } = string.Empty;
        public static string BSSID { get; set; } = string.Empty;
        public static Dictionary<string, string> StoredPasswords { get; set; } = new();
        public static List<string> BSSIDHistory { get; set; } = new();
        public static List<string> GeolocationData { get; set; } = new();
    }
    public class WiFi_Restricted()
    {
        public static WiFi_Details_Restricted WiFi_Details_Restricted { get; set; } = new WiFi_Details_Restricted();
        public static void RefreshRestrictedDetails()
        {
            UpdatePassword();
            UpdateBSSID();
            UpdateStoredPasswords();
            UpdateBSSIDHistory();
            UpdateGeolocationData();
        }

        #region Get Methods
        public static WiFi_Details_Restricted GetAllRestrictedDetails()
        {
            RefreshRestrictedDetails();
            return WiFi_Details_Restricted;
        }

        public static string GetPassword()
        {
            return WiFi_Details_Restricted.Password;
        }

        public static string GetBSSID()
        {
            return WiFi_Details_Restricted.BSSID;
        }

        public static Dictionary<string, string> GetStoredPasswords()
        {
            return WiFi_Details_Restricted.StoredPasswords;
        }

        public static List<string> GetBSSIDHistory()
        {
            return WiFi_Details_Restricted.BSSIDHistory;
        }

        public static List<string> GetGeolocationData()
        {
            return WiFi_Details_Restricted.GeolocationData;
        }
        #endregion

        #region Update Methods
        public static void UpdatePassword()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = $"grep -r \"psk=\" /etc/NetworkManager/system-connections/",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"psk=(.+)");
                    if (match.Success)
                    {
                        WiFi_Details_Restricted.Password = match.Groups[1].Value.Trim();
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details_Restricted.Password = string.Empty;
        }

        public static void UpdateBSSID()
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "iwconfig",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var match = System.Text.RegularExpressions.Regex.Match(output, @"Access Point: ([A-F0-9:]{17})");
                    if (match.Success)
                    {
                        WiFi_Details_Restricted.BSSID = match.Groups[1].Value;
                        return;
                    }
                }
            }
            catch { }

            WiFi_Details_Restricted.BSSID = string.Empty;
        }

        public static void UpdateStoredPasswords()
        {
            try
            {
                var passwords = new Dictionary<string, string>();
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "sudo",
                    Arguments = "find /etc/NetworkManager/system-connections/ -name '*.nmconnection' -exec grep -l 'psk=' {} \\;",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var files = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var file in files)
                    {
                        var fileName = System.IO.Path.GetFileNameWithoutExtension(file);
                        var content = System.IO.File.ReadAllText(file);
                        var match = System.Text.RegularExpressions.Regex.Match(content, @"psk=(.+)");
                        if (match.Success)
                        {
                            passwords[fileName] = match.Groups[1].Value.Trim();
                        }
                    }
                }

                WiFi_Details_Restricted.StoredPasswords = passwords;
            }
            catch
            {
                WiFi_Details_Restricted.StoredPasswords = new Dictionary<string, string>();
            }
        }

        public static void UpdateBSSIDHistory()
        {
            try
            {
                var history = new List<string>();
                var process = Process.Start(new ProcessStartInfo
                {
                    FileName = "journalctl",
                    Arguments = "-u NetworkManager --no-pager | grep -o '[A-F0-9:]\\{17\\}' | sort -u",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    var bssids = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    history.AddRange(bssids);
                }

                WiFi_Details_Restricted.BSSIDHistory = history;
            }
            catch
            {
                WiFi_Details_Restricted.BSSIDHistory = new List<string>();
            }
        }

        public static void UpdateGeolocationData()
        {
            try
            {
                var geoData = new List<string>();
                
                if (!string.IsNullOrEmpty(WiFi_Details_Restricted.BSSID))
                {
                    geoData.Add($"Current BSSID: {WiFi_Details_Restricted.BSSID}");
                }

                foreach (var bssid in WiFi_Details_Restricted.BSSIDHistory.Take(10))
                {
                    geoData.Add($"Historical BSSID: {bssid}");
                }

                WiFi_Details_Restricted.GeolocationData = geoData;
            }
            catch
            {
                WiFi_Details_Restricted.GeolocationData = new List<string>();
            }
        }
        #endregion
    }
    #endregion
}