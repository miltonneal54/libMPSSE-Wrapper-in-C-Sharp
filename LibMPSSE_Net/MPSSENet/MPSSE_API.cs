/***************************************************************************
 *   Copyright (C) 2023 by Milton Neal                                     *
 *   milton200954@gmail.com                                                *
 *                                                                         *
 *   This program is free software: you can redistribute it and/or modify  *
 *   it under the terms of the GNU General Public License as published by  *
 *   the Free Software Foundation, either version 3 of the License, or     *
 *   (at your option) any later version.                                   *
 *   This program is distributed in the hope that it will be useful,       *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of        *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the         *
 *   GNU General Public License for more details.                          *
 *   You should have received a copy of the GNU General Public License     *
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>. *
 ***************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace MPSSENet
{
    /// <summary>
    /// libMPSSE DLL API.
    /// </summary>
    internal static class MPSSE_API
    {
        #region Native Structures

        /// <summary>
        /// Native type that holds information for the device.
        /// </summary>
        /// <remarks>
        /// Used with FT_GetDeviceInfoList in FTD2XX.DLL.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct FT_DEVICE_LIST_INFO_NODE
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
            /// Can be one of the following: FT_DEVICE_232R, FT_DEVICE_2232C, FT_DEVICE_BM, FT_DEVICE_AM, FT_DEVICE_100AX or FT_DEVICE_UNKNOWN.
            /// </remarks>
            public uint Type;

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
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] SerialNumber;

            /// <summary>
            /// The device description.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] Description;

            /// <summary>
            /// The device handle.
            /// </summary>
            /// <remarks>
            /// If the device is not open, this value is 0.
            /// </remarks>
            public IntPtr Handle;
        }

        /// <summary>
        /// Structure that holds the parameters used for initializing a I2C channel.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct I2C_CHANNEL_CONFIG
        {
            /// <summary>
            /// I2C clock rate. Can be one of the standard clock rates or pass a non-standard value directly.
            /// </summary>
            public uint ClockRate;

            /// <summary>
            /// Latency timer value in milliseconds.
            /// </summary>
            public byte LatencyTimer;

            /// <summary>
            /// Device config options.
            /// </summary>
            public uint ConfigOptions;
        }

        /// <summary>
        /// Structure that holds the parameters used for initializing a SPI channel.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        internal struct SPI_CHANNEL_CONFIG
        {
            /// <summary>
            /// I2C clock rate. Can be one of the standard clock rates or pass a non-standard value directly.
            /// </summary>
            public uint ClockRate;

            /// <summary>
            /// Latency timer value in milliseconds.
            /// </summary>
            public byte LatencyTimer;

            /// <summary>
            /// Device config options.
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

        #endregion

        #region I2C Functions

        /// <summary>
        /// Gets the number of I2C channels that are connected to the host system.
        /// </summary>
        /// <param name="numberOfChannels">The number of channels connected to the host.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_GetNumChannels", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_GetNumChannels(ref uint numberOfChannels);

        /// <summary>
        /// Provides information about the channel in the form of a populated FT_DEVICE_LIST_INFO_NODE structure.
        /// </summary>
        /// <param name="index">Index of the channel.</param>
        /// <param name="channelInfo">Pointer to FT_DEVICE_LIST_INFO_NODE structure.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_GetChannelInfo", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_GetChannelInfo(uint index, IntPtr channelInfo);

        /// <summary>
        /// Opens the indexed channel and provides a handle to it.
        /// </summary>
        /// <param name="index">Index of the channel.</param>
        /// <param name="handle">Pointer to a variable of type FT_HANDLE (IntPtr) where the handle will be stored.</param>
        /// <returns> Returns status code of type FT_STATUS </returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_OpenChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_OpenChannel(uint index, ref IntPtr handle);

        /// <summary>
        ///  Initializes the channel and the communication parameters associated with it. 
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="config">Pointer to ChannelConfig structure.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        /// <remarks>ChannelConfig structure contains the values for I2C master clock, latency timer and options.</remarks>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_InitChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_InitChannel(IntPtr handle, IntPtr config);

        /// <summary>
        /// Closes a channel and frees all resources that were used by it.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_CloseChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_CloseChannel(IntPtr handle);

        /// <summary>
        /// Reads the specified number of bytes from an addressed I2C slave. 
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="deviceAddress">Address of the I2C slave.</param>
        /// <param name="sizeToTransfer">Number of bytes to be read.</param>
        /// <param name="buffer">Pointer to the buffer where data is to be read.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes read.</param>
        /// <param name="options">Specifies data transfer options.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_DeviceRead", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_DeviceRead(IntPtr handle, uint deviceAddress, uint sizeToTransfer, byte[] buffer, ref uint sizeTransfered, uint options);

        /// <summary>
        /// Writes the specified number of bytes from an addressed I2C slave. 
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="deviceAddress">Address of the I2C slave.</param>
        /// <param name="sizeToTransfer">Number of bytes to be written.</param>
        /// <param name="buffer">Pointer to the buffer that holds the data to be written.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes written.</param>
        /// <param name="options">Specifies data transfer options.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "I2C_DeviceWrite", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS I2C_DeviceWrite(IntPtr handle, uint deviceAddress, uint sizeToTransfer, byte[] buffer, ref uint sizeTransfered, uint options);

        #endregion

        #region SPI Functions

        /// <summary>
        /// Gets the number of SPI channels that are connected to the host system.
        /// </summary>
        /// <param name="numberOfChannels">The number of channels connected to the host.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_GetNumChannels", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_GetNumChannels(ref uint numberOfChannels);

        /// <summary>
        /// Provides information about the channel in the form of a populated FT_DEVICE_LIST_INFO_NODE structure.
        /// </summary>
        /// <param name="index">Index of the channel.</param>
        /// <param name="channelInfo">Pointer to FT_DEVICE_LIST_INFO_NODE structure.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_GetChannelInfo", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_GetChannelInfo(uint index, IntPtr channelInfo);

        /// <summary>
        /// Opens the indexed channel and provides a handle to it.
        /// </summary>
        /// <param name="index">Index of the channel.</param>
        /// <param name="handle">Pointer to a variable of type FT_HANDLE (IntPtr) where the handle will be stored.</param>
        /// <returns> Returns status code of type FT_STATUS </returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_OpenChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_OpenChannel(uint index, ref IntPtr handle);

        /// <summary>
        ///  Initializes the channel and the communication parameters associated with it. 
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="config">Pointer to ChannelConfig structure.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        /// <remarks>ChannelConfig structure contains the values for SPI master clock, latency timer and options.</remarks>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_InitChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_InitChannel(IntPtr handle, IntPtr config);

        /// <summary>
        /// Closes a channel and frees all resources that were used by it.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_CloseChannel", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_CloseChannel(IntPtr handle);

        /// <summary>
        /// Reads the specified number of bits or bytes (depending on transferOptions parameter) from an SPI slave. 
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="buffer">Pointer to the buffer where data is to be read.</param>
        /// /// <param name="sizeToTransfer">Number of bytes or bits to be read.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes or bits read.</param>
        /// <param name="transferOptions">Specifies data transfer options.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_Read", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_Read(IntPtr handle, byte[] buffer, uint sizeToTransfer, ref uint sizeTransfered, uint transferOptions);

        /// <summary>
        /// writes the specified number of bits or bytes (depending on transferOptions parameter) to a SPI slave.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="buffer">Pointer to the buffer that holds the data to be written.</param>
        /// /// <param name="sizeToTransfer">Number of bytes or bits to be written.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes or bits written.</param>
        /// <param name="transferOptions">Specifies data transfer options.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_Write", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_Write(IntPtr handle, byte[] buffer, uint sizeToTransfer, ref uint sizeTransfered, uint transferOptions);

        /// <summary>
        /// Reads from and writes to the SPI slave simultaneously.
        /// </summary>
        /// <param name="handle">Handle of the channel</param>
        /// <param name="inBuffer">Pointer to buffer to which data read will be stored.</param>
        /// <param name="ontBuffer">Pointer to the buffer from where data is to be written.</param>
        /// <param name="sizeToTransfer">Number of bytes or bits to write.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes or bits written.</param>
        /// <param name="transferOptions">Specifies data transfer options.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_ReadWrite", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_ReadWrite(IntPtr handle, byte[] inBuffer, byte[] ontBuffer, uint sizeToTransfer, ref uint sizeTransfered, uint transferOptions);

        /// <summary>
        /// Reads the state of the MISO line without clocking the SPI bus.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="state">Pointer to a variable which hold the state of the MISO line.</param>
        /// <returns>Returns status code of type FT_STATUS</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_IsBusy", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_IsBusy(IntPtr handle, ref bool state);

        /// <summary>
        /// Change the chip select line that is to be used to communicate to the SPI slave.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="configOption">Config option as per Channel Config.</param>
        /// <returns>Returns status code of type FT_STATUS</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "SPI_ChangeCS", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS SPI_ChangeCS(IntPtr handle, uint configOption);
        #endregion

        #region GPIO Functions

        /// <summary>
        ///  Writes to the 8 GPIO lines associated with the high byte of the MPSSE channel.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="direction">Each bit of this byte represents the direction of the 8 respective GPIO lines. 0 for in and 1 for out.</param>
        /// <param name="value">If the GPIO line is set to output, then each bit of this byte represent the output logic state of the 8 respective GPIO lines</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "FT_WriteGPIO", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS FT_WriteGPIO(IntPtr handle, byte direction, byte value);

        /// <summary>
        /// Reads from the 8 GPIO lines associated with the high byte of the MPSSE channel.
        /// </summary>
        /// <param name="handle">Handle of the channel.</param>
        /// <param name="value">If the GPIO line is set to input, then each bit of this byte represent the input logic state of the 8 respective GPIO lines.</param>
        /// <returns>Returns status code of type FT_STATUS.</returns>
        [DllImport("libMPSSE.dll", EntryPoint = "FT_ReadGPIO", CallingConvention = CallingConvention.StdCall)]
        internal static extern MPSSE.FT_STATUS FT_ReadGPIO(IntPtr handle, ref byte value);

        #endregion

        #region Library Infrastructure Functions

        /// <summary>
        /// Initializes the library.
        /// </summary>
        [DllImport("libMPSSE.dll", EntryPoint = "Init_libMPSSE", CallingConvention = CallingConvention.StdCall)]
        internal static extern void Init_libMPSSE();

        /// <summary>
        /// Cleans up resources used by the library.
        /// </summary>
        [DllImport("libMPSSE.dll", EntryPoint = "Cleanup_libMPSSE", CallingConvention = CallingConvention.StdCall)]
        internal static extern void Cleanup_libMPSSE();

        #endregion
    }
}
