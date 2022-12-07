namespace ScheduleTelegramBot.ResponseUtils
{
    public class PSUWebSiteConnection
    {
        public  string Url { get; private set; }

        public PSUWebSiteConnection(long userId)
        {
            Url = GetUrlToSchedule(userId);
        }

        private string GetUrlToSchedule(long userId)
        {
            //here i will get from DB GroupName or FIO for getting schedule url
            return "https://www.polessu.by/ruz/term2/?q=88A9A17514110A3E&f=1"; //now here url 20MПП-1 group
        }
    }
}
