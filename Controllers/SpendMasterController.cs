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
using Spend.Models;

namespace SupeaI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpendmasterController : ControllerBase
    {
        private MyContext _context;

        public SpendmasterController(MyContext Context)
        {
            this._context = Context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mst_Spend>>> GetTraIncome()
        {
            SpendModel spend = new SpendModel(this._context);
            return await spend.GetMst_SpendAsync();
        }
    }
}