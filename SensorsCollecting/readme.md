Это скетч для платы Arduino Ethernet.

При старте плата производит подготовительные действия по инициализации датчиков и получению IP-адреса в сети
======UPD: для экономии памяти сейчас IP-адрес установлен в конкретное значение и не получается по DHCP
После этого начинается broadcast-отправка данных по UDP на порт 4444
Первоначально отправляются только два вида данных: таймштамп раз в секунду, и сообщение "iamarduino" раз в
десять секунд.

Плата принимает команды по UDP на порту 5555
Ответом на каждую команду являются broadcast (по умолчанию) UDP - сообщения
"<repl>текст_ответа"
В зависимости от команды возможны дополнительные сообщения и действия


Воспринимаемые команды:

0 - запрос на рестарт. Выдается дополнительный вопрос следующего вида:
REALLY RESET?
Если следующая командой будет 6, плата перезапустится и исполнит все пользовательские процедуры запуска.
При этом НЕ ПРОИСХОДИТ переинициализации оборудования, сброса регистров или переключение питания
Для более глубокого перезапуска платы следует перезагружать ее по питанию

1 - запрос на состояние по сбору данных и их отправке в сеть. Выдается ответ следующего вида:
data broadcasting is OFF

2 - запрос на переключение отправки данных и выдается описание нового состояния:
data broadcasting is ON

3 - запрос на смену периода выдачи данных акселерометра. Выдается текущее значение периода в следующем виде:
accel period: 500
Из следующей команды плата получает новое значение периода (в мс) и устанавливает его вместо старого.
После этого выдается сообщение, описывающее новый период, следующего вида:
accel period: 700

4 - запрос на смену IP-адреса эксклюзивного получателя данных по UDP. Выдается текущее значение адреса
и состояние отправки эксклюзивно следующего вида:
sending to this IP is OFF
curr comp IP: 0.0.0.0
По умолчанию данные отправляются неэксклюзивно - бродкастом. Этому состоянию соответствует выключенная (OFF)
эксклюзивная отсылка данных.
Следующая команда должна содержать полный IP-адрес и только его. Адрес устанавливается вместо старого.
После этого выдается сообщение, описывающее эксклюзивную отправку данных следующего вида:
sending to this IP is OFF
curr comp IP: 192.168.192.152

5 - переключает эксклюзивную отправку данных. В качестве ответа выдается сообщение, описывающее
эксклюзивную отправку данных следующего вида:
sending to this IP is ON
curr comp IP: 192.168.192.152

6 - запрос на выдачу всей статусной информации. Выдается ответ следующего вида:
sending to this IP is ON
curr comp IP: 192.168.192.152
accel period: 500
data broadcasting is OFF

7 - запрос на обновление данных GPS. При этом новые данные сразу выдаются в поток данных,
не дожидаясь очередного периода выдачи данных GPS


8 - команда на обновление данных GPS и выдачу этих данных в поток выдачи данных.
Результат - строка в потоке выдачи данных в формате NMEA:
$GPGGA,175611.00,5540.62348,N,03734.11197,E,1,06,1.78,188.6,M,13.4,M,,*53


9 - запрос на смену локального IP-адреса самой платы. Выдается текущее значение локального адреса:
self IP: 192.168.192.244
следующая команда должна содержать полный новый IP-адрес платы (и только его!). Адрес устанавливается
в качестве текущего вместо старого. Перезапускается система приема и передачи сообщений по сети, выдается
сообщение с указанием значения нового локального адреса:
self IP: 192.168.192.228
Заводские настройки подразумевают адрес по умолчанию 192.168.192.228
Каждый раз, когда устанавливается новый локальный адрес, он записывается в EEPROM платы (сохраняется во время
отключения питания). При последующей загрузке или перезагрузке платы (soft RESET, см. команду 0) локальный адрес
считывается из EEPROM. При этом НЕ ПРОВЕРЯЕТСЯ конфликт адресов в сети, подсеть и другие параметры инфраструктуры.
Напомню, что отправка данных (в том числе и маркера-идентификатора для обнаружения) по умолчанию ведется
в режиме бродкаста, что облегчает обнаружение и определение текущего IP-адреса платы.
ВНИМАНИЕ! Очевидно, что после смены локального IP-адреса команды, отправляемые по старому IP не принимаются!
Эти команды принимаются уже по новому IP-адресу!




!!! Не забывать установить в мониторе порта COM скорость 9600. Ну или как минимум ту же самую, на которой
инициализирован этот порт в исполняемом скетче.
!!! В настройках VS2012 в разделе VisualMicro параметр Arduino 1.0.x Sketchbook следует оставлять пустым, иначе
среда не находит пользовательские библиотеки, сложенные в папку по умолчанию
======UPD: сейчас эти настройки зарыты сюда:
Tools - Options... - Visual Micro - раздел Applications & Locations - пункт Application IDE locations
Задаются местоположения для необходимыъ IDE. Для Arduino сейчас используется 1.0.x

!!! Чтобы пользовательские библиотеки были видны в IDE VS2012, они по умолчанию складываются в папку
<UserHomeDir>\Documents\Arduino\libraries\
Например:
C:\Users\krinitsky\Documents\Arduino\libraries\