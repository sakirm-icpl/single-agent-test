namespace WazuhCommon.Models
{
    public class  WazuhCommandQueueItem
    {
        public string QueueName { get; }

        public string Command { get; }

        public LabelOperation LabelOperation { get; }

        public string Label { get; }

        public WazuhCommandQueueItem(string queueName, string commandName, LabelOperation labelOperation, string labelName)
        {
            QueueName = queueName;
            Command = commandName;
            LabelOperation = labelOperation;
            Label = labelName;
        }

        public WazuhCommandQueueItem(string queueName, string commandName)
        {
            QueueName = queueName;
            Command = commandName;
            LabelOperation = LabelOperation.None;
            Label = "";
        }
    }
}
