// See https://aka.ms/new-console-template for more information

using App;

var car = new Car();
car.Do();

public partial class Car
{
    [Give("Print")]
    public partial void Do();
}

public class Functions
{
    [Define]
    public static void Print()
    {
        Console.WriteLine("Hello, World!");
    }
}