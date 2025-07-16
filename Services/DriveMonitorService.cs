using DriveMonitor.Data;
using System;
using System.IO;

namespace DriveMonitor.Services
{
    public class DriveMonitorService
    {
        private readonly SqlLogger _logger = new SqlLogger();

        // Threshold: 40 GB in bytes
        private const long ThresholdBytes = 40L * 1024 * 1024 * 1024;

        public void CheckDrives()
        {
            Console.WriteLine("🔍 Checking C: drive used space...");

            try
            {
                DriveInfo driveC = new DriveInfo("C");
                if (!driveC.IsReady)
                {
                    Console.WriteLine("❌ Drive C: is not ready.");
                    return;
                }

                long totalSize = driveC.TotalSize;
                long freeSpace = driveC.AvailableFreeSpace;
                long usedSpace = totalSize - freeSpace;

                Console.WriteLine($"ℹ️ Drive C: Used Space = {usedSpace / (1024 * 1024 * 1024)} GB");

                if (usedSpace >= ThresholdBytes)
                {
                    string message = $"Warning: Drive C: used space is {usedSpace / (1024 * 1024 * 1024)} GB, which exceeds the threshold of 40 GB.";
                    Console.WriteLine("⚠️ " + message);
                    _logger.LogMessage(message);
                }
                else
                {
                    Console.WriteLine("✅ Drive C: used space is below the 40 GB threshold.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error checking drive space: {ex.Message}");
            }
        }
    }
}


