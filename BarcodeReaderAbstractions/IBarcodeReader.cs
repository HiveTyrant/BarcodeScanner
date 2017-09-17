using System;
using System.Threading.Tasks;

namespace BarcodeReaderAbstractions
{
    public interface IBarcodeReader
    {
        /// <summary> Fires when a barcode is successfully scanned </summary>
        event EventHandler<BarcodeDataArgs> BarcodeDataReady;

        /// <summary> Used to send warnings and error messages to owner </summary>
        event EventHandler<string> StatusMessage;

        /// <summary> Specifies the barcode reader to be used.  If no name is specified, the first found/default barcode readername will be used </summary>
        Task Initialize(string readerNameOverride = "");

        /// <summary> Opens connection to specified barcode reader </summary>
        /// <returns> True if successful </returns>
        Task<bool> StartBarcodeReader();

        /// <summary> Closes connection to specified barcode reader </summary>
        /// <returns> True if successful </returns>
        Task<bool> StopBarcodeReader();
    }
}
