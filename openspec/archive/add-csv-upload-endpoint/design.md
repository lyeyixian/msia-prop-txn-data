## Context
The CSV upload feature is designed to handle property transaction data from Malaysian sources. The expected CSV format includes specific columns for property details, transaction information, and location data. Built using .NET 10 (C#) with PostgreSQL database and CsvHelper library for robust CSV parsing in the MsiaPropertyTransaction/ project.

## Goals / Non-Goals
- Goals: Accurately parse and store CSV data (~200 MB files) with the specified columns, validate data integrity, handle nullable fields appropriately
- Non-Goals: Handle arbitrary CSV formats, convert between formats, support compressed files, normalize database schema

## Decisions
- Decision: Use the provided column headers as the canonical format
- Alternatives considered: Flexible column mapping (rejected for simplicity and data consistency)
- Decision: Store stored columns in a single PostgreSQL table with appropriate data types (.NET decimal for large prices)
- Alternatives considered: Normalized database schema (rejected for initial simplicity, keep one table)
- Decision: Use Minimal APIs for endpoint implementation instead of controller classes
- Alternatives considered: Traditional controller classes (rejected for simplicity in .NET 10)

## CSV Format Specification
The CSV file must contain the following columns. Duplicated columns will be resolved by selecting the preferred version:

- Property Type
- District
- Mukim
- Scheme Name/Area
- Road Name
- Month, Year of Transaction Date (duplicate - not stored)
- Tenure
- Land/Parcel Area (preferred over Land Area)
- Unit (preferred over Unit duplicate and Unit 1)
- Main Floor Area (preferred over duplicate)
- Unit (duplicate column) - not stored
- Unit Level
- Transaction Price (duplicate column) - not stored
- Property Type (strata)
- Sector
- State
- Transaction Price (preferred)
- Land Area - not stored
- Main Floor Area (duplicate) - not stored
- Transaction Date (preferred over Month, Year of Transaction Date)
- Unit 1 - not stored

Stored columns (CSV name → DB column name):
- Property Type → property_type
- District → district
- Mukim → mukim
- Scheme Name/Area → scheme_name_area
- Road Name → road_name
- Tenure → tenure
- Land/Parcel Area → land_parcel_area
- Unit → unit
- Main Floor Area → main_floor_area
- Unit Level → unit_level
- Property Type (strata) → property_type_strata
- Sector → sector
- State → state
- Transaction Price → transaction_price
- Transaction Date → transaction_date

## Data Types and Validation (DB columns)
- property_type: string (required)
- district: string (required)
- mukim: string (required)
- scheme_name_area: string (required)
- road_name: string (nullable, empty string in CSV)
- tenure: string (required)
- land_parcel_area: decimal (nullable, "-" in CSV)
- unit: string (nullable, "-" in CSV)
- main_floor_area: decimal (required)
- unit_level: string (nullable, empty string in CSV)
- property_type_strata: string (required)
- sector: string (required)
- state: string (required)
- transaction_price: decimal (required, up to hundred millions)
- transaction_date: DateTime (required, dd/mm/yyyy format)

## Risks / Trade-offs
- Risk: Duplicate columns may cause data loss → Mitigation: Select preferred column per duplicate group
- Risk: Malformed dates → Mitigation: Validate dd/mm/yyyy format strictly
- Risk: Large CSV files (~200 MB) → Mitigation: Process in batches, monitor memory usage

## Migration Plan
No migration needed as this is a new feature.

## Resolved Decisions
- Missing values: Road Name and Unit Level treated as null (empty string in CSV), Land/Parcel Area and Unit treated as null ("-" in CSV)
- Location data: Keep as strings in single table, no normalization