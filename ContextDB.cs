using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;

namespace cngfapco.Models
{
    public class ContextDB : DbContext
    {
        public ContextDB()
            : base("ContextDB")
        {
            //this.Configuration.ProxyCreationEnabled = false; // ADD THIS LINE !
        }
        public DbSet<Workshop> tbl_Workshops { get; set; }
        public DbSet<States> tbl_States { get; set; }
        public DbSet<City> tbl_Cities { get; set; }
        public DbSet<User> tbl_Users { get; set; }
        public DbSet<UserEntranceInfo> tbl_UserEntranceInfos { get; set; }
        public DbSet<Role> tbl_Roles { get; set; }
        public DbSet<Permission> tbl_Permissions { get; set; }
        public DbSet<VehicleRegistration> tbl_VehicleRegistrations { get; set; }
        public DbSet<VehicleType> tbl_VehicleTypes { get; set; }
        public DbSet<VehicleInvoice> tbl_VehicleInvoices { get; set; }
        public DbSet<VehicleTank> tbl_VehicleTanks { get; set; }
        public DbSet<TankConstractor> tbl_TankConstractors { get; set; }
        public DbSet<Message> tbl_Messages { get; set; }
        public DbSet<SideBarItem> tbl_SideBarItems { get; set; }
        public DbSet<ValveConstractor> tbl_ValveConstractors { get; set; }
        public DbSet<RegulatorConstractor> tbl_RegulatorConstractors { get; set; }
        public DbSet<TypeofUse> tbl_TypeofUses { get; set; }
        public DbSet<ValveDivisionPlan> tbl_ValveDivisionPlans { get; set; }
        public DbSet<KitDivisionPlan> tbl_KitDivisionPlans { get; set; }
        public DbSet<DivisionPlan> tbl_DivisionPlans { get; set; }
        //-------------new in 1398-12-23-------------------------------------//
        public DbSet<TypeofTank> tbl_TypeofTanks { get; set; }
        public DbSet<TypeofTankBase> tbl_TypeofTankBases { get; set; }
        public DbSet<TypeofTankCover> tbl_TypeofTankCovers { get; set; }
        public DbSet<TankDivisionPlan> tbl_TankDivisionPlans { get; set; }
        public DbSet<TankBaseDivisionPlan> tbl_TankBaseDivisionPlans { get; set; }
        public DbSet<TankCoverDivisionPlan> tbl_TankCoverDivisionPlans { get; set; }
        public DbSet<OtherThingsDivisionPlan> tbl_OtherThingsDivisionPlans { get; set; }

        //------------------------------------1399/01/15 05:57-------------------------------------
        public DbSet<Remittances> tbl_Remittances { get; set; }
        public DbSet<Insurance> tbl_Insurances { get; set; }

        //------------------------------------1399/01/29 10:20-------------------------------------
        public DbSet<Otherthings> tbl_Otherthings { get; set; }
        public DbSet<Operator> tbl_Operators { get; set; }

        //------------------------------------1399/01/30 01:10-------------------------------------
        public DbSet<RemittanceDetails> tbl_RemittanceDetails { get; set; }

        //------------------------------------1399/03/23 12:25-------------------------------------
        public DbSet<Tank> tbl_Tanks { get; set; }
        public DbSet<TankValve> tbl_TankValves { get; set; }
        public DbSet<Kit> tbl_Kits { get; set; }

        //------------------------------------1399/04/15 16:00-------------------------------------
        public DbSet<Contradiction> tbl_Contradictions { get; set; }

        //------------------------------------1399/04/28 15:58-------------------------------------
        public DbSet<Education> tbl_Educations { get; set; }

        //------------------------------------1399/05/02 12:20-------------------------------------
        public DbSet<EquipmentList> tbl_EquipmentList { get; set; }
        public DbSet<ListofServices> tbl_ListofServices { get; set; }
        public DbSet<BOM> tbl_BOMs { get; set; }
        public DbSet<CurrencyType> tbl_CurrencyTypes { get; set; }
        public DbSet<Invoice> tbl_Invoices { get; set; }

        //------------------------------------1399/05/05 17:33------------------------------------
        public DbSet<InvoiceFapa> tbl_InvoicesFapa { get; set; }

        //------------------------------------1399/05/09 13:10------------------------------------
        public DbSet<BankTank> tbl_BankTanks { get; set; }
        public DbSet<BankTankValve> tbl_BankTankValves { get; set; }

        //------------------------------------1399/05/23 18:05------------------------------------
        public DbSet<FinancialDesc> tbl_FinancialDescs { get; set; }
        public DbSet<FinancialPayment> tbl_FinancialPayments { get; set; }

        //------------------------------------1399/06/20 20:00------------------------------------
        public DbSet<Audit> tbl_Audits { get; set; }
        public DbSet<AuditFile> tbl_AuditFiles { get; set; }
        public DbSet<AuditCategory> tbl_AuditCategories { get; set; }

        //------------------------------------1399/06/27 18:50------------------------------------
        public DbSet<Auditor> tbl_Auditors { get; set; }

        //------------------------------------1399/06/28 22:50------------------------------------
        public DbSet<Category> tbl_Categories { get; set; }
        public DbSet<Instruction> tbl_Instructions { get; set; }
        public DbSet<InstructionForm> tbl_InstructionForms { get; set; }

        //------------------------------------1399/07/03 21:00------------------------------------
        public DbSet<ContradictionTotal> tbl_ContradictionTotals { get; set; }
        public DbSet<ContradictionType> tbl_ContradictionTypes { get; set; }

        //------------------------------------1399/07/05 11:50------------------------------------
        public DbSet<Warehouse> tbl_Warehouses { get; set; }
        public DbSet<DivisionPlanBOM> tbl_DivisionPlanBOMs { get; set; }

        //------------------------------------1399/07/16 13:00------------------------------------
        public DbSet<MessageReply> tbl_MessageReplies { get; set; }
        public DbSet<MessageForward> tbl_MessageForwards { get; set; }

        //------------------------------------1399/07/17 17:40------------------------------------
        public DbSet<CRM> tbl_CRMs { get; set; }
        public DbSet<CRMIndex> tbl_CRMIndexes { get; set; }

        //------------------------------------1399/07/27 18:50------------------------------------
        public DbSet<BankKit> tbl_BankKits { get; set; }

        //------------------------------------1399/08/03 16:20------------------------------------
        public DbSet<Slider> tbl_Sliders { get; set; }
        public DbSet<ContactUs> tbl_ContactUs { get; set; }

        //------------------------------------1399/08/04 21:30------------------------------------
        public DbSet<BankAccount> tbl_BankAccounts { get; set; }

        //------------------------------------1399/08/14 21:30------------------------------------
        public DbSet<VehicleAttachment> tbl_VehicleAttachments { get; set; }

        //------------------------------------1399/09/24 17:25------------------------------------
        public DbSet<SMSPanelResult> tbl_SMSPanelResults { get; set; }
        public DbSet<RegistrationPrice> tbl_RegistrationPrice { get; set; }

        //------------------------------------1399/09/27 11:25------------------------------------
        public DbSet<Payment> tbl_Payments { get; set; }
        public DbSet<CNGHandBooks> tbl_CNGHandBooks { get; set; }

        //------------------------------------1399/09/27 11:25------------------------------------
        public DbSet<AuditCompany> tbl_AuditComponies { get; set; }

        //------------------------------------1399/09/27 11:25------------------------------------
        public DbSet<IRNGV> tbl_IRNGV { get; set; }

        //------------------------------------1399/10/24 11:10------------------------------------
        public DbSet<CRMDynamicForm> tbl_CRMDynamicForms { get; set; }

        //------------------------------------1399/10/24 19:41------------------------------------
        public DbSet<AnswerQuestion> tbl_AnswerQuestions { get; set; }

        //------------------------------------1399/12/10 10:40------------------------------------
        public DbSet<BankCutofValve> tbl_BankCutofValves { get; set; }
        public DbSet<CutofValve> tbl_CutofValves { get; set; }
        public DbSet<BankFillingValve> tbl_BankFillingValves { get; set; }
        public DbSet<FillingValve> tbl_FillingValves { get; set; }
        public DbSet<FillingValveConstractor> tbl_FillingValveConstractors { get; set; }
        public DbSet<CutofValveConstractor> tbl_CutofValveConstractors { get; set; }

        //------------------------------------1399/12/15 13:15------------------------------------
        public DbSet<InsuranceType> tbl_InsuranceTypes { get; set; }
        public DbSet<InsuranceCompany> tbl_InsuranceCompanies { get; set; }
        public DbSet<WorkshopInsurance> tbl_WorkshopInsurance { get; set; }

        //------------------------------------1400/01/24 12:50------------------------------------
        public DbSet<CylinderDetail> tbl_CylinderDetails { get; set; }

        //------------------------------------1400/02/20 23:00------------------------------------
        public DbSet<FreeSaleInvoice> tbl_FreeSaleInvoices { get; set; }

        //------------------------------------1400/02/22 22:10------------------------------------
        public DbSet<RequestFreeSale> tbl_RequestFreeSales { get; set; }

        //------------------------------------1400/02/28 23:50------------------------------------
        public DbSet<Customer> tbl_Customers { get; set; }

        //------------------------------------1400/02/31 14:20------------------------------------
        public DbSet<FinallFreeSaleInvoice> tbl_FinallFreeSaleInvoices { get; set; }

        //------------------------------------1400/02/31 23:35------------------------------------
        public DbSet<SaleWarehouse> tbl_SaleWarehouses { get; set; }

        //------------------------------------1400/03/15 12:02------------------------------------
        public DbSet<FreeSaleRemittances> tbl_FreeSaleRemittances { get; set; }
        public DbSet<FreeSaleRemittanceDetails> tbl_FreeSaleRemittanceDetails { get; set; }

        //------------------------------------1400/03/19 12:00------------------------------------
        public DbSet<CustomerRequest> tbl_CustomerRequests { get; set; }
        public DbSet<CustomerPreSaleInvoice> tbl_CustomerPreSaleInvoices { get; set; }
        public DbSet<CustomerFinallSaleInvoice> tbl_CustomerFinallSaleInvoices { get; set; }

        //------------------------------------1400/05/26 19:30------------------------------------
        public DbSet<GCR> tbl_GCR { get; set; }

        //------------------------------------1400/08/28 11:45------------------------------------
        public DbSet<BankFuelRelay> tbl_BankFuelRelays { get; set; }
        public DbSet<FuelRelay> tbl_FuelRelays { get; set; }
        public DbSet<BankGasECU> tbl_BankGasECU { get; set; }
        public DbSet<GasECU> tbl_GasECU { get; set; }
        public DbSet<FuelRelayConstractor> tbl_FuelRelayConstractors { get; set; }
        public DbSet<GasECUConstractor> tbl_GasECUConstractors { get; set; }

        //------------------------------------1400/11/25 20:05------------------------------------
        public DbSet<OfferedPrice> tbl_OfferedPrices { get; set; }
        //
        //------------------------------------1400/12/12 21:55------------------------------------
        public DbSet<ReturnofParts> tbl_ReturnofParts { get; set; }

        //------------------------------------1400/12/16 21:55------------------------------------
        public DbSet<AuditsPrice> tbl_AuditsPrice { get; set; }

        //------------------------------------1401/07/22 23:35------------------------------------
        public DbSet<GenerationofRegulator> tbl_GenerationofRegulators { get; set; }

        //------------------------------------1402/10/21 20:24------------------------------------
        public DbSet<RegistrationType> tbl_RegistrationTypes { get; set; }

        //------------------------------------1403/02/21 16:00------------------------------------
        public DbSet<TaxValueAdded> tbl_TaxValueAdded { get; set; }

        //------------------------------------1403/03/29 19:20------------------------------------
        public DbSet<ReplacementPlanPrice> tbl_ReplacementPlanPrice { get; set; }

        //------------------------------------1403/03/30 19:30------------------------------------
        public DbSet<IRNGVDamages> tbl_IRNGVDamages { get; set; }

        //------------------------------------1403/03/31 15:20------------------------------------
        public DbSet<InvoicesDamages> tbl_InvoicesDamages { get; set; }

        //------------------------------------1403/06/31 21:30------------------------------------
        public DbSet<InvoicesFapa_DamagesCylinder> tbl_InvoicesFapa_DamagesCylinder { get; set; }

        //------------------------------------1403/07/20 13:10------------------------------------
        public DbSet<TaxandComplications> tbl_TaxandComplications { get; set; }

        //------------------------------------1404/02/15 18:20------------------------------------
        public DbSet<InvoicesValveDamages> tbl_InvoicesValveDamages { get; set; }

        //------------------------------------1404/02/16 13:35------------------------------------
        public DbSet<ReplacementPlanValvePrice> tbl_ReplacementPlanValvePrice { get; set; }

        //------------------------------------1404/02/28 13:10------------------------------------
        public DbSet<Deposit> tbl_Deposits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Permissions)
                .Map(m => m.ToTable("LNK_Role_Permission").MapLeftKey("Permission_Id").MapRightKey("Role_Id"));

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("LNK_User_Role").MapLeftKey("Role_Id").MapRightKey("User_Id"));

            modelBuilder.Entity<Workshop>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Workshops)
                .Map(m => m.ToTable("LNK_User_Workshop").MapLeftKey("Workshop_Id").MapRightKey("User_Id"));

            modelBuilder.Entity<SideBarItem>()
                .HasMany(e => e.Users)
                .WithMany(e => e.SideBarItems)
                .Map(m => m.ToTable("LNK_USER_SideBarItem").MapLeftKey("SideBarItems_Id").MapRightKey("User_Id"));
            

            //modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
        
    }
}