## Purpose

Define requirements for importing property transaction data via CSV file uploads, including validation, parsing, and database storage.

## Requirements

### Requirement: CSV File Upload

The system SHALL provide an API endpoint that accepts CSV file uploads with property transaction data and stores the data in PostgreSQL database. The CSV SHALL contain the specified columns for property details, transaction information, and location data. Duplicate columns SHALL be resolved by selecting preferred versions as documented.

#### Scenario: Successful CSV Upload
- **WHEN** a valid CSV file with correct headers and data types is uploaded to the endpoint
- **THEN** the data is parsed, validated against expected formats, and stored in the database
- **AND** a success response is returned with the number of records inserted

#### Scenario: Invalid CSV Format
- **WHEN** a CSV file with incorrect headers or malformed data is uploaded (e.g., invalid dd/mm/yyyy dates, non-numeric prices)
- **THEN** an error response is returned indicating the specific validation failure
- **AND** no data is stored in the database

#### Scenario: Database Insertion Failure
- **WHEN** valid CSV data cannot be inserted due to database constraints or duplicate key violations
- **THEN** an error response is returned with details about the failure
- **AND** partial data may be rolled back if supported

### Requirement: CSV Format Validation

The system SHALL validate that uploaded CSV files contain all required columns and that data conforms to expected formats (e.g., numeric fields contain numbers, dates are valid).

#### Scenario: Missing Required Columns
- **WHEN** a CSV file is missing any of the required columns
- **THEN** validation fails with an error message listing missing columns

#### Scenario: Invalid Data Types
- **WHEN** a CSV file has invalid data types (e.g., text in numeric fields)
- **THEN** validation fails with an error message indicating the invalid fields and rows

#### Scenario: Nullable Field Handling
- **WHEN** a CSV file has "-" for Land/Parcel Area or Unit, or empty string for Road Name or Unit Level
- **THEN** these values are stored as null in the database
