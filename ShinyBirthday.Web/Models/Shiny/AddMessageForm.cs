using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ShinyBirthday.Web.Models.Shiny
{
    public class AddMessageForm
    {
        public int Id { get; set; }

        [Required, StringLength(500)]
        public string MessageWords { get; set; }

        [Required,StringLength(20)]
        public string FriendName { get; set; }

        [Required, StringLength(5)]
        public string YanzhengValue { get; set; }
    }
}