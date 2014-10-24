using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HSH.Data.Models;
using HSH.Data.Helper;

namespace HSH.Data.Business
{

    public class BusinessService
    {
        private HSHEntities db = new HSHEntities();

        public TicketPayDetailViewModels getTicketPayDetails(Guid TicketId)
        {
            var tk = db.Ticket.Where(w => w.TicketId == TicketId).FirstOrDefault();
            var tkd = new TicketPayDetailViewModels();
            tkd.TicketId = tk.TicketId;
            tkd.TicketType = tk.TicketType;
            tkd.TicketRef = tk.TicketRef;
            tkd.MemberName = tk.Member.FirstName;
            tkd.MemberRef = tk.Member.MemberRef;
            tkd.Price = tk.Price.Value;
            tkd.Quantity = tk.Quantity.Value;
            tkd.Amount = tk.Amount.Value;
            tkd.UseDeposit = tk.UseDeposit;
            var rec = db.TicketPay.Where(w => w.Active == true && w.TicketId == TicketId).ToList();
            tkd.AmountPaid = rec.Where(q => q.Type == EnumHelper.TicketPayType.Partial.ToString()).Sum(s => s.Amount).Value;
            tkd.QtyPaid = rec.Sum(s => s.Quantity).Value;
            //double QtyPaid = rec.Sum(s => s.Quantity).Value;

            tkd.QtyUnpaid = tk.Quantity.Value - rec.Sum(s => s.Quantity).Value;
            tkd.AmtUnpaid = tk.Amount.Value - tkd.AmountPaid;
            //double AmtPerQty = tkd.Amount / tkd.Quantity;
            //tkd.Amtq = AmtPerQty;
            //tkd.AmtQty = tkd.AmountPaid - (AmtPerQty * QtyPaid);
            //if (AmtPerQty >= tkd.AmountPaid)
            //{
            //    tkd.MessageWarning = "Split over limit value. Please check partial payment";
            //}
            //if (tkd.AmtUnpaid == 0)
            //{
            //    tkd.MessageWarning = "Split Complete";
            //}
            return tkd;
        }

        public TicketReceiveDetailViewModels getTicketReceiveDetails(Guid TicketId)
        {
            var tk = db.Ticket.Where(w => w.TicketId == TicketId).FirstOrDefault();
            var tkd = new TicketReceiveDetailViewModels();
            tkd.TicketId = tk.TicketId;
            tkd.TicketType = tk.TicketType;
            tkd.TicketRef = tk.TicketRef;
            tkd.MemberName = tk.Member.FirstName;
            tkd.MemberRef = tk.Member.MemberRef;
            tkd.Price = tk.Price.Value;
            tkd.Quantity = tk.Quantity.Value;
            tkd.Amount = tk.Amount.Value;
            tkd.UseDeposit = tk.UseDeposit;
            var rec = db.TicketReceive.Where(w => w.Active == true && w.TicketId == TicketId).ToList();
            tkd.AmountPaid = rec.Where(q => q.Type == EnumHelper.TicketReceiveType.Partial.ToString()).Sum(s => s.Amount).Value;
            tkd.QtyPaid = rec.Sum(s => s.Quantity).Value;
            //double QtyPaid = rec.Sum(s => s.Quantity).Value;

            tkd.QtyUnpaid = tk.Quantity.Value - rec.Sum(s => s.Quantity).Value;
            tkd.AmtUnpaid = tk.Amount.Value - tkd.AmountPaid;
            //double AmtPerQty = tkd.Amount / tkd.Quantity;
            //tkd.Amtq = AmtPerQty;
            //tkd.AmtQty = tkd.AmountPaid - (AmtPerQty * QtyPaid);
            //if (AmtPerQty >= tkd.AmountPaid)
            //{
            //    tkd.MessageWarning = "Split over limit value. Please check partial payment";
            //}
            //if (tkd.AmtUnpaid == 0)
            //{
            //    tkd.MessageWarning = "Split Complete";
            //}

            return tkd;
        }

        //For Ticket Receive
        public string checkSplitAble(Guid TicketId, double Qty,string Status)
        {
            var tk = db.Ticket.Where(w => w.TicketId == TicketId).FirstOrDefault();
            var tkd = new TicketReceiveDetailViewModels();

            tkd.Quantity = tk.Quantity.Value;
            tkd.Amount = tk.Amount.Value;
            var rec = db.TicketReceive.Where(w => w.TicketId == TicketId).ToList();
            tkd.AmountPaid = rec.Where(q => q.Active == true && q.Type == EnumHelper.TicketReceiveType.Partial.ToString()).Sum(s => s.Amount).Value;
            double qtyPaid = rec.Sum(s => s.Quantity).Value;
            tkd.QtyUnpaid = tk.Quantity.Value - rec.Sum(s => s.Quantity).Value;
            tkd.AmtUnpaid = tk.Amount.Value - tkd.AmountPaid;
            double AmtPerQty = tkd.Amount / tkd.Quantity;

            var amtTemp = Qty * AmtPerQty;
            var amtPaidTemp = tkd.AmountPaid - (AmtPerQty * qtyPaid);

            if (tkd.QtyUnpaid == 0)
            {
                return "ตัดทองเรียบร้อยแล้ว";
            }
            if (Qty > tkd.Quantity)
            {
                return "ตัดทองเกินจำนวน";
            }

            if (Status == EnumHelper.TicketBuyStatus.WaitGold.ToString())
            {
                if (Qty > (tkd.Quantity - qtyPaid))
                {
                    return "ตัดทองเกินจำนวนที่ชำระเงิน โปรดดูรายละเอียดที่การรับชำระเงิน";
                }
                else
                {
                    return "";
                }
            }

            //over limit ,pay must more amount 1 kg(ที่จ่ายแล้วต้องมากกว่า 1kg)
            if (Qty > (tkd.Quantity - qtyPaid) || tkd.AmountPaid < AmtPerQty)
            {
                return "ตัดทองเกินจำนวนที่ชำระเงิน โปรดดูรายละเอียดที่การรับชำระเงิน";
            }
            else
            {
                if (amtTemp > amtPaidTemp)
                {
                    return "ตัดทองเกินจำนวนที่ชำระเงิน";
                }
            }

            return string.Empty;
        }

        //For Ticket Pay
        public string checkPaySplitAble(Guid TicketId, double Qty, string Status)
        {
            var tk = db.Ticket.Where(w => w.TicketId == TicketId).FirstOrDefault();
            var tkd = new TicketPayDetailViewModels();

            tkd.Quantity = tk.Quantity.Value;
            tkd.Amount = tk.Amount.Value;
            var rec = db.TicketPay.Where(w => w.TicketId == TicketId).ToList();
            tkd.AmountPaid = rec.Where(q => q.Active == true && q.Type == EnumHelper.TicketPayType.Partial.ToString()).Sum(s => s.Amount).Value;
            double qtyPaid = rec.Sum(s => s.Quantity).Value;
            tkd.QtyUnpaid = tk.Quantity.Value - rec.Sum(s => s.Quantity).Value;
            tkd.AmtUnpaid = tk.Amount.Value - tkd.AmountPaid;
            double AmtPerQty = tkd.Amount / tkd.Quantity;

            var amtTemp = Qty * AmtPerQty;
            var amtPaidTemp = tkd.AmountPaid - (AmtPerQty * qtyPaid);

            if (tkd.QtyUnpaid == 0)
            {
                return "ตัดทองเรียบร้อยแล้ว";
            }
            if (Qty > tkd.Quantity)
            {
                return "ตัดทองเกินจำนวน";
            }

            if (Status == EnumHelper.TicketBuyStatus.WaitGold.ToString())
            {
                if (Qty > (tkd.Quantity - qtyPaid))
                {
                    return "ตัดทองเกินจำนวนที่ชำระเงิน โปรดดูรายละเอียดที่การรับชำระเงิน";
                }
                else
                {
                    return "";
                }
            }

            //over limit ,pay must more amount 1 kg(ที่จ่ายแล้วต้องมากกว่า 1kg)
            if (Qty > (tkd.Quantity - qtyPaid) || tkd.AmountPaid < AmtPerQty)
            {
                return "ตัดทองเกินจำนวนที่ชำระเงิน โปรดดูรายละเอียดที่การรับชำระเงิน";
            }
            else
            {
                if (amtTemp > amtPaidTemp)
                {
                    return "ตัดทองเกินจำนวนที่ชำระเงิน";
                }
            }

            return string.Empty;
        }
        public MemberCallForceViewModels getCallForce(Guid MemberId)
        {
            var mkPrice = getMarketPrice();

            var port = getPortFolio(MemberId);
            decimal bidPrice = 0; decimal askPrice = 0;
            var mcf = new MemberCallForceViewModels();
            mcf.CallForce = EnumHelper.CallForce.Normal.ToString();
            mcf.ForceBuy = false; mcf.ForceSell = false;

            if (port.AssetTradeAble != 0)
            {
                try
                {
                    var ticket = db.Ticket.Where(w => w.Active == true && w.ApproveId == null && w.MemberId == MemberId).ToList();
                    if (ticket.Count > 0)
                    {
                        foreach (var item in ticket)
                        {//Ask == buy,Bid = sell
                            if (item.TicketType == EnumHelper.TicketType.Sell.ToString())
                            {
                                if (Convert.ToDecimal(item.Price) > mkPrice.Bid99Bg)
                                {
                                    bidPrice += ((Convert.ToDecimal(item.Price) - mkPrice.Bid99Bg) * Convert.ToDecimal(item.Quantity));
                                }
                            }
                            else
                            {
                                if (Convert.ToDecimal(item.Price) < mkPrice.Ask99Bg)
                                {
                                    askPrice += ((mkPrice.Ask99Bg - Convert.ToDecimal(item.Price)) * Convert.ToDecimal(item.Quantity));
                                }
                            }
                        }
                        decimal loss; decimal lossPercent;
                        if (bidPrice != Convert.ToDecimal(0))
                        {
                            loss = port.AssetTradeAble - (bidPrice * Convert.ToDecimal(65.6));
                            lossPercent = loss / port.AssetTradeAble;
                            if (lossPercent <= Convert.ToDecimal(0.5))
                            {
                                mcf.ForceBuy = true;
                                mcf.CallForce = EnumHelper.CallForce.Call.ToString();
                                if (lossPercent <= Convert.ToDecimal(0.3))
                                {
                                    mcf.CallForce = EnumHelper.CallForce.Force.ToString();
                                }
                            }
                        }
                        if (askPrice != Convert.ToDecimal(0))
                        {
                            loss = port.AssetTradeAble - (askPrice * Convert.ToDecimal(65.6));
                            lossPercent = loss / port.AssetTradeAble;
                            if (lossPercent <= Convert.ToDecimal(0.5))
                            {
                                mcf.ForceSell = true;
                                mcf.CallForce = EnumHelper.CallForce.Call.ToString();
                                if (lossPercent <= Convert.ToDecimal(0.3))
                                {
                                    mcf.CallForce = EnumHelper.CallForce.Force.ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return mcf;
        }

        public TransferGoldViewModels getSumTransfer(Guid MemberId)
        {
            try
            {
                var tf = new TransferGoldViewModels();
                tf.Quantity = 0;
                var sumGold = db.ViewOfSummaryTransfer.Where(w => w.MemberId == MemberId);
                if (sumGold.Count() > 0)
                {
                    tf.Quantity = sumGold.SingleOrDefault().Quantity.Value;
                }
                return tf;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CompanyViewModels getSummaryStockBalance()
        {
            try
            {
                var com = new CompanyViewModels();

                //Member Gold (Member Transfer)
                double sumTransferGoldIn = db.Transfer.Where(q => q.Active == true &&
                    q.Direction == EnumHelper.DirectionAssetType.Deposit.ToString() &&
                    q.TransferType == EnumHelper.TransferType.Gold.ToString()).Sum(q => q.Quantity) ?? 0;
                double sumTransferGoldOut = db.Transfer.Where(q => q.Active == true &&
                    q.Direction == EnumHelper.DirectionAssetType.Withdraw.ToString() &&
                    q.TransferType == EnumHelper.TransferType.Gold.ToString()).Sum(q => q.Quantity) ?? 0;
                com.TransferGoldDep = sumTransferGoldIn - sumTransferGoldOut;

                //Company Gold (Import/Export)
                double sumImportExportGoldIn = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString() && q.Direction == EnumHelper.CompanyAssetType.Import.ToString()).Sum(q => q.Quantity) ?? 0;
                double sumImportExportGoldOut = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString() && q.Direction == EnumHelper.CompanyAssetType.Export.ToString()).Sum(q => q.Quantity) ?? 0;
                com.ImportExportGoldDep = sumImportExportGoldIn - sumImportExportGoldOut;

                com.CompanyGold = com.TransferGoldDep + com.ImportExportGoldDep;

                double sumCashIn = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Cash.ToString() &&
                    q.Direction == EnumHelper.CompanyAssetType.Import.ToString()).Sum(q => q.Amount) ?? 0;

                double sumCashOut = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Cash.ToString() &&
                    q.Direction == EnumHelper.CompanyAssetType.Export.ToString()).Sum(q => q.Amount) ?? 0;
                com.CompanyCash = sumCashIn - sumCashOut;

                //Gold Colleral
                decimal sumGoldColIn = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                    q.Direction == EnumHelper.DirectionAssetType.In.ToString() &&
                    q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;

                decimal sumGoldColOut = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                   q.Direction == EnumHelper.DirectionAssetType.Out.ToString() &&
                   q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;

                com.CollateralGold = Convert.ToDouble(sumGoldColIn - sumGoldColOut);


                //Cash Colleral
                decimal sumCashColIn = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                   q.Direction == EnumHelper.DirectionAssetType.In.ToString() &&
                   q.AssetType == EnumHelper.AssetType.Cash.ToString()).Sum(q => q.AssetCash) ?? 0;

                decimal sumCashColOut = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                   q.Direction == EnumHelper.DirectionAssetType.Out.ToString() &&
                   q.AssetType == EnumHelper.AssetType.Cash.ToString()).Sum(q => q.AssetCash) ?? 0;
                com.CollateralCash = Convert.ToDouble(sumCashColIn - sumCashColOut);
                return com;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StockSummaryGoldViewModels getSummaryStockGoldBalance()
        {
            try
            {
                var stock = new StockSummaryGoldViewModels();

                //Member Gold (Member Transfer)
                stock.MemGoldBegin = 0;
                stock.MemGoldIn = db.Transfer.Where(q => q.Active == true &&
                    q.Direction == EnumHelper.DirectionAssetType.Deposit.ToString()).Sum(q => q.Quantity) ?? 0;
                stock.MemGoldOut = db.Transfer.Where(q => q.Active == true &&
                    q.Direction == EnumHelper.DirectionAssetType.Withdraw.ToString()).Sum(q => q.Quantity) ?? 0;
                stock.MemGoldSum = stock.MemGoldBegin + stock.MemGoldIn - stock.MemGoldOut;

                //Company Gold (Import/Export)
                stock.ComGoldBegin = 0;
                stock.ComGoldIn = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString() && q.Direction == EnumHelper.CompanyAssetType.Import.ToString()).Sum(q => q.Quantity) ?? 0;
                stock.ComGoldOut = db.CompanyAsset.Where(q => q.Active == true && q.AssetType == EnumHelper.AssetType.Gold.ToString() && q.Direction == EnumHelper.CompanyAssetType.Export.ToString()).Sum(q => q.Quantity) ?? 0;
                stock.ComGoldSum = stock.ComGoldBegin + stock.ComGoldIn - stock.ComGoldOut;


                //Gold Colleral
                stock.ColGoldBegin = 0;
                decimal sumGoldColIn = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                    q.Direction == EnumHelper.DirectionAssetType.In.ToString() &&
                    q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;
                stock.ColGoldIn = Convert.ToDouble(sumGoldColIn);

                decimal sumGoldColOut = db.MemberAsset.Where(q => q.Active == true && q.ApproveId != null &&
                   q.Direction == EnumHelper.DirectionAssetType.Out.ToString() &&
                   q.AssetType == EnumHelper.AssetType.Gold.ToString()).Sum(q => q.AssetGold) ?? 0;
                stock.ColGoldOut = Convert.ToDouble(sumGoldColOut);
                stock.ColGoldSum = stock.ColGoldBegin + stock.ColGoldIn - stock.ColGoldOut;

                stock.PhyGoldBegin = stock.MemGoldBegin + stock.ComGoldBegin + stock.ColGoldBegin;
                stock.PhyGoldIn = stock.MemGoldIn + stock.ComGoldIn + stock.ColGoldIn;
                stock.PhyGoldOut = stock.MemGoldOut + stock.ComGoldOut + stock.ColGoldOut;
                stock.PhyGoldSum = stock.PhyGoldBegin + stock.PhyGoldIn - stock.PhyGoldOut;

                return stock;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MarketPriceViewModels getMarketPrice()
        {
            MarketPriceViewModels mp = new MarketPriceViewModels();
            var spot = db.SpotPrice.FirstOrDefault();
            var stock = db.StockOnline.FirstOrDefault();

            //Set Value
            mp.Premium = stock.Premium.Value;
            mp.Discount = stock.Discount.Value;
            mp.SystemHalt = stock.SystemHalt.Value;

            mp.Spread = stock.Spread.Value;
            mp.Spread1 = stock.Spread1.Value;
            mp.Spread2 = stock.Spread2.Value;
            mp.Spread3 = stock.Spread3.Value;
            mp.Spread4 = stock.Spread4.Value;

            //spread change price all row

            mp.MaxKg = stock.MaxKg.Value;
            mp.MaxKg1 = stock.MaxKg1.Value;
            mp.MaxKg2 = stock.MaxKg2.Value;
            mp.MaxKg3 = stock.MaxKg3.Value;
            mp.MaxKg4 = stock.MaxKg4.Value;

            mp.Duedate1 = stock.Duedate1.Value;
            mp.Duedate2 = stock.Duedate2.Value;
            mp.Duedate3 = stock.Duedate3.Value;
            mp.Duedate4 = stock.Duedate4.Value;

            mp.MarginType1 = stock.MarginType1.Value;
            mp.MarginType2 = stock.MarginType2.Value;
            mp.MarginType3 = stock.MarginType3.Value;
            mp.MarginType4 = stock.MarginType4.Value;

            mp.AskSpot = spot.Ask.Value;
            mp.BidSpot = spot.Bid.Value;
            mp.AskThb = spot.AskThb.Value;
            mp.BidThb = spot.BidThb.Value;

            mp.SpotCalculate = stock.SpotCalculate;
            mp.ThbCalculateValue = Convert.ToDouble(stock.AskThbSelf.Value);//ค่าเงินบาท ใช้จาก User


            //HSH เลือกได้ว่าจะใช้ ask หรือ bid คำนวน
            if (mp.SpotCalculate == Helper.EnumHelper.SpotCalculate.Ask.ToString())
            {
                //Ask Solution step
                //1 Ask Kg = spot + premium*fx*32.148
                //2 Ask Bg = Ask Kg/65.6 (ลงท้ายด้วย 5,10)
                //3 Bid Bg = Ask Bg - Spread
                //4 Bid Kg = Bid Bg * 65.6
                mp.SpotCalculateValue = mp.AskSpot;//set spot value
                mp.Ask99Kg = Math.Round((mp.SpotCalculateValue + mp.Premium) * Convert.ToDecimal(mp.ThbCalculateValue) * Convert.ToDecimal(32.148), 2);
                decimal ask99BgTemp = mp.Ask99Kg / Convert.ToDecimal(65.6);
                decimal mod99bg = ask99BgTemp % Convert.ToDecimal(5);
                if (mod99bg < Convert.ToDecimal(2.5))//ลงท้ายด้วย 0,5
                {
                    mp.Ask99Bg = ask99BgTemp - mod99bg;
                }
                else
                {
                    mp.Ask99Bg = ask99BgTemp - mod99bg + Convert.ToDecimal(5);
                }
                mp.Bid99Bg = mp.Ask99Bg - Convert.ToDecimal(mp.Spread1);
                mp.Bid99Kg = Math.Round(mp.Bid99Bg * Convert.ToDecimal(65.6), 4);

                //Set Level
                mp.Ask99Bg1 = Convert.ToDouble(mp.Ask99Bg);
                mp.Bid99Bg1 = mp.Ask99Bg1 - mp.Spread1;

                mp.Ask99Bg2 = mp.Bid99Bg1 + mp.Spread2;
                mp.Bid99Bg2 = mp.Bid99Bg1;

                mp.Ask99Bg3 = mp.Bid99Bg1 + mp.Spread3;
                mp.Bid99Bg3 = mp.Bid99Bg1;

                mp.Ask99Bg4 = mp.Bid99Bg1 + mp.Spread4;
                mp.Bid99Bg4 = mp.Bid99Bg1;
            }
            else
            {
                //Bid Solution step
                //1 Bid Kg = spot - discount * fx * 32.148
                //2 Bid Bg = Bid Kg/65.6 (ลงท้ายด้วย 5,10)
                //3 Ask Bg = Bid Bg + Spread
                //4 Ask Kg = Ask Bg * 65.6
                mp.SpotCalculateValue = mp.BidSpot;//set spot value
                mp.Bid99Kg = Math.Round((mp.SpotCalculateValue - mp.Discount) * Convert.ToDecimal(mp.ThbCalculateValue) * Convert.ToDecimal(32.148), 2);
                decimal bid99BgTemp = mp.Bid99Kg / Convert.ToDecimal(65.6);
                decimal mod99bg = bid99BgTemp % Convert.ToDecimal(5);
                if (mod99bg < Convert.ToDecimal(2.5))
                {
                    mp.Bid99Bg = bid99BgTemp - mod99bg;
                }
                else
                {
                    mp.Bid99Bg = bid99BgTemp - mod99bg + Convert.ToDecimal(5);
                }
                mp.Ask99Bg = mp.Bid99Bg + Convert.ToDecimal(mp.Spread1);
                mp.Ask99Kg = Math.Round(mp.Ask99Bg * Convert.ToDecimal(65.6), 2);

                //Set Level
                mp.Bid99Bg1 = Convert.ToDouble(mp.Bid99Bg);
                mp.Ask99Bg1 = mp.Bid99Bg1 + mp.Spread1;

                mp.Bid99Bg2 = mp.Ask99Bg1 - mp.Spread2;
                mp.Ask99Bg2 = mp.Ask99Bg1;

                mp.Bid99Bg3 = mp.Ask99Bg1 - mp.Spread3;
                mp.Ask99Bg3 = mp.Ask99Bg1;

                mp.Bid99Bg4 = mp.Ask99Bg1 - mp.Spread4;
                mp.Ask99Bg4 = mp.Ask99Bg1;
            }

            return mp;
        }

        public string getMarketPriceMember(Guid MemberId)
        {
            try
            {
                string price = "";
                string priceFormat = "#,##";
                string split = "|";
                MarketPriceViewModels mp = new MarketPriceViewModels();
                mp = getMarketPrice();
                var mem = db.Member.Find(MemberId);
                if (mem != null)
                {

                    switch (mem.MemberLevel.Value)
                    {
                        case 1:
                            price = mp.Bid99Bg1.ToString(priceFormat) + split + mp.Ask99Bg1.ToString(priceFormat);
                            break;
                        case 2:
                            price = mp.Bid99Bg2.ToString(priceFormat) + split + mp.Ask99Bg2.ToString(priceFormat);
                            break;
                        case 3:
                            price = mp.Bid99Bg3.ToString(priceFormat) + split + mp.Ask99Bg3.ToString(priceFormat);
                            break;
                        case 4:
                            price = mp.Bid99Bg4.ToString(priceFormat) + split + mp.Ask99Bg4.ToString(priceFormat);
                            break;
                        default:
                            break;
                    }
                }
                return price;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    if (mb.MemberLevel == null)
                    {
                        pf.MarginStatus = "";
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

        public PortFolioViewModels getPortFolio(string MemberRef)
        {
            try
            {
                PortFolioViewModels pf = new PortFolioViewModels();
                var mb = db.Member.Where(m => m.Active == true && m.MemberRef == MemberRef).FirstOrDefault();
                ViewOfCreditLine cl = db.ViewOfCreditLine.Where(c => c.MemberRef == MemberRef).FirstOrDefault();
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

        public PortFolioViewModels CheckWithdrawAble(Guid memberId,string AssetType,double Qty)
        {
            try
            {
                PortFolioViewModels pf = new PortFolioViewModels();
                var mb = db.Member.Where(m => m.MemberId == memberId).FirstOrDefault();
                ViewOfCreditLine cl = db.ViewOfCreditLine.Where(c => c.MemberId == memberId).FirstOrDefault();
                decimal CashDeposit = cl.AssetCash;
                var stock = db.StockOnline.SingleOrDefault();
                pf.WithdrawAble = false;

                if (cl != null)
                {
                    pf.MarginValue = 0;//Qty trade
                    pf.CreditLimit = 0;//Qty trade

                    if (mb.MemberLevel != null)
                    {
                        switch (mb.MemberLevel.Value)
                        {
                            case 1:
                                pf.MarginValue = stock.MarginType1.Value;
                                pf.CreditLimit = stock.CreditLimit1.Value;
                                break;
                            case 2:
                                pf.MarginValue = stock.MarginType2.Value;
                                pf.CreditLimit = stock.CreditLimit2.Value;
                                break;
                            case 3:
                                pf.MarginValue = stock.MarginType3.Value;
                                pf.CreditLimit = stock.CreditLimit3.Value;
                                break;
                            case 4:
                                pf.MarginValue = stock.MarginType4.Value;
                                pf.CreditLimit = stock.CreditLimit4.Value;
                                break;

                            default:
                                break;
                        }
                    }

                    //Remove Asset for calculate
                    if (AssetType == EnumHelper.AssetType.Gold.ToString())
                    {
                        cl.AssetGold = cl.AssetGold - Convert.ToDecimal(Qty);
                        if (cl.AssetGold < 0)
                        {
                            pf.MessageWarning = "ไม่สามารถถอนทองได้ เนื่องจากถอนเกินจำนวนที่ฝากไว้";
                            pf.WithdrawAble = false;
                            return pf;
                        }
                    }
                    else if (AssetType == EnumHelper.AssetType.Cash.ToString())
                    {
                        cl.AssetCash = cl.AssetCash - Convert.ToDecimal(Qty);
                        if (cl.AssetCash < 0)
                        {
                            pf.MessageWarning = "ไม่สามารถถอนเงินได้ เนื่องจากถอนเกินจำนวนที่ฝากไว้";
                            pf.WithdrawAble = false;
                            return pf;
                        }
                    }
                   
                    

                    decimal assetGold = cl.AssetGold * cl.GoldPerKg.Value * Convert.ToDecimal(cl.GoldPercent.Value / 100);
                    pf.AssetTradeAble = assetGold + cl.AssetCash;

                    if (pf.MarginValue == 0)
                    {
                        //no calculate colateral
                        //pf.MarginStatus = "ไม่วางหลักประกัน";
                        pf.WithdrawAble = true;
                        return pf;
                    }
                    else
                    {
                        //calculate colateral
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

                  
                    //check withdraw able
                    if (pf.CreditBuyBalance < 0 || pf.CreditSellBalance < 0)
                    {
                        pf.WithdrawAble = false;
                        pf.MessageWarning = "ไม่สามารถถอนหลักประกันได้";
                    }
                    else
                    {
                        if (Qty < 5000 & Convert.ToDouble(CashDeposit) != Qty & AssetType == EnumHelper.AssetType.Cash.ToString())
                        {
                            pf.WithdrawAble = false;
                            pf.MessageWarning = "จำนวนขั้นต่ำในการถอนต้องมากกว่า 5,000 บาท";
                        }
                        else
                        {
                            pf.WithdrawAble = true;
                        }
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