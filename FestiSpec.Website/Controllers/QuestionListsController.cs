using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FestiSpec.Entities.Dal;
using FestiSpec.Entity;

namespace FestiSpec.Website.Controllers
{
    public class QuestionListsController : Controller
    {
        private FestiSpecContext db = new FestiSpecContext();

        // GET: QuestionLists
        public ActionResult Index()
        {
            return View(db.QuestionLists.ToList());
        }

        // GET: QuestionLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionList questionList = db.QuestionLists.Find(id);
            if (questionList == null)
            {
                return HttpNotFound();
            }
            return View(questionList);
        }

        public ActionResult QuestionList(int? id, int? index, int? inspectionId)
        {
            if (index == null || index <= 0) // Cannot go back before first question
                index = 1;
            Inspection inspection = db.Inspections.FirstOrDefault(i => i.Id == inspectionId);

            QuestionList questionList = db.QuestionLists.Find(id); // Get que5stionList
            if (index > questionList.Questions.Count()) // Cannot go further then last question
                return RedirectToAction("Index", "Home", new { area = "Home" });

            //   index = questionList.Questions.Last().Index;
            questionList.Questions = questionList.Questions.Where(q => q.Index == index).ToList(); // Get only the question with given index
            questionList.Festival.Inspections = new List<Inspection> { inspection }; // Get only the inspection with given id

            if (questionList.Questions.FirstOrDefault(q => q.Index == index).Answers.Count() != 0)
            {
                questionList.Questions.FirstOrDefault().Answers = questionList.Questions.FirstOrDefault().Answers.Where(a => a.Inspection.Id == inspectionId).ToList();
            }

            return View(questionList);
        }

        [HttpPost]
        [ActionName("ProcessQuestion")]
        public HttpResponseMessage PostSimple(Question value)
        {
            // TODO save given answer by user
            if (value != null)
            {
                Answer model;
                Answer answer = null;
                Inspection inspection = db.Inspections.FirstOrDefault(i => i.Id == value.Index);
                Question question = db.Questions.FirstOrDefault(q => q.Id == value.Id);
                if (inspection != null)
                {
                    answer = db.Answers.FirstOrDefault(q => q.Inspection.Id == inspection.Id && q.Question.Id == value.Id);
                }
                if (answer == null)
                {
                    model = new Answer
                    {
                        Question = question,
                        Inspection = inspection
                    };
                    if (question.Type == "Tekening" || question.Type == "Bijlage") model.Attachments = new List<Attachment> { new Attachment { FilePath = value.QuestionText } };
                    else model.AnswerText = value.QuestionText;
                    db.Answers.Add(model);
                }
                else
                {
                    model = answer;
                    if (model.Attachments.Count != 0)
                    {
                        model.Attachments.FirstOrDefault().FilePath = value.QuestionText;
                    }
                    else
                    {
                        model.AnswerText = value.QuestionText;
                    }
                    db.Entry(model).State = EntityState.Modified;
                }

                db.SaveChanges();


                var response = new HttpResponseMessage(HttpStatusCode.Created);
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
