using System.Collections.Generic;

namespace MDPMS.Database.Localization.Translations
{
    public class Translation
    {
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Translations { get; set; }
    }

    public static class TranslationFile
    {
        public static List<Translation> GetAllSupportedTranslations()
        {
            return new List<Translation>
            {
                GetTranslationValues_en_English(),
                GetTranslationValues_es_Español()
            };
        }
        
        public static Translation GetTranslationValues_en_English()
        {
            var translation = new Translation
            {
                Abbreviation = @"en",
                Name = @"English",
                Translations = new Dictionary<string, string>()
            };            
            translation.Translations.Add(@"ProjectName", @"Direct Participant Monitoring System");
            translation.Translations.Add(@"Mobile", @"Mobile");
            translation.Translations.Add(@"ProjectPhrase", @"The more we connect, the better it gets.");           
            translation.Translations.Add(@"Continue", @"Continue");
            translation.Translations.Add(@"SelectLocalization", @"Select Localization");
            translation.Translations.Add(@"Home", @"Home");
            translation.Translations.Add(@"Households", @"Households");
            translation.Translations.Add(@"Settings", @"Settings");
            translation.Translations.Add(@"About", @"About");
            translation.Translations.Add(@"ApiKey", @"API Key");
            translation.Translations.Add(@"Sync", @"Sync");
            translation.Translations.Add(@"Syncing", @"Syncing");
            translation.Translations.Add(@"Obtained", @"Obtained");
            translation.Translations.Add(@"Username", @"Username");
            translation.Translations.Add(@"Password", @"Password");
            translation.Translations.Add(@"True", @"True");
            translation.Translations.Add(@"False", @"False");
            translation.Translations.Add(@"Yes", @"Yes");
            translation.Translations.Add(@"No", @"No");
            translation.Translations.Add(@"Error", @"Error");
            translation.Translations.Add(@"GetNewApiKey", @"Get New API Key");
            translation.Translations.Add(@"ShowPassword", @"Show Password");
            translation.Translations.Add(@"Authenticate", @"Authenticate");
            translation.Translations.Add(@"OK", @"OK");
            translation.Translations.Add(@"Cancel", @"Cancel");
            translation.Translations.Add(@"Alert", @"Alert");
            translation.Translations.Add(@"ConnectivityProblem", @"Connectivity Problem");
            translation.Translations.Add(@"AddNewHousehold", @"Add New Household");
            translation.Translations.Add(@"Submit", @"Submit");
            translation.Translations.Add(@"AddIncomeSource", @"Add Income Source");
            translation.Translations.Add(@"AddHouseholdMember", @"Add Household Member");
            translation.Translations.Add(@"IntakeDate", @"Intake Date");
            translation.Translations.Add(@"HouseholdName", @"Household Name");
            translation.Translations.Add(@"AddressLine1", @"Address Line 1");
            translation.Translations.Add(@"AddressLine2", @"Address Line 2");
            translation.Translations.Add(@"ZipCode", @"Zip Code");
            translation.Translations.Add(@"SuburbNeighborhood", @"Suburb/Neighborhood");
            translation.Translations.Add(@"City", @"City");
            translation.Translations.Add(@"State", @"State");
            translation.Translations.Add(@"County", @"County");
            translation.Translations.Add(@"Country", @"Country");
            translation.Translations.Add(@"AddressInfo", @"Address Info");
            translation.Translations.Add(@"NameOfProductOrService", @"Name of Product or Service");
            translation.Translations.Add(@"EstimatedVolumeProduced", @"Estimated Volume Produced");
            translation.Translations.Add(@"EstimatedVolumeSold", @"Estimated Volume Sold");
            translation.Translations.Add(@"UnitOfMeasure", @"Unit of Measure");
            translation.Translations.Add(@"EstimatedIncome", @"Estimated Income");
            translation.Translations.Add(@"Currency", @"Currency");
            translation.Translations.Add(@"FirstNameGivenName", @"First Name (Given Name)");
            translation.Translations.Add(@"LastNameFamilyName", @"Last Name (Family Name)");
            translation.Translations.Add(@"MiddleName", @"Middle Name");
            translation.Translations.Add(@"Gender", @"Gender");
            translation.Translations.Add(@"DateOfBirth", @"Date of Birth");
            translation.Translations.Add(@"IsTheBirthdayAnApproximateDate", @"Is the birthday an approximate date?");
            translation.Translations.Add(@"Relationship", @"Relationship");
            translation.Translations.Add(@"RelationshipOther", @"Relationship other");
            translation.Translations.Add(@"WorkActivitiesQuestion", @"During the past week, did you do any of the following activities, even for only one hour?");
            translation.Translations.Add(@"WorkActivitiesReturningQuestion", @"Even though you did not do any of these activities in the past week, do you have a job, business, or other economic or farming activity that you will definitely be returning to?");
            translation.Translations.Add(@"WorkActivitiesHoursEngagedQuestion", @"During the past week, for how many hours did you engage in this/these activities? ");
            translation.Translations.Add(@"HazardousConditionsQuestion", @"Where you exposed to any of the following at work?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"During the past week, did you do any of the tasks below for this household?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"During each day of the past week, for how many hours did you engage in this/these activities? ");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"Are you enrolled in school and/or college?");
            translation.Translations.Add(@"DPMSURL", @"DPMS URL");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"Name can not be blank");
            translation.Translations.Add(@"ErrorSyncError", @"Sync Error");
            translation.Translations.Add(@"SyncSuccessful", @"Sync Successful");
            translation.Translations.Add(@"Members", @"Members");
            translation.Translations.Add(@"HouseholdMemberId", @"Household Member ID");
            translation.Translations.Add(@"Age", @"Age");
            translation.Translations.Add(@"AddService", @"Add Service");
            translation.Translations.Add(@"Service", @"Service");
            translation.Translations.Add(@"StartDate", @"Start Date");
            translation.Translations.Add(@"EndDate", @"End Date");
            translation.Translations.Add(@"Hours", @"Hours");
            translation.Translations.Add(@"Notes", @"Notes");
            translation.Translations.Add(@"SelectService", @"Select Service");
            translation.Translations.Add(@"Validation", @"Validation");
            translation.Translations.Add(@"ValidationErrorMessageNoServiceSelected", @"Please select a service");
            translation.Translations.Add(@"ValidationErrorMessageStartDateIsAfterEndDate",@"Start date is after end date");
            translation.Translations.Add(@"ValidationErrorMessageHoursInvalid", @"Hours value is invalid");
            return translation;
        }

        public static Translation GetTranslationValues_es_Español()
        {
            var translation = new Translation
            {
                Abbreviation = @"es",
                Name = @"Español",
                Translations = new Dictionary<string, string>()
            };
            translation.Translations.Add(@"ProjectName", @"Sistema de monitoreo directo de participantes");
            translation.Translations.Add(@"Mobile", @"Móvil");
            translation.Translations.Add(@"ProjectPhrase", @"Cuanto más nos conectamos, mejor se pone.");
            translation.Translations.Add(@"Continue", @"Continuar");
            translation.Translations.Add(@"SelectLocalization", @"Seleccionar localización");
            translation.Translations.Add(@"Home", @"Casa");
            translation.Translations.Add(@"Households", @"Hogares");
            translation.Translations.Add(@"Settings", @"Configuraciones");
            translation.Translations.Add(@"About", @"Acerca de");
            translation.Translations.Add(@"ApiKey", @"Clave API");
            translation.Translations.Add(@"Sync", @"Sincronización");
            translation.Translations.Add(@"Syncing", @"Sincronizando");
            translation.Translations.Add(@"Obtained", @"Adquirido");
            translation.Translations.Add(@"Username", @"Nombre de usuario");
            translation.Translations.Add(@"Password", @"Contraseña");
            translation.Translations.Add(@"True", @"Cierto");
            translation.Translations.Add(@"False", @"Falso");
            translation.Translations.Add(@"Yes", @"Sí");
            translation.Translations.Add(@"No", @"No");
            translation.Translations.Add(@"Error", @"Error");
            translation.Translations.Add(@"GetNewApiKey", @"Obtener una nueva clave de API");
            translation.Translations.Add(@"ShowPassword", @"Mostrar contraseña");
            translation.Translations.Add(@"Authenticate", @"Autenticar");
            translation.Translations.Add(@"OK", @"De acuerdo");
            translation.Translations.Add(@"Cancel", @"Cancelar");
            translation.Translations.Add(@"Alert", @"Alerta");
            translation.Translations.Add(@"ConnectivityProblem", @"Problema de conectividad");
            translation.Translations.Add(@"AddNewHousehold", @"Agregar nuevo hogar");
            translation.Translations.Add(@"Submit", @"Enviar");
            translation.Translations.Add(@"AddIncomeSource", @"Agregar fuente de ingresos");
            translation.Translations.Add(@"AddHouseholdMember", @"Añadir miembro del hogar");
            translation.Translations.Add(@"IntakeDate", @"Fecha de admisión");
            translation.Translations.Add(@"HouseholdName", @"Nombre del hogar");
            translation.Translations.Add(@"AddressLine1", @"Dirección Línea 1");
            translation.Translations.Add(@"AddressLine2", @"Dirección Línea 2");
            translation.Translations.Add(@"ZipCode", @"Código postal");
            translation.Translations.Add(@"SuburbNeighborhood", @"Barrio");
            translation.Translations.Add(@"City", @"Ciudad");
            translation.Translations.Add(@"State", @"Estado");
            translation.Translations.Add(@"County", @"Condado");
            translation.Translations.Add(@"Country", @"País");
            translation.Translations.Add(@"AddressInfo", @"Información de dirección");
            translation.Translations.Add(@"NameOfProductOrService", @"Nombre del producto o servicio");
            translation.Translations.Add(@"EstimatedVolumeProduced", @"Volumen Estimado Producido");
            translation.Translations.Add(@"EstimatedVolumeSold", @"Volumen estimado vendido");
            translation.Translations.Add(@"UnitOfMeasure", @"Unidad de medida");
            translation.Translations.Add(@"EstimatedIncome", @"Ingresos estimados");
            translation.Translations.Add(@"Currency", @"Moneda");
            translation.Translations.Add(@"FirstNameGivenName", @"Nombre de pila");
            translation.Translations.Add(@"LastNameFamilyName", @"Apellido");
            translation.Translations.Add(@"MiddleName", @"Segundo nombre");
            translation.Translations.Add(@"Gender", @"Género");
            translation.Translations.Add(@"DateOfBirth", @"Fecha de nacimiento");
            translation.Translations.Add(@"IsTheBirthdayAnApproximateDate", @"¿Es el cumpleaños una fecha aproximada?");
            translation.Translations.Add(@"Relationship", @"Relación");
            translation.Translations.Add(@"RelationshipOther", @"Relación otra");
            translation.Translations.Add(@"WorkActivitiesQuestion", @"Durante la última semana, ¿realizó alguna de las siguientes actividades, incluso durante solo una hora?");
            translation.Translations.Add(@"WorkActivitiesReturningQuestion", @"Aunque no realizó ninguna de estas actividades la semana pasada, ¿tiene un trabajo, negocio u otra actividad económica o agrícola a la que definitivamente volverá?");
            translation.Translations.Add(@"WorkActivitiesHoursEngagedQuestion", @"Durante la última semana, ¿durante cuántas horas se involucró en estas / estas actividades?");
            translation.Translations.Add(@"HazardousConditionsQuestion", @"¿Dónde estuvo expuesto a cualquiera de los siguientes en el trabajo?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"Durante la última semana, ¿realizó alguna de las siguientes tareas para este hogar?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"Durante cada día de la semana pasada, ¿durante cuántas horas se involucró en esta / estas actividades?");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"¿Estás inscrito en la escuela y / o la universidad?");
            translation.Translations.Add(@"DPMSURL", @"DPMS URL");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"El nombre no puede estar en blanco");
            translation.Translations.Add(@"ErrorSyncError", @"Error de sincronización");
            translation.Translations.Add(@"SyncSuccessful", @"Sincronización exitosa");
            translation.Translations.Add(@"Members", @"Miembros");
            translation.Translations.Add(@"HouseholdMemberId", @"Identificación del miembro del hogar");
            translation.Translations.Add(@"Age", @"Años");
            translation.Translations.Add(@"AddService", @"Añadir servicio");
            translation.Translations.Add(@"Service", @"Servicio");
            translation.Translations.Add(@"StartDate", @"Fecha de inicio");
            translation.Translations.Add(@"EndDate", @"Fecha final");
            translation.Translations.Add(@"Hours", @"Horas");
            translation.Translations.Add(@"Notes", @"Notas");
            translation.Translations.Add(@"SelectService", @"Seleccionar servicio");
            translation.Translations.Add(@"Validation", @"Validación");
            translation.Translations.Add(@"ValidationErrorMessageNoServiceSelected", @"Por favor seleccione un servicio");
            translation.Translations.Add(@"ValidationErrorMessageStartDateIsAfterEndDate", @"La fecha de inicio es posterior a la fecha de finalización");
            translation.Translations.Add(@"ValidationErrorMessageHoursInvalid", @"El valor de las horas no es válido");
            return translation;
        }
    }     
}
