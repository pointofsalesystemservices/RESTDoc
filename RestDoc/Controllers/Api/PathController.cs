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

    [RoutePrefix("api/restapi/{apiId:guid}/pathgroup/{groupId:guid}/path")]
    public class PathController : ApiController
    {
        private readonly RestDocContext _context;
        private readonly IMappingEngine _mappingEngine;

        public PathController(RestDocContext context, IMappingEngine mappingEngine)
        {
            _context = context;
            _mappingEngine = mappingEngine;
        }
        [Route("")]
        public IEnumerable<PathViewModel> Get(Guid groupId)
        {
            return _context.ApiPaths.Where(i => i.ApiPathGroupId == groupId).Project(_mappingEngine).To<PathViewModel>();
        }

        [Route("{id:guid}", Name = "GetPathById")]
        public async Task<IHttpActionResult> Get(Guid id, Guid groupId)
        {
            var result = await _context.ApiPaths.FindAsync(id);
            if (result == null || result.ApiPathGroupId != groupId) return NotFound();
            return Ok(_mappingEngine.Map<PathViewModel>(result));
        }

        [Route("")]
        public async Task<IHttpActionResult> Post(Guid groupId, Guid apiId, [FromBody] PathViewModel value)
        {
            if (value == null) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiPaths.FirstOrDefaultAsync(i => i.ApiPathGroupId == groupId && i.Path == value.Path);
                if (existingRecord != null)
                {
                    return Conflict();
                }

                existingRecord = _mappingEngine.Map<ApiPath>(value);
                existingRecord.ApiPathGroupId = groupId;
                existingRecord.ApiPathGroup = _context.ApiPathGroups.Find(groupId);
                _context.ApiPaths.Add(existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();

                _mappingEngine.Map(existingRecord, value);
                return
                    Created(Url.Route("GetPathById",
                        new { id = value.Id, groupId, apiId }), value);
            }
        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Put(Guid groupId, Guid id, [FromBody] PathViewModel value)
        {
            if (value == null || (value.Id != Guid.Empty && value.Id != id)) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                value.Id = id;

                var existingRecord = await _context.ApiPaths.Include(i => i.ApiPathGroup).FirstOrDefaultAsync(i => i.Id == id && i.ApiPathGroupId == groupId);
                if (existingRecord == null) return NotFound();
                _mappingEngine.Map(value, existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);

                return Ok(value);
            }
        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Delete(Guid groupId, Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiPaths.FindAsync(id);
                if (existingRecord == null || existingRecord.ApiPathGroupId != groupId) return NotFound();
                _context.Entry(existingRecord).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                transaction.Commit();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}