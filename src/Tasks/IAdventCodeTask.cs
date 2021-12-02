using System.Collections.Generic;
using System.Threading.Tasks;
using AdventCode.Utils;

namespace AdventCode.Tasks
{
    public interface IAdventCodeTask
    {
        int TaskDay { get; }
        Task<string?> GetFirstTaskAnswer();

        Task<string?> GetSecondTaskAnswer();
    }

    public abstract class BaseCodeTask : IAdventCodeTask
    {
        private readonly IAdventWebClient _client;
        protected abstract string TestData { get; }
        public abstract int TaskDay { get; }

        public BaseCodeTask(IAdventWebClient client)
        {
            _client = client;
        }

        protected async Task<string> GetData()
        {
            var dataInput = await _client.GetDayInput(TaskDay);
            if (dataInput == null)
            {
                throw new NoDataException();
            }
            return dataInput;
        }

        protected Task<string> GetTestData()
        {
            return Task.FromResult(TestData);
        }

        protected Task<List<T>> GetTestDataAsList<T>()
        {
            return Task.FromResult(TestData.ToDataList<T>());
        }

        protected async Task<List<T>> GetDataAsList<T>()
        {
            var dataInput = await _client.GetDayInputList<T>(TaskDay);
            return dataInput;
        }

        public abstract Task<string?> GetFirstTaskAnswer();
        public abstract Task<string?> GetSecondTaskAnswer();
    }
}