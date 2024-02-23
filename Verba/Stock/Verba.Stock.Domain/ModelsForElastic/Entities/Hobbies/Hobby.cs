namespace Verba.Stock.Domain.ModelsForElastic.Entities.Hobbies;

public class Hobby
{
    public string? NameOfHobby { get; set; }

    public override string ToString()
    {
        return $"{(string.IsNullOrEmpty(NameOfHobby) ? "Нет данных" : NameOfHobby)} \n";
    }
}
