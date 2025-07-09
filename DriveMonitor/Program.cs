using DriveMonitor.Services;

class Program
{
    static void Main()
    {
        var monitor = new DriveMonitorService();
        monitor.CheckDrives();
        Console.WriteLine("✅ Drive monitor run completed.");  // confirm program executed

    }
}