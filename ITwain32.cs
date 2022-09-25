/* Этот файл является частью библиотеки Saraff.Twain.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2011.
 * Saraff.Twain.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Twain.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of Saraff.Twain.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2011.
 * Saraff.Twain.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Twain.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Twain.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Saraff.Twain.Aux {

#if NETCOREAPP

    [_Impl(Type = typeof(Twain32))]
    public interface ITwain32 {
        string AppProductName { get; set; }
        ITwain32.ITwainCapabilities Capabilities { get; }
        TwCountry Country { get; set; }
        byte[] CustomDSData { get; set; }
        bool DisableAfterAcquire { get; set; }
        int ImageCount { get; }
        RectangleF ImageLayout { get; set; }
        bool IsTwain2Enable { get; set; }
        bool IsTwain2Supported { get; }
        TwLanguage Language { get; set; }
        ITwain32.ITwainPalette Palette { get; }
        IWin32Window Parent { get; set; }
        bool ShowUI { get; set; }
        int SourceIndex { get; set; }
        int SourcesCount { get; }

        event EventHandler AcquireCompleted;
        event EventHandler<Twain32.AcquireErrorEventArgs> AcquireError;
        event EventHandler<Twain32.DeviceEventEventArgs> DeviceEvent;
        event EventHandler<Twain32.EndXferEventArgs> EndXfer;
        event EventHandler<Twain32.FileXferEventArgs> FileXferEvent;
        event EventHandler<Twain32.MemXferEventArgs> MemXferEvent;
        event EventHandler<Twain32.SetupFileXferEventArgs> SetupFileXferEvent;
        event EventHandler<Twain32.SetupMemXferEventArgs> SetupMemXferEvent;
        event EventHandler<Twain32.TwainStateEventArgs> TwainStateChanged;
        event EventHandler<Twain32.XferDoneEventArgs> XferDone;

        void Acquire();
        bool CloseDataSource();
        bool CloseDSM();
        object GetCap(TwCap capability);
        object GetCurrentCap(TwCap capability);
        object GetDefaultCap(TwCap capability);
        int GetDefaultSource();
        Image GetImage(int index);
        bool GetIsSourceTwain2Compatible(int index);
        Twain32.Identity GetSourceIdentity(int index);
        string GetSourceProductName(int index);
        TwQC IsCapSupported(TwCap capability);
        bool OpenDataSource();
        bool OpenDSM();
        void ResetAllCap();
        void ResetCap(TwCap capability);
        bool SelectSource();
        void SetCap(TwCap capability, Twain32.Enumeration value);
        void SetCap(TwCap capability, object value);
        void SetCap(TwCap capability, object[] value);
        void SetCap(TwCap capability, Twain32.Range value);
        void SetConstraintCap(TwCap capability, Twain32.Enumeration value);
        void SetConstraintCap(TwCap capability, object value);
        void SetConstraintCap(TwCap capability, object[] value);
        void SetConstraintCap(TwCap capability, Twain32.Range value);
        void SetDefaultSource(int index);

        [_Impl(Type = typeof(Twain32.TwainPalette))]
        public interface ITwainPalette {
            Twain32.ColorPalette Get();
            Twain32.ColorPalette GetDefault();
            void Reset(Twain32.ColorPalette palette);
            void Set(Twain32.ColorPalette palette);
        }

        [_Impl(Type = typeof(TwainCapabilities))]
        public interface ITwainCapabilities {
            ICapability2<TwAL> Alarms { get; }
            ICapability<int> AlarmVolume { get; }
            ICapability<string> Author { get; }
            ICapability<bool> AutoBright { get; }
            ICapability<TwBP> AutoDiscardBlankPages { get; }
            ICapability<bool> AutoFeed { get; }
            ICapability<bool> AutomaticBorderDetection { get; }
            ICapability<int> AutomaticCapture { get; }
            ICapability<bool> AutomaticColorEnabled { get; }
            ICapability<TwPixelType> AutomaticColorNonColorPixelType { get; }
            ICapability<bool> AutomaticCropUsesFrame { get; }
            ICapability<bool> AutomaticDeskew { get; }
            ICapability<bool> AutomaticLengthDetection { get; }
            ICapability<bool> AutomaticRotate { get; }
            ICapability<bool> AutomaticSenseMedium { get; }
            ICapability<bool> AutoScan { get; }
            ICapability<TwAS> AutoSize { get; }
            ICapability<bool> BarCodeDetectionEnabled { get; }
            ICapability<uint> BarCodeMaxRetries { get; }
            ICapability<uint> BarCodeMaxSearchPriorities { get; }
            ICapability<TwBD> BarCodeSearchMode { get; }
            ICapability2<TwBT> BarCodeSearchPriorities { get; }
            ICapability<uint> BarCodeTimeout { get; }
            ICapability<TwBM1> BatteryMinutes { get; }
            ICapability<TwBM2> BatteryPercentage { get; }
            ICapability<ushort> BitDepth { get; }
            ICapability<TwBR> BitDepthReduction { get; }
            ICapability<TwBO> BitOrder { get; }
            ICapability<TwBO> BitOrderCodes { get; }
            ICapability<float> Brightness { get; }
            ICapability<bool> CameraEnabled { get; }
            ICapability2<TwPixelType> CameraOrder { get; }
            ICapability<bool> CameraPreviewUI { get; }
            ICapability<TwCS> CameraSide { get; }
            ICapability<string> Caption { get; }
            ICapability<ushort> CcittKFactor { get; }
            ICapability<TwCB> ClearBuffers { get; }
            ICapability<bool> ClearPage { get; }
            ICapability<bool> ColorManagementEnabled { get; }
            ICapability<TwCompression> Compression { get; }
            ICapability<float> Contrast { get; }
            ICapability2<byte> CustHalftone { get; }
            ICapability<bool> CustomDSData { get; }
            ICapability<string> CustomInterfaceGuid { get; }
            ICapability2<TwDE> DeviceEvent { get; }
            ICapability<bool> DeviceOnline { get; }
            ICapability<string> DeviceTimeDate { get; }
            ICapability2<TwDF> DoubleFeedDetection { get; }
            ICapability<float> DoubleFeedDetectionLength { get; }
            ICapability2<TwDP> DoubleFeedDetectionResponse { get; }
            ICapability<TwUS> DoubleFeedDetectionSensitivity { get; }
            ICapability<TwDX> Duplex { get; }
            ICapability<bool> DuplexEnabled { get; }
            ICapability<bool> EnableDSUIOnly { get; }
            ICapability<uint> Endorser { get; }
            ICapability<float> ExposureTime { get; }
            ICapability<bool> ExtImageInfo { get; }
            ICapability<TwFA> FeederAlignment { get; }
            ICapability<bool> FeederEnabled { get; }
            ICapability<bool> FeederLoaded { get; }
            ICapability<TwFO> FeederOrder { get; }
            ICapability2<TwFP> FeederPocket { get; }
            ICapability<bool> FeederPrep { get; }
            ICapability<TwFE> FeederType { get; }
            ICapability<bool> FeedPage { get; }
            ICapability2<TwFT> Filter { get; }
            ICapability<TwFL> FlashUsed2 { get; }
            ICapability<TwFR> FlipRotation { get; }
            ICapability<RectangleF> Frames { get; }
            ICapability<float> Gamma { get; }
            ICapability<string> Halftones { get; }
            ICapability<float> Highlight { get; }
            ICapability<TwIC> IccProfile { get; }
            ICapability2<uint> ImageDataSet { get; }
            ICapability<TwFF> ImageFileFormat { get; }
            ICapability<TwIF> ImageFilter { get; }
            ICapability<TwIM> ImageMerge { get; }
            ICapability<float> ImageMergeHeightThreshold { get; }
            ICapability<bool> Indicators { get; }
            ICapability2<TwCI> IndicatorsMode { get; }
            ICapability<TwJC> JobControl { get; }
            ICapability<TwPixelType> JpegPixelType { get; }
            ICapability<TwJQ> JpegQuality { get; }
            ICapability<TwJS> JpegSubSampling { get; }
            ICapability<bool> LampState { get; }
            ICapability<TwLanguage> Language { get; }
            ICapability<TwLP> LightPath { get; }
            ICapability<TwLS> LightSource { get; }
            ICapability<uint> MaxBatchBuffers { get; }
            ICapability<ushort> MaxFrames { get; }
            ICapability<bool> MicrEnabled { get; }
            ICapability<float> MinimumHeight { get; }
            ICapability<float> MinimumWidth { get; }
            ICapability<TwNF> Mirror { get; }
            ICapability<TwNF> NoiseFilter { get; }
            ICapability<TwOR> Orientation { get; }
            ICapability<TwOV> OverScan { get; }
            ICapability<bool> PaperDetectable { get; }
            ICapability2<TwPH> PaperHandling { get; }
            ICapability<bool> PatchCodeDetectionEnabled { get; }
            ICapability<uint> PatchCodeMaxRetries { get; }
            ICapability<uint> PatchCodeMaxSearchPriorities { get; }
            ICapability<TwBD> PatchCodeSearchMode { get; }
            ICapability2<TwPch> PatchCodeSearchPriorities { get; }
            ICapability<uint> PatchCodeTimeout { get; }
            ICapability<float> PhysicalHeight { get; }
            ICapability<float> PhysicalWidth { get; }
            ICapability<TwPF> PixelFlavor { get; }
            ICapability<TwPixelType> PixelType { get; }
            ICapability<TwPC> PlanarChunky { get; }
            ICapability<int> PowerSaveTime { get; }
            ICapability<TwPS> PowerSupply { get; }
            ICapability<TwPR> Printer { get; }
            ICapability<bool> PrinterEnabled { get; }
            ICapability<uint> PrinterIndex { get; }
            ICapability<TwPM> PrinterMode { get; }
            ICapability<string> PrinterString { get; }
            ICapability<string> PrinterSuffix { get; }
            ICapability<float> PrinterVerticalOffset { get; }
            ICapability<bool> ReacquireAllowed { get; }
            ICapability<bool> RewindPage { get; }
            ICapability<float> Rotation { get; }
            ICapability<TwSG> Segmented { get; }
            ICapability<string> SerialNumber { get; }
            ICapability<float> Shadow { get; }
            ICapability<uint> SheetCount { get; }
            ICapability2<TwBT> SupportedBarCodeTypes { get; }
            ICapability2<TwCap> SupportedCaps { get; }
            ICapability2<TwEI> SupportedExtImageInfo { get; }
            ICapability2<TwPch> SupportedPatchCodeTypes { get; }
            ICapability<TwSS> SupportedSizes { get; }
            ICapability<float> Threshold { get; }
            ICapability<bool> ThumbnailsEnabled { get; }
            ICapability<bool> Tiles { get; }
            ICapability<int> TimeBeforeFirstCapture { get; }
            ICapability<int> TimeBetweenCaptures { get; }
            ICapability<string> TimeDate { get; }
            ICapability<ushort> TimeFill { get; }
            ICapability<bool> UIControllable { get; }
            ICapability<bool> UndefinedImageSize { get; }
            ICapability<TwUnits> Units { get; }
            ICapability<short> XferCount { get; }
            ICapability<TwSX> XferMech { get; }
            ICapability<float> XNativeResolution { get; }
            ICapability<float> XResolution { get; }
            ICapability<float> XScaling { get; }
            ICapability<float> YNativeResolution { get; }
            ICapability<float> YResolution { get; }
            ICapability<float> YScaling { get; }
            ICapability<short> ZoomFactor { get; }
        }

    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    internal sealed class _ImplAttribute : Attribute {

        public Type Type { get; set; }
    }

#endif

}