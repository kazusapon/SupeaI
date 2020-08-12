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
using Spend.Models;

namespace SupeaI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendController : ControllerBase
    {
        private MyContext _context;

        public SpendController(MyContext Context)
        {
            this._context = Context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tra_Spending>>> GetTraSpend()
        {
            SpendModel spend = new SpendModel(this._context);
            return await spend.GetSpendAsync();
        }


        [HttpPost]
        public async Task<bool> CreateTraSpend(SpendData json)
        {
            SpendModel spend = new SpendModel(this._context);
            //var spendObj = JsonSerializer.Deserialize<SpendData>(json); // JSONからオブジェクトにデシリアライズ
            //bool result = await spend.InsertTraSpend(spendObj);
            bool result = await spend.InsertTraSpend(json);

            return result;
        }

        [HttpPut]
        public async Task<bool> UpdateTraSpend([FromBody] string json)
        {
            SpendModel spend = new SpendModel(this._context);
            var spendObj = JsonSerializer.Deserialize<SpendData>(json); // JSONからオブジェクトにデシリアライズ
            
            bool result = await spend.UpdateTraSpend(spendObj);

            return result;
        }
    }
}