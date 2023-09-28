//class that keeps track of the active player and active user
public static class DBManager
{
    public static int activePlayerBirthYr;
    public static int activePlayerId;
    public static string activePlayerName;
    public static string activePlayerPpsNo;
    public static string activePlayerSurname;
    public static string activeUser_id;
    public static string activeUsername;

    public static bool LoggedIn
    { get { return activeUsername != null; } }

    public static bool PlayerActive
    { get { return activePlayerName != null; } }

    public static void LogedOut()
    {
        activeUsername = null;
        activeUser_id = null;
    }

    public static void PlayerInactive()
    {
        activePlayerName = null;
    }
}