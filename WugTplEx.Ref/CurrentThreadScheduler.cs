using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WugTplEx.Ref
{
  public class CurrentThreadScheduler : TaskScheduler
  {
    private FallbackScheduler m_alternateScheduler;

    public CurrentThreadScheduler(FallbackScheduler alternateScheduler)
    {
      m_alternateScheduler = alternateScheduler;
    }


    protected override void QueueTask(Task task)
    {
      m_alternateScheduler.RunTask(task);
      //TryExecuteTask(task);
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
      return m_alternateScheduler.RunTask(task);
      //return TryExecuteTask(task);
    }

    protected override IEnumerable<Task> GetScheduledTasks()
    {
      return Enumerable.Empty<Task>();
    }
  }
}