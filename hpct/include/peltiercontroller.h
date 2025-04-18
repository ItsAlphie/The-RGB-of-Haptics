#ifndef peltiercontroller_h
#define peltiercontroller_h

#include <Arduino.h>

#define POINTS 10

class PeltierController
{
private:
    // Peltiers: polarity set to true = heating
    // bool polarity;

    // Pins (pin1 on red, pin2 on black)
    int peltierDriverPin1, peltierDriverPin2, thermistorPin;

    // Variables
    int desiredTemp, currentTemp;
    int error;
    int Kp;
    bool enabled{false};
    float temp_array[POINTS] = {0.0f, 5.0f, 10.0f, 15.0f, 20.0f, 25.0f, 30.0f, 35.0f, 40.0f, 45.0f};
    float rt_array[POINTS] = {12094.0f, 10000.0f, 8310.8f, 6941.1f, 5824.9f, 4910.6f, 4158.3f, 3536.2f, 3019.7f, 2588.8f };

    // Methods
    void calculateError();
    void readThermistor();
    float interpolate(float resistanceValue);

public:
    PeltierController(int pPin1, int pPin2, int thPin, int KpVal);
    void setDesiredTemp(int temp);
    void enable();
    void disable();
    bool isEnabled();
    void temperatureControl();
};

#endif
