namespace RestDoc.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Models.Data;
    using Models.View.Api;
    [RoutePrefix("api/restapi/{apiId:guid}/pathgroup")]
    public class PathGroupController : ApiController
    {
        private readonly RestDocContext _context;
        private readonly IMappingEngine _mappingEngine;

        public PathGroupController(RestDocContext context, IMappingEngine mappingEngine)
        {
            _context = context;
            _mappingEngine = mappingEngine;
        }


        [Route("")]
        public IEnumerable<PathGroupViewModel> Get(Guid apiId)
        {
            return
                _context.ApiPathGroups.Project(_mappingEngine).To<PathGroupViewModel>().Where(i => i.RestApi.Id == apiId);
        }
        [Route("{id:guid}", Name = "GetPathGroupById")]
        public async Task<IHttpActionResult> Get(Guid id, Guid apiId)
        {
            var result = await _context.ApiPathGroups.FindAsync(id);
            if (result == null || result.RestApiId != apiId) return NotFound();
            return Ok(_mappingEngine.Map<PathGroupViewModel>(result));
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(Guid apiId, [FromBody] PathGroupViewModel value)
        {
            if (value == null) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiPathGroups.FirstOrDefaultAsync(i => i.BasePath == value.BasePath && i.RestApiId == apiId);
                if (existingRecord != null)
                {
                    return Conflict();
                }

                existingRecord = _mappingEngine.Map<ApiPathGroup>(value);
                existingRecord.RestApiId = apiId;
                _context.ApiPathGroups.Add(existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);
                return
                    Created(Url.Route("GetPathGroupById",
                        new { id = value.Id, apiId }), value);
            }
        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Put(Guid apiId, Guid id, [FromBody] PathGroupViewModel value)
        {
            if (value == null || (value.Id != Guid.Empty && value.Id != id)) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                value.Id = id;

                var existingRecord = await _context.ApiPathGroups.FindAsync(id);
                if (existingRecord == null || existingRecord.RestApiId != apiId) return NotFound();
                _mappingEngine.Map(value, existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);

                return Ok(value);
            }
        }
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Delete(Guid apiId, Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiPathGroups.FindAsync(id);
                if (existingRecord == null || existingRecord.RestApiId != apiId) return NotFound();
                _context.Entry(existingRecord).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                transaction.Commit();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}