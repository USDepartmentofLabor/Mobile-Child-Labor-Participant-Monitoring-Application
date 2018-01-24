/* Saved Queries */

/* Get Person Hazardous Conditions by Person InternalId*/
SELECT
  People.InternalId,
  FirstName,
  LastName,
  CanonicalName,
  DisplayName
FROM
  People
INNER JOIN
  PersonHazardousCondition C ON People.InternalId = C.PersonInternalId
INNER JOIN
  StatusCustomizationHazardousConditions S ON C.HazardousConditionInternalId = S.InternalId
WHERE
  People.InternalId = 1

/* Get Person Work Activities by Person InternalId */
SELECT
  People.InternalId,
  FirstName,
  LastName,
  CanonicalName,
  DisplayName
FROM
  People
INNER JOIN
  PersonWorkActivity C ON People.InternalId = C.PersonInternalId
INNER JOIN
  StatusCustomizationWorkActivities S ON C.WorkActivityInternalId = S.InternalId
WHERE
  People.InternalId = 1

/* Get Person Household Tasks by InternalId */
SELECT
  People.InternalId,
  FirstName,
  LastName,
  CanonicalName,
  DisplayName
FROM
  People
INNER JOIN
  PersonHouseholdTask C ON People.InternalId = C.PersonInternalId
INNER JOIN
  StatusCustomizationHouseholdTasks S ON C.HouseholdTaskInternalId = S.InternalId
WHERE
  People.InternalId = 1
