public class Player
{
    private int birthYr, id;
    private string name, ppsNo, surname;

    public int GetBirthYr()
    {
        return birthYr;
    }

    public int GetId()
    {
        return id;
    }

    public string GetName()
    {
        return name;
    }

    public string GetPpsNo()
    {
        return ppsNo;
    }

    public string GetSurname()
    {
        return surname;
    }

    public void SetBirthYr(int value)
    {
        birthYr = value;
    }

    public void SetId(int value)
    {
        id = value;
    }

    public void SetName(string value)
    {
        name = value;
    }

    public void SetPpsNo(string value)
    {
        ppsNo = value;
    }

    public void SetSurname(string value)
    {
        surname = value;
    }
}