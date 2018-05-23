using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Migrations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomFields",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    FieldType = table.Column<string>(nullable: true),
                    HelpText = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    ModelType = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Options = table.Column<string>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFields", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DpmsGenderNumber = table.Column<int>(nullable: false),
                    GenderReadable = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "Households",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AddressInfo = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    AdminvArea = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DependentAdminvArea = table.Column<string>(nullable: true),
                    DependentLocality = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    GpsAltitude = table.Column<double>(nullable: true),
                    GpsAltitudeAccuracy = table.Column<double>(nullable: true),
                    GpsHeading = table.Column<double>(nullable: true),
                    GpsLatitude = table.Column<double>(nullable: true),
                    GpsLongitude = table.Column<double>(nullable: true),
                    GpsPositionAccuracy = table.Column<double>(nullable: true),
                    GpsPositionTime = table.Column<DateTime>(nullable: true),
                    GpsSpeed = table.Column<double>(nullable: true),
                    HouseholdName = table.Column<string>(nullable: true),
                    IntakeDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    Locality = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Households", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "PersonRelationships",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CanonicalName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRelationships", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypeCategories",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Definition = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypeCategories", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "StatusCustomizationHazardousConditions",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CanonicalName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCustomizationHazardousConditions", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "StatusCustomizationHouseholdTasks",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CanonicalName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCustomizationHouseholdTasks", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "StatusCustomizationWorkActivities",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CanonicalName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusCustomizationWorkActivities", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "CustomHouseholdValues",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CustomFieldInternalId = table.Column<int>(nullable: true),
                    ExternalCustomFieldId = table.Column<int>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    HouseholdInternalId = table.Column<int>(nullable: true),
                    InternalCustomFieldId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomHouseholdValues", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_CustomHouseholdValues_CustomFields_CustomFieldInternalId",
                        column: x => x.CustomFieldInternalId,
                        principalTable: "CustomFields",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomHouseholdValues_Households_HouseholdInternalId",
                        column: x => x.HouseholdInternalId,
                        principalTable: "Households",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSources",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    EstimatedIncome = table.Column<decimal>(nullable: true),
                    EstimatedVolumeProduced = table.Column<int>(nullable: true),
                    EstimatedVolumeSold = table.Column<int>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    HouseholdInternalId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    ProductServiceName = table.Column<string>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    UnitOfMeasure = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSources", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_IncomeSources_Households_HouseholdInternalId",
                        column: x => x.HouseholdInternalId,
                        principalTable: "Households",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DateOfBirthIsApproximate = table.Column<bool>(nullable: true),
                    EnrolledInSchool = table.Column<bool>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Genders = table.Column<int>(nullable: true),
                    GpsAltitude = table.Column<double>(nullable: true),
                    GpsAltitudeAccuracy = table.Column<double>(nullable: true),
                    GpsHeading = table.Column<double>(nullable: true),
                    GpsLatitude = table.Column<double>(nullable: true),
                    GpsLongitude = table.Column<double>(nullable: true),
                    GpsPositionAccuracy = table.Column<double>(nullable: true),
                    GpsPositionTime = table.Column<DateTime>(nullable: true),
                    GpsSpeed = table.Column<double>(nullable: true),
                    HaveJobReturningTo = table.Column<bool>(nullable: true),
                    HoursWorked = table.Column<int>(nullable: true),
                    HouseWorkedOnHousework = table.Column<int>(nullable: true),
                    HouseholdInternalId = table.Column<int>(nullable: true),
                    IntakeDate = table.Column<DateTime>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    PersonRelationships = table.Column<int>(nullable: true),
                    RelationshipIfOther = table.Column<string>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_People_Genders_Genders",
                        column: x => x.Genders,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_Households_HouseholdInternalId",
                        column: x => x.HouseholdInternalId,
                        principalTable: "Households",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_PersonRelationships_PersonRelationships",
                        column: x => x.PersonRelationships,
                        principalTable: "PersonRelationships",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Definition = table.Column<string>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ServiceTypeCategoryInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_ServiceTypes_ServiceTypeCategories_ServiceTypeCategoryInternalId",
                        column: x => x.ServiceTypeCategoryInternalId,
                        principalTable: "ServiceTypeCategories",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomPersonValues",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CustomFieldInternalId = table.Column<int>(nullable: true),
                    ExternalCustomFieldId = table.Column<int>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    InternalCustomFieldId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    PersonInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPersonValues", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_CustomPersonValues_CustomFields_CustomFieldInternalId",
                        column: x => x.CustomFieldInternalId,
                        principalTable: "CustomFields",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomPersonValues_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonFollowUps",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EnrolledInSchool = table.Column<bool>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    FollowUpDate = table.Column<DateTime>(nullable: true),
                    GpsAltitude = table.Column<double>(nullable: true),
                    GpsAltitudeAccuracy = table.Column<double>(nullable: true),
                    GpsHeading = table.Column<double>(nullable: true),
                    GpsLatitude = table.Column<double>(nullable: true),
                    GpsLongitude = table.Column<double>(nullable: true),
                    GpsPositionAccuracy = table.Column<double>(nullable: true),
                    GpsPositionTime = table.Column<DateTime>(nullable: true),
                    GpsSpeed = table.Column<double>(nullable: true),
                    HaveJobReturningTo = table.Column<bool>(nullable: true),
                    HoursWorked = table.Column<int>(nullable: true),
                    HouseWorkedOnHousework = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    PersonInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonFollowUps", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_PersonFollowUps_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonHazardousCondition",
                columns: table => new
                {
                    PersonInternalId = table.Column<int>(nullable: false),
                    HazardousConditionInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonHazardousCondition", x => new { x.PersonInternalId, x.HazardousConditionInternalId });
                    table.ForeignKey(
                        name: "FK_PersonHazardousCondition_StatusCustomizationHazardousConditions_HazardousConditionInternalId",
                        column: x => x.HazardousConditionInternalId,
                        principalTable: "StatusCustomizationHazardousConditions",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonHazardousCondition_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonHouseholdTask",
                columns: table => new
                {
                    PersonInternalId = table.Column<int>(nullable: false),
                    HouseholdTaskInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonHouseholdTask", x => new { x.PersonInternalId, x.HouseholdTaskInternalId });
                    table.ForeignKey(
                        name: "FK_PersonHouseholdTask_StatusCustomizationHouseholdTasks_HouseholdTaskInternalId",
                        column: x => x.HouseholdTaskInternalId,
                        principalTable: "StatusCustomizationHouseholdTasks",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonHouseholdTask_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonWorkActivity",
                columns: table => new
                {
                    PersonInternalId = table.Column<int>(nullable: false),
                    WorkActivityInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonWorkActivity", x => new { x.PersonInternalId, x.WorkActivityInternalId });
                    table.ForeignKey(
                        name: "FK_PersonWorkActivity_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonWorkActivity_StatusCustomizationWorkActivities_WorkActivityInternalId",
                        column: x => x.WorkActivityInternalId,
                        principalTable: "StatusCustomizationWorkActivities",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ServiceTypeInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeInternalId",
                        column: x => x.ServiceTypeInternalId,
                        principalTable: "ServiceTypes",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomPersonFollowUpValues",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CustomFieldInternalId = table.Column<int>(nullable: true),
                    ExternalCustomFieldId = table.Column<int>(nullable: true),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    InternalCustomFieldId = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    PersonFollowUpInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPersonFollowUpValues", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_CustomPersonFollowUpValues_CustomFields_CustomFieldInternalId",
                        column: x => x.CustomFieldInternalId,
                        principalTable: "CustomFields",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomPersonFollowUpValues_PersonFollowUps_PersonFollowUpInternalId",
                        column: x => x.PersonFollowUpInternalId,
                        principalTable: "PersonFollowUps",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonFollowUpHazardousCondition",
                columns: table => new
                {
                    PersonFollowUpInternalId = table.Column<int>(nullable: false),
                    HazardousConditionInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonFollowUpHazardousCondition", x => new { x.PersonFollowUpInternalId, x.HazardousConditionInternalId });
                    table.ForeignKey(
                        name: "FK_PersonFollowUpHazardousCondition_StatusCustomizationHazardousConditions_HazardousConditionInternalId",
                        column: x => x.HazardousConditionInternalId,
                        principalTable: "StatusCustomizationHazardousConditions",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonFollowUpHazardousCondition_PersonFollowUps_PersonFollowUpInternalId",
                        column: x => x.PersonFollowUpInternalId,
                        principalTable: "PersonFollowUps",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonFollowUpHouseholdTask",
                columns: table => new
                {
                    PersonFollowUpInternalId = table.Column<int>(nullable: false),
                    HouseholdTaskInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonFollowUpHouseholdTask", x => new { x.PersonFollowUpInternalId, x.HouseholdTaskInternalId });
                    table.ForeignKey(
                        name: "FK_PersonFollowUpHouseholdTask_StatusCustomizationHouseholdTasks_HouseholdTaskInternalId",
                        column: x => x.HouseholdTaskInternalId,
                        principalTable: "StatusCustomizationHouseholdTasks",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonFollowUpHouseholdTask_PersonFollowUps_PersonFollowUpInternalId",
                        column: x => x.PersonFollowUpInternalId,
                        principalTable: "PersonFollowUps",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonFollowUpWorkActivity",
                columns: table => new
                {
                    PersonFollowUpInternalId = table.Column<int>(nullable: false),
                    WorkActivityInternalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonFollowUpWorkActivity", x => new { x.PersonFollowUpInternalId, x.WorkActivityInternalId });
                    table.ForeignKey(
                        name: "FK_PersonFollowUpWorkActivity_PersonFollowUps_PersonFollowUpInternalId",
                        column: x => x.PersonFollowUpInternalId,
                        principalTable: "PersonFollowUps",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonFollowUpWorkActivity_StatusCustomizationWorkActivities_WorkActivityInternalId",
                        column: x => x.WorkActivityInternalId,
                        principalTable: "StatusCustomizationWorkActivities",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInstances",
                columns: table => new
                {
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ExternalId = table.Column<int>(nullable: true),
                    ExternalParentId = table.Column<int>(nullable: true),
                    Hours = table.Column<int>(nullable: true),
                    InternalParentId = table.Column<int>(nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    PersonInternalId = table.Column<int>(nullable: true),
                    ServiceInternalId = table.Column<int>(nullable: true),
                    SoftDeleted = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceInstances", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_ServiceInstances_People_PersonInternalId",
                        column: x => x.PersonInternalId,
                        principalTable: "People",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceInstances_Services_ServiceInternalId",
                        column: x => x.ServiceInternalId,
                        principalTable: "Services",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomHouseholdValues_CustomFieldInternalId",
                table: "CustomHouseholdValues",
                column: "CustomFieldInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomHouseholdValues_HouseholdInternalId",
                table: "CustomHouseholdValues",
                column: "HouseholdInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPersonFollowUpValues_CustomFieldInternalId",
                table: "CustomPersonFollowUpValues",
                column: "CustomFieldInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPersonFollowUpValues_PersonFollowUpInternalId",
                table: "CustomPersonFollowUpValues",
                column: "PersonFollowUpInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPersonValues_CustomFieldInternalId",
                table: "CustomPersonValues",
                column: "CustomFieldInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPersonValues_PersonInternalId",
                table: "CustomPersonValues",
                column: "PersonInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSources_HouseholdInternalId",
                table: "IncomeSources",
                column: "HouseholdInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_People_Genders",
                table: "People",
                column: "Genders");

            migrationBuilder.CreateIndex(
                name: "IX_People_HouseholdInternalId",
                table: "People",
                column: "HouseholdInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_People_PersonRelationships",
                table: "People",
                column: "PersonRelationships");

            migrationBuilder.CreateIndex(
                name: "IX_PersonFollowUpHazardousCondition_HazardousConditionInternalId",
                table: "PersonFollowUpHazardousCondition",
                column: "HazardousConditionInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonFollowUpHouseholdTask_HouseholdTaskInternalId",
                table: "PersonFollowUpHouseholdTask",
                column: "HouseholdTaskInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonFollowUps_PersonInternalId",
                table: "PersonFollowUps",
                column: "PersonInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonFollowUpWorkActivity_WorkActivityInternalId",
                table: "PersonFollowUpWorkActivity",
                column: "WorkActivityInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonHazardousCondition_HazardousConditionInternalId",
                table: "PersonHazardousCondition",
                column: "HazardousConditionInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonHouseholdTask_HouseholdTaskInternalId",
                table: "PersonHouseholdTask",
                column: "HouseholdTaskInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonWorkActivity_WorkActivityInternalId",
                table: "PersonWorkActivity",
                column: "WorkActivityInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInstances_PersonInternalId",
                table: "ServiceInstances",
                column: "PersonInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInstances_ServiceInternalId",
                table: "ServiceInstances",
                column: "ServiceInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeInternalId",
                table: "Services",
                column: "ServiceTypeInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_ServiceTypeCategoryInternalId",
                table: "ServiceTypes",
                column: "ServiceTypeCategoryInternalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomHouseholdValues");

            migrationBuilder.DropTable(
                name: "CustomPersonFollowUpValues");

            migrationBuilder.DropTable(
                name: "CustomPersonValues");

            migrationBuilder.DropTable(
                name: "IncomeSources");

            migrationBuilder.DropTable(
                name: "PersonFollowUpHazardousCondition");

            migrationBuilder.DropTable(
                name: "PersonFollowUpHouseholdTask");

            migrationBuilder.DropTable(
                name: "PersonFollowUpWorkActivity");

            migrationBuilder.DropTable(
                name: "PersonHazardousCondition");

            migrationBuilder.DropTable(
                name: "PersonHouseholdTask");

            migrationBuilder.DropTable(
                name: "PersonWorkActivity");

            migrationBuilder.DropTable(
                name: "ServiceInstances");

            migrationBuilder.DropTable(
                name: "CustomFields");

            migrationBuilder.DropTable(
                name: "PersonFollowUps");

            migrationBuilder.DropTable(
                name: "StatusCustomizationHazardousConditions");

            migrationBuilder.DropTable(
                name: "StatusCustomizationHouseholdTasks");

            migrationBuilder.DropTable(
                name: "StatusCustomizationWorkActivities");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Households");

            migrationBuilder.DropTable(
                name: "PersonRelationships");

            migrationBuilder.DropTable(
                name: "ServiceTypeCategories");
        }
    }
}
