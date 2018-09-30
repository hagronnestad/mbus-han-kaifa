int packet[256];
int readBytes = 0;

void setup() {
  Serial.begin(2400);
}

void loop() {
  if (Serial.available() < 3) return;
  
  int b = Serial.read();
  if (b != 0x7E) return;
  if (Serial.read() == 0x7E) return;
  
  int packetLength = Serial.read();
  packet[0] = 0x7E;
  packet[1] = 0xA0;
  packet[2] = packetLength;
  readBytes = 3;
  
  while (readBytes < packetLength + 2) {
    if (Serial.available()) {
      packet[readBytes] = Serial.read();
      readBytes++;
    }
  }
  
  int packetType = packet[32];

  if (packetType == 0x01) { // LIST 1
    long activePower = packet[34] << 24;
    activePower |= packet[35] << 16;
    activePower |= packet[36] << 8;
    activePower |= packet[37];

    Serial.write("Active power: ");
    Serial.print(String(activePower));
    Serial.write(" W\n");
  }

  readBytes = 0;
}
