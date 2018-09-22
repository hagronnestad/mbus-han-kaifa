using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace MBus {

    internal class Program {

        private static SerialPort Sp = new SerialPort("COM3");

        private static void Main(string[] args) {
            Sp.BaudRate = 2400;
            Sp.DataBits = 8;
            Sp.StopBits = StopBits.One;
            Sp.Parity = Parity.Even;

            Sp.Open();

            while (true) {
                var packet = GetPacket();
                OutputPacketAsHex(packet);
                ParsePacketAndOutputData(packet);
            }
        }

        private static List<byte> GetPacket() {
            var packet = new List<byte>();

            while (true) {
                var b = Sp.ReadByte();
                if (b != 0x7E) continue;

                var nextByte = Sp.ReadByte();

                if (nextByte != 0x7E) {
                    packet.Add(0x7E);
                }

                packet.Add((byte) nextByte);

                while (packet.Count < 41) {
                    packet.Add((byte) Sp.ReadByte());
                }

                if (packet[40] != 0x7E) {
                    packet.Clear();
                    continue;
                }

                return packet;
            }
        }

        private static void OutputPacketAsHex(List<byte> packet) {
            foreach (var item in packet) {
                Console.Write(item.ToString("X2"));
                Console.Write(" ");
            }
            Console.WriteLine("");
        }

        private static void ParsePacketAndOutputData(List<byte> packet) {
            var power = BitConverter.ToUInt16(packet.Skip(36).Take(2).Reverse().ToArray(), 0);
            var year = BitConverter.ToUInt16(packet.Skip(19).Take(2).Reverse().ToArray(), 0);

            var date = new DateTime(year, packet[21], packet[22], packet[24], packet[25], packet[26]);

            Console.WriteLine($"Time: {date.ToString("yyyy-MM-dd HH\\:mm\\:ss")}");
            Console.WriteLine($"Power: {power} W");
        }

    }
}