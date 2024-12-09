#include <Wire.h>
#include <Adafruit_DRV2605.h>

#define TCAADDR 0x70

Adafruit_DRV2605 drv;
int dutyCycle = 0;
int fadeAmount = 5;

void SelectMPX(uint8_t bus){
  Serial.println("Selecting multiplexer bus " + String(bus));
  Wire.beginTransmission(0x70);  // TCA9548A address is 0x70
  Wire.write(1 << bus);          // send byte to select bus
  Wire.endTransmission();
}

void PWM(){
  drv.selectLibrary(1);
  drv.setMode(DRV2605_MODE_PWMANALOG); 
  drv.go();

  analogWrite(0, dutyCycle);

  dutyCycle = dutyCycle + fadeAmount;

  if (dutyCycle <= 0 || dutyCycle >= 255) {
    fadeAmount = -fadeAmount;
  }
  delay(30);
}

void playEffect(){
  drv.selectLibrary(1);
    // I2C trigger by sending 'go' command 
    // default, internal trigger when sending GO command
    drv.setMode(DRV2605_MODE_INTTRIG); 
  
  
  uint8_t effect = 1;
    
  // set the effect to play
  drv.setWaveform(0, effect);  // play effect 
  drv.setWaveform(1, 0);       // end waveform
  
  // play the effect!
  drv.go();
  
  // wait a bit
  delay(500);
}

void playComplex(){
  // I2C trigger by sending 'go' command 
  drv.setMode(DRV2605_MODE_INTTRIG); // default, internal trigger when sending GO command

  drv.selectLibrary(1);
  drv.setWaveform(0, 118);  // ramp up medium 1, see datasheet part 11.2
//  drv.setWaveform(1, 1);  // strong click 100%, see datasheet part 11.2
//  drv.setWaveform(2, 72);  // end of waveforms

  drv.go();
}

// ------------------------------------------------------------------------------------------------------

void setup() {
  Serial.begin(115200);

  // Start I2C communication with the Multiplexer
  Wire.begin();
}
  
void loop() {
  for(int i = 0; i < 1; i++) {
    SelectMPX(i);
    if (! drv.begin()) {
      Serial.println("Could not find Haptic Driver " + i);
    }
    else {
      PWM();
    }
  }
}
