using LibreHardwareMonitor.Hardware;
using System;
using System.Threading;

namespace power
{
    public class Power
    {
        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }
        public string Monitor(Computer computer)
        {
            computer.Accept(new UpdateVisitor());
            //Console.WriteLine(computer.GetReport());
            float? sumOfWatts = 0;
            string output = "";
            foreach (IHardware hardware in computer.Hardware)
            {
                output = output + "Hardware: " + hardware.Name + "\n";
                float? maxPower = 0;
                SensorType maxSensorType = SensorType.Power;
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Power && maxPower < sensor.Value)
                    {
                        maxPower = sensor.Value;
                        maxSensorType = sensor.SensorType;
                    }
                }
                output = output + "\t\tSensor: " + maxSensorType.ToString() + ", value: " + maxPower.ToString() + " W." + "\n";
                sumOfWatts += maxPower;
            }
            output = output + "\nTotal power consumption: , value: " + sumOfWatts + " W.";
            return output;
        }
    }
}