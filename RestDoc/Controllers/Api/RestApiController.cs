using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestDoc.Controllers.Api
{
    using System.Data;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Models.Data;
    using Models.View.Api;

    public class RestApiController : ApiController
    {
        private readonly RestDocContext _context;
        private readonly IMappingEngine _mappingEngine;

        public RestApiController(RestDocContext context, IMappingEngine mappingEngine)
        {
            _context = context;
            _mappingEngine = mappingEngine;
        }

        // GET: api/RestApi
        public IEnumerable<RestApiViewModel> Get()
        {
            return _context.RestApis.Project(_mappingEngine).To<RestApiViewModel>();
        }

        // GET: api/RestApi/5
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var result = await _context.RestApis.FindAsync(id);
            if (result == null) return NotFound();
            return Ok(_mappingEngine.Map<RestApiViewModel>(result));
        }

        // POST: api/RestApi
        public async Task<IHttpActionResult> Post([FromBody]RestApiViewModel value)
        {
            if (value == null) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.RestApis.FirstOrDefaultAsync(i => i.Name == value.Name);
                if (existingRecord != null)
                {
                    return Conflict();
                }

                existingRecord = _mappingEngine.Map<RestApi>(value);
                existingRecord.LastModified = DateTimeOffset.Now;
                existingRecord.Created = DateTimeOffset.Now;
                existingRecord.Creator = User.Identity.Name;
                _context.RestApis.Add(existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);
                return
                    Created(Url.Route("DefaultApi",
                        new { controller = ControllerContext.ControllerDescriptor.ControllerName, id = value.Id }), value);
            }
        }


        // PUT: api/RestApi/5
        public async Task<IHttpActionResult> Put(Guid id, [FromBody]RestApiViewModel value)
        {
            if (value == null || (value.Id != Guid.Empty && value.Id != id)) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                value.Id = id;

                var existingRecord = await _context.RestApis.FindAsync(id);
                if (existingRecord == null) return NotFound();
                _mappingEngine.Map(value, existingRecord);
                existingRecord.LastModified = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);

                return Ok(value);
            }
        }

        // DELETE: api/RestApi/5
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.RestApis.FindAsync(id);
                if (existingRecord == null) return NotFound();
                _context.Entry(existingRecord).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                transaction.Commit();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}
