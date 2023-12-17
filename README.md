# libMPSSE-Wrapper-in-C-Sharp
C# Wrapper class for FTDI's libMPSSE Library
LibMPSSE_Net is a .NET wrapper class  (written in C# using VS2019) for the FTDI's libMPSSE library.
This class gives you access and the ability to read, write and manipulate  I2C and SPI devices from you PC or laptop running under 
the Windows OS.

FTDI's libMPSSE.dll allows interfacing with FT232H, FT2232H and FT4232H based modules.

For this project I used:
FT232H breakout single channel module.
24C02P (2048 bit) I2C EEPROM
CAT93C46 (1024 bit) SPI EEPROM

The Library consists of 3 main classes.
1. MPSSE (Base class)
2. MPSSE_I2C (Access to the I2C methods)
3. MPSSE_SPI (Access to the SPI methods)
The test projects are based on the C source in FTDI Application Notes AN177 (I2C) and AN178 (SPI).
My test devices are a smaller in capacity than used in the original source, so the code varies a little from FTDI's sample code.

Descrepancies:
I did find some bugs in the original C source in AN177 and AN178 which I corrected when ported the code to C#.
The schematic for connection to the I2C EEPROM in AN177 is incorrect. (Clock and DO/DI reversed)

Prerequisites:
The FTDI's driver FTD2xx must be installed on your system.
