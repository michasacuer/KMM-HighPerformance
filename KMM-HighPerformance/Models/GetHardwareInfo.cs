using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace KMM_HighPerformance.Models
{
    class GetHardwareInfo
    {

        //work in progress
        static public string GetCPU()
        {
            string cpuName =" ";
            var mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            var mbsList = mbs.Get();
            
            foreach (ManagementObject mo in mbsList)
            {
                var cpuid = mo["Name"].ToString();
            }

            return cpuName;
        }




    }
}
