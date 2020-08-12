using System;
using System.Text.Json;
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
    public class IncomemasterController : ControllerBase
    {
        private MyContext _context;

        public IncomemasterController(MyContext Context)
        {
            this._context = Context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mst_Income>>> GetTraIncome()
        {
            IncomeModel income = new IncomeModel(this._context);
            return await income.GetMst_IncomeAsync();
        }
    }
}