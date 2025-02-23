#include "peltiercontroller.h"

PeltierController::PeltierController(int pPin1, int pPin2, int thPin, float KpVal) : peltierDriverPin1{pPin1}, peltierDriverPin2{pPin2}, thermistorPin{thPin}, Kp{KpVal}
{
    pinMode(thermistorPin, INPUT);
}

void PeltierController::setDesiredTemp(float temp)
{
    desiredTemp = temp;
}

void PeltierController::temperatureControl()
{
    readThermistor();
    calculateError();
    // int output = error * Kp; no pid control for now
    if (error > 3)
    { //
        Serial.println("Temperature too low. Heating up.");
        digitalWrite(peltierDriverPin1, HIGH);
        digitalWrite(peltierDriverPin2, LOW);
    }
    else if (error < -3)
    {
        Serial.println("Temperature too high. Cooling down.");
        digitalWrite(peltierDriverPin1, LOW);
        digitalWrite(peltierDriverPin2, HIGH);
    }
    else
    {
        Serial.println("Within desired temperature range.");
        digitalWrite(peltierDriverPin1, LOW);
        digitalWrite(peltierDriverPin2, LOW);
    }
    Serial.println("------");
    delay(1000);
}

void PeltierController::readThermistor()
{
    float voltage = analogRead(thermistorPin) * (5.0f / 1023.0f);
    Serial.print("Current voltage read: ");
    Serial.println(voltage);
    // float thermistorValue = 10000.0f * ((5.0f / voltage) - 1.0f); // voltage divider
    float thermistorValue = ((voltage/5.0f)*10000.0f)/(1.0f-(voltage/5.0f));
    currentTemp = 9.5093e-8f * thermistorValue * thermistorValue - 0.0047f * thermistorValue + 63.342f;
    Serial.print("Current temperature: ");
    Serial.println(currentTemp);
}




void PeltierController::calculateError()
{
    error = desiredTemp - currentTemp;
}

void PeltierController::enable()
{   
    enabled = true;
    temperatureControl();
    Serial.print("Error is: ");
    Serial.println(error);

}

void PeltierController::disable()
{
    Serial.println("Peltier element disabled");
    digitalWrite(peltierDriverPin1, LOW);
    digitalWrite(peltierDriverPin2, LOW);
}

bool PeltierController::isEnabled()
{
    return enabled;
}
