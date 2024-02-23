using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verba.Stock.Domain.ModelsForElastic.Entities.IdentityDocuments;

public class IdentityDocument
{
    public string? TypeOfDocument { get; set; }

    public string? DateOfIssue { get; set; }

    public string? DateOfExpire { get; set; }

    public string? SerialNumber { get; set; }

    public override string ToString()
    {
        return $"Тип: {(string.IsNullOrEmpty(TypeOfDocument) ? "Нет данных" : TypeOfDocument)}, " +
            $"Дата выдачи: {(string.IsNullOrEmpty(DateOfIssue) ? "Нет данных" : DateOfIssue)}, " +
            $"Дата истечения: {(string.IsNullOrEmpty(DateOfExpire) ? "Нет данных" : DateOfExpire)}, " +
            $"Серийный номер: {(string.IsNullOrEmpty(SerialNumber) ? "Нет данных" : SerialNumber)} \n";
    }
}
