using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pb.Hangfire
{
    public class JobConfig
    {
        public static void InitConfig(HangfireConfig config)
        {
            UseHangfire = config.UseHangfire;
            HangfireServerName = config.HangfireServerName;
            HangfireConnectionString = config.HangfireConnectionString;
            HangfireDashboard = config.HangfireDashboard;
            ConnectionStrings = config.ConnectionStrings;
            HangfireJobs = config.HangfireJobs;
            AppSettings = config.AppSettings;
        }

        /// <summary>  
        /// 加载Job  
        /// </summary>  
        public static void LoadJobs()
        {
            foreach (var an in HangfireJobs.GroupBy(c => c.AssemblyName))
            {
                Assembly assembly = Assembly.Load(new AssemblyName(an.Key));
                foreach (var t in an)
                {
                    Type type = assembly.GetType(t.ClassName);
                    var task = JobActivator.Current.ActivateJob(type) as IJob;
                    if (task == null)
                    {
                        continue;
                    }
                    if (t.Enable)
                    {
                        RecurringJob.AddOrUpdate(t.JobName, () => task.Run(), t.Cron, timeZone: TimeZoneInfo.Local);
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(t.JobName);
                    }
                }
            }
        }
        public static bool UseHangfire { get; set; }
        public static string HangfireServerName { get; set; }
        public static string HangfireConnectionString { get; set; }
        public static string HangfireDashboard { get; set; }
        public static Dictionary<string, string> ConnectionStrings { get; set; }
        public static List<JobRegistInfo> HangfireJobs { get; set; }
        public static Dictionary<string,string> AppSettings { get; set; }
    }

    public class JobRegistInfo
    {
        public string JobName { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string Cron { get; set; }
        public bool Enable { get; set; } = true;
    }

    public class HangfireConfig
    {
        public bool UseHangfire { get; set; }
        public string HangfireServerName { get; set; }
        public string HangfireConnectionString { get; set; }
        public string HangfireDashboard { get; set; }
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public List<JobRegistInfo> HangfireJobs { get; set; }
        public Dictionary<string, string> AppSettings { get; set; }
    }
}
