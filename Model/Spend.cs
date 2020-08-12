using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupeaI.Models;
using Json.Models;

namespace Spend.Models
{
    class SpendModel
    {

        private MyContext _context;

        public SpendModel(MyContext context)
        {
            this._context = context;
        }

        public async Task<List<Mst_Spend>> GetMst_SpendAsync()
        {
            return await this._context.Mst_Spend
                        .OrderBy(x => x.Id)
                        .ToListAsync();
        }

        public async Task<List<Tra_Spending>> GetSpendAsync()
        {
            return await this._context.Tra_Spending
                                    .Where(x => x.Money > 0)
                                    .Where(x => x.Del_Flag == false)
                                    .OrderBy(x => x.Purchase_Date)
                                    .OrderBy(x => x.Id)
                                    .ToListAsync();
        }

        public async Task<bool> InsertTraSpend(SpendData jsonSpend)
        {
            var spends = jsonSpend.Spend;

            if (spends.Count == 0) { return false; }

            List<Tra_Spending> insert = new List<Tra_Spending>();
            List<Tra_Spending> update = new List<Tra_Spending>();


            foreach (var spend in spends)
            {
                if (!this._context.Tra_Spending.Any(x => x.Created_At == spend.Created_At))
                {
                    insert.Add(spend); // 追加対象の追加
                }
                else if (this._context.Tra_Spending.Where(x => x.Created_At == spend.Created_At).Any(x => x.Updated_At < spend.Updated_At))
                {
                    update.Add(spend); // 更新対象の追加
                }
            }

            if (insert.Count == 0) { return false; }

            // 追加処理

            //this._context.Entry(this._context.Tra_Spending).State = EntityState.Added;

            foreach (var ins in insert)
            {
                this._context.Add(new Tra_Spending()
                {
                    User_Id = 1,
                    Spend_Id = ins.Spend_Id,
                    Money = ins.Money,
                    Purchase_Date = ins.Purchase_Date,
                    Description = ins.Description,
                    Linked_Flag = true,
                    Created_At = ins.Created_At,
                    Updated_At = ins.Updated_At,
                    Del_Flag = false
                });
            }

            await this._context.SaveChangesAsync();


            //this._context.Entry(this._context.Tra_Spending).State = EntityState.Modified;

            if (update.Count == 0) { return false; }
            this._context.Entry(update).State = EntityState.Modified;

            // 更新処理
            foreach (var upd in update)
            {
                var result = this._context.Tra_Spending.FirstOrDefault(x => x.Created_At == upd.Created_At);

                result.User_Id = upd.User_Id;
                result.Money = upd.Money;
                result.Linked_Flag = true;
                result.Purchase_Date = upd.Purchase_Date;
                result.Description = upd.Description;
                result.Spend_Id = upd.Spend_Id;
                result.Updated_At = upd.Updated_At;
                result.Del_Flag = upd.Del_Flag;
            }
            //this._context.Entry(update).State = EntityState.Modified;

            await this._context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTraSpend(SpendData jsonSpend)
        {
            bool result;
            try
            {
                this._context.Tra_Spending.UpdateRange(jsonSpend.Spend);
                await this._context.SaveChangesAsync();

                result = await UpdateLinkedFlagAsync();
            }
            catch
            {
                return false;
            }
            return result;

        }

        private async Task<bool> UpdateLinkedFlagAsync()
        {
            var target = await this._context.Tra_Spending
                            .Where(x => x.Linked_Flag == false)
                            .ToListAsync();
            foreach (var spend in target)
            {
                spend.Linked_Flag = true; // 連携済み(True)に更新
            }

            await this._context.SaveChangesAsync();

            return true;
        }
    }
}