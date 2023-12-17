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
    /// Wrapper class for libMPSSE - I2C.
    /// </summary>
    public sealed partial class MPSSE_I2C : MPSSE
    {
        /// <summary>
        /// Constructor for the MPSSE I2C Class.
        /// </summary>
        public MPSSE_I2C()
        { }

        /// <summary>
        /// Gets the number of channels connected to the host system that can be configured as an I2C master.
        /// </summary>
        /// <param name="numberOfChannels">Pointer to unsigned int to stores the number of channels.</param>
        /// <returns>FT_STATUS value from I2C_GetNumChannels in libMPSSE.DLL</returns>
        public MPSSE.FT_STATUS I2C_GetNumChannels(ref uint numberOfChannels)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            status = MPSSE_API.I2C_GetNumChannels(ref numberOfChannels);
            return status;
        }

        /// <summary>
        /// Provides information about the channel referenced by index..
        /// </summary>
        /// <param name="index">Index of the channel.</param>
        /// <param name="deviceInfo">Type class FT_DEVICE_INFO_NODE that contains the device information.</param>
        /// <returns>FT_STATUS value from I2C_GetChannelInfo in libMPSSE.DLL</returns>
        public MPSSE.FT_STATUS I2C_GetChannelInfo(uint index, FT_DEVICE_INFO_NODE deviceInfo)
        {
            FT_STATUS status = MPSSE.FT_STATUS.FT_OTHER_ERROR;
            MPSSE_API.FT_DEVICE_LIST_INFO_NODE deviceInfoNode;

            // Get the size of the device info structure and allocate memory on the heap.
            int size = Marshal.SizeOf<MPSSE_API.FT_DEVICE_LIST_INFO_NODE>();
            IntPtr channelInfoPointer = Marshal.AllocHGlobal(size);

            status = MPSSE_API.I2C_GetChannelInfo(index, channelInfoPointer);
            if (status == MPSSE.FT_STATUS.FT_OK)
            {
                // Copy the native data to the managed type class
                deviceInfoNode = Marshal.PtrToStructure<MPSSE_API.FT_DEVICE_LIST_INFO_NODE>(channelInfoPointer);
                deviceInfo.Flags = deviceInfoNode.Flags;
                deviceInfo.Type = (MPSSE.FT_DEVICE)deviceInfoNode.Type;
                deviceInfo.ID = deviceInfoNode.ID;
                deviceInfo.LocId = deviceInfoNode.LocId;
                deviceInfo.SerialNumber = GetString(deviceInfoNode.SerialNumber);
                deviceInfo.Description = GetString(deviceInfoNode.Description);
                deviceInfo.Handle = deviceInfoNode.Handle;
            }

            // Free the unmanaged memory.
            Marshal.FreeHGlobal(channelInfoPointer);
            return status;
        }

        /// <summary>
        /// Opens the indexed channel and provides a handle to it.
        /// </summary>
        /// <param name="index">Index of the device to open.</param>
        /// <returns>FT_STATUS value from I2C_OpenChannel in libMPSSE.DLL</returns>
        public FT_STATUS I2C_OpenChannel(uint index)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            status =  MPSSE_API.I2C_OpenChannel(index, ref handle);
            if(status != FT_STATUS.FT_OK)
            {
                handle = IntPtr.Zero;
            }

            return status;
        }

        /// <summary>
        /// Initializes the I2C channel and the communication parameters associated with it.
        /// </summary>
        /// <param name="channelConfig">Type class holding the channel configuration data</param>
        /// <returns>FT_STATUS value from I2C_InitChannel in libMPSSE.DLL</returns>
        public FT_STATUS I2C_InitChannel(ChannelConfig channelConfig)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;
            MPSSE_API.I2C_CHANNEL_CONFIG config = new MPSSE_API.I2C_CHANNEL_CONFIG();

            if (handle != IntPtr.Zero)
            {
                // Copy the managed type class into the native structure.
                config.ClockRate = channelConfig.ClockRate;
                config.LatencyTimer = channelConfig.LatencyTimer;
                config.ConfigOptions = channelConfig.ConfigOptions;

                // Get the size of the channel config structure and allocate memory on the heap.
                int size = Marshal.SizeOf<MPSSE_API.I2C_CHANNEL_CONFIG>();
                IntPtr configPointer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr<MPSSE_API.I2C_CHANNEL_CONFIG>(config, configPointer, false);

                status = MPSSE_API.I2C_InitChannel(handle, configPointer);

                // Free the unmanaged memory.
                Marshal.FreeHGlobal(configPointer);
            }

            return status;
        }

        /// <summary>
        /// Closes the channel.
        /// </summary>
        /// <returns>FT_STATUS value from I2C_CloseChannel in libMPSSE.DLL</returns>
        public FT_STATUS I2C_CloseChannel()
        {
            FT_STATUS status = FT_STATUS.FT_DEVICE_NOT_OPENED;

            if(handle != IntPtr.Zero)
            {
                status = MPSSE_API.I2C_CloseChannel(handle);
            }

            handle = IntPtr.Zero;
            return status;
        }

        /// <summary>
        /// Reads the specified number of bytes from an addressed I2C slave.
        /// </summary>
        /// <param name="deviceAddress">Address of the I2C slave.</param>
        /// <param name="sizeToTransfer">Number of bytes to be read.</param>
        /// <param name="dataBuffer">Pointer to the buffer where data is to be read.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes read.</param>
        /// <param name="options">Specifies data transfer options.</param>
        /// <returns>FT_STATUS value from I2C_DeviceRead in libMPSSE.DLL</returns>
        public FT_STATUS I2C_DeviceRead(uint deviceAddress, uint sizeToTransfer, byte[] dataBuffer, ref uint sizeTransfered, uint options)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            if(handle != IntPtr.Zero)
            {
                status = MPSSE_API.I2C_DeviceRead(handle, deviceAddress, sizeToTransfer, dataBuffer, ref sizeTransfered, options);
            }

            return status;
        }

        /// <summary>
        /// Writes the specified number of bytes to an addressed I2C slave.
        /// </summary>
        /// <param name="deviceAddress">Address of the I2C slave.</param>
        /// <param name="sizeToTransfer">Number of bytes to be written</param>
        /// <param name="dataBuffer">Pointer to the buffer where data is to be written.</param>
        /// <param name="sizeTransfered">Pointer to variable containing the number of bytes written.</param>
        /// <param name="options">Specifies data transfer options.</param>
        /// <returns>FT_STATUS value from I2C_DeviceWrite in libMPSSE.DLL</returns>
        public FT_STATUS I2C_DeviceWrite(uint deviceAddress, uint sizeToTransfer, byte[] dataBuffer, ref uint sizeTransfered, uint options)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            if (handle != IntPtr.Zero)
            {
                status = MPSSE_API.I2C_DeviceWrite(handle, deviceAddress, sizeToTransfer, dataBuffer, ref sizeTransfered, options);
            }

            return status;
        }
    }
}
