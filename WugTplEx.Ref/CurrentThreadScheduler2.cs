using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RStein.Async.Schedulers;

namespace WugTplEx.Ref
{
  public class CurrentThreadScheduler2 : TaskSchedulerBase
  {
    private const int CONCURRENCY_LEVEL = 1;
    private readonly TaskSchedulerBase m_fallbackScheduler;

    public CurrentThreadScheduler2(TaskSchedulerBase fallbackScheduler)
    {
      if (fallbackScheduler == null)
      {
        throw new ArgumentNullException("fallbackScheduler");
      }

      m_fallbackScheduler = fallbackScheduler;
    }

    public override int MaximumConcurrencyLevel
    {
      get
      {
        return CONCURRENCY_LEVEL;
      }
    }

    public override void QueueTask(Task task)
    {
      m_fallbackScheduler.TryExecuteTaskInline(task, false);
      //ProxyScheduler.DoTryExecuteTask(task);
    }

    public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
      return m_fallbackScheduler.TryExecuteTaskInline(task, taskWasPreviouslyQueued);
      //return ProxyScheduler.DoTryExecuteTask(task);
    }

    public override IEnumerable<Task> GetScheduledTasks()
    {
      return Enumerable.Empty<Task>();
    }

    protected override void Dispose(bool disposing) {}
  }
}