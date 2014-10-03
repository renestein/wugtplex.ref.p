using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RStein.Async.Schedulers;

namespace WugTplEx.Ref
{
  public class FallbackScheduler2 : TaskSchedulerBase
  {
    public override int MaximumConcurrencyLevel
    {
      get
      {
        return Int32.MaxValue;
      }
    }

    public override void QueueTask(Task task)
    {
      ProxyScheduler.DoTryExecuteTask(task);
    }

    public override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
      return ProxyScheduler.DoTryExecuteTask(task);
    }

    public override IEnumerable<Task> GetScheduledTasks()
    {
      return Enumerable.Empty<Task>();
    }

    protected override void Dispose(bool disposing) {}
  }
}