## 1. Database Schema (TDD)
- [x] 1.1 Write tests for database table structure with snake_case column names and constraints
- [x] 1.2 Create database migration scripts to define schema with conventional column names
- [x] 1.3 Run tests to verify schema creation

## 2. CSV Parsing and Validation (TDD)
- [ ] 2.1 Write unit tests in MsiaPropertyTransaction.Tests/ for CSV parsing with duplicate column resolution
- [ ] 2.2 Implement CSV parsing utilities in MsiaPropertyTransaction/Services/ that select preferred columns from duplicates
- [ ] 2.3 Write unit tests for data validation rules on stored columns (data types, required fields)
- [ ] 2.4 Implement validation logic in MsiaPropertyTransaction/Services/ for dates, numerics, and string formats

## 3. Database Insertion Logic (TDD)
- [ ] 3.1 Write integration tests for data insertion with nullable fields
- [ ] 3.2 Implement database insertion functions in MsiaPropertyTransaction/Services/ with batch processing for large files
- [ ] 3.3 Write tests for error handling during insertion
- [ ] 3.4 Implement error handling and transaction rollback logic in MsiaPropertyTransaction/Services/

## 4. API Endpoint (TDD)
- [ ] 4.1 Write integration tests for CSV upload endpoint
- [ ] 4.2 Implement Minimal API endpoint handler in MsiaPropertyTransaction/Program.cs
- [ ] 4.3 Write tests for request validation and file handling
- [ ] 4.4 Implement request validation and response formatting in MsiaPropertyTransaction/Program.cs

## 5. End-to-End Testing
- [ ] 5.1 Write end-to-end tests for complete CSV upload flow
- [ ] 5.2 Test with valid CSV files
- [ ] 5.3 Test error cases (malformed CSV, duplicate data)
- [ ] 5.4 Performance testing with large CSV files