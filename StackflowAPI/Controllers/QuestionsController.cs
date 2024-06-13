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
    public class QuestionsController : ApiController
    {
        private readonly ApplicationDbContext db;

        public QuestionsController(ApplicationDbContext context)
        {
            db = context;
        }


        // GET: api/Questions
        [AllowAnonymous]
        public IQueryable<Question> GetQuestions()
        {
            return db.Questions;
        }

        // GET: api/Questions/5
        [ResponseType(typeof(Question))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQuestion(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        // PUT: api/Questions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest("Question Id is required to edit the question");
            }

            var userId = User.Identity.GetUserId();

            var originalQuestion = db.Questions.AsNoTracking().FirstOrDefault(q => q.Id == id);

            if (originalQuestion == null)
            {
                return NotFound();
            }

            if (originalQuestion.PostedById != userId)
            {
                return Content(HttpStatusCode.Forbidden, "You are not authorized to edit this question");
            }

            if (question != null && userId != null)
            {
                db.Entry(question).State = EntityState.Modified;
                db.Entry(question).Property(q => q.PostedAt).IsModified = false;
                db.Entry(question).Property(q => q.PostedById).IsModified = false;
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
                if (!QuestionExists(id))
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

        // POST: api/Questions
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> PostQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Identity.GetUserId();
            if (question != null && userId != null)
            {
                var zone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                var timeInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
                question.PostedById = userId;
                question.PostedAt = timeInIndia;
            }
            else
            {
                return BadRequest(ModelState);
            }
            db.Questions.Add(question);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [ResponseType(typeof(Question))]
        public async Task<IHttpActionResult> DeleteQuestion(int id)
        {
            Question question = await db.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            var userId = User.Identity.GetUserId();
            var originalQuestion = db.Questions.AsNoTracking().FirstOrDefault(q => q.Id == id);

            if (originalQuestion == null)
            {
                return NotFound();
            }

            if (originalQuestion.PostedById != userId)
            {
                return Content(HttpStatusCode.Forbidden, "You are not authorized to delete this question");
            }

            if (question != null && userId != null)
            {
                db.Questions.Remove(question);
                await db.SaveChangesAsync();
            }
            else
            {
                return BadRequest(ModelState);
            }

            return Ok(question);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}