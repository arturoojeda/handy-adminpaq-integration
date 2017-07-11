using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HandySyncTaskInstaller
{
    class Program
    {
        static System.Configuration.Configuration config;

        public static void OpenConfigFile()
        {
            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string configFile = System.IO.Path.Combine(appPath, appName + ".exe.config");
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFile;
            config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        static void Main(string[] args)
        {
            try
            {
                OpenConfigFile();
            
                Console.WriteLine("Select one of the following options: ");
                Console.WriteLine("1. Create Handy Sync Task in Task Scheduler. ");
                Console.WriteLine("2. Remove Handy Sync Task from Task Scheduler. ");
                Console.WriteLine();
                ConsoleKeyInfo info = Console.ReadKey();
                if(info.Key == ConsoleKey.D1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Creating HandySync Task.");
                    // Get the service on the local machine
                    using (TaskService ts = new TaskService())
                    {
                        try
                        {
                            // Create a new task definition and assign properties
                            TaskDefinition td = ts.NewTask();
                            int minutes = Int32.Parse(config.AppSettings.Settings["SyncPeriodMinutes"].Value);
                            RepetitionPattern rp = new RepetitionPattern(new TimeSpan(0, minutes, 0), TimeSpan.Zero);
                            td.RegistrationInfo.Description = "Runs the HandySync";

                            // Create a trigger that will fire the task at this time every other day
                            td.Triggers.Add(new DailyTrigger { DaysInterval = 1, Repetition = rp, Enabled = true });

                            // Create an action that will launch Notepad whenever the trigger fires
                            string HandySyncPath = config.AppSettings.Settings["HandySyncPath"].Value;
                            td.Actions.Add(new ExecAction(HandySyncPath + @"\HandySyncService.exe"));

                            // Register the task in the root folder
                            ts.RootFolder.RegisterTaskDefinition(@"HandySync", td);

                            // Remove the task we just created
                            //ts.RootFolder.DeleteTask("Test");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    
                    }
                    Console.WriteLine("HandySync Task created successfully.");
                }
                else if (info.Key == ConsoleKey.D2)
                {
                    using (TaskService ts = new TaskService())
                    {
                        try
                        {
                            Console.WriteLine();
                            ts.RootFolder.DeleteTask("HandySync");
                            Console.WriteLine("Handy Sync Task removed successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                    }
                }
                else 
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Option.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            Console.WriteLine();
            Console.WriteLine("PRESS ANY KEY TO EXIT");
            Console.ReadKey();
        }
    }
}
