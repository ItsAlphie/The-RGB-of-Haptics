#include "vibration.h"

Vibration::Vibration(int vPin): pin{vPin}
{   
    Serial.begin(115200);

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

void Vibration::setRoughness(int frequency, int force)
{
    this->frequency = frequency;
    dutyCycle = std::max(0, std::min(255, value));
}

void Vibration::disable()
{
    drv.stop();
}

void SelectMPX(uint8_t bus){
  Serial.println("Selecting multiplexer bus " + String(bus));
  Wire.beginTransmission(0x70);  // TCA9548A address is 0x70
  Wire.write(1 << bus);          // send byte to select bus
  Wire.endTransmission();
}

void Vibration::enable()
{
    drv.go();
    analogWrite(pin, dutyCycle);
    delay((1/frequency)*1000);
}
