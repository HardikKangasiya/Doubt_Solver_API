using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using StackflowAPI.Models;

namespace StackflowAPI.Controllers
{
    [Authorize]
    public class AnswersController : ApiController
    {
        private readonly ApplicationDbContext db;

        public AnswersController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: api/Answers/5
        [HttpGet]
        [Route("api/answers/{QuestionId}")]
        [AllowAnonymous]
        public IQueryable<Answer> GetAnswers([FromUri]int QuestionId)
        {
            return db.Answers.Where(a => a.QuestionId == QuestionId);
        }

        // GET: api/Answers/5
        [ResponseType(typeof(Answer))]
        [HttpGet]
        [Route("api/answer/{AnswerId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAnswer(int AnswerId)
        {
            Answer answer = await db.Answers.FindAsync(AnswerId);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        [HttpPut]
        [Route("api/answers/{id}")]
        public async Task<IHttpActionResult> PutAnswer([FromUri]int id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.Id)
            {
                return BadRequest("Answer Id is either empty or not matching");
            }

            var userId = User.Identity.GetUserId();

            var originalAnswer = db.Answers.AsNoTracking().FirstOrDefault(q => q.Id == id);

            if (originalAnswer == null)
            {
                return Content(HttpStatusCode.NotFound, "AnswerId not found");
            }

            if (originalAnswer.PostedById != userId)
            {
                return Content(HttpStatusCode.Forbidden, "You are not authorized to edit this question");
            }

            if (answer != null && userId != null)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.Entry(answer).Property(q => q.PostedAt).IsModified = false;
                db.Entry(answer).Property(q => q.PostedById).IsModified = false;
                db.Entry(answer).Property(q => q.QuestionId).IsModified = false;
            }
            else
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Answers/5
        [ResponseType(typeof(Answer))]
        [HttpPost]
        [Route("api/answers/{QuestionId}")]
        public async Task<IHttpActionResult> PostAnswer([FromUri]int QuestionId, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (answer != null && (db.Questions.Any(q => q.Id == QuestionId)))
            {
                var userId = User.Identity.GetUserId();
                answer.QuestionId = QuestionId;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var timeInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
                answer.PostedById = userId;
                answer.PostedAt = timeInIndia;
            }
            else
            {
                ModelState.AddModelError("NullError", "Check Answer is not empty and Question is available");
                return BadRequest(ModelState);
            }
            db.Answers.Add(answer);
            await db.SaveChangesAsync();

            return Created("api/answers/{QuestionId}", answer);
        }

        // DELETE: api/Answers/5
        [ResponseType(typeof(Answer))]
        [HttpDelete]
        [Route("api/answers/{id}")]
        public async Task<IHttpActionResult> DeleteAnswer(int id)
        {
            Answer answer = await db.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            var userId = User.Identity.GetUserId();
            

            if (answer.PostedById != userId)
            {
                return Content(HttpStatusCode.Forbidden, "You are not authorized to delete this answer");
            }

            if (answer != null && userId != null)
            {
                db.Answers.Remove(answer);
                await db.SaveChangesAsync();
            }
            else
            {
                return BadRequest(ModelState);
            }

            return Ok(answer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.Id == id) > 0;
        }
    }
}