using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Programa_Odontologico.Models;
using System.Net.Mail;
using System.Linq.Expressions;

namespace Programa_Odontologico.Controllers
{
    public class PacientesController : Controller
    {
        private OdontologicoEntities5 db = new OdontologicoEntities5();

        public ActionResult Index()
        {
            return View(db.Pacientes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PacienteId,Nombre,Apellido,Edad,Direccion,Fecha,Email,Telefono")] Paciente paciente)
        {
            bool valido = validarCorreo(paciente.Email);
            if(valido == false){
                ModelState.AddModelError(
                    "Email", "Debe ingresar una direccion de correo electronico valida"
                    );
            }
            else
            {
                if (ModelState.IsValid)
                {
                    Mensaje(paciente.Email);
                    db.Pacientes.Add(paciente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(paciente);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PacienteId,Nombre,Apellido,Edad,Direccion,Fecha,Email,Telefono")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paciente);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paciente paciente = db.Pacientes.Find(id);
            db.Pacientes.Remove(paciente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public static bool validarCorreo(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void Mensaje(string correo)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ClinicaDental.Llu@gmail.com");
                mail.To.Add(correo);
                mail.Body = "Verifique su correo electronico ingresando a este link";
                mail.Subject = "Verificacion de correo";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("therangers1009@gmail.com", "Maria4030");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
