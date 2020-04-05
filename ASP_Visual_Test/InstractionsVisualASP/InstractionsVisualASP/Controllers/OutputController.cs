using InstractionsVisualASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstractionsVisualASP.Controllers
{
    public class OutputController : Controller
    {
        Output ot = new Output();
        
        string fileFormat = ".pdf";

        public ActionResult ProgramOutput(FormCollection form)
        {
            ot.FileName = $"{form["programName"]}{fileFormat}".ToLower();
            return View(ot);
        }

        
        public ActionResult WorkInstructionOutput(FormCollection form)
        {
            ot.FileName = $"{form["workInstruction"]}{fileFormat}".ToLower();
            return View(ot);
        }

    }
}