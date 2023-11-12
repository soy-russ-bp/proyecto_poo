using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ARJE.Utils.Threading
{
    public class SingleThreadSynchronizationContext : SynchronizationContext
    {
        private BlockingCollection<QueuedTask> TaskQueue { get; } = new();

        public override void Send(SendOrPostCallback d, object? state)
        {
            var task = new QueuedSendTask(d, state);
            this.TaskQueue.Add(task);
            task.Wait();
        }

        public override void Post(SendOrPostCallback d, object? state)
        {
            var task = new QueuedTask(d, state);
            this.TaskQueue.Add(task);
        }

        public void RunOnCurrentThread()
        {
            while (this.TaskQueue.TryTake(out QueuedTask? task, Timeout.Infinite))
            {
                task.Invoke();
            }
        }

        public void Complete() => this.TaskQueue.CompleteAdding();

        private class QueuedTask
        {
            public QueuedTask(SendOrPostCallback callback, object? state)
            {
                ArgumentNullException.ThrowIfNull(callback);

                this.Callback = callback;
                this.State = state;
            }

            public SendOrPostCallback Callback { get; }

            public object? State { get; }

            public virtual void Invoke()
            {
                this.Callback.Invoke(this.State);
            }
        }

        private sealed class QueuedSendTask : QueuedTask
        {
            public QueuedSendTask(SendOrPostCallback task, object? state)
                : base(task, state)
            {
                this.OnCompleted = new ManualResetEventSlim(initialState: false);
            }

            private ManualResetEventSlim OnCompleted { get; }

            public override void Invoke()
            {
                base.Invoke();
                this.OnCompleted.Set();
            }

            public void Wait()
            {
                this.OnCompleted.Wait();
            }
        }
    }
}
