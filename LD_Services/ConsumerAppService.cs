using LD_Models;
using LD_Models.Configuration;
using LD_Models.Interfaces;
using LD_Models.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LD_Services
{
    public class ConsumerAppService : IConsumerAppService
    {
        private readonly ILogger<ConsumerAppService> _logger;
        private readonly AppSettings _appSettings;
        private readonly IEventMessageData _eventMessageData;

        public ConsumerAppService(IOptions<AppSettings> appSettings, ILogger<ConsumerAppService> logger, IEventMessageData eventMessageData)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _eventMessageData = eventMessageData;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            try
            {
                var client = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(5)
                };

                string url = _appSettings.StreamUrl;

                var messageBuffer = new StringBuilder();
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        _logger.LogDebug("establishing a connection");
                        string eventType = string.Empty;
                        using (var streamReader = new StreamReader(await client.GetStreamAsync(url)))
                        {
                            
                            while (!streamReader.EndOfStream)
                            {
                                var messageLine = await streamReader.ReadLineAsync();
                                if (string.IsNullOrWhiteSpace(messageLine))
                                    continue;

                                if (string.IsNullOrWhiteSpace(eventType))
                                {
                                    eventType = await ParseMessageEventType(messageLine);
                                    continue;
                                }
                                else if (eventType != Constants.Score)
                                {
                                    _logger.LogWarning($"unsupported event type: {eventType}");

                                    messageBuffer.Clear();
                                    eventType = string.Empty;
                                    continue;
                                }

                                messageBuffer.Append(await ParseMessageData(messageLine));
                                var message = messageBuffer.ToString();
                                
                                _logger.LogInformation($" message: {message}");

                                var eventMessage = new StudentExamEventMessage() { Event = eventType, Data = JsonConvert.DeserializeObject<StudentExamData>(message) };
                                await _eventMessageData.UpsertEventMessage(eventMessage);
                                
                                messageBuffer.Clear();
                                eventType = string.Empty;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Error: {e.Message}");
                        _logger.LogError("Retrying in 5 seconds");
                        await Task.Delay(TimeSpan.FromSeconds(5));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }

        private async Task<string> ParseMessageEventType(string messageLine)
        {
            return !string.IsNullOrWhiteSpace(messageLine) && messageLine.Contains(Constants.Score) ? Constants.Score : Constants.Unknown;
        }

        private async Task<string> ParseMessageData(string messageLine)
        {
            return string.IsNullOrWhiteSpace(messageLine) ? string.Empty : messageLine.Remove(0, Constants.Data.Length);
        }
    }
}
