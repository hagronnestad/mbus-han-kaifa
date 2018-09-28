using MBus.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
                //var packet = GetPacket(Sp.BaseStream);

                var fs = File.OpenRead(@"RawPacketDumps\List3SinglePhase.dat");
                var packet = GetPacket(fs);

                switch (packet) {
                    case List1 list:
                        Console.WriteLine($"Received {list.Type}:");
                        Console.WriteLine($"DateTime: {list.Date}");
                        Console.WriteLine($"Active power: {list.ActivePower}");
                        Console.WriteLine("");
                        break;

                    case List3 list:
                        Console.WriteLine($"Received {list.Type}:");
                        Console.WriteLine($"DateTime: {list.Date}");
                        Console.WriteLine($"ListVersionIdentifier: {list.ListVersionIdentifier}");
                        Console.WriteLine($"MeterId: {list.MeterId}");
                        Console.WriteLine($"MeterType: {list.MeterType}");
                        Console.WriteLine($"ActivePowerImport: {list.ActivePowerImport} W");
                        Console.WriteLine($"ActivePowerExport: {list.ActivePowerExport} W");
                        Console.WriteLine($"ReactivePowerImport: {list.ReactivePowerImport} VAr");
                        Console.WriteLine($"ReactivePowerExport: {list.ReactivePowerExport} VAr");
                        Console.WriteLine($"CurrentL1: {list.CurrentL1}");
                        Console.WriteLine($"VoltageL1: {list.VoltageL1}");
                        Console.WriteLine($"CurrentL1Double: {list.CurrentL1Double} A");
                        Console.WriteLine($"VoltageL1Double: {list.VoltageL1Double} V");
                        Console.WriteLine($"CumulativeActiveImportEnergy: {list.CumulativeActiveImportEnergy}");
                        Console.WriteLine($"CumulativeActiveExportEnergy: {list.CumulativeActiveExportEnergy}");
                        Console.WriteLine($"CumulativeReactiveImportEnergy: {list.CumulativeReactiveImportEnergy}");
                        Console.WriteLine($"CumulativeReactiveExportEnergy: {list.CumulativeReactiveExportEnergy}");
                        Console.WriteLine($"CumulativeActiveImportEnergyDouble: {list.CumulativeActiveImportEnergyDouble} kWh");
                        Console.WriteLine($"CumulativeActiveExportEnergyDouble: {list.CumulativeActiveExportEnergyDouble} kWh");
                        Console.WriteLine($"CumulativeReactiveImportEnergyDouble: {list.CumulativeReactiveImportEnergyDouble} KVArh");
                        Console.WriteLine($"CumulativeReactiveExportEnergyDouble: {list.CumulativeReactiveExportEnergyDouble} KVArh");
                        Console.WriteLine("");
                        break;

                    case List2 list:
                        Console.WriteLine($"Received {list.Type}:");
                        Console.WriteLine($"DateTime: {list.Date}");
                        Console.WriteLine($"ListVersionIdentifier: {list.ListVersionIdentifier}");
                        Console.WriteLine($"MeterId: {list.MeterId}");
                        Console.WriteLine($"MeterType: {list.MeterType}");
                        Console.WriteLine($"ActivePowerImport: {list.ActivePowerImport} W");
                        Console.WriteLine($"ActivePowerExport: {list.ActivePowerExport} W");
                        Console.WriteLine($"ReactivePowerImport: {list.ReactivePowerImport} VAr");
                        Console.WriteLine($"ReactivePowerExport: {list.ReactivePowerExport} VAr");
                        Console.WriteLine($"CurrentL1: {list.CurrentL1}");
                        Console.WriteLine($"VoltageL1: {list.VoltageL1}");
                        Console.WriteLine($"CurrentL1Double: {list.CurrentL1Double} A");
                        Console.WriteLine($"VoltageL1Double: {list.VoltageL1Double} V");
                        Console.WriteLine("");
                        break;
                }
            }

        }

        private static ListBase GetPacket(Stream stream) {
            while (true) {
                var packet = new List<byte>();

                var b = stream.ReadByte();
                if (b != 0x7E) continue;
                if (stream.ReadByte() == 0x7E) continue;

                var packetLength = (byte) stream.ReadByte();
                packet.AddRange(new byte[] { 0x7E, 0xA0, packetLength });

                while (packet.Count < packetLength + 2) {
                    packet.Add((byte) stream.ReadByte());
                }

                var type = (ListType) packet[32];

                switch (type) {
                    case ListType.List1:
                        return DecodeList1Packet(packet);

                    case ListType.List2SinglePhase:
                        return DecodeList2SinglePhasePacket(packet);

                    case ListType.List3SinglePhase:
                        return DecodeList3SinglePhasePacket(packet);

                    default:
                        Console.WriteLine("Found unknown list:");
                        OutputPacketAsHex(packet);
                        Console.WriteLine("");
                        break;
                }
            }
        }

        private static ListBase DecodeList1Packet(List<byte> data) {
            var power = BitConverter.ToUInt32(data.Skip(34).Take(4).Reverse().ToArray(), 0);
            var year = BitConverter.ToUInt16(data.Skip(19).Take(2).Reverse().ToArray(), 0);
            var date = new DateTime(year, data[21], data[22], data[24], data[25], data[26]);

            return new List1() {
                Type = ListType.List1,
                RawData = data,
                Date = date,
                ActivePower = power
            };
        }

        private static ListBase DecodeList2SinglePhasePacket(List<byte> data) {
            var year = BitConverter.ToUInt16(data.Skip(19).Take(2).Reverse().ToArray(), 0);
            var date = new DateTime(year, data[21], data[22], data[24], data[25], data[26]);

            return new List2() {
                Type = ListType.List2SinglePhase,
                RawData = data,
                Date = date,
                ListVersionIdentifier = Encoding.ASCII.GetString(data.Skip(35).Take(data[34]).ToArray()),
                MeterId = Encoding.ASCII.GetString(data.Skip(44).Take(data[43]).ToArray()),
                MeterType = Encoding.ASCII.GetString(data.Skip(62).Take(data[61]).ToArray()),

                ActivePowerImport = BitConverter.ToUInt32(data.Skip(71).Take(4).Reverse().ToArray(), 0),
                ActivePowerExport = BitConverter.ToUInt32(data.Skip(76).Take(4).Reverse().ToArray(), 0),
                ReactivePowerImport = BitConverter.ToUInt32(data.Skip(81).Take(4).Reverse().ToArray(), 0),
                ReactivePowerExport = BitConverter.ToUInt32(data.Skip(86).Take(4).Reverse().ToArray(), 0),
                CurrentL1 = BitConverter.ToInt32(data.Skip(91).Take(4).Reverse().ToArray(), 0),
                VoltageL1 = BitConverter.ToUInt32(data.Skip(96).Take(4).Reverse().ToArray(), 0),
            };
        }

        private static ListBase DecodeList3SinglePhasePacket(List<byte> data) {
            var year = BitConverter.ToUInt16(data.Skip(19).Take(2).Reverse().ToArray(), 0);
            var date = new DateTime(year, data[21], data[22], data[24], data[25], data[26]);

            return new List3() {
                Type = ListType.List3SinglePhase,
                RawData = data,
                Date = date,
                ListVersionIdentifier = Encoding.ASCII.GetString(data.Skip(35).Take(data[34]).ToArray()),
                MeterId = Encoding.ASCII.GetString(data.Skip(44).Take(data[43]).ToArray()),
                MeterType = Encoding.ASCII.GetString(data.Skip(62).Take(data[61]).ToArray()),

                ActivePowerImport = BitConverter.ToUInt32(data.Skip(71).Take(4).Reverse().ToArray(), 0),
                ActivePowerExport = BitConverter.ToUInt32(data.Skip(76).Take(4).Reverse().ToArray(), 0),
                ReactivePowerImport = BitConverter.ToUInt32(data.Skip(81).Take(4).Reverse().ToArray(), 0),
                ReactivePowerExport = BitConverter.ToUInt32(data.Skip(86).Take(4).Reverse().ToArray(), 0),
                CurrentL1 = BitConverter.ToInt32(data.Skip(91).Take(4).Reverse().ToArray(), 0),
                VoltageL1 = BitConverter.ToUInt32(data.Skip(96).Take(4).Reverse().ToArray(), 0),

                CumulativeActiveImportEnergy = BitConverter.ToUInt32(data.Skip(115).Take(4).Reverse().ToArray(), 0),
                CumulativeActiveExportEnergy = BitConverter.ToUInt32(data.Skip(120).Take(4).Reverse().ToArray(), 0),
                CumulativeReactiveImportEnergy = BitConverter.ToUInt32(data.Skip(125).Take(4).Reverse().ToArray(), 0),
                CumulativeReactiveExportEnergy = BitConverter.ToUInt32(data.Skip(130).Take(4).Reverse().ToArray(), 0),
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