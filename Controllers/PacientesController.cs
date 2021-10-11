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
        private OdontologicoDbEntities1 db = new OdontologicoDbEntities1();

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
                    //Mensaje(paciente.Email, paciente.Nombre, paciente.Apellido, paciente.Edad, paciente.Direccion);
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

        public static void Mensaje(string correo, string nombre, string apellido, int edad, string direccion)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("ClinicaDental.Llu@gmail.com");
                mail.To.Add(correo);
                mail.Body = "Datos registrados de paciente \nNombre: " +nombre+ "\nApellido: " +
                    apellido + "\nEdad: " + edad + "\nDireccion: " + direccion;
                mail.Subject = "Envio de datos de registro";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("erickson_fana@ucne.edu.do", "Maria4020");
                SmtpServer.EnableSsl = true;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Send(mail);

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
