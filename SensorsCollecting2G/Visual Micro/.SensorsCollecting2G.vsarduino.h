#ifndef _VSARDUINO_H_
#define _VSARDUINO_H_
//Board = Arduino Ethernet
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
void init_GPS();
void writeTo(int device, byte address, byte val);
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
void reportDevID();
void reportOverallStatus();
void serialEvent();
void ProcessGPSstring();
bool reportCurrentGPSdata();
void reportCurrentIP();
bool UdpBroadcastSend(String messageIn, bool replyMessage);
String CurrentDevIDString();
bool reportAccGyroData();
int int2Char(long num, char str[]);
int int2Char(int num, char str[]);
int calcFreeRAM();
void reportFreeRam();
void reportFreeRamToSerial();

#include "C:\Program Files (x86)\Arduino\hardware\arduino\cores\arduino\arduino.h"
#include "C:\Program Files (x86)\Arduino\hardware\arduino\variants\standard\pins_arduino.h" 
#include "C:\_gulevlab\SkyIndexAnalyzerSolo\SensorsCollecting2G\SensorsCollecting2G.ino"
#endif
