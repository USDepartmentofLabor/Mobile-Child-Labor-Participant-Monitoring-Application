﻿// <auto-generated />
using MDPMS.Database.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Migrations.Migrations
{
    [DbContext(typeof(MDPMSDatabaseContext))]
    partial class MDPMSDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomField", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("ExternalId");

                    b.Property<string>("FieldType");

                    b.Property<string>("HelpText");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("ModelType");

                    b.Property<string>("Name");

                    b.Property<string>("Options");

                    b.Property<bool>("SoftDeleted");

                    b.Property<int>("SortOrder");

                    b.HasKey("InternalId");

                    b.ToTable("CustomFields");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomHouseholdValue", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("CustomFieldInternalId");

                    b.Property<int?>("ExternalCustomFieldId");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("HouseholdInternalId");

                    b.Property<int?>("InternalCustomFieldId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<bool>("SoftDeleted");

                    b.Property<string>("Value");

                    b.HasKey("InternalId");

                    b.HasIndex("CustomFieldInternalId");

                    b.HasIndex("HouseholdInternalId");

                    b.ToTable("CustomHouseholdValues");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomPersonFollowUpValue", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("CustomFieldInternalId");

                    b.Property<int?>("ExternalCustomFieldId");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("InternalCustomFieldId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<int?>("PersonFollowUpInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.Property<string>("Value");

                    b.HasKey("InternalId");

                    b.HasIndex("CustomFieldInternalId");

                    b.HasIndex("PersonFollowUpInternalId");

                    b.ToTable("CustomPersonFollowUpValues");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomPersonValue", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<int?>("CustomFieldInternalId");

                    b.Property<int?>("ExternalCustomFieldId");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("InternalCustomFieldId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<int?>("PersonInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.Property<string>("Value");

                    b.HasKey("InternalId");

                    b.HasIndex("CustomFieldInternalId");

                    b.HasIndex("PersonInternalId");

                    b.ToTable("CustomPersonValues");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Gender", b =>
                {
                    b.Property<int>("GenderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DpmsGenderNumber");

                    b.Property<string>("GenderReadable");

                    b.HasKey("GenderId");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Household", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressInfo");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("AdminvArea");

                    b.Property<string>("Country");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("DependentAdminvArea");

                    b.Property<string>("DependentLocality");

                    b.Property<int?>("ExternalId");

                    b.Property<double?>("GpsAltitude");

                    b.Property<double?>("GpsAltitudeAccuracy");

                    b.Property<double?>("GpsHeading");

                    b.Property<double?>("GpsLatitude");

                    b.Property<double?>("GpsLongitude");

                    b.Property<double?>("GpsPositionAccuracy");

                    b.Property<DateTime?>("GpsPositionTime");

                    b.Property<double?>("GpsSpeed");

                    b.Property<string>("HouseholdName");

                    b.Property<DateTime>("IntakeDate");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("Locality");

                    b.Property<string>("PostalCode");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("Households");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.IncomeSource", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Currency");

                    b.Property<decimal?>("EstimatedIncome");

                    b.Property<int?>("EstimatedVolumeProduced");

                    b.Property<int?>("EstimatedVolumeSold");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("HouseholdInternalId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("ProductServiceName");

                    b.Property<bool>("SoftDeleted");

                    b.Property<string>("UnitOfMeasure");

                    b.HasKey("InternalId");

                    b.HasIndex("HouseholdInternalId");

                    b.ToTable("IncomeSources");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Person", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<bool?>("DateOfBirthIsApproximate");

                    b.Property<bool?>("EnrolledInSchool");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<string>("FirstName");

                    b.Property<int?>("Genders");

                    b.Property<double?>("GpsAltitude");

                    b.Property<double?>("GpsAltitudeAccuracy");

                    b.Property<double?>("GpsHeading");

                    b.Property<double?>("GpsLatitude");

                    b.Property<double?>("GpsLongitude");

                    b.Property<double?>("GpsPositionAccuracy");

                    b.Property<DateTime?>("GpsPositionTime");

                    b.Property<double?>("GpsSpeed");

                    b.Property<bool?>("HaveJobReturningTo");

                    b.Property<int?>("HoursWorked");

                    b.Property<int?>("HouseWorkedOnHousework");

                    b.Property<int?>("HouseholdInternalId");

                    b.Property<DateTime?>("IntakeDate");

                    b.Property<int?>("InternalParentId");

                    b.Property<string>("LastName");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("MiddleName");

                    b.Property<int?>("PersonRelationships");

                    b.Property<string>("RelationshipIfOther");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.HasIndex("Genders");

                    b.HasIndex("HouseholdInternalId");

                    b.HasIndex("PersonRelationships");

                    b.ToTable("People");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUp", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<bool?>("EnrolledInSchool");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<DateTime?>("FollowUpDate");

                    b.Property<double?>("GpsAltitude");

                    b.Property<double?>("GpsAltitudeAccuracy");

                    b.Property<double?>("GpsHeading");

                    b.Property<double?>("GpsLatitude");

                    b.Property<double?>("GpsLongitude");

                    b.Property<double?>("GpsPositionAccuracy");

                    b.Property<DateTime?>("GpsPositionTime");

                    b.Property<double?>("GpsSpeed");

                    b.Property<bool?>("HaveJobReturningTo");

                    b.Property<int?>("HoursWorked");

                    b.Property<int?>("HouseWorkedOnHousework");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<int?>("PersonInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.HasIndex("PersonInternalId");

                    b.ToTable("PersonFollowUps");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpHazardousCondition", b =>
                {
                    b.Property<int>("PersonFollowUpInternalId");

                    b.Property<int>("HazardousConditionInternalId");

                    b.HasKey("PersonFollowUpInternalId", "HazardousConditionInternalId");

                    b.HasIndex("HazardousConditionInternalId");

                    b.ToTable("PersonFollowUpHazardousCondition");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpHouseholdTask", b =>
                {
                    b.Property<int>("PersonFollowUpInternalId");

                    b.Property<int>("HouseholdTaskInternalId");

                    b.HasKey("PersonFollowUpInternalId", "HouseholdTaskInternalId");

                    b.HasIndex("HouseholdTaskInternalId");

                    b.ToTable("PersonFollowUpHouseholdTask");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpWorkActivity", b =>
                {
                    b.Property<int>("PersonFollowUpInternalId");

                    b.Property<int>("WorkActivityInternalId");

                    b.HasKey("PersonFollowUpInternalId", "WorkActivityInternalId");

                    b.HasIndex("WorkActivityInternalId");

                    b.ToTable("PersonFollowUpWorkActivity");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonHazardousCondition", b =>
                {
                    b.Property<int>("PersonInternalId");

                    b.Property<int>("HazardousConditionInternalId");

                    b.HasKey("PersonInternalId", "HazardousConditionInternalId");

                    b.HasIndex("HazardousConditionInternalId");

                    b.ToTable("PersonHazardousCondition");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonHouseholdTask", b =>
                {
                    b.Property<int>("PersonInternalId");

                    b.Property<int>("HouseholdTaskInternalId");

                    b.HasKey("PersonInternalId", "HouseholdTaskInternalId");

                    b.HasIndex("HouseholdTaskInternalId");

                    b.ToTable("PersonHouseholdTask");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonRelationship", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CanonicalName");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("ExternalId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("PersonRelationships");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonWorkActivity", b =>
                {
                    b.Property<int>("PersonInternalId");

                    b.Property<int>("WorkActivityInternalId");

                    b.HasKey("PersonInternalId", "WorkActivityInternalId");

                    b.HasIndex("WorkActivityInternalId");

                    b.ToTable("PersonWorkActivity");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Service", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("Name");

                    b.Property<int?>("ServiceTypeInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("InternalId");

                    b.HasIndex("ServiceTypeInternalId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.ServiceInstance", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<DateTime>("EndDate");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("Hours");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("Notes");

                    b.Property<int?>("PersonInternalId");

                    b.Property<int?>("ServiceInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("InternalId");

                    b.HasIndex("PersonInternalId");

                    b.HasIndex("ServiceInternalId");

                    b.ToTable("ServiceInstances");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.ServiceType", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Definition");

                    b.Property<int?>("ExternalId");

                    b.Property<int?>("ExternalParentId");

                    b.Property<int?>("InternalParentId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("Name");

                    b.Property<int?>("ServiceTypeCategoryInternalId");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.HasIndex("ServiceTypeCategoryInternalId");

                    b.ToTable("ServiceTypes");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.ServiceTypeCategory", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("Definition");

                    b.Property<int?>("ExternalId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<string>("Name");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("ServiceTypeCategories");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.StatusCustomizationHazardousCondition", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CanonicalName");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("ExternalId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("StatusCustomizationHazardousConditions");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.StatusCustomizationHouseholdTask", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CanonicalName");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("ExternalId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("StatusCustomizationHouseholdTasks");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.StatusCustomizationWorkActivity", b =>
                {
                    b.Property<int>("InternalId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CanonicalName");

                    b.Property<string>("Code");

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<string>("DisplayName");

                    b.Property<int?>("ExternalId");

                    b.Property<DateTime?>("LastUpdatedAt");

                    b.Property<bool>("SoftDeleted");

                    b.HasKey("InternalId");

                    b.ToTable("StatusCustomizationWorkActivities");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomHouseholdValue", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.CustomField", "CustomField")
                        .WithMany("CustomHouseholdValues")
                        .HasForeignKey("CustomFieldInternalId");

                    b.HasOne("MDPMS.Database.Data.Models.Household", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomPersonFollowUpValue", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.CustomField", "CustomField")
                        .WithMany("CustomPersonFollowUpValues")
                        .HasForeignKey("CustomFieldInternalId");

                    b.HasOne("MDPMS.Database.Data.Models.PersonFollowUp", "PersonFollowUp")
                        .WithMany()
                        .HasForeignKey("PersonFollowUpInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.CustomPersonValue", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.CustomField", "CustomField")
                        .WithMany("CustomPersonValues")
                        .HasForeignKey("CustomFieldInternalId");

                    b.HasOne("MDPMS.Database.Data.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.IncomeSource", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.Household")
                        .WithMany("IncomeSources")
                        .HasForeignKey("HouseholdInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Person", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("Genders");

                    b.HasOne("MDPMS.Database.Data.Models.Household")
                        .WithMany("Members")
                        .HasForeignKey("HouseholdInternalId");

                    b.HasOne("MDPMS.Database.Data.Models.PersonRelationship", "RelationshipToHeadOfHousehold")
                        .WithMany()
                        .HasForeignKey("PersonRelationships");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUp", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.Person")
                        .WithMany("PeopleFollowUps")
                        .HasForeignKey("PersonInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpHazardousCondition", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationHazardousCondition", "HazardousCondition")
                        .WithMany("PeopleFollowUpHazardousConditions")
                        .HasForeignKey("HazardousConditionInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.PersonFollowUp", "PersonFollowUp")
                        .WithMany("PeopleFollowUpHazardousConditions")
                        .HasForeignKey("PersonFollowUpInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpHouseholdTask", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationHouseholdTask", "HouseholdTask")
                        .WithMany("PeopleFollowUpHouseholdTasks")
                        .HasForeignKey("HouseholdTaskInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.PersonFollowUp", "PersonFollowUp")
                        .WithMany("PeopleFollowUpHouseholdTasks")
                        .HasForeignKey("PersonFollowUpInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonFollowUpWorkActivity", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.PersonFollowUp", "PersonFollowUp")
                        .WithMany("PeopleFollowUpWorkActivities")
                        .HasForeignKey("PersonFollowUpInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationWorkActivity", "WorkActivity")
                        .WithMany("PeopleFollowUpWorkActivities")
                        .HasForeignKey("WorkActivityInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonHazardousCondition", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationHazardousCondition", "HazardousCondition")
                        .WithMany("PeopleHazardousConditions")
                        .HasForeignKey("HazardousConditionInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.Person", "Person")
                        .WithMany("PeopleHazardousConditions")
                        .HasForeignKey("PersonInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonHouseholdTask", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationHouseholdTask", "HouseholdTask")
                        .WithMany("PeopleHouseholdTasks")
                        .HasForeignKey("HouseholdTaskInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.Person", "Person")
                        .WithMany("PeopleHouseholdTasks")
                        .HasForeignKey("PersonInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.PersonWorkActivity", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.Person", "Person")
                        .WithMany("PeopleWorkActivities")
                        .HasForeignKey("PersonInternalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MDPMS.Database.Data.Models.StatusCustomizationWorkActivity", "WorkActivity")
                        .WithMany("PeopleWorkActivities")
                        .HasForeignKey("WorkActivityInternalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.Service", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.ServiceType")
                        .WithMany("Services")
                        .HasForeignKey("ServiceTypeInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.ServiceInstance", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.Person")
                        .WithMany("ServiceInstances")
                        .HasForeignKey("PersonInternalId");

                    b.HasOne("MDPMS.Database.Data.Models.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceInternalId");
                });

            modelBuilder.Entity("MDPMS.Database.Data.Models.ServiceType", b =>
                {
                    b.HasOne("MDPMS.Database.Data.Models.ServiceTypeCategory")
                        .WithMany("ServiceTypes")
                        .HasForeignKey("ServiceTypeCategoryInternalId");
                });
#pragma warning restore 612, 618
        }
    }
}
