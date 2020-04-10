using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace leave_management.Controllers
{
    [Authorize(Roles ="Administrator")] 
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeRepository _rep;
        private readonly IMapper _mapper;

        public LeaveTypeController(ILeaveTypeRepository rep, IMapper mapper)
        {
            _rep = rep;
            _mapper = mapper;
        }

        // GET: LeaveType
        public ActionResult Index()
        {
            var leaveType = _rep.FindAll().ToList();
            var model = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leaveType);
            return View(model);
        }

        // GET: LeaveType/Details/5
        public ActionResult Details(int id)
        {
            if (!_rep.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _rep.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leaveType);
            return View(model);
        }

        // GET: LeaveType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LeaveTypeVM  model)
        {
            try
            {
               if(!ModelState.IsValid)
                {
                    return View(model);
                }
               else
                {
                    var leaveType = _mapper.Map<LeaveType>(model);
                    leaveType.DateCreated = DateTime.Now;

                    var isSuccess=_rep.Create(leaveType);
                    if(!isSuccess)
                    {
                        ModelState.AddModelError("", "Something went wrong");
                        return View(model);
                    }
                    return RedirectToAction(nameof(Index));
                } 
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        // GET: LeaveType/Edit/5
        public ActionResult Edit(int id)
        {
            if(!_rep.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _rep.FindById(id);
            var model = _mapper.Map<LeaveTypeVM>(leaveType);
            return View(model);
        }

        // POST: LeaveType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LeaveTypeVM model)
        {
            try
            {
               if(!ModelState.IsValid)
                {
                    return View(model);
                }
                var leaveType = _mapper.Map<LeaveType>(model);
                var isSuccess = _rep.Update(leaveType);
                if(!isSuccess)
                {
                    ModelState.AddModelError("","Something went wrong");
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }
        }

        // GET: LeaveType/Delete/5
        public ActionResult Delete(int id)
        {
            if (!_rep.IsExists(id))
            {
                return NotFound();
            }
            var leaveType = _rep.FindById(id); 
            var isSuccess = _rep.Delete(leaveType);
            if (!isSuccess)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}