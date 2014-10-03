using System;
using System.Threading;

namespace WugTplEx.Ref.DataUploader
{
  public class SimpleUploader : IService
  {
    private SimpleUPloaderState m_state;

    private Object m_uploaderEngineLock;
    private Object m_uploaderLock;

    public SimpleUploader()
    {
      m_state = SimpleUPloaderState.Stopped;
      m_uploaderLock = new object();
      m_uploaderEngineLock = new object();
    }

    public void Start()
    {
      lock (m_uploaderLock)
      {
        lock (m_uploaderEngineLock)
        {
          if (m_state == SimpleUPloaderState.Running)
          {
            Console.WriteLine("SimpleUploader already running: tid {0}", Thread.CurrentThread.ManagedThreadId);
            return;
          }
          //Vytvoč graf šílených objektů - new UploadEngine(m_uploaderEngineLock).Start();
          m_state = SimpleUPloaderState.Running;
        }
        Console.WriteLine("SimpleUploader start: tid {0}");
      }
    }

    public void Stop()
    {
      lock (m_uploaderEngineLock)
      {
        lock (m_uploaderLock)
        {
          Console.WriteLine("SimpleUploader start: tid {0}", Thread.CurrentThread.ManagedThreadId);
        }

        if (m_state == SimpleUPloaderState.Stopped)
        {
          Console.WriteLine("SimpleUploader already stopped: tid {0}", Thread.CurrentThread.ManagedThreadId);
          return;
        }
        m_state = SimpleUPloaderState.Stopped;
        //Shoď graf šílených objektů a čekej na jejich ukončení
      }

      Console.WriteLine("SimpleUploader start: tid {0}");
    }

    private enum SimpleUPloaderState
    {
      None = 0,
      Running,
      Stopped
    }
  }
}