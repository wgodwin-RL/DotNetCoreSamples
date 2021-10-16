using LD.Models;
using LD.Models.Configuration;
using LD.Models.Constants;
using LD.Models.Interfaces;
using LD.Models.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LD.Services
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
                        WebRequest myReq = WebRequest.Create(url);
                        WebResponse wr = myReq.GetResponse();
                        Stream receiveStream = wr.GetResponseStream();
                        using (var streamReader = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            while (!streamReader.EndOfStream)
                            {
                                var messageLine = await streamReader.ReadLineAsync();
                                //if (string.IsNullOrWhiteSpace(messageLine))
                                //    continue;

                                try
                                {
                                    if (string.IsNullOrWhiteSpace(messageLine))
                                    {
                                        while (true)
                                        {
                                            messageLine = await streamReader.ReadLineAsync();
                                            if (!string.IsNullOrWhiteSpace(messageLine))
                                                break;
                                        }
                                    }

                                    eventType = await ParseMessageEventType(messageLine);
                                    var scoreData = await streamReader.ReadLineAsync();

                                    if (eventType != MessageConstants.Score)
                                    {
                                        _logger.LogWarning($"unsupported event type: {eventType}");

                                        messageBuffer.Clear();
                                        eventType = string.Empty;
                                        continue;
                                    }

                                    if (string.IsNullOrWhiteSpace(scoreData)) 
                                    {
                                        _logger.LogWarning($"unsupported event type: {eventType}");
                                        messageBuffer.Clear();
                                        eventType = string.Empty;
                                        continue;
                                    }

                                    messageBuffer.Append(await ParseMessageData(scoreData));
                                    var message = messageBuffer.ToString();

                                    _logger.LogInformation($" message: {message}");

                                    var eventMessage = new StudentExamEventMessage() { Event = eventType, Data = JsonConvert.DeserializeObject<StudentExamData>(message) };
                                    await _eventMessageData.UpsertEventMessage(eventMessage);
                                }
                                finally 
                                {
                                    messageBuffer.Clear();
                                    eventType = string.Empty;
                                }
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
            return !string.IsNullOrWhiteSpace(messageLine) && messageLine.Contains(MessageConstants.Score) ? MessageConstants.Score : MessageConstants.Unknown;
        }

        private async Task<string> ParseMessageData(string messageLine)
        {
            return string.IsNullOrWhiteSpace(messageLine) ? string.Empty : messageLine.Remove(0, MessageConstants.Data.Length);
        }
    }
}
