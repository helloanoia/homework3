using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Route("users")]
    public class EmployeeController : Controller
    {
        public IEmployeesData _employees { get; }

        public EmployeeController(IEmployeesData employees)
        {
            _employees = employees;
        }

        [Route("all")]
        public IActionResult Index()
        {
            //return new HttpNotAuthorizedResult();
            //return StatusCode(500);
            //return new RedirectResult("");
            //return new RedirectToActionResult();
            //return new PartialViewResult();
            //return new JsonResult("");
            //return new NotFoundResult();
            //return new FileContentResult();        
            //return new EmptyResult();
            //return new ContentResult();

            //return View("Details", _employees);
            return View(_employees.GetAll());
        }

        [Route("{id}")]
        public IActionResult Details(int id)
        {
            //throw new ApplicationException("Что-то пошло не так...");

            // Получаем сотрудника по Id
            var employee = _employees.GetById(id);

            // Если такого не существует
            if (employee == null)
                return NotFound();// возвращаем результат 404 Not Found

            // Иначе возвращаем сотрудника
            return View(employee);
        }

        [HttpGet]
        [Route("edit/{id?}")]
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return View(new EmployeeView());
            }

            EmployeeView model = _employees.GetById(id.Value);
            if(model == null)
            {
                return NotFound();// возвращаем результат 404 Not Found
            }

            return View(model);
        }

        [HttpPost]
        [Route("edit/{id?}")]
        public IActionResult Edit(EmployeeView model)
        {
            if (model.Id > 0) // если есть Id, то редактируем модель
            {
                var dbItem = _employees.GetById(model.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound();// возвращаем результат 404 Not Found

                dbItem.FirstName = model.FirstName;
                dbItem.SurName = model.SurName;
                dbItem.Age = model.Age;
                dbItem.Patronymic = model.Patronymic;
            }
            else // иначе добавляем модель в список
            {
                _employees.AddNew(model);
            }

            _employees.Commit(); // станет актуальным позднее (когда добавим БД)

            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            _employees.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}