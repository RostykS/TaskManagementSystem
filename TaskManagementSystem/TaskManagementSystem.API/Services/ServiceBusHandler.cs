using Newtonsoft.Json;
using Azure.Messaging.ServiceBus;
using TaskManagementSystem.API.Models;

namespace TaskManagementSystem.API.Services
{
    public class ServiceBusHandler : IServiceBusHandler
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;
        private readonly ServiceBusReceiver _serviceBusReceiver;
        private readonly ILogger<ServiceBusHandler> _logger;

        public ServiceBusHandler(IConfiguration configuration, ILogger<ServiceBusHandler> logger)
        {
            _connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");
            _queueName = configuration.GetValue<string>("ServiceBus:QueueName");

            var clientOptions = new ServiceBusClientOptions
            {
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    MaxRetries = 5,
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(30),
                }
            };

            _serviceBusClient = new ServiceBusClient(_connectionString, clientOptions);
            _serviceBusSender = _serviceBusClient.CreateSender(_queueName);
            _serviceBusReceiver = _serviceBusClient.CreateReceiver(_queueName);
            _logger = logger;
        }

        public async Task SendTaskCompletedEventAsync(TaskCompletedEvent completedEvent)
        {
            var messageBody = JsonConvert.SerializeObject(completedEvent);
            var message = new ServiceBusMessage(messageBody);

            try
            {
                await _serviceBusSender.SendMessageAsync(message);
                _logger.LogInformation("TaskCompletedEvent sent to Service Bus: TaskId={TaskId}", completedEvent.TaskId);
            }
            catch (ServiceBusException ex) when (ex.Reason == ServiceBusFailureReason.ServiceTimeout)
            {
                _logger.LogError("Timeout occurred while sending message to Service Bus. Retrying...");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while sending message to Service Bus: {ex.Message}");
                throw;
            }
        }

        public async Task ReceiveMessageAsync()
        {
            var receivedMessage = await _serviceBusReceiver.ReceiveMessageAsync();
            if (receivedMessage == null) return;

            var body = receivedMessage.Body.ToString();
            var completedEvent = JsonConvert.DeserializeObject<TaskCompletedEvent>(body);

            if (completedEvent != null)
            {
                _logger.LogInformation("Received TaskCompletedEvent from Service Bus: TaskId={TaskId}, Timestamp={Timestamp}",
                    completedEvent.TaskId, completedEvent.Timestamp);

                // Simulated work
                await Task.Delay(1000);
                _logger.LogInformation("Simulated processing completed for TaskId={TaskId}", completedEvent.TaskId);

                await _serviceBusReceiver.CompleteMessageAsync(receivedMessage);
            }
        }
    }
}