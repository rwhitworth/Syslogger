using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Syslogger
{
    public class Syslog
    {
        public enum FACILITY
        {
            Kernel = 0x00,
            UserLevel = 0x01,
            Mail = 0x02,
            SystemDaemon = 0x03,
            Security = 0x04,
            InternalSyslog = 0x05,
            LinePrinter = 0x06,
            News = 0x07,
            UUCP = 0x08,
            ClockDaemon = 0x09
        }
        public enum SEVERITY
        {
            Emergeny = 0x00,
            Alert = 0x01,
            Critical = 0x02,
            Error = 0x03,
            Warning = 0x04,
            Notice = 0x05,
            Information = 0x06,
            Debug = 0x07
        }

        public int port = 514;
        private int priority = 0;
        public String local_host_name = "";
        public bool throw_errors = false;
        public FACILITY facility = FACILITY.UserLevel;
        public SEVERITY severity = SEVERITY.Information;
        public String server = "";
        private UdpClient udp = new UdpClient();
        private String[] Month = new string[13];


        private void init(FACILITY _f, SEVERITY _s)
        {
            Month[1] = "Jan ";
            Month[2] = "Feb ";
            Month[3] = "Mar ";
            Month[4] = "Apr ";
            Month[5] = "May ";
            Month[6] = "Jun ";
            Month[7] = "Jul ";
            Month[8] = "Aug ";
            Month[9] = "Sep ";
            Month[10] = "Oct ";
            Month[11] = "Nov ";
            Month[12] = "Dec ";
            port = 514;
            server = "";
            facility = _f;
            severity = _s;
        }
        public Syslog()
        {
            init(FACILITY.UserLevel, SEVERITY.Information);
        }
        public Syslog(FACILITY _facility, SEVERITY _severity)
        {
            init(_facility, _severity);
        }

        public bool SendMessage(String message)
        {
            return SendMessage(message, server, port);
        }
        public bool SendMessage(String message, String _server)
        {
            return SendMessage(message, _server, port);
        }
        public bool SendMessage(String message, String _server, int _port)
        {
            try
            {
                udp = new UdpClient(_server, _port);
                #region switch(facility)
                switch (facility)
                {
                    case FACILITY.Kernel:
                        priority = 0;
                        break;
                    case FACILITY.UserLevel:
                        priority = 1;
                        break;
                    case FACILITY.Mail:
                        priority = 2;
                        break;
                    case FACILITY.SystemDaemon:
                        priority = 3;
                        break;
                    case FACILITY.Security:
                        priority = 4;
                        break;
                    case FACILITY.InternalSyslog:
                        priority = 5;
                        break;
                    case FACILITY.LinePrinter:
                        priority = 6;
                        break;
                    case FACILITY.News:
                        priority = 7;
                        break;
                    case FACILITY.UUCP:
                        priority = 8;
                        break;
                    case FACILITY.ClockDaemon:
                        priority = 9;
                        break;
                    default:
                        break;
                }
                #endregion
                priority = priority * 8;
                #region switch(severity)
                switch (severity)
                {
                    case SEVERITY.Emergeny:
                        priority += 0;
                        break;
                    case SEVERITY.Alert:
                        priority += 1;
                        break;
                    case SEVERITY.Critical:
                        priority += 2;
                        break;
                    case SEVERITY.Error:
                        priority += 3;
                        break;
                    case SEVERITY.Warning:
                        priority += 4;
                        break;
                    case SEVERITY.Notice:
                        priority += 5;
                        break;
                    case SEVERITY.Information:
                        priority += 6;
                        break;
                    case SEVERITY.Debug:
                        priority += 7;
                        break;
                    default:
                        break;
                }
                #endregion
                String PRI = "<" + priority + ">";

                DateTime dt = DateTime.Now;
                String dateinfo = Month[dt.Month];
                if (dt.Day > 9)
                {
                    dateinfo = dateinfo + dt.Day.ToString() + " ";
                }
                else
                {
                    dateinfo = dateinfo + " " + dt.Day.ToString() + " ";
                }
                dateinfo += dt.ToString("HH:mm:ss ");
                String totalstring = PRI + dateinfo + " " + get_local_hostname() + " " + message;
                if (totalstring.Length > 1024)
                {
                    // RFC3164 section 4.1 states that packets should be 1024 bytes or less in size
                    // This implimentation will truncate packets larger than 1024 bytes
                    totalstring = totalstring.Substring(0, 1024);
                    if (throw_errors) { System.Windows.Forms.MessageBox.Show("Message larger than 1024 bytes.  See section 4.1 in RFC 3164 for why this is a bad idea"); }
                    // return false;
                }
                byte[] data = new byte[1024];
                data = Encoding.ASCII.GetBytes(totalstring);
                udp.Send(data, data.Length);
                udp.Close();
            }
            catch (Exception excep)
            {
                if (throw_errors) { System.Windows.Forms.MessageBox.Show("Message cannot be sent.  Exception: " + excep.Message); }
                return false;
            }
            return true;
        }

        public String get_local_hostname()
        {
            if (local_host_name == "")
            {
                try
                {
                    return Dns.GetHostName();
                }
                catch (Exception excep)
                {
                    if (throw_errors) { System.Windows.Forms.MessageBox.Show("get_local_hostname() Exception: " + excep.Message); }
                    return "get_local_hostname_ERROR";
                }
            }
            else
            {
                return local_host_name;
            }
        }
    }
}
