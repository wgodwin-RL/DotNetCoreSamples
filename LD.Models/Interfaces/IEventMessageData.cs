using LD_Models;
using LD_Models.Messages;
using System.Threading.Tasks;

namespace LD_Models.Interfaces
{
    public interface IEventMessageData
    {
        Task UpsertEventMessage(StudentExamEventMessage data);
    }
}
