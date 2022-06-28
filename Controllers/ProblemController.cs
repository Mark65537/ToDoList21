﻿using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList21.Data;
using ToDoList21.Models;
using System.Linq;

namespace ToDoList21.Controllers
{
    public class ProblemController: Controller
    {
        private readonly AppDBContext _appDBContext=new AppDBContext();
        public IActionResult Index()
        {
            var model=_appDBContext.ProblemSet.ToList();
            return View(model);
        }

        public ViewResult Create()
        {
            return View(new ProblemCreateViewModel());
        }
        [HttpPost]
        public IActionResult Create(
    [Bind("Title, Description, Executors, " +
                    "PlannedComplexityTime")]
                           ProblemCreateViewModel model
    )
        {
            if (ModelState.IsValid)
            {
                var newProblem = new Problem
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    PlannedComplexityTime = model.PlannedComplexityTime,
                    StartDate = DateTime.Now
                };

                TempData["Message"] = "Задача " + newProblem.Title + " успешно создана!";

                _appDBContext.Add(newProblem);
                _appDBContext.SaveChanges();

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Задача " + model.Title + " не может быть создана!";

            return View(model);
        }
        public ViewResult Delete(int id)
        {
            var model = _appDBContext.ProblemSet.First(x => x.Id == id);
            return View(new ProblemDeleteViewModel(model));
        }
        [HttpPost]
        public ActionResult Delete([Bind("id, Title")]  ProblemDeleteViewModel model)
        {
            if (ModelState.IsValid) {
                var rprob = new Problem
                {
                    Id = model.id
                };

                _appDBContext.Remove(rprob);
                _appDBContext.SaveChanges();

                TempData["Message"] = "Задача " + rprob.Title + " успешно удалена!";

                return RedirectToAction("Index");
            }
            TempData["Message"] = "Задача " + model.Title + " не может быть удалена!";

            return RedirectToAction("Index");
        }
        public ViewResult Update(int id)
        {
            var model = _appDBContext.ProblemSet.First(x => x.Id == id);
            return View(new ProblemUpdateViewModel(model));
        }
        [HttpPost]
        public ActionResult Update(ProblemUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var upProblem = new Problem
                {
                    Id=model.Id,//обязательно
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors
                };

                _appDBContext.Update(upProblem);
                _appDBContext.SaveChanges();

                TempData["Message"] = "Задача " + model.Title + " изменена!";

                return RedirectToAction("Index");
            }
            TempData["Message"] = "Задача " + model.Title + " не может быть изменена!";

            return RedirectToAction("Index");
        }
        public ViewResult AddSubProblem(int ProblemId)
        {
            return View(new ProblemCreateViewModel(ProblemId));
        }
        [HttpPost]
        public ActionResult AddSubProblem(ProblemCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var m = _appDBContext.ProblemSet.First(x => x.Id == model.ProblemId);

                var newProblem = new Problem
                {
                    Title = model.Title,
                    Description = model.Description,
                    Executors = model.Executors,
                    PlannedComplexityTime = model.PlannedComplexityTime,
                    StartDate = DateTime.Now,
                    ProblemId = model.ProblemId
                };
                _appDBContext.Add(newProblem);
                _appDBContext.SaveChanges();

                //var m2 = _appDBContext.ProblemSet.First(x => x.Title.Equals(model.Title));
                //m.ProblemId = m2.Id;
                ////var upProblem = new Problem
                ////{
                ////    Id = m.ProblemId,
                ////    Title = m.Title,
                ////    Description = m.Description,
                ////    Executors = m.Executors,
                ////    PlannedComplexityTime = m.PlannedComplexityTime,
                ////    StartDate = m.StartDate,
                ////    ProblemId = model.Id
                ////};
                //_appDBContext.Update(m);
                //_appDBContext.SaveChanges();

                TempData["Message"] = "Задача " + newProblem.Title + " успешно создана!";

                return RedirectToAction("Index");
            }

            TempData["Message"] = "Задача " + model.Title + " не может быть создана!";

            return View(model);
        }
        public ActionResult GetDescription(string id)//важно что бы имя совпадало
        {
            if (!string.IsNullOrEmpty(id))
            {
                var model = _appDBContext.ProblemSet.First(x => x.Id == int.Parse(id));
                if (model != null)
                {
                    return PartialView("_DisplayProblemDescriptionPartial", new ProblemDescriptionViewModel(model));//модель должна совпадать
                }
            }
            throw new NullReferenceException();
        }
    }
}
