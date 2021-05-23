using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CalorieTracker.Controllers
{
    public class BaseController : Controller
    {
        protected static ValidationResult ValidateRequest (ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return ValidationResult.Success;
            var modelErrors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                .ToList();
            return new ValidationResult(string.Join(",\n",modelErrors));
        }
    }
}