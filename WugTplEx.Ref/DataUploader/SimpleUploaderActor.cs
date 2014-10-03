using System;
using System.Threading;

namespace WugTplEx.Ref.DataUploader
{
  public class SimpleUploaderActor : IService
  {
    private enum SimpleUploaderState
    {
      None = 0,
      Running,
      Stopped
    }

    private SimpleUploaderState m_state;

    public SimpleUploaderActor()
    {
      m_state = SimpleUploaderState.Stopped;
    }

    public virtual void Start()
    {
      if (m_state == SimpleUploaderState.Running)
      {
        Console.WriteLine("SimpleUploader already running: tid {0}", Thread.CurrentThread.ManagedThreadId);
        return;
      }

      //Vytvoř graf šílených objektů - new UploadEngine(m_uploaderEngineLock).Start();
      m_state = SimpleUploaderState.Running;

      Console.WriteLine("SimpleUploader start: tid {0}", Thread.CurrentThread.ManagedThreadId);
    }

    public virtual void Stop()
    {
      if (m_state == SimpleUploaderState.Stopped)
      {
        Console.WriteLine("SimpleUploader already stopped: tid {0}", Thread.CurrentThread.ManagedThreadId);
        return;
      }
      m_state = SimpleUploaderState.Stopped;
      //Shoď graf šílených objektů a čekej na jejich ukončení
      Console.WriteLine("SimpleUploader stop: tid {0}", Thread.CurrentThread.ManagedThreadId);
    }
  }
}