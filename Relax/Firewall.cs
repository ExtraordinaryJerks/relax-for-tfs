using System;
using System.Configuration;
using NetFwTypeLib;

namespace Relax
{
    public class Firewall
    {
        private INetFwProfile _fwProfile;
        //private const int RelaxPort = Constants.Port;
        private const string RelaxPortName = "Relax For TFS";

        public Firewall()
        {
            SetProfile();
        }

        public static Firewall Instantiate()
        {
            return new Firewall();
        }

        public void OpenFirewall()
        {
            if (!_fwProfile.FirewallEnabled) return;
            var port = (INetFwOpenPort)GetInstance("INetOpenPort");
            port.Port = Config.Port;
            port.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            port.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            port.Name = RelaxPortName;
            port.Enabled = true;

            var ports = _fwProfile.GloballyOpenPorts;
            ports.Add(port);
        }

        public void CloseFirewall()
        {
            var ports = _fwProfile.GloballyOpenPorts;
            ports.Remove(Config.Port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
        }

        private void SetProfile()
        {
            var fwMgr = (INetFwMgr)GetInstance("INetFwMgr");
            var fwPolicy = fwMgr.LocalPolicy;
            _fwProfile = fwPolicy.CurrentProfile;
        }

        private Object GetInstance(String typeName)
        {
            switch (typeName)
            {
                case "INetFwMgr":
                    {
                        var type = Type.GetTypeFromCLSID(
                            new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
                        return Activator.CreateInstance(type);
                    }
                case "INetAuthApp":
                    {
                        var type = Type.GetTypeFromCLSID(
                            new Guid("{EC9846B3-2762-4A6B-A214-6ACB603462D2}"));
                        return Activator.CreateInstance(type);
                    }
                case "INetOpenPort":
                    {
                        var type = Type.GetTypeFromCLSID(
                            new Guid("{0CA545C6-37AD-4A6C-BF92-9F7610067EF5}"));
                        return Activator.CreateInstance(type);
                    }
                default:
                    return null;
            }
        }
    }
}
