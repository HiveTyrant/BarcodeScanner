using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Honeywell.AIDC.CrossPlatform;
using BarcodeDataArgs = BarcodeReaderAbstractions.BarcodeDataArgs;

namespace BarcodeReaderImpl_Droid
{
    // ReSharper disable once InconsistentNaming
    public class BarcodeReaderWrapper_Droid : BarcodeReaderAbstractions.IBarcodeReader
    {
        #region Private fields and properties

        private object CurContext => Application.Context;
        private string ReaderName { get; set; }

        /// <summary>
        /// Honeywell.AIDC.CrossPlatform.BarcodeReader instance. The type is included with a NuGet package ("Honeywell.BarcodeReader" version="1.20.3.48)
        /// The NuGet package is NOT located on nuget.org, but the package must be downloaded from honeywells support site 
        /// 1: Goto Honeywells Technical support site (https://hsm.secure.force.com/thetechsupportall/home/home.jsp)
        /// 2: Login (registration is needed)
        /// 3: Locate and use the "Software Downloads" "Enter Site" link.  (https://hsmftp.honeywell.com/)
        /// 4: Login (another registration is needed)
        /// 5: If not already installed, install "The Download Manager" via the link on the site
        /// 6: Download the Xamarin SDK file (Software -> Software and Tools -> Developer Library -> SDKs for Xamarin)
        ///    The file "Honeywell-Xamarin-Scanning-SDK-v1.20.03.0048.zip" is used for this build
        /// 7: Unzip the file and install the nuget package in the libs-folder into Visual Studio as an Available Package Source
        ///    The process is described here: https://docs.microsoft.com/en-us/vsts/package/nuget/consume 
        /// 
        /// Remember to add the following permission in Properties\AndroidManifest.xml:
        ///   <uses-permission android:name="com.honeywell.decode.permission.DECODE" />
        /// </summary>
        private BarcodeReader Reader { get; set; }
        
        #endregion        
        
        #region IBarcodeReader implementation

        /// <summary> Fires when a barcode is successfully scanned </summary>
        public event EventHandler<BarcodeDataArgs> BarcodeDataReady;

        /// <summary> Used to send warnings and error messages to owner </summary>
        public event EventHandler<string> StatusMessage;

        /// <inheritdoc />
        public async Task Initialize(string readerNameOverride = "")
        {
            var readerNames = await GetReaderNames();

            if (!string.IsNullOrWhiteSpace(readerNameOverride) && !readerNames.Contains(readerNameOverride))
            {
                var msg = $"Specified name for barcode reader ({readerNameOverride}) is not found on this device";
                OnStatusMessage(msg);
                throw new ArgumentException(msg, nameof(readerNameOverride));
            }

            ReaderName = string.IsNullOrWhiteSpace(readerNameOverride) ? readerNames.FirstOrDefault() : readerNameOverride;
        }

        /// <inheritdoc />
        public async Task<bool> StartBarcodeReader()
        {
            if (null != Reader) await StopBarcodeReader();

            try
            {
                Reader = new BarcodeReader(ReaderName, CurContext);
                Reader.BarcodeDataReady += Reader_BarcodeDataReady;
                if (!Reader.IsReaderOpened)
                {
                    var openRes = await Reader.OpenAsync();
                    if (openRes.Code != BarcodeReaderBase.Result.Codes.SUCCESS && openRes.Code != BarcodeReaderBase.Result.Codes.READER_ALREADY_OPENED)
                        throw new ApplicationException($"Warning: Unable to open BarcodeReader. Code: {openRes.Code}");

                    //Enable selected symbologyes
                    await EnableSymbolgy();
                }
            }
            catch (Exception ex)
            {
                OnStatusMessage($"Error: StartBarcodeReader failed, error: {ex.Message}");
                return false;
            }
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> StopBarcodeReader()
        {
            if (null == Reader) return true;

            var result = true;

            if (Reader.IsReaderOpened)
            {
                BarcodeReaderBase.Result closeRes = await Reader.CloseAsync();
                if (closeRes.Code != BarcodeReaderBase.Result.Codes.SUCCESS)
                {
                    OnStatusMessage($"Warning: Unable to close BarcodeReader, code {closeRes.Code}");
                    result = false;
                }
            }
            Reader.Dispose();
            Reader = null;

            return result;
        }
        
        #endregion

        #region Helper methods

        protected virtual void OnBarcodeDataReady(BarcodeDataArgs e)
        {
            BarcodeDataReady?.Invoke(this, e);
        }

        protected virtual void OnStatusMessage(string e)
        {
            StatusMessage?.Invoke(this, e);
        }

        private async Task<List<string>> GetReaderNames()
        {
            var readers = await BarcodeReader.GetConnectedBarcodeReaders(CurContext);
            var result = readers.Select(r => r.ScannerName).ToList();

            return result;
        }

        private void Reader_BarcodeDataReady(object sender, Honeywell.AIDC.CrossPlatform.BarcodeDataArgs e)
        {
            OnBarcodeDataReady(new BarcodeDataArgs(e.Data, e.SymbologyType, e.SymbologyName, e.TimeStamp));
        }

        private async Task EnableSymbolgy()
        {
            if (Reader.IsReaderOpened)
            {
                Dictionary<string, object> settings = new Dictionary<string, object>()
                {
                    {Reader.SettingKeys.Code128Enabled, true },
                    {Reader.SettingKeys.Code39Enabled, true },
                    {Reader.SettingKeys.Ean8Enabled, true },
                    {Reader.SettingKeys.Ean8CheckDigitTransmitEnabled, true },
                    {Reader.SettingKeys.Ean13Enabled, true },
                    {Reader.SettingKeys.Ean13CheckDigitTransmitEnabled, true },
                    {Reader.SettingKeys.Interleaved25Enabled, true },
                    {Reader.SettingKeys.Interleaved25MaximumLength, 100 },
                    {Reader.SettingKeys.Postal2DMode, Reader.SettingValues.Postal2DMode_Usps }
                };

                BarcodeReaderBase.Result result = await Reader.SetAsync(settings);
                if (result.Code != BarcodeReaderBase.Result.Codes.SUCCESS) 
                    throw new ApplicationException($"EnableSymbolgy failed, code: {result.Code}");
            }
        }

        #endregion
    }
}