using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShinyBirthday.Entity;

namespace ShinyBirthday.Web.Models.Shiny
{
    public class ShinyInfoView
    {
        public ShinyInfoView() { }
        public ShinyInfoView(ShinyInformation shiny, int visitorvolume)
        {
            Id = shiny.Id;
            Name = shiny.Name;
            Age = shiny.Age;
            Hobby = shiny.Hobby;
            Visitorvolume = visitorvolume;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Hobby { get; set; }

        //
        public int Visitorvolume { get; set; }
    }
}