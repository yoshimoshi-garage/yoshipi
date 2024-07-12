from .digital_io_pin import DigitalIOPin
from .connector import Connector

class GpioConnector(Connector):
    _GPIOCHIP = '/dev/gpiochip0'
    def __init__(self):        
        self.D00 = DigitalIOPin(GpioConnector._GPIOCHIP, 7) # pin 26, GPIO7
        self.D01 = DigitalIOPin(GpioConnector._GPIOCHIP, 5) # pin 29, GPIO5
        self.D02 = DigitalIOPin(GpioConnector._GPIOCHIP, 6) # pin 31, GPIO6
        self.D03 = DigitalIOPin(GpioConnector._GPIOCHIP, 13) # pin 33, GPIO13
        self.D04 = DigitalIOPin(GpioConnector._GPIOCHIP, 27) # pin 13, GPIO27 (INT)
        self.D05 = DigitalIOPin(GpioConnector._GPIOCHIP, 22) # pin 15, GPIO22 (RST)
        self.D06 = DigitalIOPin(GpioConnector._GPIOCHIP, 25) # pin 22, GPIO25 (SPI1_CS)
        self.D06 = DigitalIOPin(GpioConnector._GPIOCHIP, 12) # pin 32, GPIO12 (PWM)
        pass
