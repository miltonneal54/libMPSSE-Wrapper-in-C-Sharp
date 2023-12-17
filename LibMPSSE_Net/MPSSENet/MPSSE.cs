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
using System.Text;

namespace MPSSENet
{
    /// <summary>
    /// Base class for the libMPSSE wrapper.
    /// </summary>
    public partial class MPSSE
    {
        internal static IntPtr handle = IntPtr.Zero;

        /// <summary>
        /// Gets the handle to the current open channel.
        /// </summary>
        /// <returns>A pointer to the handle or IntPtr.Zero if the channel has not been opened.</returns>
        public static IntPtr GetHandle()
        {
            return handle;
        }

        /// <summary>
        /// Writes to the 8 GPIO lines associated with the high byte of the MPSSE channel.
        /// </summary>
        /// <param name="direction">Each bit of this byte represents the direction of the 8 respective GPIO lines.</param>
        /// <param name="value">A byte that represents the output logic state of the 8 respective GPIO lines.</param>
        /// <returns>FT_STATUS value from FT_WriteGPIO in LibMPSSE.DLL</returns>
        public FT_STATUS FT_WriteGPIO(byte direction, byte value)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            if (handle != IntPtr.Zero)
            {
                status = MPSSE_API.FT_WriteGPIO(handle, direction, value);
            }

            return status;
        }

        /// <summary>
        /// Reads from the 8 GPIO lines associated with the high byte of the MPSSE channel.
        /// </summary>
        /// <param name="value">Pointer to a byte that represents the input logic state of the 8 respective GPIO lines</param>
        /// <returns>FT_STATUS value from FT_ReadGPIO in LibMPSSE.DLL</returns>
        public FT_STATUS FT_ReadGPIO(ref byte value)
        {
            FT_STATUS status = FT_STATUS.FT_OTHER_ERROR;

            if (handle != IntPtr.Zero)
            {
                status = MPSSE_API.FT_ReadGPIO(handle, ref value);
            }

            return status;
        }

        /// <summary>
        /// Initializes the MPSSE library.
        /// </summary>
        public void InitLibMPSSE()
        {
            MPSSE_API.Init_libMPSSE();
        }

        /// <summary>
        /// Cleans up resources used by the  MPSSE library.
        /// </summary>
        public void CleanupLibMPSSE()
        {
            MPSSE_API.Cleanup_libMPSSE();
        }

        /// <summary>
        /// Decodes all the bytes in the specified array to null terminate ASCII string.
        /// </summary>
        /// <param name="byteArray">byte array to decoded.</param>
        /// <returns>An ASCII encoded string.</returns>
        internal string GetString(byte[] byteArray)
        {
            string encodedString = Encoding.ASCII.GetString(byteArray);
            // Trim strings to first occurrence of a null terminator character.
            int nullIndex = encodedString.IndexOf("\0");
            if (nullIndex != -1)
            {
                encodedString = encodedString.Substring(0, nullIndex);
            }

            return encodedString;
        }
    }
}
