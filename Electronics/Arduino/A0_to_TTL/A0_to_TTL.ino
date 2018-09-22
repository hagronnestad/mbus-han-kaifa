void setup()
{
  pinMode(12, OUTPUT);
}

int val = 0;

void loop()
{
  val = analogRead(0);
  digitalWrite(12, val > 400);
}
