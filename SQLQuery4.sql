-- ═══════════════════════════════════════════════════════════
-- SQL Migration: Add UserId to Tasks + re-hash passwords
-- Run against: TaskTracker database (localdb)\MSSQLLocalDB
-- ═══════════════════════════════════════════════════════════

-- STEP 1: Add UserId column to Tasks (nullable first so existing rows don't fail)
ALTER TABLE Tasks ADD UserId UNIQUEIDENTIFIER NULL;

-- STEP 2: Set existing tasks to the first user (or delete orphaned tasks)
-- Option A — delete all existing tasks (recommended if data is test data)
DELETE FROM Tasks;

-- Option B — assign all existing tasks to a specific user (replace the GUID below)
-- UPDATE Tasks SET UserId = 'YOUR-USER-GUID-HERE' WHERE UserId IS NULL;

-- STEP 3: Make UserId NOT NULL and add foreign key
ALTER TABLE Tasks ALTER COLUMN UserId UNIQUEIDENTIFIER NOT NULL;

ALTER TABLE Tasks
ADD CONSTRAINT FK_Tasks_SignUps_UserId
    FOREIGN KEY (UserId) REFERENCES SignUps(Id) ON DELETE CASCADE;

-- STEP 4: Clear existing users (their passwords are plain text and invalid now)
-- New registrations will use bcrypt hashed passwords automatically.
-- WARNING: This deletes all user accounts. Re-register after running.
DELETE FROM Tasks;   -- tasks first due to FK
DELETE FROM SignUps;

-- STEP 5: Verify
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Tasks';

SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'SignUps';
