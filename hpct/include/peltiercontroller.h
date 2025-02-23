#ifndef peltiercontroller_h
#define peltiercontroller_h

#include <Arduino.h>


class PeltierController {
private:
    // Peltiers: polarity set to true = heating
    // bool polarity;

    // Pins (pin1 on red, pin2 on black)
    int peltierDriverPin1, peltierDriverPin2, thermistorPin;

    // Variables
    float desiredTemp, currentTemp;
    float error;
    float Kp;
    bool enabled{false};

    // Methods
    void temperatureControl();
    void calculateError();


public:
    PeltierController(int pPin1, int pPin2, int thPin, float KpVal);
    void setDesiredTemp(float temp);
    void enable();
    void disable();
    bool isEnabled();
    void readThermistor();

};



#endif


