#include <Arduino.h>
#include <ESP8266WiFi.h>


// SETTINGS SETUP AND LOADING

#include <EEPROM.h>
struct Settings {
  char ssid[32];
  char password[32];
};

Settings SettingsSetup() {
  Settings s;
  
  EEPROM.begin(512);
  EEPROM.get(0, s);
  
  Serial.println("\n\nType space to enter setup...");
  Serial.setTimeout(3000);
  
  String nl = Serial.readStringUntil('\n');

  if (nl != " ") {
    Serial.println("Skipping setup!");
    Serial.setTimeout(1000);
    return s;
  }

  Serial.setTimeout(60000);
  Serial.println("Setup");
  Serial.println("--------------");
  
  Serial.print("Enter new SSID [" + String(s.ssid) + "]: ");
  String ssid = Serial.readStringUntil('\n');
  if (ssid.length() > 0) ssid.toCharArray(s.ssid, sizeof(s.ssid));
  Serial.println(String(s.ssid));

  Serial.print("Enter new password [" + String(s.password) + "]: ");
  String password = Serial.readStringUntil('\n');
  if (password.length() > 0) password.toCharArray(s.password, sizeof(s.password));
  Serial.println(s.password);

  EEPROM.put(0, s);
  EEPROM.commit();
  EEPROM.end();

  Serial.println("\n\nSettings saved!\n");

  Serial.setTimeout(1000);
  return s;
}




Settings s;

void setup() {
  Serial.begin(9600);
  s = SettingsSetup();

  WiFi.disconnect(); // Clears last connected AP from flash
  WiFi.begin((String(s.ssid)).c_str(), (String(s.password).c_str()));

  Serial.print("Connecting to: '");
  Serial.print(s.ssid);
  Serial.print("', with password: '");
  Serial.print(s.password);
  Serial.print("'");
  
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.print(".");
  }
  Serial.println();

  Serial.print("Connected, IP address: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  // put your main code here, to run repeatedly:

}




