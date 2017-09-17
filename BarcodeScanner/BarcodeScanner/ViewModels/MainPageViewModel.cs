using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BarcodeScanner.Annotations;
using BarcodeReaderAbstractions;
using Xamarin.Forms;

namespace BarcodeScanner.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private IBarcodeReader _barcodeReader;
        private string _barcodeDataText;
        private string _barcodeSymbology;
        private DateTime _scanTime;
        private string _statusMessage;
        public int ScanCount { get; set; }

        public MainPageViewModel()
        {
            BarcodeDataText = "Initializing...";
        }

        public IBarcodeReader BarcodeReader
        {
            get => _barcodeReader;
            set
            {
                _barcodeReader = value;
                OnPropertyChanged();
            }
        }

        public string BarcodeDataText
        {
            get => _barcodeDataText;
            set
            {
                _barcodeDataText = value;
                OnPropertyChanged();
            }
        }

        public string BarcodeSymbology
        {
            get => _barcodeSymbology;
            set
            {
                _barcodeSymbology = value;
                OnPropertyChanged();
            }
        }

        public DateTime ScanTime
        {
            get => _scanTime;
            set
            {
                _scanTime = value;
                OnPropertyChanged();
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        internal void Initialize()
        {
            StatusMessage = "Initializing...";
            BarcodeDataText = "No barcode scanned yet";
            ScanTime = DateTime.Now;

            BarcodeReader = DependencyService.Get<IBarcodeReader>();
            if (null != BarcodeReader)
            {
                BarcodeReader.BarcodeDataReady += BarcodeReader_BarcodeDataReady;
                BarcodeReader.StatusMessage += BarcodeReader_StatusMessage;

                Task.Run(async () =>
                {
                    try
                    {
                        await BarcodeReader.Initialize();
                        await BarcodeReader.StartBarcodeReader();

                        StatusMessage = "Scan a barcode";
                    }
                    catch (Exception e)
                    {
                        StatusMessage = e.Message;
                    }
                });
            }
        }

        internal void Stop()
        {
            if (null != BarcodeReader)
                Task.Run(async () => await BarcodeReader.StopBarcodeReader());
        }

        private void BarcodeReader_BarcodeDataReady(object sender, BarcodeDataArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BarcodeDataText = e.Data;
                BarcodeSymbology = e.SymbologyName;
                ScanTime = e.TimeStamp;
                StatusMessage = $"Barcode #{++ScanCount} received";
            });
        }
        
        private void BarcodeReader_StatusMessage(object sender, string statusMessage)
        {
            Device.BeginInvokeOnMainThread(() => StatusMessage = statusMessage);
        }
        
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion        
    }
}
