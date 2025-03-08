#include <Wire.h>
#include "vibration.h"
#include <Arduino.h>
#include <ESP32Servo.h>
#include "peltiercontroller.h"
#include <WiFi.h>
#include <WiFiUdp.h>

// ------------------------- WIFI -----------------------------
// https://github.com/m5stack/azure_iothub_arduino_lib_esp32/blob/master/hardware/espressif/esp32/libraries/WiFi/examples/WiFiUDPClient/WiFiUDPClient.ino 
// WiFi credentials
#define ssid "Redmi"         // Replace with your WiFi SSID
#define password "galagala"           // Replace with your WiFi password
// IPAddress ip(192, 168, 161, 1);
// IPAddress gateway(192, 168, 24, 20);
// IPAddress subnet(255, 255, 255, 0);

// Server settings
#define serverIP "192.168.161.91"      // Unity server's IP address  
#define serverPort 11000               // Unity server's port

WiFiUDP udp;
char incomingPacket[64];  // Buffer to hold incoming message

int module;
int parameter;

// ------------------------- HARDWARE -----------------------------
Vibration vibration;

Servo servoMotor;
int pos = 0;

PeltierController peltierController(4,6,1,1);

// -----------------------------------------------------------------
TaskHandle_t TemperatureControlTask;


void connectWifi(){
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  udp.begin(serverPort);
}

void temperatureControl(void * pvParameters){
  while(1){
      if(peltierController.isEnabled()){
        peltierController.temperatureControl();
  }
  vTaskDelay(2000);
  }
}

const char * receiveMessage() {
  int packetSize = udp.parsePacket();  // Check for an incoming UDP packet
  if (packetSize) {
    
    int len = udp.read(incomingPacket, 255);
    if (len > 0) {
      incomingPacket[len] = '\0';  // Null-terminate the string
    }
    Serial.print("Received: ");
    Serial.println(String(incomingPacket));
    return incomingPacket;  // Return as a String
  }
  return "";  // Return an empty String if no message was received
}


void setup() {

  Serial.begin(9600);

  xTaskCreatePinnedToCore(
                temperatureControl,   /* Task function. */
                "TemperatureControl",     /* name of task. */
                2000,       /* Stack size of task */
                NULL,        /* parameter of the task */
                0,           /* priority of the task */
                &TemperatureControlTask,      /* Task handle to keep track of created task */
                0);          /* pin task to core 1 */
  delay(500);

  servoMotor.attach(19); 

  connectWifi();

  vibration.init();

  peltierController.setDesiredTemp(30);

  Serial.print("Free heap: ");
Serial.println(ESP.getFreeHeap());


}
  
void loop() {
  // ------------------------- VIBRATION -----------------------------

  // vibration.setFrequency(0, 10);
  // vibration.setStrength(0, 255);
  // vibration.enable(0);
  // vibration.disable(1);
  // delay(3000);
  // vibration.setFrequency(0, 3);
  // vibration.setStrength(0, 125);
  // // vibration.disable(0);
  //   vibration.enable(0);

  // vibration.enable(1);
  // delay(3000);

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


  // if (peltierController.isEnabled()){
  //     peltierController.temperatureControl();
  // }

// ------------------------- UDP COMM -----------------------------

/**
+-----------------------------------------+------+
| Function                                | Code |
+-----------------------------------------+------+
| *** Peltier ***                         |      |
| Peltier enable                          | 0:0  |
| Peltier disable                         | 0:1  |
| Peltier desired temperature set to %d   | 1:%d |
|                                         |      |
| *** Vibration ***                       |      |
| Titan haptic motor 1 enable             | 2:0  |
| Titan haptic motor 2 enable             | 2:1  |
| Titan haptic motor 1 disable            | 2:2  |
| Titan haptic motor 2 disable            | 2:3  |
| Titan haptic motor 1 frequency set to %d| 3:%d |
| Titan haptic motor 2 frequency set to %d| 4:%d |
| Titan haptic motor 1 strength set to %d | 5:%d |
| Titan haptic motor 2 strength set to %d | 6:%d |
|                                         |      |
| *** ServoMotor ***                      |      |
| Servo motor set to %d degrees           | 7:%d |
+-----------------------------------------+------+
*/

  if (sscanf(receiveMessage(), "%d:%d", &module, &parameter) == 2) {
      switch(module){
        case 0:
            switch(parameter){
              case 0:
                peltierController.enable();
                break;
              
              case 1:
                peltierController.disable();
                break;
              
              default:
                Serial.println("Invalid command");
            }
          break;

        case 1:
          peltierController.setDesiredTemp(parameter);
          break;

        case 2:
          switch(parameter){
            case 0:
              vibration.enable(0);
              break;
            
            case 1:
              vibration.enable(1);
              break;

            case 2:
              vibration.disable(0);
              break;
            
            case 3:
              vibration.disable(1);
              break;
            
            default:
              Serial.println("Invalid command");

          }

          break;

        case 3:
          vibration.setFrequency(0, parameter);
          break;

        case 4:
          vibration.setFrequency(1, parameter);
          break;

        case 5:
          vibration.setStrength(0, parameter);
          break;

        case 6:
          vibration.setStrength(1, parameter);
          break;

        case 7:
          servoMotor.write(parameter);
          break;

        default:
          Serial.println("Invalid command");
          
      }
  }

}
