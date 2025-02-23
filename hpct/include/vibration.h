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
    int dutyCycle1 = 0;
    int dutyCycle2 = 0;
    int frequency1 = 300;
    int frequency2 = 300;
    int pin1, pin2;
    void SelectMPX(uint8_t bus);
    
public:
    Vibration(int vPin1, int vPin2);
    void setRoughness(int motor, int frequency, int force);
    void disable();
    void enable(int motor);
};


#endif
