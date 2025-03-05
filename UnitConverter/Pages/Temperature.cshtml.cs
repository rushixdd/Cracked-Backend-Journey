using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using UnitConverter.Services;

namespace UnitConverter.Pages
{
    public class TemperatureModel : PageModel
    {
        [BindProperty] public double InputValue { get; set; }
        [BindProperty] public string FromUnit { get; set; }
        [BindProperty] public string ToUnit { get; set; }
        public double? Result { get; private set; }

        public void OnPost()
        {
            try
            {
                Result = ConversionService.ConvertTemperature(InputValue, FromUnit, ToUnit);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }
}
