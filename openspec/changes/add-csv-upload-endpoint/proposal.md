# Change: Add CSV Upload Endpoint

## Why
To enable users to upload CSV files containing property transaction data and automatically store it in the PostgreSQL database for further analysis and processing.

## What Changes
- Add new Minimal API endpoint `/api/upload-csv` that accepts CSV files with property transaction data
- Implement CSV parsing and validation for the specified column format using TDD
- Store parsed data into PostgreSQL tables with appropriate data types using TDD
- Add error handling for malformed CSV files, missing columns, and invalid data types
- All requirements implemented following Test-Driven Development principles

## Technical Details
- Tech stack: .NET 10 (C#), PostgreSQL database, CsvHelper library for CSV parsing
- CSV columns: Property Type, District, Mukim, Scheme Name/Area, Road Name, Month Year of Transaction Date, Tenure, Land/Parcel Area, Unit, Main Floor Area, Unit Level, Transaction Price, Property Type (strata), Sector, State, Transaction Price, Land Area, Main Floor Area, Transaction Date, Unit 1
- DB column names: snake_case convention (e.g., property_type, transaction_price)
- Data types: strings (nullable where specified), numerics (up to hundred millions for prices), dates (dd/mm/yyyy format)
- Nullable fields: Road Name (empty string), Unit Level (empty string), Land/Parcel Area ("-"), Unit ("-")
- Duplicate columns resolved by selecting preferred versions: Unit over Unit duplicate/Unit 1, Land/Parcel Area over Land Area, Transaction Date over Month Year, Transaction Price over duplicate, Main Floor Area over duplicate
- File size: ~200 MB, processed in batches

## Impact
- Affected specs: data-import (new capability)
- Affected code: Minimal API endpoints in MsiaPropertyTransaction/Program.cs, data processing utilities in MsiaPropertyTransaction/Services/, database models in MsiaPropertyTransaction/Models/