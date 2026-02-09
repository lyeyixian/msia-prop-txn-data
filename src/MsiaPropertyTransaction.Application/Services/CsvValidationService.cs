using System.Globalization;
using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Services;

public class CsvValidationService : ICsvValidationService
{
    public List<string> ValidateCsvTransaction(CsvPropertyTransaction transaction)
    {
        var errors = new List<string>();

        // Required fields validation
        if (string.IsNullOrWhiteSpace(transaction.PropertyType))
            errors.Add("Property Type is required");
        if (string.IsNullOrWhiteSpace(transaction.District))
            errors.Add("District is required");
        if (string.IsNullOrWhiteSpace(transaction.Mukim))
            errors.Add("Mukim is required");
        if (string.IsNullOrWhiteSpace(transaction.SchemeNameArea))
            errors.Add("Scheme Name/Area is required");
        if (string.IsNullOrWhiteSpace(transaction.Tenure))
            errors.Add("Tenure is required");
        if (string.IsNullOrWhiteSpace(transaction.MainFloorArea))
            errors.Add("Main Floor Area is required");
        if (string.IsNullOrWhiteSpace(transaction.PropertyTypeStrata))
            errors.Add("Property Type (strata) is required");
        if (string.IsNullOrWhiteSpace(transaction.Sector))
            errors.Add("Sector is required");
        if (string.IsNullOrWhiteSpace(transaction.State))
            errors.Add("State is required");
        if (string.IsNullOrWhiteSpace(transaction.TransactionPrice))
            errors.Add("Transaction Price is required");
        if (string.IsNullOrWhiteSpace(transaction.TransactionDate))
            errors.Add("Transaction Date is required");

        // Data type validation
        if (!string.IsNullOrWhiteSpace(transaction.LandParcelArea) && transaction.LandParcelArea != "-")
        {
            if (!decimal.TryParse(transaction.LandParcelArea, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                errors.Add("Land/Parcel Area must be a valid number");
        }

        if (!string.IsNullOrWhiteSpace(transaction.MainFloorArea))
        {
            if (!decimal.TryParse(transaction.MainFloorArea, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                errors.Add("Main Floor Area must be a valid number");
        }

        if (!string.IsNullOrWhiteSpace(transaction.TransactionPrice))
        {
            if (!decimal.TryParse(transaction.TransactionPrice, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
                errors.Add("Transaction Price must be a valid number");
        }

        // Date validation (dd/mm/yyyy format)
        if (!string.IsNullOrWhiteSpace(transaction.TransactionDate))
        {
            if (!DateTime.TryParseExact(transaction.TransactionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                errors.Add("Transaction Date must be in dd/MM/yyyy format");
        }

        return errors;
    }

    public PropertyTransaction ConvertToEntity(CsvPropertyTransaction csvTransaction)
    {
        return new PropertyTransaction
        {
            PropertyType = csvTransaction.PropertyType!,
            District = csvTransaction.District!,
            Mukim = csvTransaction.Mukim!,
            SchemeNameArea = csvTransaction.SchemeNameArea!,
            RoadName = string.IsNullOrWhiteSpace(csvTransaction.RoadName) ? null : csvTransaction.RoadName,
            Tenure = csvTransaction.Tenure!,
            LandParcelArea = ParseNullableDecimal(csvTransaction.LandParcelArea),
            Unit = csvTransaction.Unit == "-" ? null : csvTransaction.Unit,
            MainFloorArea = decimal.Parse(csvTransaction.MainFloorArea!, NumberStyles.Number, CultureInfo.InvariantCulture),
            UnitLevel = string.IsNullOrWhiteSpace(csvTransaction.UnitLevel) ? null : csvTransaction.UnitLevel,
            PropertyTypeStrata = csvTransaction.PropertyTypeStrata!,
            Sector = csvTransaction.Sector!,
            State = csvTransaction.State!,
            TransactionPrice = decimal.Parse(csvTransaction.TransactionPrice!, NumberStyles.Number, CultureInfo.InvariantCulture),
            TransactionDate = DateTime.ParseExact(csvTransaction.TransactionDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        };
    }

    private decimal? ParseNullableDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "-")
            return null;

        if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            return result;

        return null;
    }
}
