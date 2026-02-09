using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using MsiaPropertyTransaction.Application.Interfaces;
using MsiaPropertyTransaction.Domain.Entities;

namespace MsiaPropertyTransaction.Application.Services;

public class CsvParsingService : ICsvParsingService
{
    public IEnumerable<CsvPropertyTransaction> ParseCsv(Stream csvStream)
    {
        // Check if stream is empty or too small
        if (csvStream.Length == 0)
        {
            throw new InvalidOperationException("CSV file is empty");
        }

        using var reader = new StreamReader(csvStream);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null, // Ignore missing fields
            BadDataFound = null // Ignore bad data
        });

        // Read header to understand column structure
        csv.Read();
        csv.ReadHeader();

        var headers = csv.HeaderRecord ?? Array.Empty<string>();
        if (headers.Length == 0)
        {
            throw new InvalidOperationException("CSV must have a header row");
        }

        var duplicateResolvers = GetDuplicateResolvers(headers);

        var hasData = false;
        while (csv.Read())
        {
            hasData = true;
            yield return MapToCsvTransaction(csv, duplicateResolvers);
        }

        if (!hasData)
        {
            throw new InvalidOperationException("CSV must have at least one data row");
        }
    }

    private Dictionary<string, int> GetDuplicateResolvers(string[] headers)
    {
        var duplicateGroups = new Dictionary<string, List<int>>
        {
            {"Unit", new()},
            {"Transaction Price", new()},
            {"Land/Parcel Area", new()},
            {"Transaction Date", new()},
            {"Main Floor Area", new()}
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var header = headers[i].Trim();
            if (duplicateGroups.ContainsKey(header))
            {
                duplicateGroups[header].Add(i);
            }
        }

        var resolvers = new Dictionary<string, int>();
        foreach (var group in duplicateGroups)
        {
            if (group.Value.Count > 0)
            {
                resolvers[group.Key] = group.Value[0]; // Default to first occurrence
            }
        }

        return resolvers;
    }

    private CsvPropertyTransaction MapToCsvTransaction(CsvReader csv, Dictionary<string, int> duplicateResolvers)
    {
        var transaction = new CsvPropertyTransaction();

        // Map each field manually to handle duplicates
        if (csv.TryGetField("Property Type", out string? propertyType))
            transaction.PropertyType = propertyType;

        if (csv.TryGetField("District", out string? district))
            transaction.District = district;

        if (csv.TryGetField("Mukim", out string? mukim))
            transaction.Mukim = mukim;

        if (csv.TryGetField("Scheme Name/Area", out string? schemeNameArea))
            transaction.SchemeNameArea = schemeNameArea;

        if (csv.TryGetField("Road Name", out string? roadName))
            transaction.RoadName = roadName;

        if (csv.TryGetField("Tenure", out string? tenure))
            transaction.Tenure = tenure;

        if (csv.TryGetField("Land/Parcel Area", out string? landParcelArea))
            transaction.LandParcelArea = landParcelArea;

        // Handle Unit with duplicate resolution
        if (duplicateResolvers.TryGetValue("Unit", out int unitIndex))
        {
            if (csv.TryGetField(unitIndex, out string? unit))
            {
                transaction.Unit = unit;
            }
        }

        // Handle Main Floor Area with duplicate resolution
        if (duplicateResolvers.TryGetValue("Main Floor Area", out int mainFloorAreaIndex))
        {
            if (csv.TryGetField(mainFloorAreaIndex, out string? mainFloorArea))
            {
                transaction.MainFloorArea = mainFloorArea;
            }
        }

        if (csv.TryGetField("Unit Level", out string? unitLevel))
            transaction.UnitLevel = unitLevel;

        if (csv.TryGetField("Property Type (strata)", out string? propertyTypeStrata))
            transaction.PropertyTypeStrata = propertyTypeStrata;

        if (csv.TryGetField("Sector", out string? sector))
            transaction.Sector = sector;

        if (csv.TryGetField("State", out string? state))
            transaction.State = state;

        // Handle Transaction Price with duplicate resolution
        if (duplicateResolvers.TryGetValue("Transaction Price", out int transactionPriceIndex))
        {
            if (csv.TryGetField(transactionPriceIndex, out string? transactionPrice))
            {
                transaction.TransactionPrice = transactionPrice;
            }
        }

        // Handle Transaction Date with duplicate resolution
        if (duplicateResolvers.TryGetValue("Transaction Date", out int transactionDateIndex))
        {
            if (csv.TryGetField(transactionDateIndex, out string? transactionDate))
            {
                transaction.TransactionDate = transactionDate;
            }
        }

        return transaction;
    }
}
