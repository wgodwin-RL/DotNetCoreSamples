using LD.Models.Constants;
using LD.Models.Interfaces;
using LD.Models.Messages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LD.Data
{
    public class EventMessageData : IEventMessageData
    {
        private readonly StudentExamMessageDatabaseContext _dBContext;
        public EventMessageData(StudentExamMessageDatabaseContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task UpsertEventMessage(StudentExamEventMessage data)
        {
            if (data.Event.ToLower() != MessageConstants.Score)
                throw new Exception($"invalid event type '{data.Event}'");

            var msgBody = data.Data;
            msgBody.Score = Math.Round(msgBody.Score, 2);
            
            var exists = _dBContext.StudentExamData.FirstOrDefault(x=> x.StudentId.ToLower() == msgBody.StudentId.ToLower() && x.Exam == msgBody.Exam);
            if (exists == null)
                _dBContext.StudentExamData.Add(msgBody);
            else
            {
                exists.Score = data.Data.Score;
            }

            await _dBContext.SaveChangesAsync();
        }
    }
}
