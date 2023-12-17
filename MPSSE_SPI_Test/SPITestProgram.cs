using System;
using MPSSENet;

namespace MPSSE_SPITest
{
    /// <summary>
    /// Wrapper library test application.
    /// Test uses FT232H breakout module in SPI mode.
    /// CAT93C46P 1024 bit (128 byte) EEPROM.
    /// Using the eeprom in 16bit memory mode (ORG pin pulled high)
    /// </summary>
    class SPITestProgram
    {
        // Program constants.
        public const uint DeviceBufferSize = 128;
        public const byte WriteRetries = 10;
        public const byte Retries = 10;
        public const byte SPI_Slave = 0;
        public const byte DataOffset = 0x10;

        // Global variables.
        public static MPSSE_SPI mpsse_spi = new MPSSE_SPI();
        public static byte[] buffer = new byte[DeviceBufferSize];
        public static MPSSE.FT_STATUS status;
        public static uint channels = 0;
        
        static void Main()
        {
            MPSSE_SPI.ChannelConfig channelConfig = new MPSSE_SPI.ChannelConfig();
            MPSSE.FT_DEVICE_INFO_NODE deviceInfo = new MPSSE.FT_DEVICE_INFO_NODE();
            byte address;
            ushort data;

            try
            {
                status = mpsse_spi.SPI_GetNumChannels(ref channels);
                if (status == MPSSE.FT_STATUS.FT_OK)
                {
                    Console.WriteLine("Number of available I2C channels = {0}\n", channels);
                }
            }

            catch (DllNotFoundException e)
            {
                Console.WriteLine(e.Message);
                _ = Console.ReadKey();
                Environment.Exit(1);
            }

            // Display the channel info.
            for (uint i = 0; i < channels; i++)
            {
                status = mpsse_spi.SPI_GetChannelInfo(i, deviceInfo);
                Console.WriteLine("Information on channel {0}.", i + 1);
                Console.WriteLine("\tFlags: {0}", deviceInfo.Flags);
                Console.WriteLine("\tType: {0}", deviceInfo.Type);
                Console.WriteLine("\tID: {0}", deviceInfo.ID);
                Console.WriteLine("\tLocId: {0}", deviceInfo.LocId);
                Console.WriteLine("\tSerial Number: {0}", deviceInfo.SerialNumber);
                Console.WriteLine("\tDescription: {0}", deviceInfo.Description);
                Console.WriteLine("\tHandle:{0} ", deviceInfo.Handle); // Is 0 unless it is open.
            }

            // Open the the channel.
            status = mpsse_spi.SPI_OpenChannel(0);   // Using channel 1.
            if (status == MPSSE.FT_STATUS.FT_OK)
            {
                IntPtr handle = MPSSE.GetHandle();
                Console.WriteLine("Handle: {0}\tStatus: {1}", handle.ToString("x"), status.ToString());
            }

            // Set the config info and initialise the channel.
            channelConfig.ClockRate = 500000;
            channelConfig.LatencyTimer = 255;
            channelConfig.ConfigOptions = MPSSE_SPI.ConfigOptions.SPI_CONFIG_OPTION_MODE0 | MPSSE_SPI.ConfigOptions.SPI_CONFIG_OPTION_CS_DBUS3;
            channelConfig.Pins = 0x00000000;
            status = mpsse_spi.SPI_InitChannel(channelConfig);

            // Connect a LED to GPIO AC0 and Ground.
            // It will flash 10 times at 250 msec intervals.
            /*Console.WriteLine("Flashing LED, please wait....");
            for (int x = 0; x < 10; x++)
            {
                mpsse_i2c.FT_WriteGPIO(0xff, 1);
                System.Threading.Thread.Sleep(250);
                mpsse_i2c.FT_WriteGPIO(0xff, 0);
                System.Threading.Thread.Sleep(250);
            }

            mpsse_i2c.FT_WriteGPIO(0, 0);*/

            // Write 16 bytes to the EEPROM.
            for (address = 0; address < 16; address++)
            {
                Console.WriteLine("Writing Address: {0} Data: {1}", address, address + DataOffset);
                status = WriteByte(SPI_Slave, address, (ushort)(address + DataOffset));
            }

            Console.WriteLine("Write to EEPROM completed, press any key to start read.");
            _ = Console.ReadKey(true);

            for (address = 0; address < 16; address++)
            {
                data = 0;

                status = ReadByte(SPI_Slave, address, ref data);
                Console.WriteLine("Reading Address: {0} Data: {1}", address, data);
            }

            status = mpsse_spi.SPI_CloseChannel();
        }

        static MPSSE.FT_STATUS WriteByte(byte slaveAddress, byte address, ushort data)
        {
            uint sizeToTransfer = 0;
            uint sizeTransfered = 0;
            int retry = 0;
            bool state = true;

            // Write Enable Command.
            // Write command EWEN(with CS_High -> CS_Low).
            sizeToTransfer = 9;
            sizeTransfered = 0;
            buffer[0] = 0x9f;   // EWEN command -> binary 10011xxxx (9bits).
            buffer[1] = 0xff;
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE);

            // CS_High + Write command + Address.
            sizeToTransfer = 1;
            sizeTransfered = 0;
            buffer[0] = 0xa0;                           // Write command (3bits).
            buffer[0] |= (byte)((address >> 1) & 0x0f); // 5 most significant add bits.
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BYTES |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE);

            // Write 3 least sig address bits.
            sizeToTransfer = 3;
            sizeTransfered = 0;
            buffer[0] = (byte)((address & 0x07) << 7); // 3 least significant address bits.
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS);

            // Write 2 byte data + CS_Low.
            sizeToTransfer = 2;
            sizeTransfered = 0;

            buffer[0] = (byte)(data & 0xff);
            buffer[1] = (byte)((data & 0xff00) >> 8);
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BYTES |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE);

            // Strobe Chip Select and wait for DO to go high.
            sizeToTransfer = 0;
            sizeTransfered = 0;
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE);

            //System.Threading.Thread.Sleep(10);
            
            status = mpsse_spi.SPI_IsBusy(ref state);
             while (state && (retry < WriteRetries))
             {
                 Console.WriteLine("SPI device is busy({0})\n", retry);
                 status = mpsse_spi.SPI_IsBusy(ref state);
                 retry++;
             }

            sizeToTransfer = 0;
            sizeTransfered = 0;
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE);
            // Write Disable Command.
            // Write command EWDSN(with CS_High -> CS_Low).
            sizeToTransfer = 9;
            sizeTransfered = 0;
            buffer[0] = 0x87;   // EWDS Command -> binary 10000xxxx (9bits)
            buffer[1] = 0xff;
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE); 

            return status;
        }

        static MPSSE.FT_STATUS ReadByte(byte slaveAddress, byte address, ref ushort data)
        {
            uint sizeToTransfer = 0;
            uint sizeTransfered;
            

            // CS_High + Write command + Address.
            sizeToTransfer = 1;
            sizeTransfered = 0;
            buffer[0] = 0xc0;   // Read command (3bits).
            buffer[0] |= (byte)((address >> 1) & 0x0f);    /* 5 most significant add bits */
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BYTES |
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_ENABLE);

            // Write partial address bits + dummy 0 bit.
            sizeToTransfer = 4;
            sizeTransfered = 0;
            buffer[0] = (byte)((address & 0x07) << 7); // Least significant 1 address bit.
            status = mpsse_spi.SPI_Write(buffer, sizeToTransfer, ref sizeTransfered,
                                         MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BITS);

            
            // Read 2 bytes.
            sizeToTransfer = 2;
            sizeTransfered = 0;

            status = mpsse_spi.SPI_Read(buffer, sizeToTransfer, ref sizeTransfered,
                                        MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_SIZE_IN_BYTES |
                                        MPSSE_SPI.TransferOptions.SPI_TRANSFER_OPTIONS_CHIPSELECT_DISABLE);

            data = (ushort)(buffer[1] << 8);
            data = (ushort)((data & 0xff00) | (0x00ff & buffer[0]));

            return status;
        }
    }
}
