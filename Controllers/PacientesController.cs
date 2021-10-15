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
using System.Text.RegularExpressions;

namespace Programa_Odontologico.Controllers
{
    public class PacientesController : Controller
    {
        private OdontologicoDbEntities1 db = new OdontologicoDbEntities1();

        public ActionResult Index()
        {
            return View(db.Pacientes.ToList());
        }
        [HttpPost]
        public ActionResult Index(string busqueda)
        {
            ViewData["CurrentFilter"] = busqueda;
            var paciente = from s in db.Pacientes select s;

            if (!string.IsNullOrEmpty(busqueda))
            {
                paciente = paciente.Where(s => s.Nombre.Contains(busqueda) || s.Apellido.Contains(busqueda)
                || s.Edad.ToString().Contains(busqueda) || s.Direccion.Contains(busqueda) || s.Fecha.ToString().Contains(busqueda)
                || s.Email.Contains(busqueda) || s.Telefono.Contains(busqueda));
            }
            return View(paciente.ToList());
        }

        [HttpPost]
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
            bool valido = ValidarCorreo(paciente.Email);
            bool validarTelefono = ValidarTelefonos7a10Digitos(paciente.Telefono);
            if (valido == false) {
                ModelState.AddModelError(
                    "Email", "Debe ingresar una direccion de correo electronico valida"
                    );
            }
            if (validarTelefono == false)
            {
                ModelState.AddModelError(
                    "Telefono", "Debe ingresar un numero de telefono valido"
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

        public static bool ValidarCorreo(string email)
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
                mail.From = new MailAddress("therangers15@gmail.com");
                mail.To.Add(correo);
                mail.Body = "Datos registrados de paciente \nNombre: " +nombre+ "\nApellido: " +
                    apellido + "\nEdad: " + edad + "\nDireccion: " + direccion;
                mail.Subject = "Envio de datos de registro";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("erickson_fana@ucne.edu.do", "Maria4020");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static bool ValidarTelefonos7a10Digitos(string strNumber)
        {
            Regex regex = new Regex(@"\A[0-9]{7,10}\z");
            Match match = regex.Match(strNumber);

            if (match.Success)
                return true;
            else
                return false;
        }
    }
}
