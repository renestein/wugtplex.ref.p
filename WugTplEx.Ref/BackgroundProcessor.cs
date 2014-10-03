using System;
using System.Threading;
using System.Threading.Tasks;
using RStein.Async.Tasks;

namespace WugTplEx.Ref
{
  public class BackgroundProcessor
  {
    public Task RunBackgroundOperation(Action action, string key)
    {
      if (action == null)
      {
        throw new ArgumentNullException("action");
      }

      //var taskCompletionSource = new TaskCompletionSource<object>();

      var taskCompletionSource = new DebugTaskCompletionSource<object>(string.Format("Task Source: RunBackgroundOperation {0}", key));

      ThreadPool.QueueUserWorkItem(_ =>
                                   {
                                     try
                                     {
                                       action();
                                       Thread.Sleep(1000);
                                       taskCompletionSource.TrySetResult(null);
                                     }
                                     catch (Exception ex)
                                     {
                                       Console.WriteLine(ex);
                                       //taskCompletionSource.TrySetException(ex);
                                     }
                                   });

      return taskCompletionSource.Task;
    }
  }
}