﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HSH.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HSHEntities : DbContext
    {
        public HSHEntities()
            : base("name=HSHEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Collaterals> Collaterals { get; set; }
        public virtual DbSet<CompanyAsset> CompanyAsset { get; set; }
        public virtual DbSet<DrugAllergies> DrugAllergies { get; set; }
        public virtual DbSet<Drugs> Drugs { get; set; }
        public virtual DbSet<MedicalHistories> MedicalHistories { get; set; }
        public virtual DbSet<MedicalHistoryDetails> MedicalHistoryDetails { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberAsset> MemberAsset { get; set; }
        public virtual DbSet<Pay> Pay { get; set; }
        public virtual DbSet<Paytype> Paytype { get; set; }
        public virtual DbSet<Profiles> Profiles { get; set; }
        public virtual DbSet<Risk> Risk { get; set; }
        public virtual DbSet<SpotPrice> SpotPrice { get; set; }
        public virtual DbSet<StockOnline> StockOnline { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketReceive> TicketReceive { get; set; }
        public virtual DbSet<TicketSettle> TicketSettle { get; set; }
        public virtual DbSet<TicketSettleSummary> TicketSettleSummary { get; set; }
        public virtual DbSet<TicketStatus> TicketStatus { get; set; }
        public virtual DbSet<Trade> Trade { get; set; }
        public virtual DbSet<TradeLot> TradeLot { get; set; }
        public virtual DbSet<Trans> Trans { get; set; }
        public virtual DbSet<Transfer> Transfer { get; set; }
        public virtual DbSet<UserBackend> UserBackend { get; set; }
        public virtual DbSet<UserOnline> UserOnline { get; set; }
        public virtual DbSet<ViewOfCreditLine> ViewOfCreditLine { get; set; }
        public virtual DbSet<ViewOfSumaryTicketReceive> ViewOfSumaryTicketReceive { get; set; }
        public virtual DbSet<ViewOfSummaryTransfer> ViewOfSummaryTransfer { get; set; }
    }
}