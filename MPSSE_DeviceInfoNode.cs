using System;

namespace MPSSENet
{
    public partial class MPSSE
    {
        /// <summary>
        /// Type that holds device information for GetDeviceInformation method.
        /// </summary>
        /// <remarks>
        /// Used with I2C_GetChannelInfo and SPI_GetChannelInfo in libMPSSE.DLL.
        /// </remarks>
        public class FT_DEVICE_INFO_NODE
        {
            /// <summary>
            /// Indicates device state.
            /// </summary>
            /// <remarks>
            /// Can be any combination of the following: FT_FLAGS_OPENED, FT_FLAGS_HISPEED.
            /// </remarks>
            public uint Flags;

            /// <summary>
            /// Indicates the device type.
            /// </summary>
            /// <remarks>
            /// Can be one of the following: FT_DEVICE_232H, FT_DEVICE_2232H, FT_DEVICE_4232H, or FT_DEVICE_UNKNOWN.
            /// </remarks>
            public FT_DEVICE Type;

            /// <summary>
            /// The Vendor ID and Product ID of the device.
            /// </summary>
            public uint ID;

            /// <summary>
            /// The physical location identifier of the device.
            /// </summary>
            public uint LocId;

            /// <summary>
            /// The device serial number.
            /// </summary>
            public string SerialNumber;

            /// <summary>
            /// The device description.
            /// </summary>
            public string Description;

            /// <summary>
            /// The device handle.
            /// </summary>
            /// <remarks>
            /// This value is not used externally and is provided for information only. If the device is not open, this value is 0.
            /// </remarks>
            public IntPtr Handle;
        }

        /// <summary>
        /// Flags that provide information on the FTDI device state
        /// </summary>
        /// <remarks>Flag values for I2C_GetChannelInfo and SPI_GetChannelInfo</remarks>
        public class FT_FLAGS
        {
            /// <summary>
            /// Indicates that the device is open.
            /// </summary>
            public const uint FT_FLAGS_OPENED = 0x00000001;

            /// <summary>
            /// Indicates that the device is enumerated as a hi-speed USB device.
            /// </summary>
            public const uint FT_FLAGS_HISPEED = 0x00000002;
        }

    }
}
