using cloudscribe.Web.Common.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace sourceDev.WebApp.ViewModels
{
    public class DateValidationTestViewModel
    {
        public DateValidationTestViewModel()
        {
            MinDateValue = new DateTime(2019, 1, 1);
            MaxDateValue = new DateTime(2019, 8, 1);
        }

        public DateTime? MinDateValue { get; set; }

        public DateTime? MaxDateValue { get; set; }

        [DateMinValue("MinDateValue", "Start date must be greater than or equal to {0}", false)]
        [DateMaxValue("MaxDateValue", "Start date must be less than or equal to {0}", false)]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }


        [DateMaxValue("MaxDateValue", "End date must be less than or equal to {0}", false)]
        [DateGreaterOrEqual("StartDate", "End date must be greater than or equal to Start Date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }

    }
}
