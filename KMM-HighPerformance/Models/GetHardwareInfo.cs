﻿using System.Management;

namespace KMM_HighPerformance.Models
{
    class GetHardwareInfo
    {

        //work in progress, class for getting hardware info as PC's CPU, GPU etc. 
        static public string GetCPUName()
        {
            string cpuName =" ";
            var searcher = new ManagementObjectSearcher("Select * From Win32_processor");
            var searcherList = searcher.Get();
            
            foreach (ManagementObject item in searcherList)
            {
                cpuName = item["Name"].ToString();
            }

            return cpuName;
        }




    }
}
