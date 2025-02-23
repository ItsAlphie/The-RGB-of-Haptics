#ifndef vibration_h
#define vibration_h

#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_DRV2605.h>
#include <algorithm>

#define TCAADDR 0x70


class Vibration {
private:
    Adafruit_DRV2605 drv;
    int dutyCycle = 0;
    int frequency = 300;
    int pin;
    void SelectMPX(uint8_t bus);
    
public:
    Vibration(int vPin);
    void setRoughness(int frequency, int force);
    void disable();
    void enable();
};


#endif
