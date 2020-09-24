using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;

public class RemoteConnect
{
    public static readonly string _fileName = "\\prInfo.txt";
    public static string GetComputerName()
    {
        return Environment.MachineName;
    }

    public static string GetHddSerialNumber()
    {
        ConnectionOptions options = new ConnectionOptions
        {
            Impersonation = ImpersonationLevel.Impersonate
        };

        ManagementScope scope = new ManagementScope($"\\\\{GetComputerName()}\\root\\cimv2", options);
        scope.Connect();

        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

        var queryList = searcher.Get();
        string DiskNumber = "found";    
        foreach (ManagementObject m in queryList)
        {
            DiskNumber = m["SerialNumber"].ToString();
            break;
        }

        return DiskNumber;
    }

    public static string ReadKeyFromFile()
    {
        string ExecDir = Environment.CurrentDirectory;
        try
        {
            var reader = new StreamReader(ExecDir + _fileName);
            var Key = reader.ReadLine();
            reader.Close();
            return Key;
        }
        catch
        {
            throw new Exception();
        }
        
        
    }

    public static bool WriteKeyInFile(string serialNumber)
    {
        string ExecDir = Environment.CurrentDirectory;
        var writer = new StreamWriter(ExecDir + _fileName);
        writer.Write(GetHash(serialNumber));
        writer.Close();

        return true;
    }

    public static string GetHash(string input)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

        return Convert.ToBase64String(hash);
    }

    public static void Main(string[] args)
    {
        foreach (var r in args)
        {
            Console.WriteLine(r);
        }

        if (args.Length == 0)
        {
            try
            {
                if (ReadKeyFromFile() == GetHash(GetHddSerialNumber()))
                {
                    Console.WriteLine("Access granted!");
                }
                else
                {
                    Console.WriteLine("Access denied");
                }
            }
            catch
            {
                Console.WriteLine("Access denied, you should to run installer");
            }
            
        }
        else if (args.FirstOrDefault() == "install")
        {
            WriteKeyInFile(GetHddSerialNumber());
        }

        
       
    }

}