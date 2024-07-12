import gpiod
from gpiod.line import Direction, Value

# binding for examples are here: https://github.com/brgl/libgpiod/blob/master/bindings/python/

class Resistor(Enum):
    DISABLED = 0
    INTERNAL_PULL_UP = 1
    INTERNAL_PULL_DOWN = 2


class DigitalInputPort:

    def __init__(self, chip, pin_number, resistor=Resistor.DISABLED):
        self._pin_number = pin_number
        bias = Bias.DISABLED
        if resistor == Resistor.PULL_UP:
            bias = Bias.PULL_UP 
        elif resistor == Resistor.INTERNAL_PULL_DOWN:
            bias = Bias.PULL_DOWN
                
        self._request = gpiod.request_lines(chip, consumer="yoshipi",
            config={
                pin_number: gpiod.LineSettings(
                    direction=Direction.INPUT,
                    bias=bias
                )
            })

    def getState(self):
        return self._request.get_value(self._pin_number) == Value.ACTIVE

class DigitalOutputPort:
    def __init__(self, chip, pin_number):
        self._pin_number = pin_number
        self._request = gpiod.request_lines(chip, consumer="yoshipi",
            config={
                pin_number: gpiod.LineSettings(
                    direction=Direction.OUTPUT,
                    output_value=Value.INACTIVE
                )
            })

        self._lastState = False
    
    def setState(self, state):
        if state == True:
            self._request.set_value(self._pin_number, Value.ACTIVE)
            self._lastState = True
        else:
            self._request.set_value(self._pin_number, Value.INACTIVE)
            self._lastState = False
    
    def getState(self):
        return self._lastState


class DigitalIOPin:
    def __init__(self, chip, pin_number):
        self._chip = chip
        self._pin_number = pin_number

    def getOutputPort(self):
        return DigitalOutputPort(self._chip, self._pin_number)
