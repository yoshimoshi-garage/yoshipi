# Hardware v1a Validation

## Hardware Validation

- [X] 3.3V
- [X] 5.0V
- [X] Relay 1
- [X] Relay 2
- [ ] Button 1
- [ ] Button 2
- [X] MCP3008
- [X] GPIO
  - [X] D00
  - [X] D01
  - [X] D02
  - [X] D03
- [ ] Proto Pins
  - [ ] 3.3
  - [ ] GND
  - [ ] SCL
  - [ ] SDA
- [ ] DS3231 Pins
  - [ ] 3.3
  - [ ] GND
  - [ ] SCL
  - [ ] SDA
- [ ] Grove I2C
  - [ ] 3.3
  - [ ] GND
  - [ ] SCL
  - [ ] SDA
- [ ] Qwiic
  - [ ] 3.3
  - [ ] GND
  - [ ] SCL
  - [ ] SDA
- [ ] Mikrobus

## Software Validation

- [X] Relay 1
- [X] Relay 2
- [ ] Button 1
- [ ] Button 2
- [ ] GPIO Outputs
  - [ ] D00 : FAIL, yields `Failed to request line`.  `pinctrl set 7 dl` however can set the state?
  - [X] D01
  - [X] D02
  - [X] D03
- [ ] GPIO Inputs
  - [ ] D00 : FAIL, yields `Failed to request line`.
  - [X] D01
  - [X] D02
  - [X] D03
- [ ] GPIO Interrupts
  - [ ] D00 : FAIL, yields `Failed to request line`.
  - [X] D01
  - [X] D02
  - [X] D03
- [ ] RTC

