namespace dumpBot;
/*public class ChatUsers
{
    public Dictionary<int, string> DumpTestUsers { get; } = new();
    public Dictionary<int, string> NaPivchUsers { get; } = new();
    
}*/

public class ChatUsers
{
    public Dictionary<int, string> DumpTestUsers { get; } = new Dictionary<int, string>();
    public Dictionary<int, string> NaPivchUsers { get; } = new Dictionary<int, string>();
    private int lastAssignedDumpTestNumber = 0;
    private int lastAssignedNaPivchNumber = 0;

    // Метод для додавання нового користувача до словника DumpTestUsers
    public void AddDumpTestUser(string username)
    {
        lastAssignedDumpTestNumber++;
        DumpTestUsers.Add(lastAssignedDumpTestNumber, username);
    }

    // Метод для додавання нового користувача до словника NaPivchUsers
    public void AddNaPivchUser(string username)
    {
        lastAssignedNaPivchNumber++;
        NaPivchUsers.Add(lastAssignedNaPivchNumber, username);
    }
}



