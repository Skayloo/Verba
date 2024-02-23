using Nest;

namespace Verba.Stock.Domain.ModelsForElastic.Entities.Workplaces;

public class Workplace
{
    public string? NameOfOrganization { get; set; }

    public string? Department { get; set; }

    public string? Position { get; set; }

    public string? Rank { get; set; }

    public string? Contacts { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? SecretClearance { get; set; }

    public string? AreaOfActivity { get; set; }

    [Text(Index = false)]
    public string? Notes { get; set; }

    public override string ToString()
    {
        return $"Название организации: {(string.IsNullOrEmpty(NameOfOrganization) ? "Нет данных" : NameOfOrganization)}, " +
            $"Отдел: {(string.IsNullOrEmpty(Department) ? "Нет данных" : Department)}, " +
            $"Должность: {(string.IsNullOrEmpty(Position) ? "Нет данных" : Position)}, " +
            $"Звание: {(string.IsNullOrEmpty(Rank) ? "Нет данных" : Rank)}, " +
            $"Эл.почта: {(string.IsNullOrEmpty(Email) ? "Нет данных" : Email)}, " +
            $"Адрес: {(string.IsNullOrEmpty(Address) ? "Нет данных" : Address)}, " +
            $"Доступ к секретным сведениям: {(string.IsNullOrEmpty(SecretClearance) ? "Нет данных" : SecretClearance)}, " +
            $"Направление деятельности: {(string.IsNullOrEmpty(AreaOfActivity) ? "Нет данных" : AreaOfActivity)}, " +
            $"Заметки: {(string.IsNullOrEmpty(Notes) ? "Нет данных" : Notes)} \n";
    }
}
