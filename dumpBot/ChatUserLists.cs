public class ChatUsers
{
    public ChatUsers()
    {
        DampTestUsers = new Dictionary<int, string>
        {
            { 1, "sashakuzo" },
            { 2, "tod993" }
        };

        NaPivchUsers = new Dictionary<int, string>
        {
            { 1, "sashakuzo" },
            { 2, "hroshko_p" },
            { 3, "roonua1" },
            { 4, "Healermanrober" },
            { 5, "Kostya" },
            { 6, "Рузана" },
            { 7, "iamfuss" }
        };
    }

    public Dictionary<int, string> DampTestUsers { get; set; }
    public Dictionary<int, string> NaPivchUsers { get; set; }
}