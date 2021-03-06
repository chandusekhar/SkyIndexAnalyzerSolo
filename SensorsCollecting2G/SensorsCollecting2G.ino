﻿//#include <CommunicationUtils.h>
//#include <Fat16.h>
//#include <Fat16util.h>
//#include <utility.h>
#include <unwind-cxx.h>
//#include <system_configuration.h>
//#include <DebugUtils.h>
#include <CommunicationUtils.h>
#include <EEPROM.h>
#include <Ethernet.h>
#include <stdlib.h>
#include <EthernetUdp.h>
//#include <Ethernet.h>
//#include <util.h>
#include <SPI.h>
#include <Wire.h>
#include <SoftwareSerial.h>
//#include <inc_Vector.ino>
//#include <StandardCplusplus.h>
//#include <string>
//#include <vector>
//#include <iterator>

#define CurrDevID 2

#define GYRO 0x68         // gyro I2C address
#define REG_GYRO_X 0x1D   // IMU-3000 Register address for GYRO_XOUT_H
#define ACCEL 0x53        // Accel I2c Address
#define ADXL345_POWER_CTL 0x2D

byte buffer[12];   // Array to store ADC values 
int gyro_x;
int gyro_y;
int gyro_z;
int accel_x;
int accel_y;
int accel_z;
//int i;

#define localPort 5555
#define RemoteBroadcastPort 4444
//#define TCP_TX_PACKET_MAX_SIZE 64
EthernetUDP Udp;

short maxUDPMsgLength = 16;

//EthernetServer ethSrv = EthernetServer(23);
//EthernetClient ethClnt;
bool broadcastSensorsData = false;

long timecntr = 0;
int accelPeriod = 500;

int savedCommand = 0;
bool savedCommandExists = false;
bool IP2sendIsSet = false;
byte ipAddrComp[4] = { 0, 0, 0, 0 };


//String currentGPSstrBuf = "";
//String GPSstoredString = "";

String plusSign = String("<+>");
String plusplusSign = String("<++>");

void(*resetFunc) (void) = 0;



//template<typename Data>
//class Vector
//{
//	size_t d_size; // Stores no. of actually stored objects
//	size_t d_capacity; // Stores allocated capacity
//	Data *d_data; // Stores data
//public:
//	Vector() : d_size(0), d_capacity(0), d_data(0) {}; // Default constructor
//	Vector(Vector const &other) : d_size(other.d_size), d_capacity(other.d_capacity), d_data(0) { d_data = (Data *)malloc(d_capacity*sizeof(Data)); memcpy(d_data, other.d_data, d_size*sizeof(Data)); }; // Copy constuctor
//	~Vector() { free(d_data); }; // Destructor
//	Vector &operator=(Vector const &other) { free(d_data); d_size = other.d_size; d_capacity = other.d_capacity; d_data = (Data *)malloc(d_capacity*sizeof(Data)); memcpy(d_data, other.d_data, d_size*sizeof(Data)); return *this; }; // Needed for memory management
//	void push_back(Data const &x) { if (d_capacity == d_size) resize(); d_data[d_size++] = x; }; // Adds new value. If needed, allocates more space
//	size_t size() const { return d_size; }; // Size getter
//	Data const &operator[](size_t idx) const { return d_data[idx]; }; // Const getter
//	Data &operator[](size_t idx) { return d_data[idx]; }; // Changeable getter
//private:
//	void resize() { d_capacity = d_capacity ? d_capacity * 2 : 1; Data *newdata = (Data *)malloc(d_capacity*sizeof(Data)); memcpy(newdata, d_data, d_size * sizeof(Data)); free(d_data); d_data = newdata; };// Allocates double the old space
//};



void setup()
{
	byte mac[] = { 0x90, 0xA2, 0xDA, 0x0D, 0x2B, 0xC2 };
	String error1 = "";
	Serial.begin(9600);

	Serial.println(F("START"));

	byte savedIP[4] = { 0, 0, 0, 0 };
	if (readSelfIPdata(savedIP))
	{
		Ethernet.begin(mac, IPAddress(savedIP[0], savedIP[1], savedIP[2], savedIP[3]));
	}
	else
	{
		Ethernet.begin(mac, IPAddress(192, 168, 192, 228));
	}

	Udp.begin(localPort);

	Wire.begin();
	delay(5);

	
	init_sensors();
	
	// test
	//init_GPS();
	

	String myIPstr = "my IP: ";
	for (byte thisByte = 0; thisByte < 4; thisByte++)
	{
		myIPstr += String(Ethernet.localIP()[thisByte]);
		if (thisByte < 3) myIPstr += ".";
	}
	//Serial.println(myIPstr);
	UdpBroadcastSend(myIPstr, true);

	timecntr = millis();
}


//
//void init_GPS()
//{
//	//pinMode(3, OUTPUT);//The default digital driver pins for the GSM and GPS mode
//	//pinMode(4, OUTPUT);
//	//pinMode(5, OUTPUT);
//	//digitalWrite(5, HIGH);
//	//delay(1500);
//	//digitalWrite(5, LOW);
//
//	//digitalWrite(3, LOW);//Enable GSM mode
//	//digitalWrite(4, HIGH);//Disable GPS mode
//	//delay(2000);
//
//	Serial.begin(9600);
//	delay(1000);//GPS ready
//
//	UdpBroadcastSend(String("GPS start stage 1"), false);
//	Serial.println("AT");
//	UdpBroadcastSend(String("GPS start stage 1 passed"), false);
//	delay(1000);
//
//	UdpBroadcastSend(String("request software version"), false);
//	Serial.println("AT+GSV");
//	UdpBroadcastSend(String("request software version passed"), false);
//	delay(1000);
//
//
//	//turn off SLEEP mode
//	UdpBroadcastSend(String("SLEEP mode disabling"), false);
//	Serial.println("AT+CSCLK=0");
//	UdpBroadcastSend(String("SLEEP mode disabling passed"), false);
//	delay(1000);
//
//
//	//turn on GPS power supply
//	UdpBroadcastSend(String("GPS start stage 2"), false);
//	Serial.println("AT+CGPSPWR=1");
//	UdpBroadcastSend(String("GPS start stage 2 passed"), false);
//	delay(1000);
//
//
//	/*UdpBroadcastSend(String("request CGPSOUT"), false);
//	Serial.println("AT+CGPSOUT?");
//	UdpBroadcastSend(String("request CGPSOUT passed"), false);
//	delay(1000);*/
//
//
//	// set GPGGA only NMEA strings
//	/*UdpBroadcastSend(String("GPS start stage 3"), false);
//	Serial.println("AT+CGPSOUT=255");
//	UdpBroadcastSend(String("GPS start stage 3 passed"), false);
//	delay(1000);*/
//
//	//reset GPS in autonomy mode
//	UdpBroadcastSend(String("GPS start stage 4"), false);
//	Serial.println("AT+CGPSRST=1");
//	UdpBroadcastSend(String("GPS start stage 4 passed"), false);
//	delay(1000);
//
//
//
//	//digitalWrite(4, LOW);//Enable GPS mode
//	//digitalWrite(3, HIGH);//Disable GSM mode
//	//delay(2000);
//
//	/*String str2Send = "$GPGGA statement information: ";
//	UdpBroadcastSend(str2Send, false);*/
//}
//



void writeTo(int device, byte address, byte val)
{
	Wire.beginTransmission(device); // start transmission to device 
	Wire.write(address);             // send register address
	Wire.write(val);                 // send value to write
	Wire.endTransmission();         // end transmission
}




void init_sensors()
{
	//int error = 0;
	String error1 = "";

	// Set Gyro settings
	// Sample Rate 1kHz, Filter Bandwidth 42Hz, Gyro Range 250 d/s 
	writeTo(GYRO, 0x16, 0x0B);
	//set accel register data address
	writeTo(GYRO, 0x18, 0x32);
	// set accel i2c slave address
	writeTo(GYRO, 0x14, ACCEL);

	// Set passthrough mode to Accel so we can turn it on
	writeTo(GYRO, 0x3D, 0x08);
	// set accel power control to 'measure'
	writeTo(ACCEL, ADXL345_POWER_CTL, 8);
	//cancel pass through to accel, gyro will now read accel for us   
	writeTo(GYRO, 0x3D, 0x28);
}






void loop()
{
	int TimePeriod = 1000; long timeTimer = 0;//ms
	long accelTimer = 0;//ms
	int gyroPeriod = 100; long gyroTimer = 0;//ms
	int arduinoIdentityBcstPeriod = 5000; long arduinoIdentityBcstTimer = 0;//ms
	int GPSPeriod = 1000; long GPSTimer = 0;//ms


	String incomingString = "";
	int command = 0;
	//loop_cnt++;

	while (true)
	{
		char UDPmessage[UDP_TX_PACKET_MAX_SIZE];
		if (getUDPmessage(UDPmessage))
		{
			incomingString = String(UDPmessage);
			if (!savedCommandExists)
			{
				command = incomingString.toInt();
				performCommand(command);
				command = 0;
			}
			else
			{
				performSavedCommand(UDPmessage);
				savedCommandExists = false;
				savedCommand = 0;
				break;
			}
			//tmpString = "";
		}





		timecntr = millis();
		if (timecntr - timeTimer >= TimePeriod)
		{
			String str1 = "timer:";
			//char tcc[32];
			//char marker[6] = {'t','i','m','e','r',':' };
			//strncpy(tcc, marker, sizeof(marker));
			char longDigStr[20];
			memset(&longDigStr[0], 0, sizeof(longDigStr));
			int2Char(timecntr, longDigStr);
			str1 += String(longDigStr);
			//strncpy(&tcc[0]+sizeof(marker), longDigStr, sizeof(longDigStr));
			//UdpBroadcastSendCharArray(tcc);
			UdpBroadcastSend(str1, false);

			timeTimer = timecntr;
		}


		if ((timecntr - accelTimer >= accelPeriod) && (broadcastSensorsData))
		{
			reportAccGyroData();
			accelTimer = timecntr;
		}





		if (timecntr - arduinoIdentityBcstTimer >= arduinoIdentityBcstPeriod)
		{
			UdpBroadcastSend(String("imarduino"), false);
			arduinoIdentityBcstTimer = timecntr;
		}


		// test
		/*if ((timecntr - GPSTimer >= GPSPeriod) && (broadcastSensorsData))
		{
			reportCurrentGPSdata();
			GPSTimer = millis();
		}*/

		
		/*if (Serial.available() > 0)
		{
			serialEvent();
		}*/
	}
}



bool getUDPmessage(char UDPstr[UDP_TX_PACKET_MAX_SIZE])
{
	char packetBuffer[UDP_TX_PACKET_MAX_SIZE];
	memset(&packetBuffer[0], 0, sizeof(packetBuffer));

	int packetSize = Udp.parsePacket();
	if (packetSize == 0)
	{
		//tmpString = String("");
		memset(&UDPstr[0], 0, sizeof(UDPstr));
		return false;
	}

	Udp.read(packetBuffer, UDP_TX_PACKET_MAX_SIZE);
	String s = String(packetBuffer);
	//tmpString = String(packetBuffer);
	s.trim();
	s.toCharArray(UDPstr, UDP_TX_PACKET_MAX_SIZE);
	//Serial.println(UDPstr);


	memset(&packetBuffer[0], 0, sizeof(packetBuffer));

	return true;
}





bool performCommand(int command)
{
	if (command == 0)
	{
		UdpBroadcastSend(String("REALLY RESET?"), true);
		savedCommandExists = true;
		savedCommand = 0;
		return true;
	}

	if (command == 1)
	{
		reportStatus();
		return true;
	}

	if (command == 2)
	{
		switchBcstStatus();
		return true;
	}

	if (command == 3)
	{
		changeAccPeriod();
		return true;
	}

	if (command == 4)
	{
		aboutToChangeCurrentIP();
		return true;
	}

	if (command == 5)
	{
		IP2sendIsSet = !IP2sendIsSet;
		reportCurrentIP();
	}

	if (command == 6)
	{
		reportOverallStatus();
		return true;
	}


	//if (command == 7)
	//{
	//	//probeGPSdata();
	//	return true;
	//}

	/*if (command == 8)
	{
		aboutToSendSerialCommand();
		return true;
	}*/


	if (command == 9)
	{
		aboutToChangeSelfIP();
		return true;
	}




	return false;
}






//void aboutToSendSerialCommand()
//{
//	UdpBroadcastSend(String("input GPS command"), true);
//	savedCommandExists = true;
//	savedCommand = 8;
//}





bool performSavedCommand(char str[UDP_TX_PACKET_MAX_SIZE])
{
	if (savedCommand == 0)
	{
		//int newValue = tmpString.toInt();
		int newValue = atoi(str);
		if (newValue == 6) resetFunc();
		savedCommandExists = false;
		savedCommand = 0;
		return true;
	}

	if (savedCommand == 3)
	{
		//int newValue = tmpString.toInt();
		int newValue = atoi(str);
		accelPeriod = newValue;
		reportAccelPeriod();
		savedCommandExists = false;
		savedCommand = 0;
		return true;
	}

	if (savedCommand == 4)
	{
		SetCurrIPAddrData(str);
		reportCurrentIP();
		savedCommandExists = false;
		savedCommand = 0;
		return true;
	}



	if (savedCommand == 8)
	{
		Serial.println("AT");
		delay(500);
		Serial.println(str);
		delay(500);

		savedCommandExists = false;
		savedCommand = 0;
		return true;
	}



	if (savedCommand == 9)
	{
		SetSelfIPAddrData(str);
		reportSelfIP();
		savedCommandExists = false;
		savedCommand = 0;
		return true;
	}


	return false;
}




void aboutToChangeCurrentIP()
{
	reportCurrentIP();
	savedCommandExists = true;
	savedCommand = 4;
}



void aboutToChangeSelfIP()
{
	reportSelfIP();
	savedCommandExists = true;
	savedCommand = 9;
}



void reportSelfIP()
{
	String s = String("self IP: ");
	for (int i = 0; i < 4; i++)
	{
		s += String((int)Ethernet.localIP()[i]);
		if (i < 3) s += ".";
	}
	UdpBroadcastSend(s, true);
}




void changeAccPeriod()
{
	reportAccelPeriod();
	savedCommandExists = true;
	savedCommand = 3;
}




bool SetSelfIPAddrData(char str[UDP_TX_PACKET_MAX_SIZE])
{
	//String str2Parse = String(tmpString);
	byte mac[] = { 0x90, 0xA2, 0xDA, 0x0D, 0x2B, 0xC1 };
	byte newSelfIP[4] = { 0, 0, 0, 0 };
	String str2Parse = String(str);
	int index = -1;
	int i = 0;
	//Serial.println(str2Parse);

	while (true)
	{
		index = str2Parse.indexOf('.');
		if (index == -1)
		{
			newSelfIP[i] = str2Parse.toInt();
			Udp.stop();
			Ethernet.begin(mac, IPAddress(newSelfIP[0], newSelfIP[1], newSelfIP[2], newSelfIP[3]));
			Udp.begin(localPort);
			//reportFreeRamToSerial();

			recordSelfIPdata(newSelfIP);

			return true;
		}
		String tmp1 = str2Parse.substring(0, index);
		newSelfIP[i] = tmp1.toInt();
		str2Parse = str2Parse.substring(index + 1);
		i++;
	}
	return true;
}





void recordSelfIPdata(byte newSelfIP[4])
{
	for (short i = 0; i < 4; i++) EEPROM.write(i, newSelfIP[i]);
}



bool readSelfIPdata(byte newSelfIP[4])
{
	byte b = 0;
	for (short i = 0; i < 4; i++)
	{
		b = EEPROM.read(i);
		if (b == 255) return false;
		newSelfIP[i] = b;
	}
	return true;
}





bool SetCurrIPAddrData(char str[UDP_TX_PACKET_MAX_SIZE])
{
	//String str2Parse = String(tmpString);
	String str2Parse = String(str);
	int index = -1;
	int i = 0;
	//Serial.println(str2Parse);

	while (true)
	{
		index = str2Parse.indexOf('.');
		if (index == -1)
		{
			ipAddrComp[i] = str2Parse.toInt();
			return true;
		}
		//String tmp1 = ;
		ipAddrComp[i] = str2Parse.substring(0, index).toInt();
		str2Parse = str2Parse.substring(index + 1);
		i++;
	}
	return true;
}


void switchBcstStatus()
{
	broadcastSensorsData = !broadcastSensorsData;
	reportStatus();
}



void reportAccelPeriod()
{
	String s = String("accel period: ");
	s += accelPeriod;
	UdpBroadcastSend(s, true);
}


void reportStatus()
{
	String s = String("data broadcasting is ");
	if (broadcastSensorsData) s += "ON"; else s += "OFF";
	UdpBroadcastSend(s, true);
}


void reportDevID()
{
	String s = "current dev.ID: ";
	s += String(CurrDevID);
	UdpBroadcastSend(s, true);
}




void reportOverallStatus()
{
	reportStatus();
	reportAccelPeriod();
	reportCurrentIP();
	reportFreeRam();
	reportSelfIP();
	reportDevID();
}


//
//void serialEvent()
//{
//	String gotSerialStr = "";
//	while (Serial.available() > 0)
//	{
//		// get the new byte:
//		char inChar = (char)Serial.read();
//
//		if (inChar == '\n')
//		{
//			// currentGPSstrBuf += gotSerialStr;
//			// ProcessGPSstring();
//			if (gotSerialStr.length() > 0)
//			{
//				UdpBroadcastSend(gotSerialStr, false);
//				gotSerialStr = "";
//				return;
//				break;
//			}
//			continue;
//		}
//		else
//		{
//			gotSerialStr += String(inChar);
//			if (gotSerialStr.length() >= 32)
//			{
//				UdpBroadcastSend(gotSerialStr, false);
//				gotSerialStr = "";
//				return;
//				break;
//			}
//		}
//	}
//
//	/*String str1 = String("Serial event at ") + millis();
//	UdpBroadcastSend(str1, true);*/
//
//	/*gotSerialStr = String("<gps>") + gotSerialStr;
//	UdpBroadcastSend(gotSerialStr, false);*/
//
//	/*currentGPSstrBuf += gotSerialStr;
//	if (currentGPSstrBuf.length() >= 128)
//	{
//	ProcessGPSstring();
//	}*/
//}
//
//
//
//void ProcessGPSstring()
//{
//	if (currentGPSstrBuf.substring(0, 7) == String("$GPGGA,"))
//	{
//		int strLen = currentGPSstrBuf.length();
//
//		if (currentGPSstrBuf.substring(strLen - 3, strLen - 2) != "*")
//		{
//			GPSstoredString = currentGPSstrBuf;
//			//UdpBroadcastSend(currentGPSstrBuf, false);
//		}
//	}
//
//	currentGPSstrBuf = "";
//}
//
//
//
//
//
//bool reportCurrentGPSdata()
//{
//	if (GPSstoredString[0])
//	{
//		if (GPSstoredString.length() >= 32)
//		{
//			GPSstoredString = GPSstoredString.substring(0, 32);
//		}
//		UdpBroadcastSend(GPSstoredString, false);
//		GPSstoredString = "";
//	}
//
//	return 1;
//}
//


void reportCurrentIP()
{
	String s = String("curr comp IP: ");
	for (int i = 0; i < 4; i++)
	{
		s += String((int)ipAddrComp[i]);
		if (i < 3) s += ".";
	}
	UdpBroadcastSend(s, true);

	s = String("sending to this IP is ");
	if (IP2sendIsSet) s += "ON"; else s += "OFF";
	UdpBroadcastSend(s, true);
}





//void SerialNote(String messageString)
//{
//	char CharReply[64];
//	messageString.toCharArray(CharReply, 64);
//	Serial.println(CharReply);
//}






bool UdpBroadcastSendSingleMessage(String messageIn, bool whetherOpenConn = true, bool whetherCloseConn = true)
{
	/*Serial.println(F("sending:"));
	SerialNote(messageIn);*/

	EthernetUDP bcstUDP;

	/*if (whetherOpenConn)
	{*/
		Udp.stop();
		bcstUDP.begin(localPort);
	//}

	char bcstCharArray[64];
	memset(&bcstCharArray[0], 0, sizeof(bcstCharArray));


	messageIn.toCharArray(bcstCharArray, sizeof(bcstCharArray));
	if (IP2sendIsSet)
	{
		IPAddress theIP(ipAddrComp[0], ipAddrComp[1], ipAddrComp[2], ipAddrComp[3]);
		bcstUDP.beginPacket(theIP, RemoteBroadcastPort);
	}
	else
	{
		IPAddress bcstIP(255, 255, 255, 255);
		bcstUDP.beginPacket(bcstIP, RemoteBroadcastPort);
	}

	bcstUDP.write(bcstCharArray);

	int packetSended = bcstUDP.endPacket();

	
	/*if (packetSended == 0)
	{
		Serial.print(F("error sending broadcast packet\n"));
	}*/
	/*else
	{
		Serial.println(F("sent packet successfully\n"));
	}*/


	memset(&bcstCharArray[0], 0, sizeof(bcstCharArray));

	//if (whetherCloseConn)
	//{
		bcstUDP.stop();
		Udp.begin(localPort);
	//}
}






bool UdpBroadcastSend(String messageIn, bool replyMessage)
{
	
	String msgRest = String(messageIn);

	if (replyMessage)
	{
		msgRest = String("<repl>") + msgRest;
	}

	bool multipleMessages = (msgRest.length() > maxUDPMsgLength);
	
	if (multipleMessages)
	{
		String msgBoundleID = String(millis());
		int msgSubID = 0;
		while (msgRest.length() > maxUDPMsgLength)
		{
			String msg2Send = msgRest.substring(0, maxUDPMsgLength);
			msg2Send = CurrentDevIDString() + msg2Send;

			//SerialNote(String("msgSubID = ") + String(msgSubID));

			if (msgSubID == 0)
			{
				msg2Send = plusplusSign + String("<mid:") + msgBoundleID + String("><sub:") + String(msgSubID) + String(">") + msg2Send + plusSign;
			}
			else
			{
				msg2Send = plusSign + String("<mid:") + msgBoundleID + String("><sub:") + String(msgSubID) + String(">") + msg2Send + plusSign;
			}
			
			if (msgSubID == 0)
			{
				UdpBroadcastSendSingleMessage(msg2Send, true, false);
			}
			else
			{
				UdpBroadcastSendSingleMessage(msg2Send, false, false);
			}
			
			msgSubID++;

			msgRest = msgRest.substring(maxUDPMsgLength);
		}

		//остаток
		if (msgRest.length() > 0)
		{
			String msg2Send = plusSign + String("<mid:") + msgBoundleID + String("><sub:") + String(msgSubID) + String(">") + CurrentDevIDString() + msgRest + plusplusSign;
			UdpBroadcastSendSingleMessage(msg2Send);
		}
	}
	else
	{
		msgRest = CurrentDevIDString() + msgRest;
		UdpBroadcastSendSingleMessage(msgRest);
	}
}








String CurrentDevIDString()
{
	String retStr = String("<id");
	retStr += String(CurrDevID);
	retStr += ">";
	return retStr;
}



bool reportAccGyroData()
{
	//char str2send[64];

	Wire.beginTransmission(GYRO);
	Wire.write(REG_GYRO_X); //Register Address GYRO_XOUT_H
	Wire.endTransmission();

	Wire.beginTransmission(GYRO);
	Wire.requestFrom(GYRO, 12); // Read 12 bytes
	int i = 0;
	while (Wire.available())
	{
		buffer[i] = Wire.read();
		i++;
	}
	Wire.endTransmission();


	gyro_x = buffer[0] << 8 | buffer[1];
	gyro_y = buffer[2] << 8 | buffer[3];
	gyro_z = buffer[4] << 8 | buffer[5];

	accel_y = buffer[7] << 8 | buffer[6];
	accel_x = buffer[9] << 8 | buffer[8];
	accel_z = buffer[11] << 8 | buffer[10];

	String s = String("<dta>");
	s += String(accel_x) + ";";
	s += String(accel_y) + ";";
	s += String(accel_z) + ";";
	//s.toCharArray(str2send, 64);
	UdpBroadcastSend(s, false);



	//memset(&str2send[0], 0, sizeof(str2send));
	s = String("<dtg>");
	s += String(gyro_x) + ";";
	s += String(gyro_y) + ";";
	s += String(gyro_z) + ";";

	//s.toCharArray(str2send, 64);
	UdpBroadcastSend(s, false);

	return true;
}




int int2Char(long num, char str[])
{
	String tmps = String(num, DEC);
	tmps.toCharArray(str, 20);
	return tmps.length();
}


int int2Char(int num, char str[])
{
	String tmps = String(num, DEC);
	tmps.toCharArray(str, 10);
	return tmps.length();
}




int calcFreeRAM()
{
	extern int __heap_start, *__brkval;
	char v;
	int freeRam = (int)&v - (__brkval == 0 ? (int)&__heap_start : (int)__brkval);
	return freeRam;
}



void reportFreeRam()
{
	String s = "free RAM: ";
	//char str2send[64]; memset(&str2send[0], 0, sizeof(str2send));
	//char marker[16] = {'<','r','e','p','l','>','F','r','e','e',' ','R','A','M',':',' '};
	//strncpy(str2send, marker, sizeof(marker));
	char digStr[10]; memset(&digStr[0], 0, sizeof(digStr));
	int2Char(calcFreeRAM(), digStr);
	s += String(digStr);
	//strncpy(&str2send[0]+sizeof(marker), digStr, sizeof(digStr));

	UdpBroadcastSend(s, true);
	//UdpBroadcastSend(String("Free RAM: ") + String(calcFreeRAM()), true);
}



//void reportFreeRamToSerial()
//{
//	Serial.println(F("### free RAM: "));
//	Serial.println(calcFreeRAM());
//}




void probeGPSdata()
{
	// AT command to request current GPS info
	UdpBroadcastSend(String("requesting current GPS info"), false);
	Serial.println("AT+CGPSOUT=255");
}
