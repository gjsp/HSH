using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using HSH.Data.Models;

namespace HSH.Data.Models
{
    public class MemberViewModels
    {
        public System.Guid MemberId { get; set; }

        public string MemberRef { get; set; }
        public string MemberClass { get; set; }
        [Required]
        public string MemberType { get; set; }
        public string MemberName { get; set; }
        public Nullable<int> MemberGroup { get; set; }
        public string MemberTitle { get; set; }
        public string MemberLevel { get; set; }

        [StringLength(100, ErrorMessage = "Over length character")]
        [Required]
        public string FirstName { get; set; }


        [StringLength(100, ErrorMessage = "Over length character")]
        public string LastName { get; set; }

        [DisplayName("วันเกิด")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> BirthDate { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Passport ID 13 digit")]
        [DataType(DataType.Currency)]
        public string PassportId { get; set; }
        public Nullable<System.DateTime> DateOfIssue { get; set; }

        [DisplayName("บัตรหมดอายุ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DateOfExpire { get; set; }
        public string Political { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeNo { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeSoi { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeStreet { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeSubDistrict { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeDistrict { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomeProvince { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string HomePostcode { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string PresentAddress { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Phone 10 digit")]
        public string Mobile { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string Telephone { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string Fax { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Over length character")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkType { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkPlace { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkBusinessType { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkTelephone { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkFax { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string Status { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateName { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateCareer { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MatePolitical { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateTelephone { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateWorkplace { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateBusinessType { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyPersonName { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyRelationship { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyAddress { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyTelephone { get; set; }


        public string CorporateType { get; set; }
        public string CorporateName { get; set; }
        public string CorporateBusinessType { get; set; }
        public string CorporateTradeRegNo { get; set; }
        public string CorporateTaxNo { get; set; }
        public string CorporateHomeNo { get; set; }
        public string CorporateHomeSoi { get; set; }
        public string CorporateHomeStreet { get; set; }
        public string CorporateHomeSubDistrict { get; set; }
        public string CorporateHomeDistrict { get; set; }
        public string CorporateHomeProvince { get; set; }
        public string CorporateHomePostcode { get; set; }
        public string CorporateTelephone { get; set; }
        public string CorporateFax { get; set; }
        public string CorporatePresidentSigned { get; set; }
        public string CorporateContactName { get; set; }
        public string CorporateContactPassportId { get; set; }
        public string CorporateContactTelephone { get; set; }
        public string CorporateContactEmail { get; set; }
        public string PayType { get; set; }
        public string PayBank { get; set; }
        public string PayBankBanch { get; set; }
        public string PayAccountType { get; set; }
        public string PayAccountNo { get; set; }
        public string PayAccountName { get; set; }
        public Nullable<System.DateTime> PayBankApprove { get; set; }
        public string PayApproveId { get; set; }
        public string PayApproveBy { get; set; }
        public Nullable<System.DateTime> PayApporveDate { get; set; }
        //public Nullable<double> GuaranteeAmount { get; set; }
        //public Nullable<double> GuaranteeApprovedAmount { get; set; }
        //public Nullable<int> CollateralCash { get; set; }
        //public Nullable<int> CollateralGold { get; set; }
        //public bool Active { get; set; }
        public PortFolioViewModels MemberPortFolio { get; set; }

    }

    public class MemberIndividualViewModels
    {
        public System.Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }

        [DisplayName("ประเภทสมาชิก *")]
        [Required(ErrorMessage = "Required")]
        public string MemberType { get; set; }

        [DisplayName("เลเวล")]
        public Nullable<int> MemberLevel { get; set; }
        
        [DisplayName("กลุ่มสมาชิก *")]
        [Required(ErrorMessage = "Required")]
        public string MemberGroup { get; set; }

        [DisplayName("เจ้าหน้าที่การตลาด *")]
        [Required(ErrorMessage = "Required")]
        public string MarketingName { get; set; }

        [DisplayName("คำนำหน้าชื่อ *")]
        [Required(ErrorMessage = "Required")]
        public string MemberTitle { get; set; }
        //public string MemberLevel { get; set; }

        [DisplayName("ชื่อสมาชิก *")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Must have a minimum length of 4 character, maximum length 50 character")]
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [DisplayName("นามสกุล")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string LastName { get; set; }

        [DisplayName("วันเกิด")]
        public string BirthDate { get; set; }

        [DisplayName("เลขที่บัตรประชาชน *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "เฉพาะตัวเลข 13 หลัก")]
        [DataType(DataType.Currency)]
        public string PassportId { get; set; }

        [DisplayName("วันออกบัตร")]
        public string DateOfIssue { get; set; }

        [DisplayName("วันหมดอายุ")]
        public string DateOfExpire { get; set; }


        //[DisplayName("บ้านเลขที่ *")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeNo { get; set; }

        //[DisplayName("ซอย")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeSoi { get; set; }

        //[DisplayName("ถนน")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeStreet { get; set; }

        //[DisplayName("ตำบล *")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeSubDistrict { get; set; }

        //[DisplayName("อำเภอ *")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeDistrict { get; set; }

        //[DisplayName("จังหวัด *")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomeProvince { get; set; }

        //[DisplayName("รหัสไปรษณีย์ *")]
        //[Required(ErrorMessage = "Required")]
        //[StringLength(50, ErrorMessage = "Over length character")]
        //public string HomePostcode { get; set; }

        [DisplayName("ที่อยู่ตามทะเบียนบ้าน *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Over length character")]
        public string HomeAddress { get; set; }

        [DisplayName("ที่อยู่ปัจจุบัน/ที่อยู่ติดต่อ *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Over length character")]
        public string PresentAddress { get; set; }

        [DisplayName("โทรศัพท์มือถือ")]
        //[Required(ErrorMessage = "Required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Phone 10 digit")]
        public string Mobile { get; set; }

        [DisplayName("โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Telephone { get; set; }

        [StringLength(50, ErrorMessage = "Over length character")]
        [DisplayName("โทรสาร")]
        public string Fax { get; set; }

        [DisplayName("อีเมล์ *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Over length character")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("เลขที่หนังสือเดินทาง")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string PassportNo { get; set; }
        
        [DisplayName("เลขที่บัตรข้าราชการ/รัฐวิสาหกิจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string GovernmentCard { get; set; }


        [DisplayName("ตำแหน่งทางการเมือง")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Political { get; set; }

        [DisplayName("เพศ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Sex { get; set; }

        [DisplayName("สัญชาติ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Nation { get; set; }
    
        [DisplayName("เชื้อชาติ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Ethnicity { get; set; }
    
        [DisplayName("ศาสนา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Religion { get; set; }
    
        [DisplayName("การศึกษา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Education { get; set; }

        [DisplayName("รายได้ต่อเดือน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string IncomeMonth { get; set; }

        [DisplayName("ธนาคาร")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankName { get; set; }

        [DisplayName("ชื่อบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankAccountName { get; set; }

        [DisplayName("เลขที่บัญชี")]
        [StringLength(10, ErrorMessage = "เลขที่บัญชีเฉพาะตัวเลข 10 หลัก")]
        public string BankAccountNo { get; set; }

        [DisplayName("ประเภทบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankAccountType { get; set; }

        [DisplayName("สาขา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankBranch { get; set; }

        [DisplayName("วิธีการชำระเงิน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankPayType { get; set; }

        
        [DisplayName("ประเภทการทำงาน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkType { get; set; }

        [DisplayName("สถานที่ทำงาน")]
        [StringLength(500, ErrorMessage = "Over length character")]
        public string WorkPlace { get; set; }

        [DisplayName("ประเภทธุรกิจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkBusinessType { get; set; }

        [DisplayName("โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkTelephone { get; set; }

        [DisplayName("โทรสาร")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string WorkFax { get; set; }

        [DisplayName("สถานภาพ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Status { get; set; }

        [DisplayName("ชื่อคู่สมรส")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateName { get; set; }

        [DisplayName("อาชีพ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateCareer { get; set; }

        [DisplayName("ตำแหน่งทางการเมือง")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MatePolitical { get; set; }

        [DisplayName("โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateTelephone { get; set; }

        [DisplayName("สถานที่ทำงาน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateWorkplace { get; set; }

        [DisplayName("ประเภทธุรกิจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string MateBusinessType { get; set; }

        [DisplayName("ชื่อ-นามสกุล")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyPersonName { get; set; }

        [DisplayName("ความสัมพันธ์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyRelationship { get; set; }

        [DisplayName("ที่อยู่")]
        [StringLength(500, ErrorMessage = "Over length character")]
        public string EmergencyAddress { get; set; }

        [DisplayName("โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string EmergencyTelephone { get; set; }


        [DisplayName("ชื่อผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerName { get; set; }

        [DisplayName("เลขที่บัตรประชาชนผู้รับมอบอำนาจ")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "เฉพาะตัวเลข 13 หลัก")]
        public string AuthorizerId { get; set; }

        [DisplayName("ที่อยู่ผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerAddress { get; set; }

        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerTel { get; set; }
    }

    public class MemberCorporateViewModels
    {
        public System.Guid MemberId { get; set; }

        [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
        public string MemberRef { get; set; }


        [DisplayName("ประเภทสมาชิก")]
        public string MemberType { get; set; }

        [DisplayName("เลเวล")]
        public Nullable<int> MemberLevel { get; set; }

        [DisplayName("กลุ่มสมาชิก")]
        [Required(ErrorMessage = "Required")]
        public Nullable<int> MemberGroup { get; set; }

        [DisplayName("เจ้าหน้าที่การตลาด *")]
        [Required(ErrorMessage = "Required")]
        public string MarketingName { get; set; }

        [DisplayName("ชื่อนิติบุคคล *")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "ความยาวอย่างน้อย 4 ตัวอักษร และมากสุด 50 ตัวอักษร")]
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }

        [DisplayName("ที่อยู่ *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(500, ErrorMessage = "Over length character")]
        public string PresentAddress { get; set; }

        [DisplayName("เลขที่บัตรประชาชน *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "เฉพาะตัวเลข 13 หลัก")]
        [DataType(DataType.Currency)]
        public string PassportId { get; set; }



        [DisplayName("โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Telephone { get; set; }


        [DisplayName("โทรสาร")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Fax { get; set; }

        [DisplayName("อีเมล์ *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Over length character")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("ประเภทธุรกิจ *")]
        [Required(ErrorMessage = "Required")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateBusinessType { get; set; }

        [DisplayName("เลขทะเบียนนิติบุคคล/เลขทะเบียนการค้า")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateTradeRegNo { get; set; }

        [DisplayName("เลขประจำตัวผู้เสียภาษี (ภ.พ.20)")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateTaxNo { get; set; }




        [DisplayName("ผู้มอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string HasAuthorizer { get; set; }

        [DisplayName("สัญชาติ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string Nation { get; set; }

        [DisplayName("ผู้มีอำนาจลงนาม/กรรมการบริษัท")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporatePresidentSigned { get; set; }

        [DisplayName("ผู้ติดต่อ/ตัวแทนบริษัท")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateContactName { get; set; }

        [DisplayName("รายรับรวมต่อเดือน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string IncomeMonth { get; set; }


        //Share Holder
        [DisplayName("ผู้ถือหุ้นคนที่ 1")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolder1 { get; set; }

        [DisplayName("สัดส่วนการถือหุ้นคนที่ 1")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolderRate1 { get; set; }


        [DisplayName("ผู้ถือหุ้นคนที่ 2")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolder2 { get; set; }

        [DisplayName("สัดส่วนการถือหุ้นคนที่ 2")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolderRate2 { get; set; }


        [DisplayName("ผู้ถือหุ้นคนที่ 3")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolder3 { get; set; }

        [DisplayName("สัดส่วนการถือหุ้นคนที่ 3")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolderRate3 { get; set; }


        [DisplayName("ผู้ถือหุ้นคนที่ 4")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolder4 { get; set; }

        [DisplayName("สัดส่วนการถือหุ้นคนที่ 4")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolderRate4 { get; set; }


        [DisplayName("ผู้ถือหุ้นคนที่ 5")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolder5 { get; set; }

        [DisplayName("สัดส่วนการถือหุ้นคนที่ 5")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string CorporateShareHolderRate5 { get; set; }


        //authorizer
        [DisplayName("ชื่อผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerName { get; set; }

        [DisplayName("เลขที่บัตรประชาชนผู้รับมอบอำนาจ")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "เฉพาะตัวเลข 13 หลัก")]
        public string AuthorizerId { get; set; }

        [DisplayName("ที่อยู่ผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerAddress { get; set; }

        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerTel { get; set; }

        [DisplayName("อีเมล์ ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AuthorizerEmail{ get; set; }

        //Attorney
        [DisplayName("ชื่อผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AttorneyName { get; set; }

        [DisplayName("เลขที่บัตรประชาชนผู้รับมอบอำนาจ")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "เฉพาะตัวเลข 13 หลัก")]
        public string AttorneyId { get; set; }

        [DisplayName("ที่อยู่ผู้รับมอบอำนาจ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AttorneyAddress { get; set; }

        [DisplayName("เบอร์โทรศัพท์")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AttorneyTel { get; set; }

        [DisplayName("อีเมล์ ")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string AttorneyEmail { get; set; }


        //Bank

        [DisplayName("ธนาคาร")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankName { get; set; }

        [DisplayName("ชื่อบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankAccountName { get; set; }

        [DisplayName("เลขที่บัญชี")]
        [StringLength(10, ErrorMessage = "เลขที่บัญชีเฉพาะตัวเลข 10 หลัก")]
        public string BankAccountNo { get; set; }

        [DisplayName("ประเภทบัญชี")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankAccountType { get; set; }

        [DisplayName("สาขา")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankBranch { get; set; }

        [DisplayName("วิธีการชำระเงิน")]
        [StringLength(50, ErrorMessage = "Over length character")]
        public string BankPayType { get; set; }

        //public string CorporateContactPassportId { get; set; }
        //public string CorporateContactTelephone { get; set; }
        //public string CorporateContactEmail { get; set; }
        //public string CorporateHomeNo { get; set; }
        //public string CorporateHomeSoi { get; set; }
        //public string CorporateHomeStreet { get; set; }
        //public string CorporateHomeSubDistrict { get; set; }
        //public string CorporateHomeDistrict { get; set; }
        //public string CorporateHomeProvince { get; set; }
        //public string CorporateHomePostcode { get; set; }
        //public string CorporateTelephone { get; set; }
        //public string CorporateFax { get; set; }

    }

    public class MemberEditRiskViewModels
    {
        public System.Guid MemberId { get; set; }

        [DisplayName("เลเวล")]
        [Required(ErrorMessage = "Required")]
        public int MemberLevel { get; set; }

     

    }


    public class MemberCallForceViewModels
    {
        public System.Guid MemberId { get; set; }

        public string CallForce { get; set; }
        public bool ForceBuy { get; set; }
        public bool ForceSell { get; set; }
        public Member MemberDetail { get; set; }
        public PortFolioViewModels PortFolio { get; set; }
    }

    public class MemberCallForcePauseViewModels
    {
        public System.Guid MemberId { get; set; }

        public string PauseType { get; set; }
        public bool Paused { get; set; }
        public Member MemberDetail { get; set; }
        public PortFolioViewModels PortFolio { get; set; }
    }

    public class MemberSettingViewModels
    {
        public System.Guid MemberId { get; set; }

        public Member MemberDetail { get; set; }
        public PortFolioViewModels PortFolio { get; set; }
    }

    //public class PortFolioViewModels
    //{
    //    public Guid MemberId { get; set; }

    //    [DisplayName("เลขที่บัญชีซื้อขายทองคำ")]
    //    public string MemberRef { get; set; }

    //    [DisplayName("ประเภทสมาชิก")]
    //    public string MemberType { get; set; }

    //    [DisplayName("กลุ่มสมาชิก")]
    //    public string MemberGroup { get; set; }

    //    [DisplayName("คำนำหน้าชื่อ")]
    //    public string MemberTitle { get; set; }

    //    [DisplayName("เลเวล")]
    //    public string MemberLevel { get; set; }

    //    [DisplayName("ชื่อสมาชิก")]
    //    public string FirstName { get; set; }

    //    [DisplayName("นามสกุล")]
    //    public string LastName { get; set; }

    //    [DisplayName("อีเมล์")]
    //    public string Email { get; set; }


    //    [DisplayName("Duedate T+")]
    //    public double DuedateValue { get; set; }


    //    [DisplayName("ซื้อขายได้สูงสุดต่อครั้ง (Kg)")]
    //    public double MaxKg { get; set; }


    //    [DisplayName("Credit Limit (Kg)")]
    //    public double CreditLimit { get; set; }


    //    [DisplayName("Margin Type")]
    //    public string MarginStatus { get; set; }

    //    public double MarginValue { get; set; }



    //    [DisplayName("ปริมาณที่ซื้อ/ขายได้ (Kg)")]
    //    public decimal CreditLine { get; set; }


    //    [DisplayName("ปริมาณที่ลูกค้าซื้อได้คงเหลือ (Kg)")]
    //    public decimal CreditBuyBalance { get; set; }


    //    [DisplayName("ปริมาณที่ลูกค้าขายได้คงเหลือ (Kg)")]
    //    public decimal CreditSellBalance { get; set; }


    //    [DisplayName("จำนวนการซื้อทองคำล่วงหน้า (Kg)")]
    //    public decimal SumBuyPlaceOrder { get; set; }


    //    [DisplayName("จำนวนการขายทองคำล่วงหน้า (Kg)")]
    //    public decimal SumSellPlaceOrder { get; set; }


    //    [DisplayName("จำนวนการซื้อทองคำ (Kg)")]
    //    public decimal SumBuy { get; set; }


    //    [DisplayName("จำนวนการขายทองคำ (Kg)")]
    //    public decimal SumSell { get; set; }


    //    [DisplayName("เงินหลักประกัน (THB)")]
    //    public decimal AssetCash { get; set; }


    //    [DisplayName("ทองหลักประกัน (Kg)")]
    //    public decimal AssetGold { get; set; }


    //    [DisplayName("ทองฝาก (Kg)")]
    //    public double DepositGold { get; set; }


    //    [DisplayName("สถานะการซื้อ")]
    //    public bool PauseBuy { get; set; }


    //    [DisplayName("สถานะการขาย")]
    //    public bool PauseSell { get; set; }

    //    public bool WithdrawAble { get; set; }

    //    //For Calculate,not display
    //    public decimal AssetTradeAble { get; set; }

    //    //For Calculate,not display ,for check case ขายทองฝาก
    //    public decimal QuantityBalanceSellGoldDep { get; set; }

    //    public List<Ticket> TicketPendingList { get; set; }
    //    public List<Ticket> TicketCompleteList { get; set; }
    //    public List<Transfer> TransferList { get; set; }
    //}
}