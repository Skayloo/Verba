namespace Verba.Stock.Domain.ModelsForElastic.Entities.Languages;

public class Language
{
    public string? NameOfLanguage { get; set; }

    public string? ProficiencyLevel { get; set; }

    public override string ToString()
    {
        return $"Название: {(string.IsNullOrEmpty(NameOfLanguage) ? "Нет данных" : NameOfLanguage)}, " +
            $"Уровень владения: {(string.IsNullOrEmpty(ProficiencyLevel) ? "Нет данных" : ProficiencyLevel)} \n";
    }
}
