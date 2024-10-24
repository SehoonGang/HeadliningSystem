using CoPick.Logging;
using CoPick.Plc;
using CoPick.Robot;
using CoPick.Setting;
using System.ComponentModel;
using System.IO;

namespace HeadliningSystem.Models
{
    [Serializable]
    public class Config
    {
        public Dictionary<string, Dictionary<RobotAttribute, string>>? RobotConfigs { get; set; }
        public Dictionary<string, RobotPoseVariable>? RobotPoseVariables { get; set; }
        public Dictionary<string, List<Dictionary<CameraAttribute, string>>>? CameraConfigs { get; set; }
        public Dictionary<PlcModel, Dictionary<PlcAttribute, string>>? PlcConfigs { get; set; }
        public PlcModel Plc { get; set; }
        public int RecenltyUsedTask { get; set; } = 101;
        public Dictionary<int, InspectionConfig> ConfigDict { get; set; } = new Dictionary<int, InspectionConfig>();
        private string _logPath = "./log";
        public string LogPath
        {
            get => _logPath;
            set
            {
                try
                {
                    Path.GetFullPath(value);
                    _logPath = value;
                }
                catch { }
            }
        }
        public LogLevel MinimumFileLogLevel { get; set; }
        public LogLevel MinimumUiLogLevel { get; set; }
        public OperationMode StartMode { get; set; }
        public long MaxScanTime { get; set; } = 5000;
        public string Language { get; set; } = "ko-KR";
        public Config()
        {
            Plc = PlcModel.HYUNDAI;
            PlcConfigs = new Dictionary<PlcModel, Dictionary<PlcAttribute, string>>()
            {
                [PlcModel.HYUNDAI] = DefaultSettingLoader.Plcs[PlcModel.HYUNDAI](),
            };

            RobotConfigs = new Dictionary<string, Dictionary<RobotAttribute, string>>();
            CameraConfigs = new Dictionary<string, List<Dictionary<CameraAttribute, string>>>();
            RobotPoseVariables = new Dictionary<string, RobotPoseVariable>();

            ConfigDict[101] = new InspectionConfig();
            ConfigDict[102] = new InspectionConfig();
            ConfigDict[103] = new InspectionConfig();
            ConfigDict[104] = new InspectionConfig();
        }

        public InspectionConfig this[int key]
        {
            get
            {
                if (key == -1)
                {
                    key = RecenltyUsedTask;
                }

                if (!ConfigDict.TryGetValue(key, out InspectionConfig res))
                {
                    res = default;
                }
                return res;
            }

            set
            {
                if (key == -1)
                {
                    key = RecenltyUsedTask;
                }

                ConfigDict[key] = value;
            }
        }

        public bool Delete(int k)
        {
            return ConfigDict.Remove(k);
        }

        public KeyValuePair<int, InspectionConfig> GetFirstOneKeyValue()
        {
            if (ConfigDict.Count < 1) ConfigDict[0] = new InspectionConfig();
            return ConfigDict.First();
        }

        public List<int> GetTaskList()
        {
            return ConfigDict.Keys.ToList();
        }

        public List<string> GetTaskStringList()
        {
            return ConfigDict.Keys.Select(k => k.ToString()).ToList();
        }

        public string ValidateRobotConfig(bool wantCorrection = false)
        {
            string ret = string.Empty;
            foreach (var config in ConfigDict.Values)
            {
                if (RobotConfigs != null && RobotConfigs.Count > 0)
                {
                    if (config.InstallRobot == null)
                    {
                        ret += $"{config.CarName}'s InstallRobot not set.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s InstallRobot set. ({RobotConfigs.Keys.First()}){Environment.NewLine}";
                            config.InstallRobot = RobotConfigs.Keys.First();
                        }
                    }
                    else if (!RobotConfigs.ContainsKey(config.InstallRobot))
                    {
                        ret += $"{config.CarName}'s InstallRobot({config.InstallRobot}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s InstallRobot changed. ({config.InstallRobot} -> {RobotConfigs.Keys.First()}){Environment.NewLine}";
                            config.InstallRobot = RobotConfigs.Keys.First();
                        }
                    }

                    if (config.ScanRobot == null)
                    {
                        ret += $"{config.CarName}'s ScanRobot not set.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s ScanRobot set. ({RobotConfigs.Keys.First()}){Environment.NewLine}";
                            config.ScanRobot = RobotConfigs.Keys.First();
                        }
                    }
                    else if (!RobotConfigs.ContainsKey(config.ScanRobot))
                    {
                        ret += $"{config.CarName}'s ScanRobot({config.ScanRobot}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s ScanRobot changed. ({config.ScanRobot} -> {RobotConfigs.Keys.First()}){Environment.NewLine}";
                            config.ScanRobot = RobotConfigs.Keys.First();
                        }
                    }
                }
                else
                {
                    if (config.InstallRobot != null)
                    {
                        ret += $"{config.CarName}'s InstallRobot({config.InstallRobot}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s InstallRobot unset.{Environment.NewLine}";
                            config.InstallRobot = null;
                        }
                    }

                    if (config.ScanRobot != null)
                    {
                        ret += $"{config.CarName}'s ScanRobot({config.ScanRobot}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s ScanRobot unset.{Environment.NewLine}";
                            config.ScanRobot = null;
                        }
                    }
                }
            }

            return ret;
        }

        public string ValidateCameraConfig(bool wantCorrection = false)
        {
            string ret = string.Empty;
            foreach (var config in ConfigDict.Values)
            {
                if (CameraConfigs != null && CameraConfigs.Count > 0)
                {
                    if (config.Camera == null)
                    {
                        ret += $"{config.CarName}'s VehicleCamera not set.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s VehicleCamera set. ({CameraConfigs.Keys.First()}){Environment.NewLine}";
                            config.Camera = CameraConfigs.Keys.First();
                        }
                    }
                    else if (!CameraConfigs.ContainsKey(config.Camera))
                    {
                        ret += $"{config.CarName}'s VehicleCamera({config.Camera}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s VehicleCamera changed. ({config.Camera} -> {CameraConfigs.Keys.First()}){Environment.NewLine}";
                            config.Camera = CameraConfigs.Keys.First();
                        }
                    }
                }
                else
                {
                    if(config.Camera != null)
                    {
                        ret += $"{config.CarName}'s VehicleCamera({config.Camera}) not found.{Environment.NewLine}";
                        if (wantCorrection)
                        {
                            ret += $"{config.CarName}'s VehicleCamera unset.{Environment.NewLine}";
                            config.Camera = null;
                        }
                    }
                }
            }
            return ret;
        }

        public string ValidateAllConfig(bool wantCorrection = false)
        {
            string ret = string.Empty;

            string retRobot = ValidateRobotConfig(wantCorrection);
            if (!string.IsNullOrEmpty(retRobot))
            {
                ret += retRobot;
            }

            string retCamera = ValidateCameraConfig(wantCorrection);
            if (!string.IsNullOrEmpty(retCamera))
            {
                ret += retCamera;
            }

            return ret;
        }
    }
}
