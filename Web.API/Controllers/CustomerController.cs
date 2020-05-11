using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<List<Customer>> Get()
        {
            try
            {
                var data = _customerService.Get();
                return data;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        [HttpGet("{id:length(24)}")]
        [Route("customers/{id}")]
        public ActionResult<Customer> Get(string id)
        {
            var customer = _customerService.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
        [HttpPost]
        [Route("create")]
        public ActionResult<Customer> Create([FromBody]Customer customer)
        {
            var entity = _customerService.Create(customer);
            return entity;
        }
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, [FromBody]Customer customerIn)
        {
            var customer = _customerService.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            _customerService.Update(id, customerIn);
            return Ok();
        }
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var customer = _customerService.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            _customerService.Remove(customer.Id);

            return Ok();
        }
    }
}