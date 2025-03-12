using Confluent.Kafka;
using Confluent.Kafka.Admin;
using OrderAPIV1.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace OrderAPIV1.Services
{
    public class OrderSevice : IOrderService
    {
        public async Task<string> PublishOrder(Order order, IConfiguration configuration)
        {
            string TopicName = configuration["TopicName"];
            var adminConfig = new AdminClientConfig
            {
                BootstrapServers = configuration["BootStrapServer"],
                ClientId = Dns.GetHostName()
            };
            using var adminClient = new AdminClientBuilder(adminConfig).Build();
            var topicSpecification = new TopicSpecification
            {
                Name = TopicName,
                NumPartitions = 1,
                ReplicationFactor = 1
            };
            try
            {
                await adminClient.CreateTopicsAsync(new List<TopicSpecification> { topicSpecification });
            }
            catch (CreateTopicsException e)
            {
                Debug.WriteLine($"An error occured creating the topic: {e.Results[0].Error.Reason}");
            }
            ProducerConfig producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["BootStrapServer"],
                ClientId = Dns.GetHostName()
            };

            try
            {
                //publish the message
                using (var producer = new ProducerBuilder
               <Null, string>(producerConfig).Build())
                {
                    var result = await producer.ProduceAsync
                   (TopicName, new Message<Null, string>
                   {
                       Value = JsonSerializer.Serialize(order)
                   });
                    Debug.WriteLine($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");
                    return await Task.FromResult($"Delivery Timestamp:{result.Timestamp.UtcDateTime}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Occurred: {ex.Message}");
                return await Task.FromResult($"Error Occurred: {ex.Message}");


            }
        }
    }
}
