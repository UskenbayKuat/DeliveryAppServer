using ApplicationCore.Entities;

namespace ApplicationCore.Models.Entities.Dictionaries
{
    public class DicSmsLog : BaseEntity
    {
        public DicSmsLog(int minuteLife, int tryCount, int inputCount, string message, string kazInfoErrorMessage, string tryCountMessage, string inputCountMessage, string codeErrorMessage, string lifeTimeErrorMessage)
        {
            MinuteLife = minuteLife;
            TryCount = tryCount;
            InputCount = inputCount;
            Message = message;
            KazInfoErrorMessage = kazInfoErrorMessage;
            TryCountMessage = tryCountMessage;
            InputCountMessage = inputCountMessage;
            CodeErrorMessage = codeErrorMessage;
            LifeTimeErrorMessage = lifeTimeErrorMessage;
        }
        public DicSmsLog()
        {
            
        }

        public int MinuteLife { get; private set; }
        public int TryCount { get; private set; }
        public int InputCount { get; private set; }
        public string Message { get; private set; }
        public string KazInfoErrorMessage { get; private set; }
        public string TryCountMessage { get; private set; }
        public string InputCountMessage { get; private set; }
        public string CodeErrorMessage { get; private set; }
        public string LifeTimeErrorMessage { get; private set; }
    }
}
