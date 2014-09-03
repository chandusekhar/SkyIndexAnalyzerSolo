/* 
	Editor: http://www.visualmicro.com
	        visual micro and the arduino ide ignore this code during compilation. this code is automatically maintained by visualmicro, manual changes to this file will be overwritten
	        the contents of the Visual Micro sketch sub folder can be deleted prior to publishing a project
	        all non-arduino files created by visual micro and all visual studio project or solution files can be freely deleted and are not required to compile a sketch (do not delete your own code!).
	        note: debugger breakpoints are stored in '.sln' or '.asln' files, knowledge of last uploaded breakpoints is stored in the upload.vmps.xml file. Both files are required to continue a previous debug session without needing to compile and upload again
	
	Hardware: Arduino Ethernet, Platform=avr, Package=arduino
*/

#ifndef _VSARDUINO_H_
#define _VSARDUINO_H_
#define __AVR_ATmega328p__
#define __AVR_ATmega328P__
#define ARDUINO 105
#define ARDUINO_MAIN
#define __AVR__
#define __avr__
#define F_CPU 16000000L
#define __cplusplus
#define __inline__
#define __asm__(x)
#define __extension__
#define __ATTR_PURE__
#define __ATTR_CONST__
#define __inline__
#define __asm__ 
#define __volatile__

#define __builtin_va_list
#define __builtin_va_start
#define __builtin_va_end
#define __DOXYGEN__
#define __attribute__(x)
#define NOINLINE __attribute__((noinline))
#define prog_void
#define PGM_VOID_P int
            
typedef unsigned char byte;
extern "C" void __cxa_pure_virtual() {;}

//
void init_sensors();
//
bool getUDPmessage(char UDPstr[UDP_TX_PACKET_MAX_SIZE]);
bool performCommand(int command);
bool performSavedCommand(char str[UDP_TX_PACKET_MAX_SIZE]);
void aboutToChangeCurrentIP();
void aboutToChangeSelfIP();
void reportSelfIP();
void changeAccPeriod();
bool SetSelfIPAddrData(char str[UDP_TX_PACKET_MAX_SIZE]);
void recordSelfIPdata(byte newSelfIP[4]);
bool readSelfIPdata(byte newSelfIP[4]);
bool SetCurrIPAddrData(char str[UDP_TX_PACKET_MAX_SIZE]);
void switchBcstStatus();
void reportAccelPeriod();
void reportStatus();
void reportOverallStatus();
bool reportCurrentGPSdata();
void reportCurrentIP();
bool UdpBroadcastSend(String messageIn, bool reply);
bool UdpBroadcastSendCharArray(char messageIn[]);
bool report6DOFData();
int int2Char(long num, char str[]);
int int2Char(int num, char str[]);
bool reportPressureData();
int calcFreeRAM();
void reportFreeRam();
void reportFreeRamToSerial();
bool probeGPSdata();
void bmp085Calibration();
short bmp085GetTemperature(unsigned int ut);
long bmp085GetPressure(unsigned long up);
char bmp085Read(unsigned char address);
int bmp085ReadInt(unsigned char address);
unsigned int bmp085ReadUT();
unsigned long bmp085ReadUP();

#include "C:\Program Files (x86)\Arduino\hardware\arduino\cores\arduino\arduino.h"
#include "C:\Program Files (x86)\Arduino\hardware\arduino\variants\standard\pins_arduino.h" 
#include "D:\_gulevlab\SkyIndexAnalyzerSolo\SensorsCollecting\SensorsCollecting.ino"
#include "D:\_gulevlab\SkyIndexAnalyzerSolo\SensorsCollecting\BMP085.ino"
#endif
