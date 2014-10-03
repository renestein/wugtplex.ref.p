using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WugTplEx.Ref
{
  public class FallbackScheduler : TaskScheduler
  {
    public virtual bool RunTask(Task task)
    {
      return TryExecuteTaskInline(task, false);
    }

    protected override void QueueTask(Task task)
    {
      TryExecuteTask(task);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
      return TryExecuteTask(task);
    }

    protected override IEnumerable<Task> GetScheduledTasks()
    {
      return Enumerable.Empty<Task>();
    }
  }
}