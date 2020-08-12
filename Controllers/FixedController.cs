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

namespace Fixed.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedController : ControllerBase
    {
        private MyContext _context;

        public FixedController(MyContext Context)
        {
            this._context = Context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mst_Fixed>>> GetMstFiexd()
        {
            return await this._context.Mst_Fixed
                                        .Where(x => x.Del_Flag == false)
                                        .Where(x => x.Money > 0)
                                        .OrderBy(x => x.Id)
                                        .ToListAsync();
        }
    }
}