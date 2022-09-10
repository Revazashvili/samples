namespace App;

public class GiveAttribute : Attribute
{
    public string Name { get; }

    public GiveAttribute(string name)
    {
        Name = name;
    }
}

public class DefineAttribute : Attribute
{
    
}