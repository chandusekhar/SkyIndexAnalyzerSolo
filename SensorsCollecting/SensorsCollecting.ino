//#include <CommunicationUtils.h>
//#include <Fat16.h>
//#include <Fat16util.h>

#include <EEPROM.h>
#include <Ethernet.h>
#include <stdlib.h>
#include <EthernetUdp.h>
#include <util.h>
#include <SPI.h>
#include <Wire.h>
//#include <FreeSixIMU.h>
//#include <FIMU_ADXL345.h> //for accel
//#include <FIMU_ITG3200.h> //for gyroscope
//#include <HMC5883L.h>
#include <SoftwareSerial.h>
#include "C:/Program Files (x86)/Arduino/libraries/HMC5883L/HMC5883L.h"
#include "C:/Program Files (x86)/Arduino/libraries/FreeSixIMU/FreeSixIMU.h"
#include "C:/Program Files (x86)/Arduino/libraries/FreeSixIMU/FIMU_ADXL345.h"
#include "C:/Program Files (x86)/Arduino/libraries/FreeSixIMU/FIMU_ITG3200.h"

//#define CurrDevID 1

//EthernetClient client;
const int led = 9;
//const int S1btn = 8;
#define localPort 5555
#define RemoteBroadcastPort 4444
EthernetUDP Udp;
bool broadcastSensorsData = false;

long timecntr = 0;
int accelPeriod = 500; 

HMC5883L compass = HMC5883L();
int savedCommand = 0;
bool savedCommandExists = false;
bool IP2sendIsSet = false;
byte ipAddrComp[4] = {0, 0, 0, 0};


FreeSixIMU sixDOF = FreeSixIMU();


char currentGPSstrBuf[128];
#define GPSRXPIN 0
#define GPSTXPIN 1

void(* resetFunc) (void) = 0;

void setup()
{
	byte mac[] = {0x90, 0xA2, 0xDA, 0x0D, 0x2B, 0xC1};
	String error1 = "";
	Serial.begin(9600);

	Serial.println(F("START"));

	//reportFreeRamToSerial();



	/*IPAddress myIPaddr(192,168,192,228);
	IPAddress DNSarrd(192,168,192,1);*/
	byte savedIP[4] = {0,0,0,0};
	if (readSelfIPdata(savedIP))
	{
		Ethernet.begin(mac, IPAddress(savedIP[0],savedIP[1],savedIP[2],savedIP[3]));
	}
	else
	{
		Ethernet.begin(mac, IPAddress(192,0,0,101));
	}

	//reportFreeRamToSerial();

	//if (Ethernet.begin(mac) == 0)
	//{
	//	Serial.println(F("Failed to configure Ethernet using DHCP"));
	//	IPAddress myIPaddr(192,168,192,228);
	//	IPAddress DNSarrd(192,168,192,1);
	//	Ethernet.begin(mac, myIPaddr, DNSarrd);
	//	//ErrorOn = true;
	//}
	//IPAddress myIPaddr = Ethernet.localIP();
	//Ethernet.begin(mac, myIPaddr);

	//reportFreeRamToSerial();

	Udp.begin(localPort);

	Wire.begin();
	delay(5);

	init_sensors();

	/*pinMode(led, OUTPUT);
	digitalWrite(led, HIGH);
	pinMode(S1btn, INPUT);*/

	String myIPstr = "my IP: ";
	for (byte thisByte = 0; thisByte < 4; thisByte++)
	{
		myIPstr += String(Ethernet.localIP()[thisByte]);
		if (thisByte < 3) myIPstr += ".";
	}
	Serial.println(myIPstr);
	UdpBroadcastSend(myIPstr, true);

	timecntr = millis();
}



void init_sensors()
{
	int error = 0;
	String error1 = "";

	sixDOF.init(true); //init the Acc and Gyro
	delay(5);

	//compass = HMC5883L(); // init HMC5883
	//error = compass.SetScale(1.3); // Set the scale of the compass.
	//if(error != 0)
	//{
	//	error1 = String("<err>") + String(compass.GetErrorText(error));
	//	Serial.println(error1);
	//	UdpBroadcastSend(error1, false);
	//	delay(50);
	//}
	//error = compass.SetMeasurementMode(Measurement_Continuous); // Set the measurement mode to Continuous
	//if(error != 0)
	//{
	//	error1 = String("<err>") + String(compass.GetErrorText(error));
	//	Serial.println(error1);
	//	UdpBroadcastSend(error1, false);
	//	delay(50);
	//}

	bmp085Calibration(); // init barometric pressure sensor
}




//void initConfigFile()
//{
//	SdCard card;
//	Fat16 file;
//
//	if (!card.init()) Serial.println(F("error card.init"));
//	if (!Fat16::init(&card)) Serial.println(F("error Fat16::init"));
//
//	if (file.open("config.txt", O_READ))
//	{
//		Serial.println(F("config file opened"));
//	}
//
//	int16_t n;
//	char buf[32];
//	while ((n = file.read(buf, sizeof(buf))) > 0)
//	{
//		Serial.println(buf);
//	}
//
//}





void loop()
{
	int TimePeriod = 1000; long timeTimer = 0;//ms
	int PTperiod = 10000; long PTtimer = 0;//ms
	long accelTimer = 0;//ms
	int gyroPeriod = 100; long gyroTimer = 0;//ms
	//int magnPeriod = 1000; long magnTimer = 0;//ms
	//int LEDPeriod = 500; long LEDTimer = 0; uint8_t LEDval = LOW;
	int arduinoIdentityBcstPeriod = 5000; long arduinoIdentityBcstTimer = 0;//ms
	//uint8_t S1btnInputValue = LOW;// = digitalRead(S1btn);
	int GPSPeriod = 1000; long GPSTimer = 0;//ms
	int GPSprobingPeriod = 10000;
	long GPSprobingTimer = 0;


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
			/*char str2send[32];
			memset(&str2send[0], 0, sizeof(str2send));
			char marker[5] = {'<','d','t','p','>'};
			strncpy(str2send, marker, sizeof(marker));
			char pressStr[20];
			memset(&pressStr[0], 0, sizeof(pressStr));
			int2Char(pressure, pressStr);
			strncpy(&str2send[0]+sizeof(marker), pressStr, sizeof(pressStr));
			UdpBroadcastSendCharArray(str2send);*/


			char tcc[32];
			char marker[6] = {'t','i','m','e','r',':' };
			strncpy(tcc, marker, sizeof(marker));
			char longDigStr[20];
			memset(&longDigStr[0], 0, sizeof(longDigStr));
			int2Char(timecntr, longDigStr);
			strncpy(&tcc[0]+sizeof(marker), longDigStr, sizeof(longDigStr));
			UdpBroadcastSendCharArray(tcc);

			timeTimer = timecntr;
		}

		if ((timecntr - PTtimer >= PTperiod) && (broadcastSensorsData))
		{
			reportPressureData();
			PTtimer = timecntr;
		}

		if ((timecntr - accelTimer >= accelPeriod) && (broadcastSensorsData))
		{
			//sixDOF.getRawValues(raw6DOF);
			//UdpBroadcastSend(AccData(), false);
			//UdpBroadcastSend(GyroData(), false);
			report6DOFData();
			accelTimer = timecntr;
		}


		//if ((timecntr - magnTimer >= magnPeriod) && (broadcastSensorsData))
		//{
		//	//getHeading();
		//	//UdpBroadcastSend(MagnetometerData(), false);
		//	reportMagnetometerData();
		//	magnTimer = timecntr;
		//}


		if (timecntr - arduinoIdentityBcstTimer >= arduinoIdentityBcstPeriod)
		{
			UdpBroadcastSend(String("imarduino"), false);
			arduinoIdentityBcstTimer = timecntr;
		}



		/*if (timecntr - LEDTimer >= LEDPeriod)
		{
			LEDval = ledSwitch(LEDval);
		}*/

		/*uint8_t newS1btnInputValue = digitalRead(S1btn);
		if (newS1btnInputValue != S1btnInputValue)
		{
			S1btnInputValue = newS1btnInputValue;
			UdpBroadcastSend(String("======btn S1 switched====="), true);
			UdpBroadcastSend(String("S1 val = ") + String(S1btnInputValue), true);
		}*/



		if ((timecntr - GPSTimer >= GPSPeriod) && (broadcastSensorsData))
		{
			//performGPSactions();
			reportCurrentGPSdata();
			GPSTimer = millis();
		}


		if ((broadcastSensorsData) && (timecntr - GPSprobingTimer >= GPSprobingPeriod))
		{
			//probe GPS data
			probeGPSdata();
			GPSprobingTimer = timecntr;
		}
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
	Serial.println(UDPstr);

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


	if (command == 7)
	{
	probeGPSdata();
	return true;
	}

	if (command == 8)
	{
	probeGPSdata();
	reportCurrentGPSdata();
	return true;
	}


	if (command == 9)
	{
		aboutToChangeSelfIP();
		return true;
	}


	return false;
}


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
		if (i<3) s += ".";
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
	byte mac[] = {0x90, 0xA2, 0xDA, 0x0D, 0x2B, 0xC1};
	byte newSelfIP[4] = {0, 0, 0, 0};
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
			Ethernet.begin(mac, IPAddress(newSelfIP[0],newSelfIP[1],newSelfIP[2],newSelfIP[3]));
			Udp.begin(localPort);
			//reportFreeRamToSerial();

			recordSelfIPdata(newSelfIP);

			return true;
		}
		String tmp1 = str2Parse.substring(0, index);
		newSelfIP[i] = tmp1.toInt();
		str2Parse = str2Parse.substring(index+1);
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
		str2Parse = str2Parse.substring(index+1);
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


void reportOverallStatus()
{
	reportStatus();
	reportAccelPeriod();
	reportCurrentIP();
	reportFreeRam();
	reportSelfIP();
}


bool reportCurrentGPSdata()
{
	if (currentGPSstrBuf[0])
	{
		UdpBroadcastSendCharArray(currentGPSstrBuf);
	}

	return true;
}


void reportCurrentIP()
{
	String s = String("curr comp IP: ");
	for (int i = 0; i < 4; i++)
	{
		s += String((int)ipAddrComp[i]);
		if (i<3) s += ".";
	}
	UdpBroadcastSend(s, true);

	s = String("sending to this IP is ");
	if (IP2sendIsSet) s += "ON"; else s += "OFF";
	UdpBroadcastSend(s, true);
}



//void SerialNote(String messageString)
//{
//	char CharReply[32];
//	messageString.toCharArray(CharReply, 32);
//	Serial.println(CharReply);
//}





//uint8_t ledSwitch(bool prevValue)
//{
//	if (prevValue == LOW) prevValue = HIGH; else prevValue = LOW;
//	digitalWrite(led, prevValue);
//	return prevValue;
//}


//
//String CurrentDevIDString()
//{
//	String retStr = String("<id");
//	retStr += String(CurrDevID);
//	retStr += ">";
//	return retStr;
//}



bool UdpBroadcastSend(String messageIn, bool reply)
{
	//short RemoteBroadcastPort = 4444;

	char bcstCharArray[128];
	memset(&bcstCharArray[0], 0, sizeof(bcstCharArray));
	Udp.stop();
	EthernetUDP bcstUDP;
	bcstUDP.begin(localPort);

	String msgMarks = String("<id1>");
	if (reply)
	{
		msgMarks = "<repl>" + msgMarks;
	}

	String message = msgMarks + messageIn;
	
	message = message.substring(0, 127);
	message.toCharArray(bcstCharArray, 128);

	//Serial.println(bcstCharArray);

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
	bcstUDP.stop();
	Udp.begin(localPort);

	if (packetSended == 0)
	{
		Serial.print(F("error sending broadcast packet\n"));
	}
}


bool UdpBroadcastSendCharArray(char messageIn[])
{
	//Serial.println(messageIn);

	Udp.stop();
	EthernetUDP bcstUDP;
	bcstUDP.begin(localPort);

	String str1 = "<id1>" + String(messageIn);

	char mgs2Send[128];
	memset(&mgs2Send[0], 0, sizeof(mgs2Send));
	str1.toCharArray(mgs2Send, sizeof(mgs2Send));
	//char marker[5] = {'<','i','d','1','>' };
	//strncpy(mgs2Send, marker, sizeof(marker));
	//strncpy(&mgs2Send[5], messageIn, sizeof(messageIn));
	
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
	bcstUDP.write(mgs2Send);
	int packetSended = bcstUDP.endPacket();
	bcstUDP.stop();
	Udp.begin(localPort);

	if (packetSended == 0)
	{
		Serial.print(F("error sending broadcast packet\n"));
	}
}







//String doubleToString(double val, short signs)
//{
//	int mult = 1;
//	for (int i = 0; i < signs; i++)
//	{
//		mult *= 10;
//	}
//
//	int valInt = (int)val;
//	String outStr = String(valInt) + ".";
//	valInt = (int)((val-valInt) * mult);
//	if (valInt < 0)
//	{
//		valInt *= -1;
//	}
//	outStr += String(valInt);
//	return outStr;
//}




//String AccDataEuler(float *accDataEuler)
//{
//	String s = String("<dae>");
//	s += floatToString(accDataEuler[0]) + "; ";
//	s += floatToString(accDataEuler[1]) + "; ";
//	s += floatToString(accDataEuler[2]) + ";";
//	return s;
//}



bool report6DOFData()
{
	int raw6DOF[6];
	sixDOF.getRawValues(raw6DOF);
	char str2send[64];

	String s = String("<dta>");
	s += String(raw6DOF[0]) + ";";
	s += String(raw6DOF[1]) + ";";
	s += String(raw6DOF[2]) + ";";

	s.toCharArray(str2send, 64);
	UdpBroadcastSendCharArray(str2send);



	memset(&str2send[0], 0, sizeof(str2send));
	s = String("<dtg>");
	s += String(raw6DOF[3]) + ";";
	s += String(raw6DOF[4]) + ";";
	s += String(raw6DOF[5]) + ";";

	s.toCharArray(str2send, 64);
	UdpBroadcastSendCharArray(str2send);

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


bool reportPressureData()
{
	//short temperature = bmp085GetTemperature(bmp085ReadUT());
	long pressure = bmp085GetPressure(bmp085ReadUP());

	char pressStr[20];
	memset(&pressStr[0], 0, sizeof(pressStr));
	int2Char(pressure, pressStr);

	String s = "<dtp>";
	s += String(pressStr);

	char str2send[64];
	memset(&str2send[0], 0, sizeof(str2send));

	s.toCharArray(str2send, 32);


	UdpBroadcastSendCharArray(str2send);
	return true;
}

//
//bool reportMagnetometerData()
//{
//	MagnetometerRaw mgnetRaw = compass.ReadRawAxis();
//	String s = "<dtm>";
//
//	char str2send[64];
//	memset(&str2send[0], 0, sizeof(str2send));
//
//	s += String(mgnetRaw.XAxis) + ";";
//	s += String(mgnetRaw.YAxis) + ";";
//	s += String(mgnetRaw.ZAxis) + ";";
//
//	s.toCharArray(str2send, 64);
//
//	/*char marker[5] = {'<','d','t','m','>'};
//	strncpy(str2send, marker, sizeof(marker));
//
//	char digStr[10];
//	memset(&digStr[0], 0, sizeof(digStr));
//	int charCountX = int2Char(mgnetRaw.XAxis, digStr);
//	strncpy(&str2send[0]+sizeof(marker), digStr, charCountX);
//
//	str2send[sizeof(marker)+charCountX] = ';';
//
//	memset(&digStr[0], 0, sizeof(digStr));
//	int charCountY = int2Char(mgnetRaw.YAxis, digStr);
//	strncpy(&str2send[0]+sizeof(marker)+charCountX+1, digStr, charCountY);
//
//	str2send[sizeof(marker)+charCountX+1+charCountY] = ';';
//
//	memset(&digStr[0], 0, sizeof(digStr));
//	int charCountZ = int2Char(mgnetRaw.ZAxis, digStr);
//	strncpy(&str2send[0]+sizeof(marker)+charCountX+1+charCountY+1, digStr, charCountZ);*/
//
//	UdpBroadcastSendCharArray(str2send);
//	return true;
//}
//



int calcFreeRAM()
{
	extern int __heap_start, *__brkval;
	char v;
	int freeRam = (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval);
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



void reportFreeRamToSerial()
{
	Serial.println(F("### free RAM: "));
	Serial.println(calcFreeRAM());
}





bool probeGPSdata()
{
	String GPSstrBuf = "";
	char c = 0;
	SoftwareSerial nss(GPSRXPIN, GPSTXPIN);
	long begin = millis();
	long dt = 0;
	nss.begin(9600);
	bool foundDollarSign = false;
	bool gpggaMarker = false;

	while (true)
	{
		dt = millis()-begin;
		if (dt > 10000)
		{
			UdpBroadcastSend(String("=== GPS timeout ==="), true);
			return false;
		}

		c = nss.read();
		if (c<1) continue;
		if (c == '$')
		{
			GPSstrBuf = String(c);
			if (!foundDollarSign) foundDollarSign = true;
			continue;
		}


		if ((foundDollarSign) && (!gpggaMarker))
		{
			GPSstrBuf += c;
			if (GPSstrBuf.length() == 7)
			{
				if (GPSstrBuf.startsWith(String("$GPGGA,")))
				{
					gpggaMarker = true;
				}
				else
				{
					gpggaMarker = false;
					foundDollarSign = false;
					GPSstrBuf = "";
					continue;
				}				
			}
			continue;
		}


		if (c == '\n')
		{
			if (gpggaMarker)
			{
				//the end of right sentence
				break;
			}
			foundDollarSign = false;
			gpggaMarker = false;
		}

		if (gpggaMarker)
		{
			GPSstrBuf += String(c);
			if (GPSstrBuf.length() == 128)
			{
				break;
			}
		}
	}

	nss.end();
	memset(&currentGPSstrBuf[0], 0, sizeof(currentGPSstrBuf));
	GPSstrBuf.toCharArray(currentGPSstrBuf, 128);
	return true;
}


//int GPSchecksum(char s[])
//{
//	int c = 0;
//	char i = 0;
//
//	while(s[i])
//	{
//		if (s[i] == '$') continue;
//		c ^= s[i];
//		i++;
//	}
//
//	return c;
//}
