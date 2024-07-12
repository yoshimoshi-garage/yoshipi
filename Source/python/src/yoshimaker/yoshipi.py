from .gpio_connector import GpioConnector

class YoshiPi:
    def __init__(self):
        self.gpio = GpioConnector()

