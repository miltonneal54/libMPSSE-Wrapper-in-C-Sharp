using System;
using MPSSENet;

namespace MPSSE_Test
{
    /// <summary>
    /// Wrapper library test application.
    /// Test uses FT232H breakout module in I2C mode.
    /// X24C02P 2048 bit(256 byte) EEPROM.
    /// </summary>
    class I2CTestProgram
    {
        // Program constants.
        public const uint DeviceBufferSize = 256;
        public const byte WriteRetries = 10;
        public const byte Retries = 10;
        public const byte I2C_Slave = 0x57;
        public const byte DataOffset = 0x10;

        // Global variables.
        public static MPSSE_I2C mpsse_i2c = new MPSSE_I2C();
        public static byte[] buffer = new byte[DeviceBufferSize];
        public static MPSSE.FT_STATUS status;
        public static uint channels = 0;

        static void Main()
        {
            MPSSE_I2C.ChannelConfig channelConfig = new MPSSE_I2C.ChannelConfig();
            MPSSE.FT_DEVICE_INFO_NODE deviceInfo = new MPSSE.FT_DEVICE_INFO_NODE();
            uint address;
            byte data;

            try
            {
                status = mpsse_i2c.I2C_GetNumChannels(ref channels);
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
                status = mpsse_i2c.I2C_GetChannelInfo(i, deviceInfo);
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
            status = mpsse_i2c.I2C_OpenChannel(0);   // Using channel 1.
            if (status == MPSSE.FT_STATUS.FT_OK)
            {
                IntPtr handle = MPSSE.GetHandle();
                Console.WriteLine("Handle: {0}\tStatus: {1}", handle.ToString("x"), status.ToString());
            }

            // Set the config info and initialise the channel.
            channelConfig.ClockRate = MPSSE_I2C.ClockRate.I2C_CLOCK_STANDARD_MODE;
            channelConfig.LatencyTimer = 75;
            channelConfig.ConfigOptions = MPSSE_I2C.ConfigOptions.I2C_ENABLE_DRIVE_ONLY_ZERO;
            status = mpsse_i2c.I2C_InitChannel(channelConfig);

            // Connect a LED to GPIO AC7 and Ground.
            // It will flash 10 tens at 250 msec intervals.
            Console.WriteLine("Flashing LED, please wait....");
            for(int x = 0; x < 10; x++)
            {
                mpsse_i2c.FT_WriteGPIO(0xff, 0x80);
                System.Threading.Thread.Sleep(250);
                mpsse_i2c.FT_WriteGPIO(0xff, 0);
                System.Threading.Thread.Sleep(250);
            }

            mpsse_i2c.FT_WriteGPIO(0, 0);

            // Write 16 bytes to the EEPROM.
            for (address = 0; address < 16; address++)
            {
                Console.Write("Writing Address: {0} Data: {1}", address, address + DataOffset);
                status = WriteByte(I2C_Slave, address, (byte)(address + DataOffset));

                // Write failed, retry.
                for (int j = 0; (j < Retries) && (status != MPSSE.FT_STATUS.FT_OK); j++)
                {
                    Console.Write("\nWrite failed, retrying Address: {0}, Data: {1}", address, address + DataOffset);
                    status = WriteByte(I2C_Slave, address, (byte)(address + DataOffset));
                }
            }

            Console.WriteLine("Write to EEPROM completed, press any key to start read.");
            _ = Console.ReadKey(true);

            for (address = 0; address < 16; address++)
            {
                data = 0;
 
                status = ReadByte(I2C_Slave, address, ref data);
                for (int j = 0; ((j < Retries) && (MPSSE.FT_STATUS.FT_OK != status)); j++)
                {
                    Console.WriteLine("Read error... retrying \n");
                    status = ReadByte(I2C_Slave, address, ref data);
                }

                Console.WriteLine("Reading Address: {0} Data: {1}", address, data);
            }

            status = mpsse_i2c.I2C_CloseChannel();
        }

        static MPSSE.FT_STATUS WriteByte(byte deviceAddress, uint location, byte data)
        {
            uint bytesToTransfer = 0;
            uint bytesTransfered = 0;
            bool writeComplete = false;
            uint retry = 0;
            
            // Resolve device and register address.
            byte slaveAddress = (byte)(deviceAddress | ((location >> 7) & 0x0e));
            byte registerAddress = (byte)(location & 0xff);

            buffer[bytesToTransfer++] = registerAddress; 
            buffer[bytesToTransfer++] = data;

            status = mpsse_i2c.I2C_DeviceWrite(slaveAddress, bytesToTransfer, buffer, ref bytesTransfered,
                                               MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_START_BIT |
                                               MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_STOP_BIT);

            while (!writeComplete && retry < WriteRetries)
            {
                bytesToTransfer = 0;
                bytesTransfered = 0;
                buffer[bytesToTransfer++] = registerAddress;

                status = mpsse_i2c.I2C_DeviceWrite(slaveAddress, bytesToTransfer, buffer, ref bytesTransfered,
                                                   MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_START_BIT |
                                                   MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_BREAK_ON_NACK);

                if (status == MPSSE.FT_STATUS.FT_OK)
                {
                    writeComplete = true;
                    Console.WriteLine(" ... Write done.");
                }

                retry++;
            }

            return status;
        }

        static MPSSE.FT_STATUS ReadByte(byte deviceAddress, uint location, ref byte data)
        {
            uint bytesToTransfer = 0;
            uint bytesTransfered = 0;

            // Resolve device and register address.
            byte slaveAddress = (byte)(deviceAddress | ((location >> 7) & 0x0e));
            byte registerAddress = (byte)(location & 0xff);

            buffer[bytesToTransfer++] = registerAddress; 
            status = mpsse_i2c.I2C_DeviceWrite(slaveAddress, bytesToTransfer, buffer, ref bytesTransfered,
                                               MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_START_BIT);

            bytesToTransfer = 1;
            status = mpsse_i2c.I2C_DeviceRead(slaveAddress, bytesToTransfer, buffer, ref bytesTransfered,
                                              MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_START_BIT |
                                              MPSSE_I2C.TransferOptions.I2C_TRANSFER_OPTIONS_NACK_LAST_BYTE);
            data = buffer[0];
            return status;
        }
    }
}
