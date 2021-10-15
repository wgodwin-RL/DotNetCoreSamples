using LD_Data;
using LD_Models;
using LD_Models.Interfaces;
using LD_Models.Messages;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LD.Data
{
    public class EventMessageData : IEventMessageData
    {
        private readonly DatabaseContext _dBContext;
        public EventMessageData(DatabaseContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task UpsertEventMessage(StudentExamEventMessage data)
        {
            if (data.Event.ToLower() != LD_Models.Messages.Constants.Score)
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
