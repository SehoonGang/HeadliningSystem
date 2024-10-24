using CoPick.Plc;
using CoPick.Robot;
using CoPick.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadliningSystem.Models
{
    [Serializable]
    public class InspectionConfig : ICustomTypeDescriptor
    {
        [LocalizedDescription("DescCarType")]
        [LocalizedCategory("CategoryGeneral", 1, 4)]
        public int CarType { get; set; } = 1;

        [LocalizedCategory("CategoryGeneral", 1, 4)]
        [LocalizedDescription("DescCarName")]
        public string CarName { get; set; } = "unknown";

        [LocalizedCategory("CategoryGeneral", 1, 4)]
        [LocalizedDescription("DescObjectId")]
        public int ObjectId { get; set; } = 1;

        [Browsable(false)]
        public string InstallRobot { get; set; }

        [Browsable(false)]
        public string ScanRobot { get; set; }
        [Browsable(false)]
        public string Camera { get; set; }

        #region vehicle aligner
        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVehicleScanMode")]
        public CameraScanMode VehicleScanMode { get; set; } = CameraScanMode.SingleCamera;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescSamplingRate")]
        [ValidatorType(ValidatorType.Int, 1, 6)]
        public int SamplingRate { get; set; } = 3;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVehicleRefDataPath")]
        [Editor(typeof(FolderPathEditor), typeof(UITypeEditor)), ValidatorType(ValidatorType.FolderPath)]
        public string VehicleRefDataPath { get; set; } = "C:/";

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVehicleHandEyeCalibFilePath")]
        [Editor(typeof(FolderPathEditor), typeof(UITypeEditor)), ValidatorType(ValidatorType.FolderPath)]
        public string VehicleHandEyeCalibFilePath { get; set; } = "C:/";

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVehicleBirdEyeCalibFilePath")]
        //[Editor(typeof(FilePathEditor), typeof(UITypeEditor)), ValidatorType(ValidatorType.FilePath)]
        [Editor(typeof(FolderPathEditor), typeof(UITypeEditor)), ValidatorType(ValidatorType.FolderPath)]
        public string VehicleBirdEyeCalibFilePath { get; set; } = "C:/";

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescNumPoses")]
        [ValidatorType(ValidatorType.Int, 1, 3)]
        public int NumPosesForVehicle { get; set; } = 1;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescScanInstallMode")]
        public ScanInstallMode ScanInstallMode { get; set; } = ScanInstallMode.DIFF_SCAN_INSTALL;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescNumScanPoints")]
        [ValidatorType(ValidatorType.Int, 1, 4)]
        public int NumScanPointsForVehicle { get; set; } = 1;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationX1AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, -2000.0f, 2000.0f)]
        public float VisualizationX1AverageForVehicle { get; set; } = 0.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationY1AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, -2000.0f, 2000.0f)]
        public float VisualizationY1AverageForVehicle { get; set; } = 0.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ1AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, 300.0f, 2000.0f)]
        public float VisualizationZ1AverageForVehicle { get; set; } = 500.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ1MinForVehicle")]
        [ValidatorType(ValidatorType.Float, 300.0f, 2000.0f)]
        public float VisualizationZ1MinForVehicle { get; set; } = 300.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ1MaxForVehicle")]
        [ValidatorType(ValidatorType.Float, 600.0f, 2000.0f)]
        public float VisualizationZ1MaxForVehicle { get; set; } = 2000.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ1RotationForVehicle")]
        [ValidatorType(ValidatorType.Float, -180.0f, 180.0f)]
        public float VisualizationZ1RotationForVehicle { get; set; } = 90.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationX2AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, -2000.0f, 2000.0f)]
        public float VisualizationX2AverageForVehicle { get; set; } = 0.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationY2AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, -2000.0f, 2000.0f)]
        public float VisualizationY2AverageForVehicle { get; set; } = 0.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ2AverageForVehicle")]
        [ValidatorType(ValidatorType.Float, 300.0f, 2000.0f)]
        public float VisualizationZ2AverageForVehicle { get; set; } = 500.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ2MinForVehicle")]
        [ValidatorType(ValidatorType.Float, 300.0f, 2000.0f)]
        public float VisualizationZ2MinForVehicle { get; set; } = 300.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ2MaxForVehicle")]
        [ValidatorType(ValidatorType.Float, 600.0f, 2000.0f)]
        public float VisualizationZ2MaxForVehicle { get; set; } = 2000.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescVisualizationZ2RotationForVehicle")]
        [ValidatorType(ValidatorType.Float, -180.0f, 180.0f)]
        public float VisualizationZ2RotationForVehicle { get; set; } = 90.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxNumIterations")]
        [ValidatorType(ValidatorType.Int, 1, 1000)]
        public int MaxNumIterations { get; set; } = 100;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescOptPolicyForVehicle")]
        public string OptPolicyForVehicle { get; set; } = "10 2 1 0.5";

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxTranslationXForVehicle")]
        [ValidatorType(ValidatorType.Float, 5.0f, 1000.0f)]
        public float MaxTranslationXForVehicle { get; set; } = 30.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxTranslationYForVehicle")]
        [ValidatorType(ValidatorType.Float, 5.0f, 1000.0f)]
        public float MaxTranslationYForVehicle { get; set; } = 30.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxTranslationZForVehicle")]
        [ValidatorType(ValidatorType.Float, 5.0f, 1000.0f)]
        public float MaxTranslationZForVehicle { get; set; } = 30.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxRotationX")]
        public float MaxRotationX { get; set; } = 3.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxRotationY")]
        public float MaxRotationY { get; set; } = 3.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxRotationZ")]
        public float MaxRotationZ { get; set; } = 3.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMinDepthForVehicle")]
        [ValidatorType(ValidatorType.Float, 0.0f, 1500.0f)]
        public float MinDepthForVehicle { get; set; } = 300.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescMaxDepthForVehicle")]
        [ValidatorType(ValidatorType.Float, 0.0f, 3000.0f)]
        public float MaxDepthForVehicle { get; set; } = 1000.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescReducedStageThreshold")]
        [ValidatorType(ValidatorType.Float, 0.0f, 1000.0f)]
        public float ReducedStageThreshold { get; set; } = 10.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescFinerStageThreshold")]
        [ValidatorType(ValidatorType.Float, 0.0f, 1000.0f)]
        public float FinerStageThreshold { get; set; } = 1.0f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescFinalStageThreshold")]
        [ValidatorType(ValidatorType.Float, 0.0f, 1000.0f)]
        public float FinalStageThreshold { get; set; } = 0.5f;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescReducedMaxNumIterations")]
        [ValidatorType(ValidatorType.Int, 1, 1000)]
        public int ReducedMaxNumIterations { get; set; } = 10;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescFinerMaxNumIterations")]
        [ValidatorType(ValidatorType.Int, 1, 1000)]
        public int FinerMaxNumIterations { get; set; } = 10;

        [LocalizedCategory("CategoryVehicleAligner", 3, 4)]
        [LocalizedDescription("DescFinalMaxNumIterations")]
        [ValidatorType(ValidatorType.Int, 1, 1000)]
        public int FinalMaxNumIterations { get; set; } = 5;

        #endregion

        [NonSerialized]
        private PropertyDescriptorCollection _pdColl;
        public InspectionConfig()
        {

        }
        public void UpdatePropertyDescriptors()
        {
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this);
            PropertyDescriptor[] propertyDescriptorArray = typeof(InspectionConfig).GetProperties()
                .Select(m => new InspectionConfigPropertyDescriptor(pdc[m.Name], m.GetCustomAttributes(false).Cast<Attribute>().ToArray()))
                .ToArray();
            _pdColl = new PropertyDescriptorCollection(propertyDescriptorArray);
        }

        public AttributeCollection GetAttributes() => TypeDescriptor.GetAttributes(this, true);
        public string GetClassName() => TypeDescriptor.GetClassName(this, true);
        public string GetComponentName() => TypeDescriptor.GetComponentName(this, true);
        public TypeConverter GetConverter() => TypeDescriptor.GetConverter(this, true);
        public EventDescriptor GetDefaultEvent() => TypeDescriptor.GetDefaultEvent(this, true);
        public PropertyDescriptor GetDefaultProperty() => TypeDescriptor.GetDefaultProperty(this, true);
        public object GetEditor(Type editorBaseType) => TypeDescriptor.GetEditor(this, editorBaseType, true);
        public EventDescriptorCollection GetEvents() => TypeDescriptor.GetEvents(this, true);
        public EventDescriptorCollection GetEvents(Attribute[] attributes) => TypeDescriptor.GetEvents(this, attributes, true);
        public PropertyDescriptorCollection GetProperties() => _pdColl;
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes) => _pdColl;
        public object GetPropertyOwner(PropertyDescriptor pd) => this;
    }

    public class InspectionConfigPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _originalPd;
        public InspectionConfigPropertyDescriptor(PropertyDescriptor pd, Attribute[] attrs) : base(pd, attrs)
        {
            _originalPd = pd;
        }
        public override Type ComponentType
        {
            get => _originalPd.ComponentType;
        }
        public override bool IsReadOnly
        {
            get => _originalPd.IsReadOnly;
        }

        public override Type PropertyType
        {
            get => _originalPd.PropertyType;
        }

        public override bool CanResetValue(object component) => _originalPd.CanResetValue(component);
        public override object GetValue(object component) => _originalPd.GetValue(component);
        public override void ResetValue(object component) => _originalPd.ResetValue(component);
        public override void SetValue(object component, object value) => _originalPd.SetValue(component, value);
        public override bool ShouldSerializeValue(object component) => _originalPd.ShouldSerializeValue(component);
    }

    public static class DefaultSettingLoader
    {
        public static Dictionary<RobotMaker, Func<Dictionary<RobotAttribute, string>>> Robots = new Dictionary<RobotMaker, Func<Dictionary<RobotAttribute, string>>>()
        {
            [RobotMaker.FANUC] = GetFanucSettings,
            [RobotMaker.HYUNDAI] = GetHyundaiSettings,
        };

        public static Dictionary<RobotAttribute, string> GetFanucSettings()
        {
            return new Dictionary<RobotAttribute, string>()
            {
                [RobotAttribute.Maker] = RobotMaker.FANUC.ToString(),
                [RobotAttribute.Ip] = "",
                [RobotAttribute.Port] = "",
                [RobotAttribute.VehicleInstallVars] = "",
                [RobotAttribute.VehicleShiftVars] = "",
                [RobotAttribute.GapScanPoseVars] = "",
                [RobotAttribute.GapScanPoseShiftVars] = "",
                [RobotAttribute.GlassGripPoseVars] = "",
                [RobotAttribute.GlassGripPoseShiftVars] = "",
                [RobotAttribute.UserFrame] = FanucUserFrame.WORLD.ToString()
            };
        }

        public static Dictionary<RobotAttribute, string> GetHyundaiSettings()
        {
            return new Dictionary<RobotAttribute, string>()
            {
                [RobotAttribute.Maker] = RobotMaker.HYUNDAI.ToString(),
                [RobotAttribute.Ip] = "192.168.178.206",
                [RobotAttribute.ClientIp] = "192.168.178.102",
                [RobotAttribute.VehicleInstallVars] = "",
                [RobotAttribute.VehicleShiftVars] = "",
                [RobotAttribute.GapScanPoseVars] = "",
                [RobotAttribute.GapScanPoseShiftVars] = "",
                [RobotAttribute.GlassGripPoseVars] = "",
                [RobotAttribute.GlassGripPoseShiftVars] = "",
                [RobotAttribute.HrCoordinateSystem] = HrCoordinateSystem.BASE.ToString()
            };
        }

        public static Dictionary<CameraModel, Func<Dictionary<CameraAttribute, string>>> Cameras = new Dictionary<CameraModel, Func<Dictionary<CameraAttribute, string>>>()
        {
            [CameraModel.CoPick3D_250] = GetCoPick3D250Settings,
            [CameraModel.CoPick3D_350] = GetCoPick3D350Settings,
        };

        private static Dictionary<CameraAttribute, string> GetCoPick3D350Settings()
        {
            return new Dictionary<CameraAttribute, string>()
            {
                [CameraAttribute.Model] = CameraModel.CoPick3D_350.ToString(),
                [CameraAttribute.Serial] = "",
                [CameraAttribute.Use] = "True",
                [CameraAttribute.Alias] = "",
                [CameraAttribute.Ip] = "",
                [CameraAttribute.ScanMode] = CameraScanMode.MultiCamera.ToString(),
                [CameraAttribute.OutputResolution] = OutputResolution.W1224xH1024.ToString(),
                [CameraAttribute.IsolationDistance] = "1.0",
                [CameraAttribute.IsolationMinNeighbors] = "10",
                [CameraAttribute.SendNormalMap] = "False",
                [CameraAttribute.TextureExposureMultiplier] = "1",
                [CameraAttribute.TextureExposure1] = "16.0",
                [CameraAttribute.TextureExposure2] = "16.0",
                [CameraAttribute.TextureExposure3] = "16.0",
                [CameraAttribute.TextureGain1] = "5.0",
                [CameraAttribute.TextureGain2] = "5.0",
                [CameraAttribute.TextureGain3] = "5.0",
                [CameraAttribute.PatternExposureMultiplier] = "1",
                [CameraAttribute.PatternExposure1] = "10.0",
                [CameraAttribute.PatternExposure2] = "20.0",
                [CameraAttribute.PatternExposure3] = "30.0",
                [CameraAttribute.PatternGain1] = "3.0",
                [CameraAttribute.PatternGain2] = "3.0",
                [CameraAttribute.PatternGain3] = "3.0",
                [CameraAttribute.DecodeThreshold1] = "1",
                [CameraAttribute.DecodeThreshold2] = "1",
                [CameraAttribute.DecodeThreshold3] = "1",
                [CameraAttribute.NormalEstimationRadius] = "2.0",
                [CameraAttribute.SurfaceSmoothness] = SurfaceSmoothness.Sharp.ToString(),
                [CameraAttribute.StructurePatternType] = StructurePatternType.NormalAndInverted.ToString(),
                [CameraAttribute.LedPower] = "1",
                [CameraAttribute.PatternStrategy] = PatternStrategy.PhaseShiftDouble.ToString(),
                [CameraAttribute.PatternColor] = "3",
                [CameraAttribute.TextureSource] = "2",
                [CameraAttribute.MaxNomalAngle] = "90"
            };
        }

        private static Dictionary<CameraAttribute, string> GetCoPick3D250Settings()
        {
            return new Dictionary<CameraAttribute, string>()
            {
                [CameraAttribute.Model] = CameraModel.CoPick3D_250.ToString(),
                [CameraAttribute.Serial] = "",
                [CameraAttribute.Use] = "True",
                [CameraAttribute.Alias] = "",
                [CameraAttribute.Ip] = "",
                [CameraAttribute.ScanMode] = CameraScanMode.MultiCamera.ToString(),
                [CameraAttribute.OutputResolution] = OutputResolution.W1224xH1024.ToString(),
                [CameraAttribute.IsolationDistance] = "1.0",
                [CameraAttribute.IsolationMinNeighbors] = "10",
                [CameraAttribute.SendNormalMap] = "False",
                [CameraAttribute.TextureExposureMultiplier] = "1",
                [CameraAttribute.TextureExposure1] = "16.0",
                [CameraAttribute.TextureExposure2] = "16.0",
                [CameraAttribute.TextureExposure3] = "16.0",
                [CameraAttribute.TextureGain1] = "5.0",
                [CameraAttribute.TextureGain2] = "5.0",
                [CameraAttribute.TextureGain3] = "5.0",
                [CameraAttribute.PatternExposureMultiplier] = "1",
                [CameraAttribute.PatternExposure1] = "10.0",
                [CameraAttribute.PatternExposure2] = "20.0",
                [CameraAttribute.PatternExposure3] = "30.0",
                [CameraAttribute.PatternGain1] = "3.0",
                [CameraAttribute.PatternGain2] = "3.0",
                [CameraAttribute.PatternGain3] = "3.0",
                [CameraAttribute.DecodeThreshold1] = "1",
                [CameraAttribute.DecodeThreshold2] = "1",
                [CameraAttribute.DecodeThreshold3] = "1",
                [CameraAttribute.NormalEstimationRadius] = "2.0",
                [CameraAttribute.SurfaceSmoothness] = SurfaceSmoothness.Sharp.ToString(),
                [CameraAttribute.StructurePatternType] = StructurePatternType.NormalAndInverted.ToString(),
                [CameraAttribute.LedPower] = "1",
                [CameraAttribute.PatternStrategy] = PatternStrategy.PhaseShiftDouble.ToString(),
                [CameraAttribute.PatternColor] = PatternColor.Blue.ToString(),
                [CameraAttribute.TextureSource] = TextureSource.Led.ToString(),
                [CameraAttribute.MaxNomalAngle] = "90"
            };
        }

        public static Dictionary<PlcModel, Func<Dictionary<PlcAttribute, string>>> Plcs = new Dictionary<PlcModel, Func<Dictionary<PlcAttribute, string>>>()
        {
            [PlcModel.HYUNDAI] = GetHrPlcSettings
        };

        private static Dictionary<PlcAttribute, string> GetHrPlcSettings()
        {
            return new Dictionary<PlcAttribute, string>()
            {
                [PlcAttribute.IP] = "0.0.0.0",
                [PlcAttribute.CLIENT_IP] = "0.0.0.0",
                [PlcAttribute.HrReadIntSignalBase] = "0",
                [PlcAttribute.HrWriteFloatPoseBase] = "0",
            };
        }
    }
}
