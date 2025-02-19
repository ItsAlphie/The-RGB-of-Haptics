#include <Wire.h>
#include <Adafruit_DRV2605.h>

Adafruit_DRV2605 drv1;
Adafruit_DRV2605 drv2;

int dutyCycle = 0;
int fadeAmount = 5;

#define Titan1_SCL 43
#define Titan2_SCL 44
#define Titan1_SDA 1
#define Titan2_SDA 2
#define Titan1_PWM 42
#define Titan2_PWM 14

TwoWire Titan1 = TwoWire(0);
TwoWire Titan2 = TwoWire(1);

void setup() {
  Serial.begin(115200);
  Serial.println("Setting up dual Titan Haptics I2C bus");
  Titan1.begin(Titan1_SDA, Titan1_SCL);
  Titan2.begin(Titan2_SDA, Titan2_SCL);

  if(!drv1.begin(&Titan1)) {
    Serial.println("Could not find Haptic Driver 1");
    while(1){}
  }
  drv1.selectLibrary(1);
  drv1.setMode(DRV2605_MODE_PWMANALOG); 

  if(!drv2.begin(&Titan2)) {
    Serial.println("Could not find Haptic Driver 2");
    while(1){}
  }
  drv2.selectLibrary(1);
  drv2.setMode(DRV2605_MODE_PWMANALOG); 
}

void PWM(int pin){
  if(pin == Titan1_PWM
  ){
    analogWrite(pin, dutyCycle);
    dutyCycle = dutyCycle + fadeAmount;
    if (dutyCycle <= 0 || dutyCycle >= 255) {
      fadeAmount = -fadeAmount;
    }
  }
  else{
    analogWrite(pin, (255-dutyCycle));
    dutyCycle = dutyCycle + fadeAmount;
    if (dutyCycle <= 0 || dutyCycle >= 255) {
      fadeAmount = -fadeAmount;
    }
  }
  
}

void loop(){
  PWM(Titan1_PWM);
  PWM(Titan2_PWM);
  drv1.go();
  drv2.go();
  delay(300);
}
