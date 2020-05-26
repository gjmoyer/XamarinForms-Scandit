using Scandit.BarcodePicker.Unified;
using Scandit.BarcodePicker.Unified.Abstractions;

namespace XamarinForms_Scandit
{
    public class ScanService
    {
        static ScanService _instance;

        static ScanService()
        {
            ScanditService.ScanditLicense.AppKey = "";
        }

        public static ScanService Instance
        {
            get
            {
                return _instance ?? (_instance = new ScanService());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Engage(DidScanDelegate scanDelegate)
        {
            // Configure the barcode picker through a scan settings instance by defining which
            // symbologies should be enabled.
            var settings = ScanditService.BarcodePicker.GetDefaultScanSettings();
            // prefer backward facing camera over front-facing cameras.
            settings.CameraPositionPreference = CameraPosition.Back;
            // Enable symbologies that you want to scan.
            settings.EnableSymbology(Symbology.MsiPlessey, true);
            settings.Symbologies[Symbology.MsiPlessey].Checksums = Checksum.Mod1010;
            settings.EnableSymbology(Symbology.Interleaved2Of5, true);
            settings.EnableSymbology(Symbology.Gs1Databar, true);
            settings.EnableSymbology(Symbology.Gs1DatabarExpanded, true);
            settings.EnableSymbology(Symbology.Gs1DatabarLimited, true);
            settings.EnableSymbology(Symbology.Upca, true);
            settings.EnableSymbology(Symbology.Upce, true);
            settings.EnableSymbology(Symbology.Ean8, true);
            settings.EnableSymbology(Symbology.Ean13, true);
            settings.EnableSymbology(Symbology.Code128, true);

            //reduce the area in which barcodes are detected and decoded
            settings.RestrictedAreaScanningEnabled = true;
            settings.ActiveScanningAreaPortrait = new Rect(0.0, 0.35, 1.0, 0.30);
            settings.ActiveScanningAreaLandscape = new Rect(0.2, 0.35, 0.6, 0.30);

            ScanditService.BarcodePicker.ScanOverlay.CameraSwitchVisibility = CameraSwitchVisibility.Never;

            ScanditService.BarcodePicker.DidStop -= StopScan;
            ScanditService.BarcodePicker.DidStop += StopScan;

            ScanditService.BarcodePicker.DidScan -= scanDelegate; //in case it is already there
            ScanditService.BarcodePicker.DidScan += scanDelegate;

            ScanditService.BarcodePicker.CancelButtonText = "Cancel"; //user has option to cancel the scan
            await ScanditService.BarcodePicker.ApplySettingsAsync(settings);
            // Start the scanning process.
            await ScanditService.BarcodePicker.StartScanningAsync();
        }

        void StopScan(DidStopReason reason)
        {
            ScanditService.BarcodePicker.DidStop -= StopScan;
        }
    }
}
