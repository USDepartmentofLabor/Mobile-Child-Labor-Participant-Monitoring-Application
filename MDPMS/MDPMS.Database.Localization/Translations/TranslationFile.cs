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
                GetTranslationValues_es_Español(),
                GetTranslationValues_fr_Français()
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
            translation.Translations.Add(@"Household", @"Household");
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
            translation.Translations.Add(@"WorkActivitiesHoursEngagedQuestion", @"During the past week, for how many hours did you engage in this/these activities?");
            translation.Translations.Add(@"HazardousConditionsQuestion", @"Were you exposed to any of the following at work?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"During the past week, did you do any of the tasks below for this household?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"During each day of the past week, for how many hours did you engage in this/these activities?");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"Are you enrolled in school and/or college?");
            translation.Translations.Add(@"DPMSURL", @"DPMS URL");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"Name can not be blank");
            translation.Translations.Add(@"ErrorHouseholdNameCanNotBeBlank", @"Household name can not be blank");
            translation.Translations.Add(@"ErrorIncomeSourceNameCanNotBeBlank", @"Income source name can not be blank");
            translation.Translations.Add(@"ErrorLastNameCanNotBeBlank", @"Last name can not be blank");
            translation.Translations.Add(@"ErrorFirstNameCanNotBeBlank", @"First name can not be blank");
            translation.Translations.Add(@"ErrorGenderMustBeSelected", @"A gender must be selected");
            translation.Translations.Add(@"ErrorSyncError", @"Sync Error");
            translation.Translations.Add(@"SyncSuccessful", @"Sync Successful");
            translation.Translations.Add(@"Members", @"Members");
            translation.Translations.Add(@"HouseholdMemberId", @"Household Member ID");
            translation.Translations.Add(@"Age", @"Age");
            translation.Translations.Add(@"AssignService", @"Assign Service");
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
            translation.Translations.Add(@"AlertMessageActionNotAllowedUntilInitialSyncIsPerformed", @"Action not allowed until initial sync is performed");
            translation.Translations.Add(@"AlertMessageSyncCancelledNoInternetConnectivity", @"Sync cancelled there is no internet connectivity");
            translation.Translations.Add(@"FollowUp", @"Follow Up");
            translation.Translations.Add(@"HouseholdMembers",@"Household Members");
            translation.Translations.Add(@"FollowUpDate", @"Follow Up Date");
            translation.Translations.Add(@"AlertMessageActionNotAllowedNoServices", @"Action not allowed, no services exist");
            translation.Translations.Add(@"Version", @"Version");
            translation.Translations.Add(@"APK", @"APK");
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
            translation.Translations.Add(@"Household", @"Casa");
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
            translation.Translations.Add(@"HazardousConditionsQuestion", @"¿Estuviste expuesto a alguno de los siguientes en el trabajo?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"Durante la última semana, ¿realizó alguna de las siguientes tareas para este hogar?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"Durante cada día de la semana pasada, ¿durante cuántas horas se involucró en esta / estas actividades?");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"¿Estás inscrito en la escuela y / o la universidad?");
            translation.Translations.Add(@"DPMSURL", @"DPMS URL");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"El nombre no puede estar en blanco");
            translation.Translations.Add(@"ErrorHouseholdNameCanNotBeBlank", @"El nombre del hogar no puede estar en blanco");
            translation.Translations.Add(@"ErrorIncomeSourceNameCanNotBeBlank", @"El nombre de la fuente de ingresos no puede estar en blanco");
            translation.Translations.Add(@"ErrorLastNameCanNotBeBlank", @"El apellido no puede estar en blanco");
            translation.Translations.Add(@"ErrorFirstNameCanNotBeBlank", @"El primer nombre no puede estar en blanco");
            translation.Translations.Add(@"ErrorGenderMustBeSelected", @"Se debe seleccionar un género");
            translation.Translations.Add(@"ErrorSyncError", @"Error de sincronización");
            translation.Translations.Add(@"SyncSuccessful", @"Sincronización exitosa");
            translation.Translations.Add(@"Members", @"Miembros");
            translation.Translations.Add(@"HouseholdMemberId", @"Identificación del miembro del hogar");
            translation.Translations.Add(@"Age", @"Años");
            translation.Translations.Add(@"AssignService", @"Asignar servicio");
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
            translation.Translations.Add(@"AlertMessageActionNotAllowedUntilInitialSyncIsPerformed", @"Acción no permitida hasta que se realice la sincronización inicial");
            translation.Translations.Add(@"AlertMessageSyncCancelledNoInternetConnectivity", @"Sincronización cancelada no hay conectividad a Internet");
            translation.Translations.Add(@"FollowUp", @"Seguir");
            translation.Translations.Add(@"HouseholdMembers", @"Los miembros del hogar");
            translation.Translations.Add(@"FollowUpDate", @"Fecha de seguimiento");
            translation.Translations.Add(@"AlertMessageActionNotAllowedNoServices", @"Acción no permitida, no hay servicios");
            translation.Translations.Add(@"Version", @"Versión");
            translation.Translations.Add(@"APK", @"APK");
            return translation;
        }

        public static Translation GetTranslationValues_fr_Français()
        {
            var translation = new Translation
            {
                Abbreviation = @"fr",
                Name = @"Français",
                Translations = new Dictionary<string, string>()
            };
            translation.Translations.Add(@"ProjectName", @"Système de surveillance directe des participants");
            translation.Translations.Add(@"Mobile", @"Mobile");
            translation.Translations.Add(@"ProjectPhrase", @"Plus nous nous connectons, mieux c'est.");
            translation.Translations.Add(@"Continue", @"Continuer");
            translation.Translations.Add(@"SelectLocalization", @"Sélectionner la localisation");
            translation.Translations.Add(@"Home", @"Accueil");
            translation.Translations.Add(@"Household", @"Ménage");
            translation.Translations.Add(@"Households", @"Ménages");
            translation.Translations.Add(@"Settings", @"Paramètres");
            translation.Translations.Add(@"About", @"Sur");
            translation.Translations.Add(@"ApiKey", @"clé API");
            translation.Translations.Add(@"Sync", @"Synchronisation");
            translation.Translations.Add(@"Syncing", @"Synchronisation");
            translation.Translations.Add(@"Obtained", @"Obtenu");
            translation.Translations.Add(@"Username", @"Nom d'utilisateur");
            translation.Translations.Add(@"Password", @"Mot de passe");
            translation.Translations.Add(@"True", @"Vrai");
            translation.Translations.Add(@"False", @"Faux");
            translation.Translations.Add(@"Yes", @"Oui");
            translation.Translations.Add(@"No", @"Non");
            translation.Translations.Add(@"Error", @"Erreur");
            translation.Translations.Add(@"GetNewApiKey", @"Obtenir la nouvelle clé d'API");
            translation.Translations.Add(@"ShowPassword", @"Montrer le mot de passe");
            translation.Translations.Add(@"Authenticate", @"Authentifier");
            translation.Translations.Add(@"OK", @"D'accord");
            translation.Translations.Add(@"Cancel", @"Annuler");
            translation.Translations.Add(@"Alert", @"Alerte");
            translation.Translations.Add(@"ConnectivityProblem", @"Problème de connectivité");
            translation.Translations.Add(@"AddNewHousehold", @"Ajouter un nouveau ménage");
            translation.Translations.Add(@"Submit", @"Soumettre");
            translation.Translations.Add(@"AddIncomeSource", @"Ajouter une source de revenu");
            translation.Translations.Add(@"AddHouseholdMember", @"Ajouter un membre du ménage");
            translation.Translations.Add(@"IntakeDate", @"Date d'admission");
            translation.Translations.Add(@"HouseholdName", @"Nom du ménage");
            translation.Translations.Add(@"AddressLine1", @"Adresse 1");
            translation.Translations.Add(@"AddressLine2", @"Adresse Ligne 2");
            translation.Translations.Add(@"ZipCode", @"Code postal");
            translation.Translations.Add(@"SuburbNeighborhood", @"Banlieue / Quartier");
            translation.Translations.Add(@"City", @"Ville");
            translation.Translations.Add(@"State", @"Etat");
            translation.Translations.Add(@"County", @"Comté");
            translation.Translations.Add(@"Country", @"Pays");
            translation.Translations.Add(@"AddressInfo", @"Infos sur l'adresse");
            translation.Translations.Add(@"NameOfProductOrService", @"Nom du produit ou du service");
            translation.Translations.Add(@"EstimatedVolumeProduced", @"Volume estimé produit");
            translation.Translations.Add(@"EstimatedVolumeSold", @"Volume estimé vendu");
            translation.Translations.Add(@"UnitOfMeasure", @"Unité de mesure");
            translation.Translations.Add(@"EstimatedIncome", @"Revenu estimé");
            translation.Translations.Add(@"Currency", @"Devise");
            translation.Translations.Add(@"FirstNameGivenName", @"Prénom (Prénom)");
            translation.Translations.Add(@"LastNameFamilyName", @"Nom de famille (nom de famille)");
            translation.Translations.Add(@"MiddleName", @"Deuxième nom");
            translation.Translations.Add(@"Gender", @"Le genre");
            translation.Translations.Add(@"DateOfBirth", @"Date de naissance");
            translation.Translations.Add(@"IsTheBirthdayAnApproximateDate", @"L'anniversaire est-il une date approximative?");
            translation.Translations.Add(@"Relationship", @"Relation");
            translation.Translations.Add(@"RelationshipOther", @"Relation autre");
            translation.Translations.Add(@"WorkActivitiesQuestion", @"Au cours de la dernière semaine, avez-vous fait l'une des activités suivantes, même pour une heure seulement?");
            translation.Translations.Add(@"WorkActivitiesReturningQuestion", @"Même si vous n'avez fait aucune de ces activités au cours de la dernière semaine, avez-vous un emploi, une entreprise ou une autre activité économique ou agricole à laquelle vous retournerez certainement?");
            translation.Translations.Add(@"WorkActivitiesHoursEngagedQuestion", @"Au cours de la dernière semaine, pendant combien d'heures avez-vous participé à ces activités?");
            translation.Translations.Add(@"HazardousConditionsQuestion", @"Étiez-vous exposé à l'un des éléments suivants au travail?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"Au cours de la dernière semaine, avez-vous effectué les tâches ci-dessous pour ce ménage?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"Au cours de chaque jour de la semaine dernière, pendant combien d'heures avez-vous participé à ces activités?");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"Êtes-vous inscrit à l'école et / ou au collège?");
            translation.Translations.Add(@"DPMSURL", @"URL DPMS");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"Le nom ne peut pas être vide");
            translation.Translations.Add(@"ErrorHouseholdNameCanNotBeBlank", @"Le nom du ménage ne peut pas être vide");
            translation.Translations.Add(@"ErrorIncomeSourceNameCanNotBeBlank", @"Le nom de la source de revenu ne peut pas être vide");
            translation.Translations.Add(@"ErrorLastNameCanNotBeBlank", @"Le nom ne peut pas être vide");
            translation.Translations.Add(@"ErrorFirstNameCanNotBeBlank", @"Le prénom ne peut pas être vide");
            translation.Translations.Add(@"ErrorGenderMustBeSelected", @"Un genre doit être sélectionné");
            translation.Translations.Add(@"ErrorSyncError", @"Erreur de synchronisation");
            translation.Translations.Add(@"SyncSuccessful", @"Synchroniser avec succès");
            translation.Translations.Add(@"Members", @"Membres");
            translation.Translations.Add(@"HouseholdMemberId", @"ID du membre du ménage");
            translation.Translations.Add(@"Age", @"Âge");
            translation.Translations.Add(@"AssignService", @"Affecter un service");
            translation.Translations.Add(@"Service", @"Un service");
            translation.Translations.Add(@"StartDate", @"Date de début");
            translation.Translations.Add(@"EndDate", @"Date de fin");
            translation.Translations.Add(@"Hours", @"Heures");
            translation.Translations.Add(@"Notes", @"Remarques");
            translation.Translations.Add(@"SelectService", @"Sélectionnez le service");
            translation.Translations.Add(@"Validation", @"Validation");
            translation.Translations.Add(@"ValidationErrorMessageNoServiceSelected", @"Veuillez sélectionner un service");
            translation.Translations.Add(@"ValidationErrorMessageStartDateIsAfterEndDate", @"La date de début est après la date de fin");
            translation.Translations.Add(@"ValidationErrorMessageHoursInvalid", @"La valeur des heures est invalide");
            translation.Translations.Add(@"AlertMessageActionNotAllowedUntilInitialSyncIsPerformed", @"Action non autorisée jusqu'à la synchronisation initiale");
            translation.Translations.Add(@"AlertMessageSyncCancelledNoInternetConnectivity", @"Synchronisation annulée il n'y a pas de connectivité Internet");
            translation.Translations.Add(@"FollowUp", @"Suivre");
            translation.Translations.Add(@"HouseholdMembers", @"Membres du foyer");
            translation.Translations.Add(@"FollowUpDate", @"Date de suivi");
            translation.Translations.Add(@"AlertMessageActionNotAllowedNoServices", @"Action non autorisée, aucun service n'existe");
            translation.Translations.Add(@"Version", @"Version");
            translation.Translations.Add(@"APK", @"APK");
            return translation;
        }
    }     
}
