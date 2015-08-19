namespace UTHPortal.Models
{
    public class PushEvent
    {
        public PushEvent()
        { }

        public PushEvent(string Url, string Name)
        {
            this.Url = Url;
            this.Name = Name;
        }

        public string Url { get; set; }
        public string Name { get; set; }
    }
}
