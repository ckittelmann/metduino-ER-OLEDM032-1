# netduino-ER-OLEDM032-1
Sample project for ER-OLEDM032-1 powered by netduino 3 wi-fi

This is a quick and dirty (and very slow) implementation of conways game of life using a ER-OLEDM032-1 oled display.

## Here you can find the documentation to the display.
Datasheet: http://www.buydisplay.com/download/manual/ER-OLEDM032-1_Series_Datasheet.pdf  
Driver: http://www.buydisplay.com/download/ic/SSD1322.pdf  
Interface: http://www.buydisplay.com/download/interfacing/ER-OLEDM032-1_Interfacing.pdf  
Demo Code: http://www.buydisplay.com/download/democode/ER-OLEDM032-1_DemoCode.txt  

## Wiring
I'm using 4 line SPI. Therefore I had to move jumper BS1 to R19 (0) and keep BS0 at R21 (0). See Interface documentation for more informations.
| netduino port | ER-OLEDM032-1 | description |
|---|---|---|
| Digital I/O 0 | 15 | reset |
| Digital I/O 1 | 14 | data/command control |
| Digital I/O 10 | 16 | chip select |
| Digital I/O 11 | 5 | MOSI |
| Digital I/O 13 | 4 | SPCK |
| Power 5V | 2 | Power supply |
| Power GND | 1, 7, 8, 9, 10, 11, 12, 13 | gnd |
| - | 3, 6 | not connected |

## Code
I ported the demo code to .net MF and the display was running.
One byte transfered to the display controller represents 2 pixels. Every pixel has 16 brightness levels (off, 1, 2, 3 .. 15 or 0x00 - 0x0F/0xF0).

## Project
Its only a sample project for others who want to use the same display. I have no plans to continue or update this project.
