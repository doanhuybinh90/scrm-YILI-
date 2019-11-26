using Abp.Dependency;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Pb.Hangfire.Tool;

namespace Pb.Hangfire.Jobs
{
    public class TestJob : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public TestJob(AbpTimer timer) : base(timer)
        {
            Timer.Period = 600000;
        }

        protected override void DoWork()
        {
            IDBHelper mh = new MssqlHelper(JobConfig.ConnectionStrings["TestDb"]);
            //mh.ExcuteNonQuery("insert into testtable(Name) values('1');");
        }
    }
}
