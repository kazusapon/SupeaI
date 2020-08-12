using System;
using System.Text.Json;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupeaI.Models;
using Json.Models;
using Income.Models;

namespace SupeaI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private MyContext _context;

        public IncomeController(MyContext Context)
        {
            this._context = Context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tra_Income>>> GetTraIncome()
        {
            IncomeModel income = new IncomeModel(this._context);
            return await income.GetIncomesAsync();
        }


        [HttpPost]
        public async Task<bool> CreateTraIncome(IncomeData json)
        {
            IncomeModel income = new IncomeModel(this._context);
            Console.WriteLine(json);
            //var incomeObj = JsonSerializer.Deserialize<IncomeData>(json); // JSONからオブジェクトにデシリアライズ
            //bool result = await income.InsertTraIncome(incomeObj);
            bool result = await income.InsertTraIncome(json);

            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateTraIncome([FromBody] string json)
        {
            IncomeModel income = new IncomeModel(this._context);
            var incomeObj = JsonSerializer.Deserialize<IncomeData>(json); // JSONからオブジェクトにデシリアライズ
            
            bool result = await income.UpdateTraIncome(incomeObj);

            return result;
        }
    }
}