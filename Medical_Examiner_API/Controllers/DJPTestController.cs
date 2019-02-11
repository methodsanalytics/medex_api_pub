﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Examiner_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DJPTestController : ControllerBase
    {
        // GET: api/DJPTest
        [HttpGet]
        [ServiceFilter(typeof(ControllerActionFilter))]
        public IEnumerable<string> Get()
        {
            return new string[] { "DJPvalue1", "DJPvalue2" };
        }

        // GET: api/DJPTest/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DJPTest
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/DJPTest/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
