// See https://aka.ms/new-console-template for more information

using App;

Test.P();


public partial class Car
{
    [Give("Print")]
    partial void Do();
}

public class Functions
{
    [Define]
    public static void Print()
    {
        Console.WriteLine("Hello, World!");
    }
}