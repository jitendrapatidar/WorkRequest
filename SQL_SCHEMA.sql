-- SQL_SCHEMA.sql
-- This provides the conceptual SQL schema for the Work Request Tracker based on Entity Framework Core.

CREATE TABLE WorkRequests (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    ClientName TEXT NOT NULL,
    Description TEXT NULL,
    Priority TEXT NOT NULL,
    Status TEXT NOT NULL,
    DueDate TEXT NOT NULL,  
    CreatedDate TEXT NOT NULL,
    UpdatedDate TEXT NOT NULL
);

CREATE TABLE Notes (
    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    Text TEXT NOT NULL,
    CreatedDate TEXT NOT NULL,
    WorkRequestId INTEGER NOT NULL,
    CONSTRAINT FK_Notes_WorkRequests_WorkRequestId FOREIGN KEY (WorkRequestId) REFERENCES WorkRequests (Id) ON DELETE CASCADE
);

-- Index 1: Status
CREATE INDEX IX_WorkRequest_Status ON WorkRequests (Status);
-- Explanation: Users will frequently filter the list by status (e.g., viewing all "New" or "InProgress" items). This index speeds up those queries.

-- Index 2: ClientName
CREATE INDEX IX_WorkRequest_ClientName ON WorkRequests (ClientName);
-- Explanation: Users will search by Client Name. Having an index on this column will improve the performance of search queries using `LIKE` or direct matches.

-- Index 3: Foreign Key Index
CREATE INDEX IX_Notes_WorkRequestId ON Notes (WorkRequestId);
-- Explanation: When retrieving a WorkRequest, we also fetch its Notes. This index ensures that finding the Notes for a specific WorkRequest is fast.
