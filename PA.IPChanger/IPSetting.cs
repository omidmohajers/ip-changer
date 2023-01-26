using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace PA.IPChanger
{
    public class IPProfile : ICloneable
    {
        public string IP { get; set; }
        public string Subnet { get; set; }
        public string Getway { get; set; }
        public string Dns1 { get; set; }
        public string Dns2 { get; set; }
        public string Win1 { get; set; }
        public string Win2 { get; set; }
        public bool IsStatic { get; set; }
        public bool DHCPEnabled
        {
            get => !IsStatic;
            private set => IsStatic = !value;
        }
        public string NIC { get; set; }
        public string Name { get; set; }

        public object Clone()
        {
            IPProfile instance = new IPProfile()
            {
                IP = this.IP,
                Subnet = this.Subnet,
                Getway = this.Getway,
                Dns1 = this.Dns1,
                Dns2 = this.Dns2,
                Win1 = this.Win1,
                Win2 = this.Win2,
                DHCPEnabled = this.DHCPEnabled,
                NIC = this.NIC,
                Name = this.Name
            };
            return instance;
        }
        public void Copy(IPProfile instance)
        {
            this.IP = instance.IP;
            this.Subnet = instance.Subnet;
            this.Getway = instance.Getway;
            this.Dns1 = instance.Dns1;
            this.Dns2 = instance.Dns2;
            this.Win1 = instance.Win1;
            this.Win2 = instance.Win2;
            this.DHCPEnabled = instance.DHCPEnabled;
            this.NIC = instance.NIC;
            this.Name = instance.Name;
        }

        public void Validate()
        {

        }
        public void Deserialize(string info)
        {
            string[] data = info.Split(',');
            this.IP = string.IsNullOrWhiteSpace(data[0]) ? "0.0.0.0" : data[0];
            this.Subnet = string.IsNullOrWhiteSpace(data[1]) ? "0.0.0.0" : data[1];
            this.Getway = string.IsNullOrWhiteSpace(data[2]) ? "0.0.0.0" : data[2];
            this.Dns1 = string.IsNullOrWhiteSpace(data[3]) ? "0.0.0.0" : data[3];
            this.Dns2 = string.IsNullOrWhiteSpace(data[4]) ? "0.0.0.0" : data[4];
            this.Win1 = string.IsNullOrWhiteSpace(data[5]) ? "0.0.0.0" : data[5];
            this.Win2 = string.IsNullOrWhiteSpace(data[6]) ? "0.0.0.0" : data[6];
            this.DHCPEnabled = string.IsNullOrWhiteSpace(data[7]) ? false : bool.Parse(data[7]);
            this.NIC = string.IsNullOrWhiteSpace(data[8]) ? "" : data[8];
            this.Name = string.IsNullOrWhiteSpace(data[9]) ? "noname" : data[9];
        }
        public string Serialize()
        {
            string[] data = new string[10];
            data[0] = IP;
            data[1] = Subnet;
            data[2] = Getway;
            data[3] = Dns1;
            data[4] = Dns2;
            data[5] = Win1;
            data[6] = Win2;
            data[7] = DHCPEnabled.ToString();
            data[8] = NIC;
            data[9] = Name;
            return string.Join(",", data);
        }
        public IPProfile()
        {
            this.IP = IPAddress.Any.ToString();
            this.Subnet = IPAddress.Broadcast.ToString();
            this.Getway = IPAddress.Any.ToString();
            this.Dns1 = "4.2.2.4";
            this.Dns2 = "8.8.8.8";
            this.Win1 = IPAddress.Any.ToString();
            this.Win2 = IPAddress.Any.ToString();
            this.DHCPEnabled = false;
            this.NIC = "";
            this.Name = "setting";
        }
    }
}
