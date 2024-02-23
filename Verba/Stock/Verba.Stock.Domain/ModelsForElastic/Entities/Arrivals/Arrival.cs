namespace Verba.Stock.Domain.ModelsForElastic.Entities.Arrivals;

public class Arrival
{
    public string? DateOfArrival { get; set; }

    public string? PlaceOfStay { get; set; }

    public string? AimOfVisit { get; set; }

    public string? ArrivalCountry { get; set; }

    public override string ToString()
    {
        return $"Дата прибытия: {(string.IsNullOrEmpty(DateOfArrival) ? "Нет данных" : DateOfArrival)}, " +
            $"Место пребывания: {(string.IsNullOrEmpty(PlaceOfStay) ? "Нет данных" : PlaceOfStay)}, " +
            $"Цель прибытия: {(string.IsNullOrEmpty(AimOfVisit) ? "Нет данных" : AimOfVisit)}, " +
            $"Страна: {(string.IsNullOrEmpty(ArrivalCountry) ? "Нет данных" : ArrivalCountry)} \n";
    }
}
