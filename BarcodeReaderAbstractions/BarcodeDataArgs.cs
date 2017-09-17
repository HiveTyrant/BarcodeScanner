using System;

namespace BarcodeReaderAbstractions
{
    /// <inheritdoc />
    /// <summary>
    /// Provides data for the <see cref="E:Honeywell.BarcodeReaderAbstractions.IBarcodeReader.BarcodeDataReady" /> event.
    /// </summary>
    public class BarcodeDataArgs : EventArgs
    {
        /// <inheritdoc />
        /// <summary> Creates a BarcodeDataArgs object containing the specified info. </summary>
        /// <param name="data">The scanned barcode data.</param>
        /// <param name="symbType">The symbology type of the scanned barcode.</param>
        /// <param name="symbName">The string representation of symbType.</param>
        /// <param name="time">The time when the barcode was scanned.</param>
        public BarcodeDataArgs(string data, uint symbType, string symbName, DateTime time)
        {
            Data = data;
            SymbologyType = symbType;
            SymbologyName = symbName;
            TimeStamp = time;
        }

        /// <summary>The scanned barcode data </summary>
        public string Data { get; }

        /// <summary> The symbology type of the scanned barcode </summary>
        public uint SymbologyType { get; }

        /// <summary> The string representation of SymbologyType </summary>
        public string SymbologyName { get; }

        /// <summary>The time when the barcode was scanned </summary>
        public DateTime TimeStamp { get; }
    }
}