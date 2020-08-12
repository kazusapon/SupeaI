using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SupeaI.Models;
using Json.Models;

namespace Income.Models
{
    class IncomeModel
    {

        private MyContext _context;

        public IncomeModel(MyContext context)
        {
            this._context = context;
        }

        public async Task<List<Mst_Income>> GetMst_IncomeAsync()
        {
            return await this._context.Mst_Income
                        .OrderBy(x => x.Id)
                        .ToListAsync();
        }

        public async Task<List<Tra_Income>> GetIncomesAsync()
        {
            return await this._context.Tra_Income
                                    .Where(x => x.Money > 0)
                                    .Where(x => x.Del_Flag == false)
                                    .OrderBy(x => x.Payment_Date)
                                    .OrderBy(x => x.Id)
                                    .ToListAsync();
        }

        public async Task<bool> InsertTraIncome(IncomeData jsonIncome)
        {
            var incomes = jsonIncome.Income;

            if (incomes.Count == 0) { return false; }

            List<Tra_Income> insert = new List<Tra_Income>();
            List<Tra_Income> update = new List<Tra_Income>();

            foreach (var income in incomes)
            {

                if (!this._context.Tra_Income.Any(x => x.Created_At == income.Created_At))
                {
                    insert.Add(income); // 追加対象の追加
                }
                else if (this._context.Tra_Income.Where(x => x.Created_At == income.Created_At).Where(x => x.Updated_At < income.Updated_At).Count() > 0)
                {
                    update.Add(income); // 更新対象の追加
                }
            }

            Console.WriteLine("ガンダム");
            foreach (var u in update)
            {
                Console.WriteLine(u.Del_Flag);
                Console.WriteLine(u.Linked_Flag);
            }

            if (insert.Count == 0) { return false; }

            // 追加処理
            //this._context.Entry(insert).State = EntityState.Added;
            foreach (var ins in insert)
            {
                this._context.Add(new Tra_Income()
                {
                    User_Id = 1,
                    Income_Id = ins.Income_Id,
                    Money = ins.Money,
                    Payment_Date = ins.Payment_Date,
                    Description = ins.Description,
                    Linked_Flag = true,
                    Created_At = ins.Created_At,
                    Updated_At = ins.Updated_At,
                    Del_Flag = false
                });
            }

            await this._context.SaveChangesAsync();

            if (update.Count == 0) { return false; }

            // 更新処理
            foreach (var upd in update)
            {
                var result = this._context.Tra_Income.FirstOrDefault(x => x.Created_At == upd.Created_At);

                result.User_Id = upd.User_Id;
                result.Money = upd.Money;
                result.Linked_Flag = true;
                result.Payment_Date = upd.Payment_Date;
                result.Description = upd.Description;
                result.Income_Id = upd.Income_Id;
                result.Updated_At = upd.Updated_At;
                result.Del_Flag = upd.Del_Flag;
            }

            //this._context.Entry(this._context.Tra_Income).State = EntityState.Modified;
            await this._context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateTraIncome(IncomeData jsonIncome)
        {
            bool result;
            try
            {
                this._context.Tra_Income.UpdateRange(jsonIncome.Income);
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
            var target = await this._context.Tra_Income
                            .Where(x => x.Linked_Flag == false)
                            .ToListAsync();
            foreach (var income in target)
            {
                income.Linked_Flag = true; // 連携済み(True)に更新
            }

            await this._context.SaveChangesAsync();

            return true;
        }
    }
}