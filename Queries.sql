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

/* Search Localization DB for existing keys and show en and us translation values */
SELECT
  Keys.KeyName,
  EN.KeyLocalizationValue AS EN,
  ES.KeyLocalizationValue AS ES,
  FR.KeyLocalizationValue AS FR,
  PT.KeyLocalizationValue AS PT
FROM
  Keys
LEFT JOIN
  "Values" AS EN ON Keys.Id = EN.KeyId
  AND
  EN.LocalizationId = (SELECT Localizations.Id FROM Localizations WHERE Localizations.Code = 'en')
LEFT JOIN
  "Values" AS ES ON Keys.Id = ES.KeyId
  AND
  ES.LocalizationId = (SELECT Localizations.Id FROM Localizations WHERE Localizations.Code = 'es')
LEFT JOIN
  "Values" AS FR ON Keys.Id = FR.KeyId
  AND
  FR.LocalizationId = (SELECT Localizations.Id FROM Localizations WHERE Localizations.Code = 'fr')
LEFT JOIN
  "Values" AS PT ON Keys.Id = PT.KeyId
  AND
  PT.LocalizationId = (SELECT Localizations.Id FROM Localizations WHERE Localizations.Code = 'pt')
WHERE
  Keys.KeyName LIKE '%%';
