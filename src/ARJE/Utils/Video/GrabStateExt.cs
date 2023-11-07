namespace ARJE.Utils.Video
{
    public static class GrabStateExt
    {
        public static bool IsStop(this GrabState state)
        {
            return state is GrabState.Stopped or GrabState.Stopping;
        }
    }
}
