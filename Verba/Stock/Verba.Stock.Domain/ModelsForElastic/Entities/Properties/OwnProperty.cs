using Nest;

namespace Verba.Stock.Domain.ModelsForElastic.Entities.Properties
{
    public class OwnProperty
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        [Text(Index = false)]
        public string Description { get; set; }

        public string PropertyCost { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"Тип: {(string.IsNullOrEmpty(Type) ? "Нет данных" : Type)}," +
                $" Название: {(string.IsNullOrEmpty(Name) ? "Нет данных" : Name)}, " +
                $"Регистрационный номер: {(string.IsNullOrEmpty(RegistrationNumber) ? "Нет данных" : RegistrationNumber)}, " +
                $"Описание: {(string.IsNullOrEmpty(Description) ? "Нет данных" : Description)}, " +
                $"Стоимость: {(string.IsNullOrEmpty(PropertyCost) ? "Нет данных" : PropertyCost)}, " +
                $"Расположение: {(string.IsNullOrEmpty(Location) ? "Нет данных" : Location)} \n";
        }
    }
}
