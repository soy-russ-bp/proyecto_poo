namespace ARJE.Shared.Models
{
    public readonly struct PySysConsoleLogState : IDisposable
    {
        private PySysConsoleLogState(bool state)
        {
            this.State = state;
        }

        public bool State { get; }

        private static bool LogEnabled
        {
            get => !Keras.Keras.DisablePySysConsoleLog;
            set => Keras.Keras.DisablePySysConsoleLog = !value;
        }

        public static PySysConsoleLogState SetState(bool logEnabled)
        {
            PySysConsoleLogState oldState = new(LogEnabled);
            LogEnabled = logEnabled;
            return oldState;
        }

        public void Restore()
        {
            LogEnabled = this.State;
        }

        void IDisposable.Dispose()
        {
            this.Restore();
        }
    }
}
