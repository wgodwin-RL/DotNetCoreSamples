using LD.Models.Messages;
using System.Threading.Tasks;

namespace LD.Models.Interfaces
{
    public interface IEventMessageData
    {
        Task UpsertEventMessage(StudentExamEventMessage data);
    }
}
