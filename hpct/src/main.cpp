#include <Wire.h>
#include <Adafruit_DRV2605.h>
#include "vibration.h"
#include <Arduino.h>
#include <ESP32Servo.h>
#include "peltiercontroller.h"

Vibration vibration(36, 37);

Servo servoMotor;
//int pos = 0;

PeltierController peltierController(15,16,6,1);

void setup() {
  
  Serial.begin(9600);
  // servoMotor.attach(36);


  // peltierController.setDesiredTemp(30);

}
  
void loop() {
  // ------------------------- VIBRATION -----------------------------

  // // for(int i = 0; i <= 1; i++) {
  // //   SelectMPX(i);
  // //   PWM();
  // // }
  // // delay(300);

  // vibration.enable(1);
  // vibration.enable(2);
  // delay(5000);
  // vibration.disable();
  // delay(5000);

  // ------------------------- SERVO -----------------------------

  // for (pos = 0; pos <= 180; pos += 1) { // goes from 0 degrees to 180 degrees
  //   // in steps of 1 degree
  //   servoMotor.write(pos);              // tell servo to go to position in variable 'pos'
  //   delay(15);                       // waits 15ms for the servo to reach the position
  // }
  // for (pos = 180; pos >= 0; pos -= 1) { // goes from 180 degrees to 0 degrees
  //   servoMotor.write(pos);              // tell servo to go to position in variable 'pos'
  //   delay(15);                       // waits 15ms for the servo to reach the position
  // }

  // ------------------------- PELTIER -----------------------------
  // peltierController.enable();

}
