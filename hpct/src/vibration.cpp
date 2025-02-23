#include "vibration.h"

Vibration::Vibration(int vPin1, int vPin2) : pin1{vPin1}, pin2{vPin2}
{  

    // I2C communication with Multiplexer
    Wire.begin();
    delay(1000);
    for(int i = 0; i <= 1; i++) {
        SelectMPX(i);
        drv.selectLibrary(1);
        drv.setMode(DRV2605_MODE_PWMANALOG); 
        if (! drv.begin()) {
            Serial.println("Could not find Haptic Driver " + i);
        }
    }

}

void Vibration::SelectMPX(uint8_t bus)
{
    Serial.println("Selecting multiplexer bus " + String(bus));
    Wire.beginTransmission(0x70);  // TCA9548A address is 0x70
    Wire.write(1 << bus);          // send byte to select bus
    Wire.endTransmission();
}

void Vibration::setRoughness(int motor, int frequency, int force)
{   
    Serial.print("Current frequency : ");
    Serial.println(frequency);
    Serial.print("Current force : ");
    Serial.println(force);

    if(motor == 1){
        frequency1 = frequency;
        dutyCycle1 = std::max(0, std::min(255, force));
    }
    else if(motor == 2){
        frequency2 = frequency;
        dutyCycle2 = std::max(0, std::min(255, force));
    }

}
    

void Vibration::disable()
{
    drv.stop();
}

void Vibration::enable(int motor)
{

    drv.go();
    if(motor == 1){
        analogWrite(pin1, dutyCycle1);
        delay((1/frequency1)*1000);
    }
    else if(motor == 2){
        analogWrite(pin2, dutyCycle1);
        delay((1/frequency2)*1000);
    }
}
