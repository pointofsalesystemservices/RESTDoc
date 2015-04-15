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

    [RoutePrefix("api/restapi/{apiId:guid}/pathgroup/{groupId:guid}/path/{pathId:guid}/verb")]
    public class VerbController : ApiController
    {
        private readonly RestDocContext _context;
        private readonly IMappingEngine _mappingEngine;

        public VerbController(RestDocContext context, IMappingEngine mappingEngine)
        {
            _context = context;
            _mappingEngine = mappingEngine;
        }

        [Route("")]
        public IEnumerable<VerbViewModel> Get(Guid pathId)
        {
            return _context.ApiVerbs.Where(i => i.ApiPathId == pathId).Project(_mappingEngine).To<VerbViewModel>();
        }

        [Route("{id:guid}", Name = "GetVerbById")]
        public async Task<IHttpActionResult> Get(Guid pathId, Guid id)
        {
            var result = await _context.ApiVerbs.FindAsync(id);
            if (result == null || result.ApiPathId != pathId) return NotFound();
            return Ok(_mappingEngine.Map<VerbViewModel>(result));
        }


        [Route("")]
        public async Task<IHttpActionResult> Post(Guid pathId, Guid groupId, Guid apiId, [FromBody] VerbViewModel value)
        {
            if (value == null) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiVerbs.FirstOrDefaultAsync(i => i.ApiPathId == pathId && i.Verb == value.Verb);
                if (existingRecord != null)
                {
                    return Conflict();
                }

                existingRecord = _mappingEngine.Map<ApiVerb>(value);
                existingRecord.ApiPathId = pathId;
                existingRecord.ApiPath = _context.ApiPaths.Find(pathId);
                _context.ApiVerbs.Add(existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();

                _mappingEngine.Map(existingRecord, value);
                return
                    Created(Url.Route("GetVerbById",
                        new { id = value.Id, pathId, groupId, apiId }), value);
            }
        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Put(Guid pathId, Guid id, [FromBody] PathViewModel value)
        {
            if (value == null || (value.Id != Guid.Empty && value.Id != id)) return BadRequest();
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                value.Id = id;

                var existingRecord =
                    await
                    _context.ApiVerbs.Include(i => i.ApiStatusCodes)
                            .Include(i => i.ApiPath)
                            .Include(i => i.Parameters)
                            .Include(i => i.RequestBody)
                            .Include(i => i.ResponseBody)
                            .FirstOrDefaultAsync(i => i.Id == id && i.ApiPathId == pathId);
                if (existingRecord == null) return NotFound();
                _mappingEngine.Map(value, existingRecord);
                await _context.SaveChangesAsync();
                transaction.Commit();
                _mappingEngine.Map(existingRecord, value);

                return Ok(value);
            }
        }

        [Route("{id:guid}")]
        public async Task<IHttpActionResult> Delete(Guid pathId, Guid id)
        {
            using (var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var existingRecord = await _context.ApiVerbs.FindAsync(id);
                if (existingRecord == null || existingRecord.ApiPathId != pathId) return NotFound();
                _context.Entry(existingRecord).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                transaction.Commit();
                return StatusCode(HttpStatusCode.NoContent);
            }
        }
    }
}