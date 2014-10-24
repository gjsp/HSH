using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSH.Data.Models;


namespace HSH.Shared.Business
{
    public class BusinessService
    {

        private HSHEntities db = new HSHEntities();
        public PortFolioViewModels getPortFolio(Guid memberId)
        {
            try
            {
                PortFolioViewModels pf = new PortFolioViewModels();
                var mb = db.Member.Where(m => m.MemberId == memberId).FirstOrDefault();
                ViewOfCreditLine cl = db.ViewOfCreditLine.Where(c => c.MemberId == memberId).FirstOrDefault();
                var stock = db.StockOnline.SingleOrDefault();

                if (cl != null)
                {
                    pf.MemberId = mb.MemberId;
                    pf.MemberLevel = mb.MemberLevel == null ? "" : mb.MemberLevel.Value.ToString();
                    pf.MemberGroup = mb.MemberGroup.ToString();
                    pf.MemberRef = mb.MemberRef;
                    pf.MemberTitle = mb.MemberTitle;
                    pf.MemberType = mb.MemberType;
                    pf.FirstName = mb.FirstName;
                    pf.LastName = mb.LastName;
                    pf.Email = mb.Email;

                    //set Limit
                    pf.DuedateValue = 0; //T+1,2,3
                    pf.MarginValue = 0;//Qty trade
                    pf.CreditLimit = 0;//Qty trade
                    pf.MaxKg = 0;//Qty

                    if (mb.MemberLevel != null)
                    {
                        switch (mb.MemberLevel.Value)
                        {
                            case 1:
                                pf.MaxKg = stock.MaxKg1.Value;
                                pf.DuedateValue = stock.Duedate1.Value;
                                pf.MarginValue = stock.MarginType1.Value;
                                pf.CreditLimit = stock.CreditLimit1.Value;
                                break;
                            case 2:
                                pf.MaxKg = stock.MaxKg2.Value;
                                pf.DuedateValue = stock.Duedate2.Value;
                                pf.MarginValue = stock.MarginType2.Value;
                                pf.CreditLimit = stock.CreditLimit2.Value;
                                break;
                            case 3:
                                pf.MaxKg = stock.MaxKg3.Value;
                                pf.DuedateValue = stock.Duedate3.Value;
                                pf.MarginValue = stock.MarginType3.Value;
                                pf.CreditLimit = stock.CreditLimit3.Value;
                                break;
                            case 4:
                                pf.MaxKg = stock.MaxKg4.Value;
                                pf.DuedateValue = stock.Duedate4.Value;
                                pf.MarginValue = stock.MarginType4.Value;
                                pf.CreditLimit = stock.CreditLimit4.Value;
                                break;

                            default:
                                break;
                        }
                    }

                    //cal credit line  Convert.ToDecimal(pf.MarginValue) *
                    decimal assetGold = cl.AssetGold * cl.GoldPerKg.Value * Convert.ToDecimal(cl.GoldPercent.Value / 100);
                    pf.AssetTradeAble = assetGold + cl.AssetCash;

                    //100k tradealbe 1kg --> 100k tradealbe 5kg
                    if (pf.MarginValue == 0)
                    {
                        //no calculate colateral
                        pf.MarginStatus = "ไม่วางหลักประกัน";
                        pf.CreditLine = 0;
                    }
                    else
                    {
                        //calculate colateral
                        pf.MarginStatus = cl.CashPerKg.Value.ToString("#,#00") + " : " + pf.MarginValue.ToString() + " Kg";
                        decimal tempAsset = pf.AssetTradeAble / (cl.CashPerKg.Value / Convert.ToDecimal(pf.MarginValue));
                        decimal modtempAsset = tempAsset % Convert.ToDecimal(1);
                        pf.CreditLine = tempAsset - modtempAsset;

                    }
                    double SumBuyQty = cl.BuyQuantity + cl.BuyQuantityLeave;
                    double SumSellQty = cl.SellQuantity + cl.SellQuantityLeave;

                    decimal CreditBuyBalance = pf.CreditLine - Convert.ToDecimal(SumBuyQty);
                    pf.CreditBuyBalance = CreditBuyBalance;
                    decimal CreditSellBalance = pf.CreditLine - Convert.ToDecimal(SumSellQty);
                    pf.CreditSellBalance = CreditSellBalance;

                    pf.SumBuyPlaceOrder = Convert.ToDecimal(cl.BuyQuantityLeave);
                    pf.SumSellPlaceOrder = Convert.ToDecimal(cl.SellQuantityLeave);
                    pf.SumBuy = Convert.ToDecimal(cl.BuyQuantity);
                    pf.SumSell = Convert.ToDecimal(cl.SellQuantity);

                    pf.AssetCash = cl.AssetCash;
                    pf.AssetGold = cl.AssetGold;
                    pf.DepositGold = cl.DepositGold;
                    pf.PauseBuy = mb.PauseBuy == null ? false : mb.PauseBuy.Value;
                    pf.PauseSell = mb.PauseSell == null ? false : mb.PauseSell.Value;
                    pf.QuantityBalanceSellGoldDep = Convert.ToDecimal(pf.DepositGold - cl.SellQuantityGoldDep);

                    //check withdraw able
                    pf.WithdrawAble = true;
                    if (pf.SumBuy > 0 || pf.SumSell > 0)
                    {
                        pf.WithdrawAble = false;
                    }
                }

                return pf;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}