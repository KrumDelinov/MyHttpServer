using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DemoApp
{
    public class Tweet
    {
        public int Id { get; set; }

        public DateTime CreatOn { get; set; }

        [Required]
        public string Creator { get; set; }

        [Required]
        public string Content { get; set; }
    }

}
