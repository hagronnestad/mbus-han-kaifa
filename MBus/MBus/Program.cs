using MBus.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

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

                switch (packet) {
                    case List1 list:
                        Console.WriteLine($"Received {list.Type}:");
                        Console.WriteLine($"DateTime: {list.Date}");
                        Console.WriteLine($"Active power: {list.ActivePower}");
                        Console.WriteLine("");
                        break;

                    case List9 list:
                        Console.WriteLine($"Received {list.Type}:");
                        Console.WriteLine($"DateTime: {list.Date}");
                        Console.WriteLine($"ListVersionIdentifier: {list.ListVersionIdentifier}");
                        Console.WriteLine($"MeterId: {list.MeterId}");
                        Console.WriteLine($"MeterType: {list.MeterType}");
                        Console.WriteLine("");
                        break;
                }
            }

        }

        private static ListBase GetPacket() {
            while (true) {
                var packet = new List<byte>();

                var b = Sp.ReadByte();
                if (b != 0x7E) continue;
                if (Sp.ReadByte() == 0x7E) continue;

                var packetLength = (byte) Sp.ReadByte();
                packet.AddRange(new byte[] { 0x7E, 0xA0, packetLength });

                while (packet.Count < packetLength + 2) {
                    packet.Add((byte) Sp.ReadByte());
                }

                var type = (ListType) packet[32];

                switch (type) {
                    case ListType.List1:
                        return DecodeList1Packet(packet);

                    case ListType.List9:
                        return DecodeList9Packet(packet);

                    default:
                        Console.WriteLine("Found unknown list:");
                        OutputPacketAsHex(packet);
                        Console.WriteLine("");
                        break;
                }
            }
        }

        private static ListBase DecodeList1Packet(List<byte> data) {
            var power = BitConverter.ToUInt16(data.Skip(36).Take(2).Reverse().ToArray(), 0);
            var year = BitConverter.ToUInt16(data.Skip(19).Take(2).Reverse().ToArray(), 0);
            var date = new DateTime(year, data[21], data[22], data[24], data[25], data[26]);

            return new List1() {
                Type = ListType.List1,
                RawData = data,
                Date = date,
                ActivePower = power
            };
        }

        private static ListBase DecodeList9Packet(List<byte> data) {
            var year = BitConverter.ToUInt16(data.Skip(19).Take(2).Reverse().ToArray(), 0);
            var date = new DateTime(year, data[21], data[22], data[24], data[25], data[26]);

            return new List9() {
                Type = ListType.List9,
                RawData = data,
                Date = date,
                ListVersionIdentifier = Encoding.ASCII.GetString(data.Skip(35).Take(data[34]).ToArray()),
                MeterId = Encoding.ASCII.GetString(data.Skip(44).Take(data[43]).ToArray()),
                MeterType = Encoding.ASCII.GetString(data.Skip(62).Take(data[61]).ToArray()),
            };
        }

        private static void OutputPacketAsHex(List<byte> packet) {
            foreach (var item in packet) {
                Console.Write(item.ToString("X2"));
                Console.Write(" ");
            }
            Console.WriteLine("");
        }

    }
}