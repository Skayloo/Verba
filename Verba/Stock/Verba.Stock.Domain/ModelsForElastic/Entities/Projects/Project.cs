namespace Verba.Stock.Domain.ModelsForElastic.Entities.Projects;

public class Project
{
    public string? NameOfProject { get; set; }

    public string? ProjectDescription { get; set; }

    public string? ProjectDetails { get; set; }

    public override string ToString()
    {
        return $" Название: {(string.IsNullOrEmpty(NameOfProject) ? "Нет данных" : NameOfProject)}, " +
            $"Описание: {(string.IsNullOrEmpty(ProjectDescription) ? "Нет данных" : ProjectDescription)}, " +
            $"Детали: {(string.IsNullOrEmpty(ProjectDetails) ? "Нет данных" : ProjectDetails)} \n";
    }
}
