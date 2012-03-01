using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShinyBirthday.Web.Models.Shiny
{
    public class AddTopMessage
    {
        [Required, StringLength(100)]
        public string TopIdentity { get; set; }

        [Required, StringLength(1000)]
        public string TopMessage { get; set; }

    }
}