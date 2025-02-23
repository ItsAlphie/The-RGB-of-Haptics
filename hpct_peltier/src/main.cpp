#include <Arduino.h>
#include "peltiercontroller.h"

PeltierController peltierController(A0, A1, A2, 3.0f);

void setup() {
  peltierController.setDesiredTemp(30);
}

void loop() {
  peltierController.enable();
}

