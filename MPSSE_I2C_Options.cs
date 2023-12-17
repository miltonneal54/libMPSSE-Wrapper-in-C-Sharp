namespace MPSSENet
{
    public partial class MPSSE_I2C
    {
        /// <summary>
        /// Type class that holds the parameters used for initializing a channel.
        /// </summary>
        public class ChannelConfig
        {
            /// <summary>
            /// Device clock rate.
            /// </summary>
            public uint ClockRate;

            /// <summary>
            /// Latency timer in milliseconds.
            /// </summary>
            /// <remarks>FT2232D 2 - 255 milliseconds, 1 - 255 milliseconds for others</remarks>
            public byte LatencyTimer;

            /// <summary>
            /// Configuration options.
            /// </summary>
            public uint ConfigOptions;
        }

        /// <summary>
        /// I2C standard clockrates.
        /// </summary>
        /// <remarks>Alternatively a value for a non-standard clock rate may be passed directly.</remarks>
        public class ClockRate
        {
            /// <summary>
            /// Standard clock mode - 100KHz.
            /// </summary>
            public const uint I2C_CLOCK_STANDARD_MODE = 100000;

            /// <summary>
            /// Fast clock mode - 400KHz.
            /// </summary>
            public const uint I2C_CLOCK_FAST_MODE = 400000;

            /// <summary>
            /// Fast plus clock mode - 1Mhz.
            /// </summary>
            public const uint I2C_CLOCK_FAST_MODE_PLUS = 1000000;

            /// <summary>
            /// High seed clock mode - 3.4MHz
            /// </summary>
            public const uint I2C_CLOCK_HIGH_SPEED_MODE = 3400000;
        }

        /// <summary>
        /// Configuration option flags.
        /// </summary>
        public class ConfigOptions
        {
            /// <summary>
            /// Bit 0 set disables 3 Phase Clocking.
            /// </summary>
            /// <remarks>Only availabel for Hi-speed devices (FT232H, FT2232H, FT4232H).</remarks>
            public const uint I2C_DISABLE_3PHASE_CLOCKING = 0x00000001;

            /// <summary>
            /// Bit 1 set enables Drive Only Zero.
            /// </summary>
            /// <remarks>Available only in a FT232H device.</remarks>
            public const uint I2C_ENABLE_DRIVE_ONLY_ZERO = 0x00000002;

            // Bits 2 to 31 reserved.
        }

        /// <summary>
        /// I2C device transfer option flags.
        /// </summary>
        public class TransferOptions
        {
            /// <summary>
            /// If set then a start condition is generated in the I2C bus before the transfer begins.
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_START_BIT = 0x00000001;

            /// <summary>
            /// If set then a stop condition is generated in the I2C bus after the transfer ends.
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_STOP_BIT = 0x00000002;

            /// <summary>
            /// If set then the function will return when a device nAcks after a byte has been transferred. (Write Option)
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_BREAK_ON_NACK = 0x00000004;

            /// <summary>
            /// If set generates a NAK for the last data byte read. (Read Option)
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_NACK_LAST_BYTE = 0x00000008;

            /// <summary>
            /// Setting this bit will invoke a multi byte I2C transfer.
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_FAST_TRANSFER_BYTES = 0x00000010;

            /// <summary>
            /// Setting this bit would invoke a multi bit transfer.
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_FAST_TRANSFER_BITS = 0x00000020;

            /// <summary>
            /// The device address parameter is ignored if this bit is set.
            /// </summary>
            public const uint I2C_TRANSFER_OPTIONS_NO_ADDRESS = 0x00000040;

            // Bits 7 to 31 reserved.
        }
    }
}
