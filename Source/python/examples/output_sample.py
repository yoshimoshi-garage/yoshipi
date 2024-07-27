#!/usr/bin/env python3

import sys,os,time

sys.path.append(os.path.join(os.path.dirname(sys.path[0]), 'src'))

from yoshimaker.yoshipi import YoshiPi

hardware = YoshiPi()

d00 = hardware.gpio.D00.getOutputPort()
d01 = hardware.gpio.D01.getOutputPort()

state = True
while True:
    d00.setState(state)
    d01.setState(not state)

    print("tick")
    
    state = not state
    time.sleep(1)
