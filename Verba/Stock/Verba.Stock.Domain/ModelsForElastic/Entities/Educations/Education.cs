namespace Verba.Stock.Domain.ModelsForElastic.Entities.Educations;

public class Education
{
    public string? PlaceOfStudy { get; set; }

    public string? AcademicDegree { get; set; }

    public override string ToString()
    {
        return $"Наименование заведения: {(string.IsNullOrEmpty(PlaceOfStudy) ? "Нет данных" : PlaceOfStudy)}, " +
            $"Ученая степень: {(string.IsNullOrEmpty(AcademicDegree) ? "Нет данных" : AcademicDegree)} \n";
    }
}
