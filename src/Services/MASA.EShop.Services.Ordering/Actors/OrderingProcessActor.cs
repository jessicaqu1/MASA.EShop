namespace MASA.EShop.Services.Ordering.Actors
{
    public class OrderingProcessActor : Actor, IOrderingProcessActor, IRemindable
    {
        public OrderingProcessActor(ActorHost host) : base(host)
        {
        }

        public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
        {
            throw new NotImplementedException();
        }
    }
}
