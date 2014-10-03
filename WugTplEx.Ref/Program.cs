using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RStein.Async.Actors.ActorsCore;
using RStein.Async.Schedulers;
using RStein.Async.Tasks;
using WugTplEx.Ref.DataUploader;

namespace WugTplEx.Ref
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      runSimpleUploaderActor();
       //runSimpleUploader(new SimpleUploader());
       //runTaskOnCurrentThreadScheduler();
      //runTaskunTaskOnCurrentThreadScheduler2();
      //runTask();
      //runBackgroundProcessor().Wait();
      Console.ReadLine();
    }

    private static void runTaskunTaskOnCurrentThreadScheduler2()
    {
      const int MAX_TASKS = 100;

      Console.WriteLine("Main thread id: {0}", Thread.CurrentThread.ManagedThreadId);

      var fallBackScheduler = new FallbackScheduler2();
      var scheduler = new CurrentThreadScheduler2(fallBackScheduler);
      var proxyScheduler = new ProxyScheduler(scheduler);
      fallBackScheduler.ProxyScheduler = proxyScheduler;

      var taskfactory = new TaskFactory(proxyScheduler.AsTplScheduler());
      var tasks = Enumerable.Range(0, MAX_TASKS).Select(taskId => taskfactory.StartNew(() =>
                                                                                       {
                                                                                         Console.WriteLine(" Task: {0} tid: {1}", taskId,
                                                                                           Thread.CurrentThread.ManagedThreadId);
                                                                                       }));

      Task.WhenAll(tasks).Wait();
    }

    private static void runSimpleUploaderActor()
    {
      const int NUMBER_OF_THREADS = 1;
      var ioService = new IoServiceScheduler();
      var primaryScheduler = new IoServiceThreadPoolScheduler(ioService, NUMBER_OF_THREADS);
      var proxyScheduler = new ProxyScheduler(primaryScheduler);

      var proxyEngine = new ProxyEngine(primaryScheduler);
      var simpleUploaderActor = new SimpleUploaderActor();

      var uploaderActorProxy = proxyEngine.CreateProxy<IService>(simpleUploaderActor);

      runSimpleUploader(uploaderActorProxy);
    }

    private static void runSimpleUploader(IService uploader)
    {
      var cancelationToken = new CancellationTokenSource();


      var startTask = Task.Run(() =>
                               {
                                 while (!cancelationToken.IsCancellationRequested)
                                 {
                                   uploader.Start();
                                 }
                               });

      var stopTask = Task.Run(() =>
                              {
                                while (!cancelationToken.IsCancellationRequested)
                                {
                                  uploader.Stop();
                                }
                              });

      Console.ReadKey();
      cancelationToken.Cancel();
      Console.WriteLine("Cancel request...");
      Task.WhenAll(startTask, stopTask).Wait();
      Console.WriteLine("Uploader finished...");
    }

    private static void runTaskOnCurrentThreadScheduler()
    {
      const int MAX_TASKS = 100;

      Console.WriteLine("Main thread id: {0}", Thread.CurrentThread.ManagedThreadId);

      var fallBackScheduler = new FallbackScheduler();
      var scheduler = new CurrentThreadScheduler(new FallbackScheduler());
      var taskfactory = new TaskFactory(scheduler);
      var tasks = Enumerable.Range(0, MAX_TASKS).Select(taskId => taskfactory.StartNew(() =>
                                                                                       {
                                                                                         Console.WriteLine(" Task: {0} tid: {1}", taskId,
                                                                                           Thread.CurrentThread.ManagedThreadId);
                                                                                       }));

      Task.WhenAll(tasks).Wait();
    }

    private static void runTask()
    {
      var task = Task.Run(() =>
                          {
                            Console.WriteLine("Running task: thread {0}", Thread.CurrentThread.ManagedThreadId);
                          });
      task.Wait();
    }

    public static async Task runBackgroundProcessor()
    {
      const int DELAY_MS = 2000;
      var processor = new BackgroundProcessor();
      var task = processor.RunBackgroundOperation(() => Console.WriteLine("Moje operace 1"), "Operace 1");
      var task2 = processor.RunBackgroundOperation(() =>
                                                   {
                                                     Console.WriteLine("Moje operace 2");
                                                     throw new InvalidOperationException();
                                                   }, "Operace s výjimkou");

      //Race condition!
      await Task.Delay(DELAY_MS);
      //Race condition!
      DebugTaskCompletionSourceServices.DetectBrokenTaskCompletionSources();
      await task;      
      Console.WriteLine("Wait 1 done");

      try
      {
        await task2;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
      finally
      {
        Console.WriteLine("Wait 2 done");
      }
    }
  }
}