EESchema Schematic File Version 4
EELAYER 29 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title ""
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Connector:Conn_01x02_Male J1
U 1 1 5CAD7DCC
P 1850 1550
F 0 "J1" H 1822 1432 50  0000 R CNN
F 1 "MBUS" H 1822 1523 50  0000 R CNN
F 2 "Pin_Headers:Pin_Header_Straight_2x01_Pitch2.54mm" H 1850 1550 50  0001 C CNN
F 3 "~" H 1850 1550 50  0001 C CNN
	1    1850 1550
	-1   0    0    1   
$EndComp
$Comp
L RF_Module:ESP32-WROOM-32 U1
U 1 1 5CADA975
P 5800 2500
F 0 "U1" H 5800 919 50  0000 C CNN
F 1 "ESP32-WROOM-32" H 5800 1010 50  0000 C CNN
F 2 "RF_Module:ESP32-WROOM-32" H 5800 1000 50  0001 C CNN
F 3 "https://www.espressif.com/sites/default/files/documentation/esp32-wroom-32_datasheet_en.pdf" H 5500 2550 50  0001 C CNN
	1    5800 2500
	-1   0    0    1   
$EndComp
$Comp
L Device:R R47K1
U 1 1 5CADD3DD
P 3100 1600
F 0 "R47K1" H 3170 1646 50  0000 L CNN
F 1 "R" H 3170 1555 50  0000 L CNN
F 2 "Resistors_THT:R_Axial_DIN0207_L6.3mm_D2.5mm_P7.62mm_Horizontal" V 3030 1600 50  0001 C CNN
F 3 "~" H 3100 1600 50  0001 C CNN
	1    3100 1600
	1    0    0    -1  
$EndComp
$Comp
L Device:R R4K7
U 1 1 5CAE138D
P 3100 2100
F 0 "R4K7" H 3170 2146 50  0000 L CNN
F 1 "R" H 3170 2055 50  0000 L CNN
F 2 "Resistors_THT:R_Axial_DIN0207_L6.3mm_D2.5mm_P7.62mm_Horizontal" V 3030 2100 50  0001 C CNN
F 3 "~" H 3100 2100 50  0001 C CNN
	1    3100 2100
	1    0    0    -1  
$EndComp
Wire Wire Line
	1850 1450 3100 1450
Wire Wire Line
	1850 1550 2650 1550
$Comp
L power:GND #PWR0101
U 1 1 5CAE2832
P 3100 2800
F 0 "#PWR0101" H 3100 2550 50  0001 C CNN
F 1 "GND" H 3105 2627 50  0000 C CNN
F 2 "" H 3100 2800 50  0001 C CNN
F 3 "" H 3100 2800 50  0001 C CNN
	1    3100 2800
	1    0    0    -1  
$EndComp
Wire Wire Line
	3100 2800 3100 2350
Wire Wire Line
	3100 1750 3100 1850
Wire Wire Line
	3100 1850 4550 1850
Wire Wire Line
	4550 1850 4550 1500
Wire Wire Line
	4550 1500 5200 1500
Connection ~ 3100 1850
Wire Wire Line
	3100 1850 3100 1950
Wire Wire Line
	2650 1550 2650 2350
Wire Wire Line
	2650 2350 3100 2350
Connection ~ 3100 2350
Wire Wire Line
	3100 2350 3100 2250
$Comp
L power:GND #PWR0102
U 1 1 5CAECE61
P 6750 1100
F 0 "#PWR0102" H 6750 850 50  0001 C CNN
F 1 "GND" V 6755 972 50  0000 R CNN
F 2 "" H 6750 1100 50  0001 C CNN
F 3 "" H 6750 1100 50  0001 C CNN
	1    6750 1100
	0    -1   -1   0   
$EndComp
Wire Wire Line
	5800 1100 6750 1100
Wire Wire Line
	5200 2700 4950 2700
Wire Wire Line
	4950 2700 4950 1800
Wire Wire Line
	4950 1800 5200 1800
$EndSCHEMATC
