namespace MPSSENet
{
    public partial class MPSSE_SPI
    {
        /// <summary>
        /// Type class that holds the parameters used for initializing a channel.
        /// </summary>
        public class ChannelConfig
        {
            /// <summary>
            /// Value of the clock rate of the SPI bus in hertz.
            /// </summary>
            /// <remarks>Valid range for ClockRate is 0 to 30MHz.</remarks>
            public uint ClockRate;

            /// <summary>
            /// Latency timer in millseconds.
            /// </summary>
            ///  <remarks>FT2232D 2 - 255 milliseconds, 1 - 255 milliseconds for others</remarks>
            public byte LatencyTimer;

            /// <summary>
            /// Configuration options.
            /// </summary>
            public uint ConfigOptions;

            /// <summary>
            /// Specifies the directions and values of the lines associated with the lower byte of the MPSSE channel after SPI_InitChannel and SPI_CloseChannel functions are called.
            /// </summary>
            public uint Pins;

            /// <summary>
            /// This parameter is reserved and should not be used.
            /// </summary>
            public uint Reserved;
        }

        /// <summary>
        /// Config options.
        /// </summary>
        public class ConfigOptions
        {
            /// <summary>
            /// SPI MODE0.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_MODE0 = 0;

            /// <summary>
            /// SPI MODE1.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_MODE1 = 0x00000001;

            /// <summary>
            /// SPI MODE1.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_MODE2 = 0x00000002;

            /// <summary>
            /// SPI MODE1.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_MODE3 = 0x00000003;

            /// <summary>
            /// xDBUS3 of MPSSE is chip select.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_DBUS3 = 0;

            /// <summary>
            /// xDBUS4 of MPSSE is chip select.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_DBUS4 = 0x00000004;

            /// <summary>
            /// xDBUS5 of MPSSE is chip select.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_DBUS5 = 0x00000008;

            /// <summary>
            /// xDBUS6 of MPSSE is chip select.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_DBUS6 = 0x0000000c;

            /// <summary>
            /// xDBUS7 of MPSSE is chip select.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_DBUS7 = 0x00000010;

            /// <summary>
            /// Chip select is active low.
            /// </summary>
            public const uint SPI_CONFIG_OPTION_CS_ACTIVELOW = 0x00000020;

            // Bits 6 to 31 reserved.
        }

        /// <summary>
        /// Transfer option for SPI read, write and read/write operations.
        /// </summary>
        public class TransferOptions
        {
            /// <summary>
            /// Transfer size in bits.
            /// </summary>
            /// <remarks>Bit 0 reset.</remarks>
            public const uint SPI_TRANSFER_OPTIONS_SIZE_IN_BYTES = 0;

            /// <summary>
            /// Transfer size in bytes.
            /// </summary>
            /// <remarks>Bit 0 set.</remarks>
            public const uint SPI_TRANSFER_OPTIONS_SIZE_IN_BITS = 0x00000001;

            /// <summary>
            /// Chip select line is asserted before beginning the transfer.
            /// </summary>
            /// <remarks>Bit1 set.</remarks>
            public const uint SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE = 0x00000002;

            /// <summary>
            /// Chip select line is disserted after the transfer ends.
            /// </summary>
            /// <remarks>Bit 2 set.</remarks>
            public const uint SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE = 0x00000004;

            // Bits 3 to 31 reserved.
        }
    }
}
