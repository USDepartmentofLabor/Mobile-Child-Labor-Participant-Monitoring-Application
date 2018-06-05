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
                GetTranslationValues_fr_Français(),
                GetTranslationValues_pt_Português()
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
            translation.Translations.Add(@"Participant", @"Participant");
            translation.Translations.Add(@"Participants", @"Participants");
            translation.Translations.Add(@"Male", @"Male");
            translation.Translations.Add(@"Female", @"Female");
            translation.Translations.Add(@"SelectGender", @"Select Gender");
            translation.Translations.Add(@"SelectRelationship", @"Select Relationship");
            translation.Translations.Add(@"OTHER", @"OTHER");
            translation.Translations.Add(@"AddAnswer", @"Add Answer");
            translation.Translations.Add(@"RemoveAnswer", @"Remove Answer");
            translation.Translations.Add(@"GridHeaderTitleID", @"ID");
            translation.Translations.Add(@"GridHeaderTitleBio", @"Bio");
            translation.Translations.Add(@"Children", @"Children");
            translation.Translations.Add(@"Adult", @"Adult");
            translation.Translations.Add(@"Confirm", @"Confirm");
            translation.Translations.Add(@"ConfirmationMessageDeleteHousehold", @"Are you sure you want to delete this household?  All associated income sources, household members, follow ups, and service assignments will also be removed.");
            translation.Translations.Add(@"Edit", @"Edit");
            translation.Translations.Add(@"Delete", @"Delete");
            translation.Translations.Add(@"IncomeSources",@"Income Sources");
            translation.Translations.Add(@"ConfirmationMessageDeleteIncomeSource", @"Are you sure you want to delete this income source?");
            translation.Translations.Add(@"Save", @"Save");
            translation.Translations.Add(@"ConfirmationMessageDeleteHouseholdMember", @"Are you sure you want to delete this household member?  All associated follow ups and service assignments will also be removed.");
            translation.Translations.Add(@"FollowUps", @"Follow Ups");
            translation.Translations.Add(@"AddFollowUp", @"Add Follow Up");
            translation.Translations.Add(@"ConfirmationMessageDeletePersonFollowUp", @"Are you sure you want to delete this follow up?");
            translation.Translations.Add(@"ServiceAssignments", @"Service Assignments");
            translation.Translations.Add(@"ConfirmationMessageDeleteServiceAssignment", @"Are you sure you want to delete this service assignment?");
            translation.Translations.Add(@"EditHousehold", @"Edit Household");
            translation.Translations.Add(@"IncomeSource", @"Income Source");
            translation.Translations.Add(@"EditIncomeSource", @"Edit Income Source");
            translation.Translations.Add(@"HouseholdMember", @"Household Member");
            translation.Translations.Add(@"EditHouseholdMember", @"Edit Household Member");
            translation.Translations.Add(@"EditFollowUp", @"Edit Follow Up");
            translation.Translations.Add(@"ServiceAssignment", @"Service Assignment");
            translation.Translations.Add(@"EditServiceAssignment", @"Edit Service Assignment");
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
            translation.Translations.Add(@"Participant", @"Partícipe");
            translation.Translations.Add(@"Participants", @"Participantes");
            translation.Translations.Add(@"Male", @"Masculino");
            translation.Translations.Add(@"Female", @"Hembra");
            translation.Translations.Add(@"SelectGender", @"Seleccione género");
            translation.Translations.Add(@"SelectRelationship", @"Seleccionar relación");
            translation.Translations.Add(@"OTHER", @"OTRO");
            translation.Translations.Add(@"AddAnswer", @"Agregar respuesta");
            translation.Translations.Add(@"RemoveAnswer", @"Eliminar respuesta");
            translation.Translations.Add(@"GridHeaderTitleID", @"ID");
            translation.Translations.Add(@"GridHeaderTitleBio", @"Bio");
            translation.Translations.Add(@"Children", @"Niños");
            translation.Translations.Add(@"Adult", @"Adulto");
            translation.Translations.Add(@"Confirm", @"Confirm");
            translation.Translations.Add(@"ConfirmationMessageDeleteHousehold", @"¿Seguro que quieres eliminar esta casa? También se eliminarán todas las fuentes de ingresos asociadas, los miembros del hogar, los seguimientos y las asignaciones de servicios.");
            translation.Translations.Add(@"Edit", @"Editar");
            translation.Translations.Add(@"Delete", @"Borrar");
            translation.Translations.Add(@"IncomeSources", @"Fuentes de ingresos");
            translation.Translations.Add(@"ConfirmationMessageDeleteIncomeSource", @"¿Seguro que quieres eliminar esta fuente de ingresos?");
            translation.Translations.Add(@"Save", @"Guardar");
            translation.Translations.Add(@"ConfirmationMessageDeleteHouseholdMember", @"¿Confirma que desea eliminar este miembro del hogar?  También se eliminarán todos los seguimientos y asignaciones de servicios asociados.");
            translation.Translations.Add(@"FollowUps", @"Seguimiento de");
            translation.Translations.Add(@"AddFollowUp", @"Agregar seguimiento");
            translation.Translations.Add(@"ConfirmationMessageDeletePersonFollowUp", @"¿Estás seguro de que deseas eliminar este seguimiento?");
            translation.Translations.Add(@"ServiceAssignments", @"Asignaciones de servicio");
            translation.Translations.Add(@"ConfirmationMessageDeleteServiceAssignment", @"¿Seguro que quieres eliminar esta asignación de servicio?");
            translation.Translations.Add(@"EditHousehold", @"Editar hogar");
            translation.Translations.Add(@"IncomeSource", @"Fuente de ingresos");
            translation.Translations.Add(@"EditIncomeSource", @"Editar fuente de ingresos");
            translation.Translations.Add(@"HouseholdMember", @"Miembro del hogar");
            translation.Translations.Add(@"EditHouseholdMember", @"Editar miembro del hogar");
            translation.Translations.Add(@"EditFollowUp", @"Editar seguimiento");
            translation.Translations.Add(@"ServiceAssignment", @"Asignación de servicio");
            translation.Translations.Add(@"EditServiceAssignment", @"Editar tarea de servicio");
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
            translation.Translations.Add(@"Participant", @"Participant");
            translation.Translations.Add(@"Participants", @"Participants");
            translation.Translations.Add(@"Male", @"Mâle");
            translation.Translations.Add(@"Female", @"Femelle");
            translation.Translations.Add(@"SelectGender", @"Sélectionnez le sexe");
            translation.Translations.Add(@"SelectRelationship", @"Sélectionner une relation");
            translation.Translations.Add(@"OTHER", @"AUTRE");
            translation.Translations.Add(@"AddAnswer", @"Ajouter une réponse");
            translation.Translations.Add(@"RemoveAnswer", @"Supprimer la réponse");
            translation.Translations.Add(@"GridHeaderTitleID", @"ID");
            translation.Translations.Add(@"GridHeaderTitleBio", @"Bio");
            translation.Translations.Add(@"Children", @"Enfants");
            translation.Translations.Add(@"Adult", @"Adulte");
            translation.Translations.Add(@"Confirm", @"Confirm");
            translation.Translations.Add(@"ConfirmationMessageDeleteHousehold", @"Êtes-vous sûr de vouloir supprimer ce ménage? Toutes les sources de revenu associées, les membres du ménage, les suivis et les affectations de service seront également supprimés.");
            translation.Translations.Add(@"Edit", @"Modifier");
            translation.Translations.Add(@"Delete", @"Effacer");
            translation.Translations.Add(@"IncomeSources", @"Sources de revenus");
            translation.Translations.Add(@"ConfirmationMessageDeleteIncomeSource", @"Êtes-vous sûr de vouloir supprimer cette source de revenus?");
            translation.Translations.Add(@"Save", @"Enregistrer");
            translation.Translations.Add(@"ConfirmationMessageDeleteHouseholdMember", @"Êtes-vous sûr de vouloir supprimer ce membre du ménage?  Tous les suivis associés et les affectations de service seront également supprimés.");
            translation.Translations.Add(@"FollowUps", @"Suivis");
            translation.Translations.Add(@"AddFollowUp", @"Ajouter un suivi");
            translation.Translations.Add(@"ConfirmationMessageDeletePersonFollowUp", @"Êtes-vous sûr de vouloir supprimer ce suivi?");
            translation.Translations.Add(@"ServiceAssignments", @"Affectations de service");
            translation.Translations.Add(@"ConfirmationMessageDeleteServiceAssignment", @"Êtes-vous sûr de vouloir supprimer cette attribution de service?");
            translation.Translations.Add(@"EditHousehold", @"Modifier le ménage");
            translation.Translations.Add(@"IncomeSource", @"Source de revenu");
            translation.Translations.Add(@"EditIncomeSource", @"Modifier la source de revenu");
            translation.Translations.Add(@"HouseholdMember", @"Membre du ménage");
            translation.Translations.Add(@"EditHouseholdMember", @"Modifier le membre du ménage");
            translation.Translations.Add(@"EditFollowUp", @"Modifier le suivi");
            translation.Translations.Add(@"ServiceAssignment", @"Affectation de service");
            translation.Translations.Add(@"EditServiceAssignment", @"Modifier l'attribution de service");
            return translation;
        }

        public static Translation GetTranslationValues_pt_Português()
        {
            var translation = new Translation
            {
                Abbreviation = @"pt",
                Name = @"Português",
                Translations = new Dictionary<string, string>()
            };
            translation.Translations.Add(@"ProjectName", @"Sistema de monitoramento direto do participante");
            translation.Translations.Add(@"Mobile", @"Móvel");
            translation.Translations.Add(@"ProjectPhrase", @"Quanto mais nos conectarmos, melhor será.");
            translation.Translations.Add(@"Continue", @"Continuar");
            translation.Translations.Add(@"SelectLocalization", @"Selecionar localização");
            translation.Translations.Add(@"Home", @"Casa");
            translation.Translations.Add(@"Household", @"Casa");
            translation.Translations.Add(@"Households", @"Famílias");
            translation.Translations.Add(@"Settings", @"Configurações");
            translation.Translations.Add(@"About", @"Sobre");
            translation.Translations.Add(@"ApiKey", @"Chave API");
            translation.Translations.Add(@"Sync", @"Sincronizar");
            translation.Translations.Add(@"Syncing", @"Sincronização");
            translation.Translations.Add(@"Obtained", @"Obtido");
            translation.Translations.Add(@"Username", @"Nome de usuário");
            translation.Translations.Add(@"Password", @"Senha");
            translation.Translations.Add(@"True", @"Verdade");
            translation.Translations.Add(@"False", @"Falso");
            translation.Translations.Add(@"Yes", @"Sim");
            translation.Translations.Add(@"No", @"Não");
            translation.Translations.Add(@"Error", @"Erro");
            translation.Translations.Add(@"GetNewApiKey", @"Obter nova chave da API");
            translation.Translations.Add(@"ShowPassword", @"Mostrar Senha");
            translation.Translations.Add(@"Authenticate", @"Autenticar");
            translation.Translations.Add(@"OK", @"Está bem");
            translation.Translations.Add(@"Cancel", @"Cancelar");
            translation.Translations.Add(@"Alert", @"Alerta");
            translation.Translations.Add(@"ConnectivityProblem", @"Problema de conectividade");
            translation.Translations.Add(@"AddNewHousehold", @"Adicionar nova casa");
            translation.Translations.Add(@"Submit", @"Enviar");
            translation.Translations.Add(@"AddIncomeSource", @"Adicionar fonte de rendimento");
            translation.Translations.Add(@"AddHouseholdMember", @"Adicionar membro do agregado familiar");
            translation.Translations.Add(@"IntakeDate", @"Data de entrada");
            translation.Translations.Add(@"HouseholdName", @"Nome familiar");
            translation.Translations.Add(@"AddressLine1", @"Endereço Linha 1");
            translation.Translations.Add(@"AddressLine2", @"Endereço Linha 2");
            translation.Translations.Add(@"ZipCode", @"Código postal");
            translation.Translations.Add(@"SuburbNeighborhood", @"Suburbano / Bairro");
            translation.Translations.Add(@"City", @"Cidade");
            translation.Translations.Add(@"State", @"Estado");
            translation.Translations.Add(@"County", @"Município");
            translation.Translations.Add(@"Country", @"País");
            translation.Translations.Add(@"AddressInfo", @"Informação de endereço");
            translation.Translations.Add(@"NameOfProductOrService", @"Nome do Produto ou Serviço");
            translation.Translations.Add(@"EstimatedVolumeProduced", @"Volume Estimado Produzido");
            translation.Translations.Add(@"EstimatedVolumeSold", @"Volume Estimado Vendido");
            translation.Translations.Add(@"UnitOfMeasure", @"Unidade de medida");
            translation.Translations.Add(@"EstimatedIncome", @"Renda estimada");
            translation.Translations.Add(@"Currency", @"Moeda");
            translation.Translations.Add(@"FirstNameGivenName", @"Nome");
            translation.Translations.Add(@"LastNameFamilyName", @"Sobrenome");
            translation.Translations.Add(@"MiddleName", @"Nome do meio");
            translation.Translations.Add(@"Gender", @"Gênero");
            translation.Translations.Add(@"DateOfBirth", @"Data de nascimento");
            translation.Translations.Add(@"IsTheBirthdayAnApproximateDate", @"O aniversário é uma data aproximada?");
            translation.Translations.Add(@"Relationship", @"Relação");
            translation.Translations.Add(@"RelationshipOther", @"Outro relacionamento");
            translation.Translations.Add(@"WorkActivitiesQuestion", @"Durante a semana passada, você fez alguma das seguintes atividades, mesmo por apenas uma hora?");
            translation.Translations.Add(@"WorkActivitiesReturningQuestion", @"Embora você não tenha feito nenhuma dessas atividades na semana passada, você tem um emprego, negócios ou outras atividades econômicas ou agrícolas que você certamente irá retornar?");
            translation.Translations.Add(@"WorkActivitiesHoursEngagedQuestion", @"Durante a semana passada, por quantas horas você participou dessas atividades?");
            translation.Translations.Add(@"HazardousConditionsQuestion", @"Você esteve exposto a qualquer um dos seguintes no trabalho?");
            translation.Translations.Add(@"HouseholdTasksQuestion", @"Durante a semana passada, você fez alguma das tarefas abaixo para esta casa?");
            translation.Translations.Add(@"HouseholdTasksHoursEngagedQuestion", @"Durante cada dia da semana passada, por quantas horas você participou dessas atividades?");
            translation.Translations.Add(@"AreYouEnrolledInSchoolAndOrCollege", @"Você está matriculado na escola e / ou na faculdade?");
            translation.Translations.Add(@"DPMSURL", @"URL do DPMS");
            translation.Translations.Add(@"ErrorNameCanNotBeBlank", @"O nome não pode estar em branco");
            translation.Translations.Add(@"ErrorHouseholdNameCanNotBeBlank", @"O nome do agregado familiar não pode estar em branco");
            translation.Translations.Add(@"ErrorIncomeSourceNameCanNotBeBlank", @"O nome da fonte de renda não pode estar em branco");
            translation.Translations.Add(@"ErrorLastNameCanNotBeBlank", @"O apelido não pode estar em branco");
            translation.Translations.Add(@"ErrorFirstNameCanNotBeBlank", @"O primeiro nome não pode estar em branco");
            translation.Translations.Add(@"ErrorGenderMustBeSelected", @"Um gênero deve ser selecionado");
            translation.Translations.Add(@"ErrorSyncError", @"Erro de sincronização");
            translation.Translations.Add(@"SyncSuccessful", @"Sync Successful");
            translation.Translations.Add(@"Members", @"Membros");
            translation.Translations.Add(@"HouseholdMemberId", @"ID do membro do agregado familiar");
            translation.Translations.Add(@"Age", @"Era");
            translation.Translations.Add(@"AssignService", @"Atribuir Serviço");
            translation.Translations.Add(@"Service", @"Serviço");
            translation.Translations.Add(@"StartDate", @"Data de início");
            translation.Translations.Add(@"EndDate", @"Data final");
            translation.Translations.Add(@"Hours", @"Horas");
            translation.Translations.Add(@"Notes", @"Notas");
            translation.Translations.Add(@"SelectService", @"Selecione Serviço");
            translation.Translations.Add(@"Validation", @"Validação");
            translation.Translations.Add(@"ValidationErrorMessageNoServiceSelected", @"Selecione um serviço");
            translation.Translations.Add(@"ValidationErrorMessageStartDateIsAfterEndDate", @"A data de início é após a data de término");
            translation.Translations.Add(@"ValidationErrorMessageHoursInvalid", @"O valor das horas é inválido");
            translation.Translations.Add(@"AlertMessageActionNotAllowedUntilInitialSyncIsPerformed", @"Ação não é permitida até que a sincronização inicial seja realizada");
            translation.Translations.Add(@"AlertMessageSyncCancelledNoInternetConnectivity", @"Sincronização cancelada, não há conectividade com a internet");
            translation.Translations.Add(@"FollowUp", @"Acompanhamento");
            translation.Translations.Add(@"HouseholdMembers", @"Membros do lar");
            translation.Translations.Add(@"FollowUpDate", @"Data de acompanhamento");
            translation.Translations.Add(@"AlertMessageActionNotAllowedNoServices", @"Ação não permitida, nenhum serviço existe");
            translation.Translations.Add(@"Version", @"Versão");
            translation.Translations.Add(@"APK", @"APK");
            translation.Translations.Add(@"Participant", @"Participante");
            translation.Translations.Add(@"Participants", @"Participantes");
            translation.Translations.Add(@"Male", @"Masculino");
            translation.Translations.Add(@"Female", @"Fêmea");
            translation.Translations.Add(@"SelectGender", @"Selecione gênero");
            translation.Translations.Add(@"SelectRelationship", @"Selecione Relacionamento");
            translation.Translations.Add(@"OTHER", @"DE OUTROS");
            translation.Translations.Add(@"AddAnswer", @"Adicionar Resposta");
            translation.Translations.Add(@"RemoveAnswer", @"Remover Resposta");
            translation.Translations.Add(@"GridHeaderTitleID", @"ID");
            translation.Translations.Add(@"GridHeaderTitleBio", @"Bio");
            translation.Translations.Add(@"Children", @"Crianças");
            translation.Translations.Add(@"Adult", @"Adulto");
            translation.Translations.Add(@"Confirm", @"Confirm");
            translation.Translations.Add(@"ConfirmationMessageDeleteHousehold", @"Tem certeza de que deseja excluir este domicílio? Todas as fontes de renda associadas, membros do agregado familiar, acompanhamentos e atribuições de serviços também serão removidos.");
            translation.Translations.Add(@"Edit", @"Editar");
            translation.Translations.Add(@"Delete", @"Excluir");
            translation.Translations.Add(@"IncomeSources", @"Fontes de Renda");
            translation.Translations.Add(@"ConfirmationMessageDeleteIncomeSource", @"Tem certeza de que deseja excluir essa fonte de receita?");
            translation.Translations.Add(@"Save", @"Salvar");
            translation.Translations.Add(@"ConfirmationMessageDeleteHouseholdMember", @"Tem certeza de que deseja excluir este membro do agregado familiar?  Todos os follow ups associados e atribuições de serviço também serão removidos.");
            translation.Translations.Add(@"FollowUps", @"Na sequência");
            translation.Translations.Add(@"AddFollowUp", @"Adicionar Acompanhamento");
            translation.Translations.Add(@"ConfirmationMessageDeletePersonFollowUp", @"Tem certeza de que deseja excluir este acompanhamento?");
            translation.Translations.Add(@"ServiceAssignments", @"Atribuições de serviço");
            translation.Translations.Add(@"ConfirmationMessageDeleteServiceAssignment", @"Tem certeza de que deseja excluir esta atribuição de serviço?");
            translation.Translations.Add(@"EditHousehold", @"Editar casa");
            translation.Translations.Add(@"IncomeSource", @"Fonte de renda");
            translation.Translations.Add(@"EditIncomeSource", @"Editar fonte de renda");
            translation.Translations.Add(@"HouseholdMember", @"Membro do agregado familiar");
            translation.Translations.Add(@"EditHouseholdMember", @"Editar membro do agregado familiar");
            translation.Translations.Add(@"EditFollowUp", @"Editar acompanhamento");
            translation.Translations.Add(@"ServiceAssignment", @"Atribuição de serviço");
            translation.Translations.Add(@"EditServiceAssignment", @"Editar atribuição de serviço");
            return translation;
        }
    }     
}
