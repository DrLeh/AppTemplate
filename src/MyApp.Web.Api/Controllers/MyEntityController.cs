using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyApp.Contracts;
using MyApp.Core.Models;
using MyApp.Core.MyEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Web.Api.Controllers
{
    [Produces("application/json")]
    [Route("myentities")]
    [ApiController]
    public class MyEntityController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMyEntityService _myEntityService;

        public MyEntityController(IMapper mapper, IMyEntityService myEntityService)
        {
            _mapper = mapper;
            _myEntityService = myEntityService;
        }

        [HttpGet]
        public MyEntityView Get(long id)
        {
            var model = _myEntityService.Get(id);
            var res = _mapper.Map<MyEntityView>(model);
            return res;
        }

        [HttpPost]
        public MyEntityView Post(MyEntityView view)
        {
            var model = _mapper.Map<MyEntity>(view);
            var created = _myEntityService.Add(model);
            var res = _mapper.Map<MyEntityView>(created);
            return res;
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult<MyEntityView> Put(long id, MyEntityView view)
        {
            if (id <= 0)
                return BadRequest();

            var model = _mapper.Map<MyEntity>(view);
            var updated = _myEntityService.Update(id, x =>
            {
                _mapper.Map(view, x);
            });
            var res = _mapper.Map<MyEntityView>(updated);
            return res;
        }
    }
}
