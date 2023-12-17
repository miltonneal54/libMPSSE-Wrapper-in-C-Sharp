
namespace MPSSENet
{
    public partial class MPSSE
    {
        /// <summary>
        /// Status return values for FTDI devices.
        /// </summary>
        public enum FT_STATUS : uint
        {
            /// <summary>
            /// Status OK.
            /// </summary>
            FT_OK = 0,

            /// <summary>
            /// The device handle is invalid.
            /// </summary>
            FT_INVALID_HANDLE,

            /// <summary>
            /// Device not found.
            /// </summary>
            FT_DEVICE_NOT_FOUND,

            /// <summary>
            /// Device is not open.
            /// </summary>
            FT_DEVICE_NOT_OPENED,

            /// <summary>
            /// IO error.
            /// </summary>
            FT_IO_ERROR,

            /// <summary>
            /// Insufficient resources.
            /// </summary>
            FT_INSUFFICIENT_RESOURCES,

            /// <summary>
            /// A parameter was invalid.
            /// </summary>
            FT_INVALID_PARAMETER,

            /// <summary>
            /// The requested baud rate is invalid.
            /// </summary>
            FT_INVALID_BAUD_RATE,

            /// <summary>
            /// Device not opened for erase
            /// </summary>
            FT_DEVICE_NOT_OPENED_FOR_ERASE,

            /// <summary>
            /// Device not opened for write.
            /// </summary>
            FT_DEVICE_NOT_OPENED_FOR_WRITE,

            /// <summary>
            /// Failed to write to device.
            /// </summary>
            FT_FAILED_TO_WRITE_DEVICE,

            /// <summary>
            /// Failed to read the device EEPROM.
            /// </summary>
            FT_EEPROM_READ_FAILED,

            /// <summary>
            /// Failed to write the device EEPROM.
            /// </summary>
            FT_EEPROM_WRITE_FAILED,

            /// <summary>
            /// Failed to erase the device EEPROM.
            /// </summary>
            FT_EEPROM_ERASE_FAILED,

            /// <summary>
            /// An EEPROM is not fitted to the device.
            /// </summary>
            FT_EEPROM_NOT_PRESENT,

            /// <summary>
            /// Device EEPROM is blank.
            /// </summary>
            FT_EEPROM_NOT_PROGRAMMED,

            /// <summary>
            /// Invalid arguments.
            /// </summary>
            FT_INVALID_ARGS,

            /// <summary>
            /// An other error has occurred.
            /// </summary>
            FT_OTHER_ERROR
        };

        /// <summary>
        /// FTDI device enumeration.
        /// </summary>
        public enum FT_DEVICE
        {
            /// <summary>
            /// FT232B or FT245B device.
            /// </summary>
            FT_DEVICE_BM = 0,

            /// <summary>
            /// FT8U232AM or FT8U245AM device.
            /// </summary>
            FT_DEVICE_AM,

            /// <summary>
            /// FT8U100AX device.
            /// </summary>
            FT_DEVICE_100AX,

            /// <summary>
            /// Unknown device.
            /// </summary>
            FT_DEVICE_UNKNOWN,

            /// <summary>
            /// FT2232 device.
            /// </summary>
            FT_DEVICE_2232,

            /// <summary>
            /// FT232R or FT245R device.
            /// </summary>
            FT_DEVICE_232R,

            /// <summary>
            /// FT2232H device.
            /// </summary>
            FT_DEVICE_2232H,

            /// <summary>
            /// FT4232H device.
            /// </summary>
            FT_DEVICE_4232H,

            /// <summary>
            /// FT232H device.
            /// </summary>
            FT_DEVICE_232H,

            /// <summary>
            /// FT X-Series device.
            /// </summary>
            FT_DEVICE_X_SERIES,

            /// <summary>
            /// FT4222 hi-speed device Mode 0 - 2 interfaces.
            /// </summary>
            FT_DEVICE_4222H_0,

            /// <summary>
            /// FT4222 hi-speed device Mode 1 or 2 - 4 interfaces.
            /// </summary>
            FT_DEVICE_4222H_1_2,

            /// <summary>
            /// FT4222 hi-speed device Mode 3 - 1 interface
            /// </summary>
            FT_DEVICE_4222H_3,

            /// <summary>
            /// OTP programmer board for the FT4222.
            /// </summary>
            FT_DEVICE_4222_PROG,
        };
    }
}
